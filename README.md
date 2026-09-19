# JMWPlayer

A lightweight media player for Windows, built with C#, WPF and [LibVLCSharp](https://github.com/videolan/libvlcsharp). It aims to give you VLC-style playback of almost any audio or video format with a small, simple interface.

## Features

- Plays most common video and audio formats (MP4, MKV, AVI, MOV, WEBM, MP3, FLAC, WAV, M4A and more)
- Open files with a dialog or by dragging and dropping them onto the window
- Play / pause, seek bar with current and total time, volume control
- Keyboard shortcuts and fullscreen mode

## Keyboard shortcuts

| Key | Action |
| --- | --- |
| `Space` | Play / pause |
| `Left` / `Right` | Seek back / forward 5 seconds |
| `Up` / `Down` | Volume up / down |
| `M` | Mute / unmute |
| `F` | Toggle fullscreen |
| `Esc` | Exit fullscreen |

## Install (pre-built)

1. Go to the [Releases page](../../releases) and download the latest `JMWPlayer-win-x64.zip`.
2. Extract the zip to any folder. Keep all files together, including the `libvlc` folder.
3. Run `JMWPlayer.exe`.

No installer and no .NET installation are needed. The download is self-contained and runs on 64-bit Windows 10 and 11.

> **Windows SmartScreen:** the app is not code-signed, so Windows may show a warning the first time. Choose **More info → Run anyway**.

## Build and run from source

### Requirements

- Windows 10 or 11 (64-bit)
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- Git
- Optional: Visual Studio 2022 with the ".NET desktop development" workload

### Steps

```bash
git clone https://github.com/JoelwMulongo/JMWPlayer.git
cd JMWPlayer
dotnet restore
dotnet run --project JMWPlayer/JMWPlayer.csproj
```

You can also open `JMWPlayer.sln` in Visual Studio and press `F5`.

### Publish a distributable build

```bash
dotnet publish JMWPlayer/JMWPlayer.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o publish
```

The output is in the `publish` folder. Zip the whole folder to share it. `JMWPlayer.exe` needs the `libvlc` folder next to it, so it won't work as a single loose file.

