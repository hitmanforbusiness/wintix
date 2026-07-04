# Wintix

Native Windows event ticketing app built with **C#** and **WinUI 3**.

Wintix helps you discover events, manage tickets, and check in — with a modern, fluent Windows experience powered by the Windows App SDK.

## Features

- **Home** — Featured events at a glance
- **Events** — Browse upcoming events
- **My Tickets** — View purchased tickets and barcodes
- **Settings** — App preferences (expandable)

## Requirements

| Tool | Version |
|------|---------|
| Windows | 10 1809+ (19041+ recommended) |
| [.NET SDK](https://dotnet.microsoft.com/download) | 8.0+ |
| [Visual Studio 2022](https://visualstudio.microsoft.com/) | 17.8+ with **WinUI / Windows App SDK** workload |
| Windows App SDK | 1.6+ (bundled via self-contained deployment) |

## Getting started

### Clone

```bash
git clone https://github.com/your-org/wintix.git
cd wintix
```

### Build & run

**Visual Studio**

1. Open `Wintix.sln`
2. Set startup project to **Wintix**
3. Select **x64** (or your target platform)
4. Press **F5**

**Command line**

```bash
dotnet restore Wintix.sln
dotnet build Wintix.sln -c Debug -r win-x64
dotnet run --project src/Wintix/Wintix.csproj
```

The app is configured as a **self-contained win-x64** deployment. The Windows App SDK and .NET runtimes are bundled with the app — end users do not need to install the Windows App Runtime separately.

**Publish for distribution**

```bash
dotnet publish src/Wintix/Wintix.csproj -c Release -r win-x64
```

Ship the contents of `src/Wintix/bin/Release/net8.0-windows10.0.19041.0/win-x64/publish/` (zip the folder or copy it as-is).

### Install workloads (if needed)

```bash
dotnet workload install maui-windows
# Or install "Windows application development" workload in Visual Studio Installer
```

## Project structure

```
wintix/
├── src/
│   └── Wintix/
│       ├── Models/          # Domain models
│       ├── Services/        # Data & API abstractions
│       ├── ViewModels/      # MVVM view models (CommunityToolkit.Mvvm)
│       ├── Views/           # XAML pages
│       ├── Themes/          # Shared styles & resources
│       └── Assets/          # App icons & splash images
├── .github/workflows/       # CI build workflow
├── Wintix.sln
├── ROADMAP.md
├── CHANGELOG.md
└── LICENSE
```

## Architecture

- **WinUI 3** shell with `NavigationView` and frame-based navigation
- **MVVM** via [CommunityToolkit.Mvvm](https://learn.microsoft.com/en-us/dotnet/communitytoolkit/mvvm/)
- **Sample services** provide in-memory demo data until a backend is connected

## Contributing

Issues and pull requests are welcome. See [ROADMAP.md](ROADMAP.md) for planned work.

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/my-change`)
3. Commit your changes
4. Open a pull request

## License

This project is licensed under the [MIT License](LICENSE).
