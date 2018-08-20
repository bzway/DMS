@echo off
dotnet pack
echo ====================
echo build sucessful
echo ====================
dir *.nupkg /B /S >loglist.txt
setlocal enabledelayedexpansion
for /f %%a in (.\loglist.txt) do (
   Xcopy /f /y %%a "pack\"
)
del /q loglist.txt
echo ====================
echo copy files sucessful
echo ====================
endlocal
@echo on

 