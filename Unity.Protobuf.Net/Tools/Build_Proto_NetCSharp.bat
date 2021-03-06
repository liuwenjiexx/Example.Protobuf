@echo off
setlocal EnableDelayedExpansion
@rem Unity 工程目录
set UNITY_PROJ_DIR=%cd%\..
set PROJ_TOOLS_DIR=%UNITY_PROJ_DIR%\Tools
set BUILDPROTOBUF_DIR=%UNITY_PROJ_DIR%\..\Tools\BuildProtobuf
set MSBuild="%VisualStudio%\MSBuild\Current\Bin\MSBuild.exe"
@rem 协议源文件目录
set PROTO_DIR=%UNITY_PROJ_DIR%\..\Proto
set CSHARP_PROJ_DIR=%cd%\ProtoProject
set CSHARP_OUT=%CSHARP_PROJ_DIR%\Proto
set CSHARP_SLN=%CSHARP_PROJ_DIR%\ProtoProject.sln



@rem 消息ID 枚举
set MSG_ID_ENUM="MessageID"

if defined OUTPUT_CSHARP_ASSEMBLY (
    if "%VisualStudio%"=="" (echo Missing environment variable 'VisualStudio' 
    pause
    exit
    )
)

echo BuildProtobuf Dir: %BUILDPROTOBUF_DIR%

if exist "%CSHARP_OUT%" (rmdir /Q /S "%CSHARP_OUT%")
mkdir "%CSHARP_OUT%"


pushd %cd%
cd %BUILDPROTOBUF_DIR%
BuildProtobuf.exe -source="%PROTO_DIR%" -msg_id_enum=%MSG_ID_ENUM% -msg=%MSG% -netcsharp=%CSHARP_OUT%
popd

if defined OUTPUT_CSHARP_ASSEMBLY (
    
    %MSBuild% %CSHARP_SLN% /property:Configuration=Release
    copy /Y %CSHARP_PROJ_DIR%\bin\Release\Proto.dll %OUTPUT_CSHARP_ASSEMBLY%
)

pause