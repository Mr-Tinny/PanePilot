# PanePilot

**[Download PanePilot Portable 1.0.0.38 for Windows (ZIP)](https://github.com/Mr-Tinny/PanePilot/releases/download/portable-v1.0.0.38/PanePilot-1.0.0.38-Portable.zip)** · [Latest release](https://github.com/Mr-Tinny/PanePilot/releases/latest)

Watch multiple streams side by side. Each pane has its own address, volume, mute, and retry controls.

## Open PanePilot

1. Extract the entire portable ZIP to a folder you plan to keep.
2. Open the extracted `PanePilot` folder and run `PanePilot.exe`. Keep the other files alongside it.
3. Paste a stream URL and select **Open page**. Repeat for another stream, then select **Tile** to arrange them.

The `PanePilotSetup.exe` and `PanePilot.exe` files listed in the repository root belong to the older installer-based version. Use the portable ZIP linked above.

## Keyboard shortcuts and layout history

Press **F1** or choose **Help → Keyboard shortcuts** for the full reference. **Ctrl+Shift+T** reopens the most recently closed pane with its address, name, position, volume, and mute setting. You can reopen up to 20 panes from the current session.

Use **Edit → Undo layout** or **Redo layout** after swapping panes, tiling, cascading, or resizing the split. The command names show the action available. **Ctrl+Z** and **Ctrl+Y** work on PanePilot controls. While editing text or using a website, use the Edit menu for layout history.

## Swap two streams

Once the panes are arranged, choose **View → Lock layout**. This blocks title clicks, drags, pane resizing, tiling, Focus changes, and layout undo or redo. Choose the same item to unlock. The lock works in fullscreen and PanePilot remembers it the next time it opens. Volume, playback, and **Listen** stay available. Opening a saved layout or a new lineup still applies the arrangement you chose.

Drag one pane's title onto the other stream and release when **Drop to swap** appears. The lower pane in a stacked pair has its title at the bottom. Swapping the two side streams keeps the main game and each side stream's audio settings in place. A swap with the main tile or focused pane uses the existing main and side volume levels.

In fullscreen, open **View → Swap panes** from the fading menu. Drag one of the temporary pane labels onto another stream. Press **Esc** to cancel.

## Open a lineup

Use **Open multiple streams** on the toolbar or in **File** to paste a chat message. Uncheck unrelated links, enter optional player names, choose **Set main**, and open the lineup. A link already open in PanePilot reuses that pane. The toolbar is also available at the top in fullscreen.

Hold **Alt** to see numbers and player names. Use **Alt+1**, **Alt+2**, or **Alt+3** to switch among the first three panes. You can change a name under **Edit → Rename pane**. Saved layouts and reopened sessions remember the names.

## Update a portable copy

1. Download the portable ZIP from the latest release. Version 1.0.0.20 and later can also open the release page from an update notice.
2. If you want your open panes back, make sure **Settings → Reopen previous panes at startup** is enabled before closing PanePilot.
3. Open the folder for the copy you normally use.
   - **1.0.0.21 or later:** Choose **Help → Prepare this copy for update**, then **Open folder and close**. If Explorer cannot open the folder, paste the selected path into File Explorer's address bar after copying it, then choose **Close PanePilot**.
   - **Earlier portable versions:** Right-click your usual PanePilot desktop shortcut, choose **Open file location**, and close PanePilot.
4. Open the ZIP and its inner `PanePilot` folder. Copy the files inside it into the app folder you opened and select **Replace the files in the destination**. Copying the entire folder creates a nested copy and leaves the shortcut on the old version.
5. Launch your usual shortcut. The title bar shows the running version.

## Recover a buffering stream

Select a pane and enable **Settings → Auto-recover this pane**, or right-click the page and select **Auto-recover this pane**. Each pane starts with recovery off. Saved layouts and reopened sessions remember the setting.

After five seconds of continuous buffering on a supported live video, PanePilot tries to resume at the live edge. If the video is still buffering five seconds later, it reloads that pane. The notice shows the attempt number. After five attempts, use **Retry stream** to try again. A site that requires a click to play after reloading will wait for you.

## Pane volume controls

Select the headphones beside a pane's speaker to hear just that pane at its current volume. Select them again to restore your mix. In fullscreen, move the pointer near the top and open **Listen** to choose any pane by name while your main stream stays in place. Closing the pane you are listening to also restores the mix.

Press **Ctrl+Shift** and a number from **1** to **9** on the top row to listen to that pane in windowed or fullscreen mode. Press the same shortcut again to restore your mix. Hold **Alt** to see the pane numbers.

Set the level with the pane slider or a supported player volume control. The two controls share the same level. PanePilot keeps that level when a player reloads or tries to change it. Opening the same web address again restores the last level you chose, even if you closed the pane before quitting. New addresses use your default volume, and saved layouts restore their own levels. Switching Focus trades the main and selected pane levels. Other panes keep their levels.

## Share window audio in Discord

If you can hear PanePilot but viewers hear silence, enable **Settings → Discord window-share audio**. Close every PanePilot window, reopen one copy, and start a new share of the **PanePilot** application window with sound enabled in Discord. The pane volume sliders, mute, and Audio follows Focus control the shared mix. You can use this setting with hardware acceleration on or off. To return to normal audio routing, clear the setting and restart PanePilot.

## Troubleshoot black video when screen sharing

Under **Settings**, clear **Use hardware acceleration**, close every PanePilot window, and reopen the app. Share the PanePilot window again and compare the same video. To restore the default, check the setting and restart PanePilot again.

## Watch in fullscreen

Use the slider beside the pane title to adjust a player in fullscreen. In a three-stream layout, the lower stream has its title below the picture. In PanePilot's **F11** mode, hover near the top to see the selected stream's name, slider, and volume percentage. The controls stay visible while you drag.

Choose **View → Fullscreen (theater mode)** or press **F11**. Move the pointer into the top band to reveal the controls. They stay open while you drag the volume slider or use a menu. Hold **Alt** to see the pane numbers over the videos. Release **Alt** to hide the numbers. Choose **Focus pane** or press **Alt+1**, **Alt+2**, or **Alt+3** on the number row to feature a stream. The same shortcuts work in windowed mode. See the assignments under **View** → **Focus pane**. Each stream keeps its number when you switch Focus or reopen the layout. A new pane takes the lowest free number. Hover near the vertical split and drag to resize the columns. With three streams, the two smaller videos meet in the middle of the right column. Press **Esc** or **F11** to return to your previous window and layout.
