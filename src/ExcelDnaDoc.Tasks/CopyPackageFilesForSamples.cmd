mkdir ..\samples\Package\tools
xcopy %1..\* ..\samples\Package\tools /S /I /Y /Q

mkdir ..\samples\Package\build
copy ExcelDnaDoc.targets ..\samples\Package\build