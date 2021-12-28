@echo off
setlocal EnableDelayedExpansion
@rem Unity 工程目录
set UNITY_PROJ_DIR=%cd%\..
set PROJ_TOOLS_DIR=%UNITY_PROJ_DIR%\Tools
set BUILDPROTOBUF_DIR=%UNITY_PROJ_DIR%\..\Tools\BuildProtobuf
set MSBuild="%VisualStudio%\MSBuild\Current\Bin\MSBuild.exe"
@rem 协议源文件目录
set PROTO_DIR=%UNITY_PROJ_DIR%\..\Proto
set CSHARP_OUT=%cd%\ProtoProject\Proto
set CSHARP_PROJ_DIR=ProtoProject
set CSHARP_PROJ=%CSHARP_PROJ_DIR%\ProtoProject.sln


@rem 消息ID 枚举
set MSG_ID_ENUM="MessageID"
@rem 消息名格式
@rem CS_msg, SC_msg
set MSG="(^|\.)(((?<cs>CS)|(?<sc>SC))_)?(?<name>[^\.]+)$"

if "%VisualStudio%"=="" (echo Missing environment variable 'VisualStudio' 
pause
exit
)

echo BuildProtobuf Dir: %BUILDPROTOBUF_DIR%

if exist "%CSHARP_OUT%" (rmdir /Q /S "%CSHARP_OUT%")
mkdir "%CSHARP_OUT%"

pushd %cd%
cd %PROTO_DIR%

set PROTO_FILES=

FOR %%i IN (*.proto) DO (
    set PROTO_FILES=!PROTO_FILES! "%%i"
)

echo Proto files=%PROTO_FILES%
protogen --csharp_out="%CSHARP_OUT%" %PROTO_FILES%

popd

pushd %cd%
cd %BUILDPROTOBUF_DIR%
BuildProtobuf.exe -source="%PROTO_DIR%" -msg_id_enum=%MSG_ID_ENUM% -msg=%MSG% -netcsharp=%CSHARP_OUT%
popd

%MSBuild% %CSHARP_PROJ% /property:Configuration=Release
copy /Y %CSHARP_PROJ_DIR%\bin\Release\Proto.dll %UNITY_PROJ_DIR%\Assets\Plugins\Proto.dll

pause