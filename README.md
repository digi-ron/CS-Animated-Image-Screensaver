# Animated Image Screensaver
## Instructions
1. add all images from your animation *as separate frames* within the resources of the C# project (right-click the "WindowsScreensaverTemplate" csproj and click Properties -> Resources -> Add Resource -> Add Existing File...)
    - ensure the frames are in the format "frame_XXX" where XXX is the order number for the frame
    - you can select multiple images at the same time and they will all be inserted using the property name of the image less the extension (e.g. frame_001.jpg becomes frame_001)
1. verify that the developer variables in the "Main.cs" file are all correct (e.g. framerate)
1. (optional) Modify the C# files as required for any further modifications you would like to add
1. build the project
1. rename the .exe file created from the build process to .scr
1. right click the .scr file and click "Install"
1. done

## Limitations
- for some reason all my attempts to make the animation show in the preview window in Windows Screen Saver change dialog has failed, the rough code is there (originally adapted from [HERE](https://sites.harding.edu/fmccown/screensaver/screensaver.html)) but it only shows a black screen, which is not ideal
- I decided that having an animated screensaver would be better as an adapted PictureBox control and timer rather than something like the WMP or VLC dlls, but as a result if you wanted to make a video into this screensaver, you will need to convert. using [FFmpeg](https://ffmpeg.org), a command similar to the below can easily convert a source video into the correct format;
    ```powershell
    .\ffmpeg.exe -i source.mp4 -qscale:v 2 frame_%03d.jpg
    ```

    > Should you wish to compress the image further, a tool such as [ImageMagick](https://imagemagick.org) can be used to compress each image, using the flag `-quality 90`

- I can't seem to modify the build command to output a scr file instead of exe, so it has to be renamed each build. As far as i could find this is a limitation with Visual Studio, and/or Microsoft, who have clearly ditched this technology since it stopped needing to be a thing for CRT displays
- the speed of the animation changes if it's built in 64-bit mode, so I've set the config to export to x86
