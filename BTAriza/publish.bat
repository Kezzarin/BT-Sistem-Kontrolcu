@echo off
echo BTAriza Publish islemi baslatiliyor...
dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true -o publish\
echo.
echo Tamamlandi! Cikti: publish\ klasorunde
pause
