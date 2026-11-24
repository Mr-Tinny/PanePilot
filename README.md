# PanePilot

**[Download PanePilot Portable 1.0.0.25 for Windows (ZIP)](https://github.com/Mr-Tinny/PanePilot/releases/download/portable-v1.0.0.25/PanePilot-1.0.0.25-Portable.zip)** · [Latest release](https://github.com/Mr-Tinny/PanePilot/releases/latest)

Watch multiple streams side by side. Each pane has its own address, volume, mute, and retry controls.

## Open PanePilot

1. Extract the entire portable ZIP to a folder you plan to keep.
2. Open the extracted `PanePilot` folder and run `PanePilot.exe`. Keep the other files alongside it.
3. Paste a stream URL and select **Open page**. Repeat for another stream, then select **Tile** to arrange them.

The `PanePilotSetup.exe` and `PanePilot.exe` files listed in the repository root belong to the older installer-based version. Use the portable ZIP linked above.

## Update a portable copy

1. Download the portable ZIP from the latest release. Version 1.0.0.20 and later can also open the release page from an update notice.
2. If you want your open panes back, make sure **Settings → Reopen previous panes at startup** is enabled before closing PanePilot.
3. Open the folder for the copy you normally use.
   - **1.0.0.21 or later:** Choose **Help → Prepare this copy for update**, then **Open folder and close**. If Explorer cannot open the folder, paste the selected path into File Explorer's address bar after copying it, then choose **Close PanePilot**.
   - **Earlier portable versions:** Right-click your usual PanePilot desktop shortcut, choose **Open file location**, and close PanePilot.
4. Open the ZIP and its inner `PanePilot` folder. Copy the files inside it into the app folder you opened and select **Replace the files in the destination**. Copying the entire folder creates a nested copy and leaves the shortcut on the old version.
5. Launch your usual shortcut. The title bar shows the running version.

## Pane volume

Use the slider in each pane to set its audio and video volume. PanePilot keeps that level when a player reloads or tries to change it. Opening the same web address again restores the last level you chose, even if you closed the pane before quitting. New addresses use your default volume, and saved layouts restore their own levels.

## Share window audio in Discord

If you can hear PanePilot but viewers hear silence, enable **Settings → Discord window-share audio**. Close every PanePilot window, reopen one copy, and start a new share of the **PanePilot** application window with sound enabled in Discord. The pane volume sliders, mute, and Audio follows Focus control the shared mix. You can use this setting with hardware acceleration on or off. To return to normal audio routing, clear the setting and restart PanePilot.

## Troubleshoot black video when screen sharing

Under **Settings**, clear **Use hardware acceleration**, close every PanePilot window, and reopen the app. Share the PanePilot window again and compare the same video. To restore the default, check the setting and restart PanePilot again.

## Watch in fullscreen

Choose **View → Fullscreen (theater mode)** or press **F11**. Move the pointer to the top edge of the monitor to reveal the app menu, toolbar, and active pane controls. They fade away after the pointer leaves. Press **Esc** or **F11** to return to your previous window and layout.
