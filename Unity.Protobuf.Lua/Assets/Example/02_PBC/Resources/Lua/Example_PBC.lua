--.Proto 源文件在 仓库/Proto 目录
--包名用 '.' 分隔
local pb = require "pb"
local protoc = require 'protobuf/protoc'



local protoFiles = { "CS_Login" }

for i = 1, #protoFiles do
    local protoData = CS.UnityEngine.Resources.Load("Lua/Proto/" .. protoFiles[i], typeof(CS.UnityEngine.TextAsset)).bytes
    local pbData = protoc.new():compile(protoData)
    print("protoc compile ".."Lua/Proto/" .. protoFiles[i], pbData)
    local success, code = pb.load(pbData)
    if success == false then
        error("pb.load fail. Lua/Proto/" .. protoFiles[i])
    end
end


local data = {
    userName = "xxx",
    userPwd = "yyyyy"
}
local chunk2, _ = pb.encode("Example.CS_Login", data)
print('encode', chunk2)
local data2 = pb.decode("Example.CS_Login", chunk2)
print('decode', data2.userName, data2.userPwd)