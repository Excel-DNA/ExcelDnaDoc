@echo off
setlocal

set currentPath=%~dp0
set basePath=%currentPath:~0,-1%
set outputPath=%basePath%\nupkg

if exist "%outputPath%\*.nupkg" del "%outputPath%\*.nupkg"

if not exist "%outputPath%" mkdir "%outputPath%"

echo on
nuget.exe pack "%basePath%\ExcelDnaDoc\ExcelDnaDoc.nuspec" -BasePath "%basePath%\ExcelDnaDoc" -OutputDirectory "%outputPath%" -Verbosity detailed -NonInteractive
@if errorlevel 1 goto end

:end
