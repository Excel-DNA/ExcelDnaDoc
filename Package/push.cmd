@echo off
setlocal

set currentPath=%~dp0
set basePath=%currentPath:~0,-1%
set outputPath=%basePath%\nupkg

nuget.exe push "%outputPath%\ExcelDnaDoc.1.6.1-alpha2.nupkg" -Source  https://api.nuget.org/v3/index.json -Verbosity detailed -NonInteractive
@if errorlevel 1 goto end
