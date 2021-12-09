--.Proto 源文件在 仓库/Proto 目录
--包名用 '.' 分隔
local pb = require "pb"

print("load pb: ", pb)

local pbData
pbData = CS.UnityEngine.Resources.Load("Lua/Proto/Proto.pb", typeof(CS.UnityEngine.TextAsset)).bytes
print('load pbData')
pb.load(pbData)

local data = {
    userName = "xxx",
    userPwd = "yyyyy"
}



local chunk, _ = pb.encode("Example.CS_Login", data)
print('encode', chunk)

local data = pb.decode("Example.CS_Login", chunk)
print('decode', data.userName, data.userPwd)