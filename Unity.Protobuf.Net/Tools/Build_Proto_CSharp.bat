@rem echo off
setlocal EnableDelayedExpansion

set MSBuild="%VisualStudio%\MSBuild\Current\Bin\MSBuild.exe"
set PROTO_SRC=..\..\Proto
set CSHARP_OUT=%cd%\ProtoProject\Proto
set CSHARP_PROJ_DIR=ProtoProject
set CSHARP_PROJ=%CSHARP_PROJ_DIR%\ProtoProject.csproj

if exist "%CSHARP_OUT%" (rmdir /Q /S "%CSHARP_OUT%")
mkdir "%CSHARP_OUT%"

pushd %cd%
cd %PROTO_SRC%

set PROTO_FILES=

FOR %%i IN (*.proto) DO (
    set PROTO_FILES=!PROTO_FILES! "%%i"
)

echo Proto files=%PROTO_FILES%
protogen --csharp_out="%CSHARP_OUT%" %PROTO_FILES%

popd

%MSBuild% %CSHARP_PROJ% /property:Configuration=Release
copy /Y %CSHARP_PROJ_DIR%\bin\Release\Proto.dll ..\Assets\Plugins\Proto.dll
pause