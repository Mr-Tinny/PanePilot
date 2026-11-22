using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;

[assembly:System.Reflection.AssemblyTitle("PanePilot Launcher")]
[assembly:System.Reflection.AssemblyProduct("PanePilot")]
[assembly:System.Reflection.AssemblyDescription("Checks the PanePilot release feed and opens the installed app")]
[assembly:System.Reflection.AssemblyVersion(ReleaseInfo.Version)]

class Launcher {
    static readonly string root=Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),"PanePilot");
    static readonly string[] files={"PanePilot.exe","WebView2Loader.dll","Microsoft.Web.WebView2.Core.dll","Microsoft.Web.WebView2.WinForms.dll","AdAway-hosts.txt"};
    static string Executable(string directory){return Path.Combine(directory,"PanePilot.exe");}
    static Version ReleaseVersion(string directory){
        return new Version(FileVersionInfo.GetVersionInfo(Executable(directory)).FileVersion);
    }
    const string feed="https://raw.githubusercontent.com/Mr-Tinny/PanePilot/refs/heads/main/";
    const int packageLimit=32*1024*1024;
    sealed class Release {
        public Version Version;
        public string Digest;
        public byte[] Metadata,Signature;
    }
    static Release SignedRelease(byte[] metadata,byte[] signature){
        using(var key=typeof(Launcher).Assembly.GetManifestResourceStream("release-public-key.xml")){
            if(key==null)throw new InvalidDataException("No release verification key is configured.");
            using(var reader=new StreamReader(key))
            using(var rsa=new RSACryptoServiceProvider(new CspParameters(24){Flags=CspProviderFlags.CreateEphemeralKey})){
                rsa.FromXmlString(reader.ReadToEnd());
                if(rsa.KeySize<2048||!rsa.VerifyData(metadata,"SHA256",signature))
                    throw new InvalidDataException("Invalid release signature.");
            }
        }
        var info=XElement.Parse(Encoding.UTF8.GetString(metadata));
        string digest=(string)info.Attribute("sha256");
        if(info.Name!="release"||digest==null||digest.Length!=64||!digest.All(Uri.IsHexDigit))
            throw new InvalidDataException("Invalid release metadata.");
        return new Release{Version=Version.Parse((string)info.Attribute("version")),Digest=digest,
            Metadata=metadata,Signature=signature};
    }
    static string Hash(Stream input){
        using(var sha=SHA256.Create())return BitConverter.ToString(sha.ComputeHash(input)).Replace("-","");
    }
    static void CheckPackage(Stream package,Release release,string directory,bool extract){
        if(package.Length>packageLimit||!String.Equals(Hash(package),release.Digest,StringComparison.OrdinalIgnoreCase))
            throw new InvalidDataException("The update checksum did not match.");
        package.Position=0;
        if(!extract){
            // App-local dependencies and configuration can affect what the executable loads.
            if(Directory.GetDirectories(directory).Length!=0||Directory.GetFiles(directory).Any(path=>
                !files.Contains(Path.GetFileName(path),StringComparer.OrdinalIgnoreCase)))
                throw new InvalidDataException("Unexpected files in the application directory.");
        }
        using(var archive=new ZipArchive(package,ZipArchiveMode.Read,true)){
            foreach(string name in files){
                var entry=archive.Entries.Single(e=>e.FullName.Replace('\\','/')=="PanePilot/"+name);
                if(entry.Length>64*1024*1024)throw new InvalidDataException("Unexpected package size.");
                string path=Path.Combine(directory,name);
                using(var source=entry.Open()){
                    if(extract){using(var output=File.Create(path))source.CopyTo(output);}
                    else {
                        using(var installed=File.OpenRead(path))
                            if(installed.Length!=entry.Length||Hash(installed)!=Hash(source))
                                throw new InvalidDataException("Installed file failed verification: "+name);
                    }
                }
            }
        }
        if(ReleaseVersion(directory)!=release.Version)throw new InvalidDataException("Unexpected executable version.");
    }
    static string Install(Stream package,Release release,string destination){
        string target=Path.Combine(destination,release.Version.ToString());
        if(Directory.Exists(target)){CheckPackage(package,release,target,false);return target;}
        string staging=Path.Combine(destination,"staging-"+Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(staging);
        try{
            CheckPackage(package,release,staging,true);
            Directory.Move(staging,target);return target;
        }finally{if(Directory.Exists(staging))Directory.Delete(staging,true);}
    }
    static Release Embedded(){
        return new Release{Version=new Version(ReleaseInfo.Version),Digest=ReleaseInfo.Hash};
    }
    static string VerifyCurrent(Release release,string destination){
        string directory=Path.Combine(destination,release.Version.ToString());
        using(var package=release.Metadata==null?typeof(Launcher).Assembly.GetManifestResourceStream("package"):
            File.OpenRead(Path.Combine(destination,"verified-releases",release.Version.ToString(),"package.zip"))){
            CheckPackage(package,release,directory,false);
        }
        return directory;
    }
    static byte[] Download(string url,int limit){
        var req=(HttpWebRequest)WebRequest.Create(url);req.AllowAutoRedirect=false;req.Timeout=4500;req.ReadWriteTimeout=8000;req.UserAgent="PanePilot-Updater";
        using(var response=req.GetResponse())using(var stream=response.GetResponseStream())using(var output=new MemoryStream()){
            byte[] buffer=new byte[16384];int count;
            while((count=stream.Read(buffer,0,buffer.Length))>0){
                if(output.Length+count>limit)throw new InvalidDataException("Unexpected update size.");
                output.Write(buffer,0,count);
            }
            return output.ToArray();
        }
    }
    static Release NewestCached(Release current,string cache){
        if(!Directory.Exists(cache))return current;
        foreach(string dir in Directory.GetDirectories(cache)){
            Version version;
            if(!Version.TryParse(Path.GetFileName(dir),out version)||version<=current.Version)continue;
            var release=SignedRelease(File.ReadAllBytes(Path.Combine(dir,"release.xml")),
                File.ReadAllBytes(Path.Combine(dir,"release.sig")));
            if(release.Version!=version)throw new InvalidDataException("Cached release version mismatch.");
            current=release;
        }
        return current;
    }
    static Release Update(Release current,string cache){
        byte[] metadata=Download(feed+"PanePilot-release.xml",16384);
        byte[] signature=Download(feed+"PanePilot-release.xml.sig",1024);
        var release=SignedRelease(metadata,signature);
        if(release.Version<=current.Version)return current;
        byte[] bytes=Download(feed+"PanePilot-"+release.Version+".zip",packageLimit);
        using(var package=new MemoryStream(bytes))Install(package,release,root);
        string staging=Path.Combine(cache,"staging-"+Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(staging);
        try{
            File.WriteAllBytes(Path.Combine(staging,"release.xml"),metadata);
            File.WriteAllBytes(Path.Combine(staging,"release.sig"),signature);
            File.WriteAllBytes(Path.Combine(staging,"package.zip"),bytes);
            Directory.Move(staging,Path.Combine(cache,release.Version.ToString()));
        }finally{if(Directory.Exists(staging))Directory.Delete(staging,true);}
        return release;
    }
    static void Shortcut(string launcher,string current){
        dynamic shell=Activator.CreateInstance(Type.GetTypeFromProgID("WScript.Shell"));
        dynamic link=shell.CreateShortcut(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Programs),"PanePilot.lnk"));
        link.TargetPath=launcher;link.IconLocation=Executable(current)+",0";link.WorkingDirectory=root;link.Save();
    }
    [STAThread] static int Main(string[] args){
        try{
            var assembly=typeof(Launcher).Assembly;
            if(args.Contains("--verify-package")){
                using(var package=assembly.GetManifestResourceStream("package")){
                    Install(package,Embedded(),args[1]);
                }
                return 0;
            }
            Directory.CreateDirectory(root);
            string current;
            using(var gate=new Mutex(false,@"Local\PanePilotInstall")){
                try{if(!gate.WaitOne(20000))throw new IOException("Another installation is still running.");}catch(AbandonedMutexException){}
                try{
                    string launcher=Path.Combine(root,"PanePilotLauncher.exe");
                    bool standalone=String.Equals(Path.GetFileName(Application.ExecutablePath),"PanePilot.exe",StringComparison.OrdinalIgnoreCase);
                    bool setup=!standalone&&!String.Equals(Application.ExecutablePath,launcher,StringComparison.OrdinalIgnoreCase);
                    if(setup)File.Copy(Application.ExecutablePath,launcher,true);
                    string cache=Path.Combine(root,"verified-releases");
                    var selected=NewestCached(Embedded(),cache);
                    if(selected.Metadata==null){
                        using(var package=assembly.GetManifestResourceStream("package"))Install(package,selected,root);
                    }
                    try{
                        ServicePointManager.SecurityProtocol=SecurityProtocolType.Tls12;
                        selected=Update(selected,cache);
                    }catch(Exception error){File.WriteAllText(Path.Combine(root,"last-update-error.txt"),error.Message);}
                    current=VerifyCurrent(selected,root);
                    if(setup)Shortcut(launcher,current);
                }finally{gate.ReleaseMutex();}
            }
            Process.Start(new ProcessStartInfo(Executable(current)){WorkingDirectory=current});return 0;
        }catch(Exception error){if(!args.Contains("--verify-package"))MessageBox.Show(error.Message,"PanePilot");return 1;}
    }
}
