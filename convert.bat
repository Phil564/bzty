@echo off
del /q baseapk.1.0.6.apk
"tools\7z.exe" x "baseapk.1.0.6.7z" -o%~dp0tmp_convert
echo ^<Converter^>[92m Successfully extracted the 7z. [0m
"tools\7z.exe" a -tzip "baseapk.1.0.6.apk" "%~dp0tmp_convert\*" -mx9 -aoa
echo ^<Converter^>[92m Successfully converted the 7z file to an apk. [0m
rd /s /q %~dp0tmp_convert
echo ^<Converter^>[92m Successfully deleted the temporary directory. [0m
echo ^<Converter^>[92m Checking if the converted apk is OK. [0m
for /f "tokens=2 delims=: " %%a in ('tools\aapt.exe dump badging baseapk.1.0.6.apk ^| findstr "package"') do (
	set "PACKAGE_NAME=%%a"
)
for /f "tokens=4 delims=: " %%a in ('tools\aapt.exe dump badging baseapk.1.0.6.apk ^| findstr "package"') do (
	set "VERSION_NAME=%%a"
)
echo ^<Compiler^>[92m %PACKAGE_NAME% [0m
echo ^<Compiler^>[92m %VERSION_NAME% [0m
if "%PACKAGE_NAME%"=="name='com.homy.graffiti.uc'" if "%VERSION_NAME%"=="versionName='1.0.6'" (
    echo ^<Compiler^>[92m Correct APK! You can now compile... [0m
) else (
	echo ^<Compiler^>[91m Wrong APK! Please find the correct apk. Correct Package Name: com.homy.graffiti.uc, Correct Version: 1.0.6. [0m
)
pause