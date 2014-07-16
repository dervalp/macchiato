@echo Off
set target=BuildAndDeployToProfile

set publishProfile=%1
if "%publishProfile%" == "" (
   set publishProfile="speak"
)

set config=%2
if "%config%" == "" (
   set config=Debug
)
%WINDIR%\Microsoft.NET\Framework\v4.0.30319\msbuild Build\Build.proj /t:"%target%" /p:Configuration="%config%" /fl /flp:LogFile=msbuild.log;Verbosity=Detailed /nr:false