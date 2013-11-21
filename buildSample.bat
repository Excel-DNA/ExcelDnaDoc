@echo off
cls
if not exist tools\FAKE\tools\Fake.exe (
"tools\nuget\nuget.exe" "install" "FAKE" "-OutputDirectory" "tools" "-ExcludeVersion"
)
if not exist build\ExcelDnaDoc.exe (
"tools\FAKE\tools\Fake.exe" buildApp.fsx
)
"tools\FAKE\tools\Fake.exe" buildSample.fsx
pause