@echo off
setlocal EnableDelayedExpansion
set MSBuild="%VisualStudio%\MSBuild\Current\Bin\MSBuild.exe"

@rem 协议源文件目录
set PROTO_DIR=%cd%\..\..\Proto
set CSHARP_PROJ_DIR=%cd%\ProtoProject
@rem 输出目录
set CSHARP_OUT=%CSHARP_PROJ_DIR%\Proto
set CSHARP_SLN=%CSHARP_PROJ_DIR%\ProtoProject.sln


if defined OUTPUT_CSHARP_ASSEMBLY (  
    if "%VisualStudio%"=="" (echo Missing environment variable 'VisualStudio' 
    pause
    exit
    )
)

if exist "%CSHARP_OUT%" (rmdir /Q /S "%CSHARP_OUT%")
mkdir "%CSHARP_OUT%"

pushd %cd%
echo Proto files:
cd %PROTO_DIR%

set PROTO_FILES=

FOR %%i IN (*.proto) DO (
    set PROTO_FILES=!PROTO_FILES! "%%i"
    echo %%i
)


echo gen csharp
protogen --csharp_out="%CSHARP_OUT%" %PROTO_FILES%

popd


if defined OUTPUT_CSHARP_ASSEMBLY (

    %MSBuild% %CSHARP_SLN% /property:Configuration=Release
    copy /Y %CSHARP_PROJ_DIR%\bin\Release\Proto.dll %OUTPUT_CSHARP_ASSEMBLY%
)

pause