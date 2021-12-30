@echo off
@rem Unity 工程目录
set UNITY_PROJ_DIR=%cd%\..

set OUTPUT_CSHARP_ASSEMBLY="%UNITY_PROJ_DIR%\Assets\Plugins\Proto.dll"
call Build_Proto_NetCSharp.bat
