## UnityDesktopPetFramework

https://github.com/user-attachments/assets/74aa5786-9882-47b5-ab6b-a5299a2f99a3

A transparent window interactive example project based on Unity URP pipeline, suitable for desktop pets, desktop gadgets, etc.

Compatible with Windows Build

## Readme

- [中文](README-sc.md)
- [English](README.md)

## Version

Unity 2021.3.9f1, using Universal RP 12.1.7

Should be compatible with newer versions, but newer versions of the URP pipeline may require switching RenderFeature

## Download

Download this project and open it with Unity Hub

Does not use UnityPackage, see **Notes** for reasons

## Features

- Post-processing, shaders, Physical system... basically everything Unity can do

- Transparent background top-level display window

- Switch window mouse penetration/interactive state

- Hide taskbar icon at runtime

- Show system tray icon and customizable right-click menu at runtime

## Notes

1. For higher versions (14+) of the URP pipeline:

   The Blit RenderFeature used in the project may no longer be applicable, consider switching to the built-in [Full Screen Pass Renderer Feature](https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@14.0/manual/renderer-features/renderer-feature-full-screen-pass.html) of this pipeline

2. The project includes a localized Universal RP Package

   ```csharp
   Packages/com.unity.render-pipelines.universal/Shaders/PostProcessing/UberPost.shader
   ```

   Since the UberPost.shader in Universal RP has been processed to output actual transparency, it has been localized, [reference link](https://discussions.unity.com/t/urp-camera-doesnt-allow-transparency-obs-overlay/878585/13)

   If you want to use it in an existing project, please note this

## Known Limitations

- Not suitable for HDR

- Calls User32.dll, currently only suitable for Windows platform builds

- Performance requirements may be high, after all, this is basically a Unity game running in the background

## References

- System tray logic from [rocksdanister/rePaper: Desktop that changes based on weather & time](https://github.com/rocksdanister/rePaper)

- Some logic from [kirurobo/UniWindowController: Makes your Unity window transparent and allows you to drop files](https://github.com/kirurobo/uniwindowcontroller)

- DLL call logic reference [\[SOLVED!\] Windows: Transparent window with opaque contents (LWA_COLORKEY)? - Unity Engine - Unity Discussions](https://discussions.unity.com/t/solved-windows-transparent-window-with-opaque-contents-lwa-colorkey/578948/97)
