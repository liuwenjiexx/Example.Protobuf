Proto = require("protobuf/LuaProtoBuf")

print('Proto:initialize')
local proto = require("Proto/Proto")

Proto:initialize({
    bytes = CS.UnityEngine.Resources.Load("Lua/Proto/Proto.pb", typeof(CS.UnityEngine.TextAsset)).bytes,
    CSCmd = proto.cs,
    SCCmd = proto.sc
})


local bytes, msgId = Proto:cs("Login", { userName = "xxx", userPwd = "yyyyy" })
print('Proto:encode', msgId, bytes)

local data = Proto:decode("cs", 10001, bytes)
print('Proto:decode', data.userName, data.userPwd)