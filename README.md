# EZSF2

[![NuGet](https://img.shields.io/nuget/v/EZSF2.svg)](https://www.nuget.org/packages/EZSF2/)
[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](https://opensource.org/licenses/MIT)

C# library for loading SoundFont 2 files.

## Installation

Install via [NuGet](https://www.nuget.org/):

```sh
Install-Package EZSF2 -Version x.x.x
```

Or via the .NET CLI:

```sh
dotnet add package EZSF2 --version x.x.x
```

## Usage

```csharp
using EZSF2;

using BinaryReader br = new BinaryReader(File.OpenRead("soundfont.sf2"));
SF2 soundFont = new SF2(br);
```

You can access SoundFont data with:
- `soundFont.Presets`
- `soundFont.Instruments`
- `soundFont.Samples`

If you want to get note data for a given preset, note, and velocity, you can use:
- `soundFont.GetNote(int preset, int note, int velocity)`


