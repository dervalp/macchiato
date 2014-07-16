@echo Off
set target=PublishDemoSite

set publishProfile=%1
if "%publishProfile%" == "" (
   set publishProfile="Speak-demo"
)

set config=%2
if "%config%" == "" (
   set config=Debug
)
%WINDIR%\Microsoft.NET\Framework\v4.0.30319\msbuild Build\Build.proj /t:"%target%" /p:Configuration="%config%" /p:PublishProfile=%publishProfile% /fl /flp:LogFile=msbuild.log;Verbosity=Detailed /nr:false

Publish.cmd