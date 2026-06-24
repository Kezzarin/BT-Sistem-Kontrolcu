# BTAriza - Kurulum Talimatları

## 1. .NET 8 SDK Kurulumu
Projeyi derlemek için .NET 8 SDK gereklidir.

İndirme linki:
https://dotnet.microsoft.com/en-us/download/dotnet/8.0

"SDK 8.0.x" → "Windows x64" Installer'ı indirin ve kurun.

## 2. Proje Derleme
SDK kurulunca CMD veya PowerShell'de bu klasörde:

```
dotnet restore
dotnet build
dotnet run
```

## 3. Publish (Tek EXE)
```
publish.bat
```
veya:
```
dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true -o publish\
```

Çıktı: `publish\BTAriza.exe`

## 4. NuGet Paketleri (otomatik indirilir)
- Microsoft.Data.Sqlite 8.0.10 — SQLite veritabanı
- System.Management 8.0.0 — WMI sistem bilgisi
- PdfSharp-WPF 1.50.5147 — PDF rapor
- ClosedXML 0.102.2 — Excel export
