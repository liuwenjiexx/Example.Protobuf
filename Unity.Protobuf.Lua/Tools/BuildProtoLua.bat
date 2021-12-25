@echo off
@rem Unity 工程目录
set UNITY_PROJ=%cd%\..\
@rem 协议源文件目录
set PROTO_DIR=%UNITY_PROJ%\..\Proto
set LUA_DIR=%UNITY_PROJ%\Assets\Example\03_MsgId\Resources\Lua\Proto
@rem 输出 pb 文件位置
set PB_FILE=%LUA_DIR%\Proto.pb.bytes
@rem 输出 Proto.lua 位置
set LUA_FILE=%LUA_DIR%\Proto.lua
@rem 消息ID 枚举
set MSG_ID_ENUM="MessageID"
@rem 协议文件扩展名
set EXTENSION=.proto

@rem 消息名格式
@rem CS_msg, SC_msg
set MSG="(^|\.)(((?<cs>CS)|(?<sc>SC))_)?(?<name>[^\.]+)$"
@rem 无命名空间
@rem set MSG="([^\.]+\.)?(?<name>.*)$"
@rem msgRequest, msgResponse
@rem set MSG="(^|\.)(?<name>[^\.]+?)((?<cs>Request)|(?<sc>Response))?$"

cd BuildProtobuf

BuildProtobuf.exe -protoc="..\protoc.exe" -source="%PROTO_DIR%" -pb="%PB_FILE%" -lua="%LUA_FILE%" -msg_id_enum=%MSG_ID_ENUM% -msg=%MSG% -extension="%EXTENSION%"

pause