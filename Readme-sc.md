# UnityDesktopPetFramework

https://github.com/user-attachments/assets/8096b714-465f-4d7c-b9a4-662d0bb3a9e1

**更新：添加活动窗口的识别与物理碰撞示例** 

https://github.com/user-attachments/assets/74aa5786-9882-47b5-ab6b-a5299a2f99a3

一个基于Unity URP管线的，可用于桌宠、桌面小工具等用途的透明窗口可交互示例项目

适用于Windows Build

## Readme

- [中文](Readme-sc.md)
- [English](Readme.md)

## 版本

Unity 2021.3.9f1，使用Universal RP 12.1.7

应该兼容更新版本，不过更新版本的URP管线可能需要切换RenderFeature

## 下载

下载这一项目并使用Unity Hub打开

不使用UnityPackage，原因请参见**注意事项**

## 特性

- 后处理、shader、Physical system..总之就是Unity能干的那些事情

- 透明背景的顶层显示窗口

- 切换窗口的鼠标穿透/可交互状态

- 与当前活动的窗口进行物理交互！

- 运行时隐藏任务栏图标

- 运行时显示系统托盘图标与可自定义的右键菜单

## 注意事项

1. 对于更高版本（14+）的URP管线：

   项目中使用的Blit RenderFeature可能不再适用，可考虑切换至这一管线自带的[Full Screen Pass Renderer Feature](https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@14.0/manual/renderer-features/renderer-feature-full-screen-pass.html)

2. 项目中包含Local化的Universal RP Package

   ```csharp
   Packages/com.unity.render-pipelines.universal/Shaders/PostProcessing/UberPost.shader
   ```

   由于对Universal RP中的UberPost.shader做了处理来让其输出实际透明度，对其做了Local化，[参考链接](https://discussions.unity.com/t/urp-camera-doesnt-allow-transparency-obs-overlay/878585/13)

   若想在已有项目中使用也请注意这点

3. 需要确保活动窗口的Collider Layer不包含在交互检测的LayerMask中，否则会无法拖动窗口

## 已知限制

- 不适用于HDR

- 调用User32.dll，当前只适用于Windows平台构建

- 性能需求可能较高，毕竟这玩意基本就是一个后台运行的Unity游戏

## 参考

- 系统托盘逻辑来自[rocksdanister/rePaper: Desktop that changes based on weather & time](https://github.com/rocksdanister/rePaper)

- 部分逻辑来自[kirurobo/UniWindowController: Makes your Unity window transparent and allows you to drop files](https://github.com/kirurobo/uniwindowcontroller)

- DLL调用逻辑参考[\[SOLVED!\] Windows: Transparent window with opaque contents (LWA_COLORKEY)? - Unity Engine - Unity Discussions](https://discussions.unity.com/t/solved-windows-transparent-window-with-opaque-contents-lwa-colorkey/578948/97)

- Blit RenderFeature 来自 Cyanilux : https://github.com/Cyanilux/URP_BlitRenderFeature
