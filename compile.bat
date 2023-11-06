@echo off
for /f "tokens=2 delims=: " %%a in ('tools\aapt.exe dump badging baseapk.apk ^| findstr "package"') do (
	set "PACKAGE_NAME=%%a"
)
for /f "tokens=4 delims=: " %%a in ('tools\aapt.exe dump badging baseapk.apk ^| findstr "package"') do (
	set "VERSION_NAME=%%a"
)
echo ^<Compiler^>[92m %PACKAGE_NAME% [0m
echo ^<Compiler^>[92m %VERSION_NAME% [0m
if "%PACKAGE_NAME%"=="name='com.homy.graffiti.uc'" if "%VERSION_NAME%"=="versionName='1.0.6'" (
    echo ^<Compiler^> Correct APK! Proceeding to compile...
	rd /s /q %~dp0build
	md build
	copy baseapk.apk build
	echo ^<Compiler^> Successfully copied the baseapk for rezipping.
	ren "build\baseapk.apk" "bzty_temp.apk"
	"tools\7z.exe" a -tzip "build\bzty_temp.apk" "%~dp0src\1.0.6\*" -mx9 -aoa
	echo ^<Compiler^> Successfully added the new files to the apk.
	java.exe -jar "tools\ApkSigner.jar" sign  --key "tools\apkeasytool.pk8" --cert "tools\apkeasytool.pem"  --out "build\bzty.apk" "build\bzty_temp.apk"
	echo ^<Compiler^> Successfully signed the apk.
	del "build\bzty_temp.apk"
) else (
	echo ^<Compiler^> Wrong APK! Please find the correct apk. Correct Package Name & Version: com.homy.graffiti.uc, 1.0.6
)
pause