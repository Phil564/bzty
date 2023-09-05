@echo off
for /f "tokens=2 delims=: " %%a in ('tools\aapt.exe dump badging baseapk.1.0.6.apk ^| findstr "package"') do (
	set "PACKAGE_NAME=%%a"
)
for /f "tokens=4 delims=: " %%a in ('tools\aapt.exe dump badging baseapk.1.0.6.apk ^| findstr "package"') do (
	set "VERSION_NAME=%%a"
)
echo ^<Compiler^>[92m %PACKAGE_NAME% [0m
echo ^<Compiler^>[92m %VERSION_NAME% [0m
if "%PACKAGE_NAME%"=="name='com.homy.graffiti.uc'" if "%VERSION_NAME%"=="versionName='1.0.6'" (
    echo ^<Compiler^>[92m Correct APK! Proceeding to compile... [0m
	rd /s /q %~dp0build
	md build
	copy baseapk.1.0.6.apk build
	echo ^<Compiler^>[92m Successfully copied the baseapk for rezipping. [0m
	ren "build\baseapk.1.0.6.apk" "bzty_temp.apk"
	"tools\7z.exe" a -tzip "build\bzty_temp.apk" "%~dp0src\*" -mx9 -aoa
	echo ^<Compiler^>[92m Successfully added the new files to the apk. [0m
	java.exe -jar "tools\ApkSigner.jar" sign  --key "tools\apkeasytool.pk8" --cert "tools\apkeasytool.pem"  --out "build\bzty.apk" "build\bzty_temp.apk"
	echo ^<Compiler^>[92m Successfully signed the apk. [0m
	del "build\bzty_temp.apk"
) else (
	echo ^<Compiler^>[91m Wrong APK! Please find the correct apk. Correct Package Name: com.homy.graffiti.uc, Correct Version: 1.0.6. [0m
)
pause