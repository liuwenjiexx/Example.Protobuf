Proto = require("protobuf/LuaProtoBuf")

print('Proto:initialize')
local msgIds = require("Proto/MsgIds")

Proto:initialize({
    bytes = CS.UnityEngine.Resources.Load("Lua/Proto/Proto.pb", typeof(CS.UnityEngine.TextAsset)).bytes,
    CSCmd = msgIds.cs,
    SCCmd = msgIds.sc
})


local bytes, msgId = Proto:cs("Login", { userName = "xxx", userPwd = "yyyyy" })
print('Proto:encode', msgId, bytes)

local data = Proto:decode("cs", "Login", bytes)
print('Proto:decode', data.userName, data.userPwd)