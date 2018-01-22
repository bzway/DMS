@echo off
dir project.json /b /s >loglist.txt
dotnet restore
echo rem pack >release.bat
setlocal enabledelayedexpansion
for /f %%a in (./loglist.txt) do (
	echo dotnet pack %%a >>release.bat
	dotnet pack %%a
)
del /q loglist.txt
endlocal
