@rem echo off
setlocal EnableDelayedExpansion

set PROTO_SRC=.\Proto
set CSHARP_OUT=%cd%\ProtoProject\Proto

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

pause