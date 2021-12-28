@echo off
@rem Unity 工程目录
set UNITY_PROJ_DIR=%cd%\..
set PROJ_TOOLS_DIR=%UNITY_PROJ_DIR%\Tools
set BUILDPROTOBUF_DIR=%UNITY_PROJ_DIR%\..\Tools\BuildProtobuf
@rem 协议源文件目录
set PROTO_DIR=%UNITY_PROJ_DIR%\..\Proto
set LUA_DIR=%UNITY_PROJ_DIR%\Assets\Example\03_MsgId\Resources\Lua\Proto
@rem 输出 pb 文件位置
set PB_FILE=%LUA_DIR%\Proto.pb.bytes
@rem 输出 Proto.lua 位置
set LUA_FILE=%LUA_DIR%\Proto.lua
@rem 消息ID 枚举
set MSG_ID_ENUM="MessageID"


@rem 消息名格式
@rem CS_msg, SC_msg
set MSG="(^|\.)(((?<cs>CS)|(?<sc>SC))_)?(?<name>[^\.]+)$"
@rem 无命名空间
@rem set MSG="([^\.]+\.)?(?<name>.*)$"
@rem msgRequest, msgResponse
@rem set MSG="(^|\.)(?<name>[^\.]+?)((?<cs>Request)|(?<sc>Response))?$"

echo BuildProtobuf Dir: %BUILDPROTOBUF_DIR%

cd %BUILDPROTOBUF_DIR%

BuildProtobuf.exe -protoc="%UNITY_PROJ_DIR%\..\Tools\protoc.exe" -source="%PROTO_DIR%" -pb="%PB_FILE%" -lua="%LUA_FILE%" -msg_id_enum=%MSG_ID_ENUM% -msg=%MSG%

pause