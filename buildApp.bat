@echo off
cls
if not exist tools\FAKE\tools\Fake.exe (
"tools\nuget\nuget.exe" "install" "FAKE" "-OutputDirectory" "tools" "-ExcludeVersion"
)
"tools\FAKE\tools\Fake.exe" buildApp.fsx
pause