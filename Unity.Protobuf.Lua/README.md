# Protobuf Lua

## 环境

### xLua

[xLua 仓库](https://github.com/Tencent/xLua.git) [build_xlua_with_libs 仓库](https://github.com/chexiongsheng/build_xlua_with_libs.git)

`build_xlua_with_libs` 包含 `xLua` 扩展插件，如 `protobuf` 的 `pb`，建议使用该仓库编译的 `xlua.dll`

### protoc

[protoc.exe 下载地址](https://github.com/protocolbuffers/protobuf/releases)

编译 `protobuf` 多个协议为一个字节码文件，`lua` 运行时 `pb.load` 加载该字节码文件





## **样例详解**

样例位置：`Assets\Example`

所有样例场景都可以直接运行

### 01_PB

使用 `pb` 加载 `protobuf` 协议

1. **定义 `.proto` 协议文件**
    位置 `../Proto`
    `CS_Login.proto`

   ```protobuf
   syntax = "proto3";
   package Example;
   
   message CS_Login {
       string userName = 1;
       string userPwd = 2;
   }
   ```
   
   CS: Client To Server
   
   SC: Server To Client
   
2. 编译协议字节码，运行 `Tools/BuildProtoLua.bat`

   **生成**

   ```tex
   Assets\Example\01_PB\Resources\Lua\Proto\Proto.pb.bytes
   ```
   
3. `LuaEnv` 注册 `protobuf` `pb` 模块

    ```c#
    #if (UNITY_IPHONE || UNITY_TVOS || UNITY_WEBGL || UNITY_SWITCH) && !UNITY_EDITOR
    	const string LUADLL = "__Internal";
    #else
        const string LUADLL = "xlua";
    #endif
    
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern int luaopen_pb(System.IntPtr L);
    
    [MonoPInvokeCallback(typeof(XLua.LuaDLL.lua_CSFunction))]
    public static int LoadPb(System.IntPtr L)
    {
    	return luaopen_pb(L);
    }
    ```

    注册加载 `pb` 方法

    ```c#
    luaEnv.AddBuildin("pb", LoadPb);
    ```

    如果运行报错缺少 `LoadPb` 方法，当前使用的 `xlua.dll` 库没有带 `pb` 模块，需要使用 `build_xlua_with_libs` 仓库重新编译

4. 加载协议字节码

    `require "pb"` 之前需要 `LuaEnv` 注册 `pb` 接口

    ```lua
    local pb = require "pb"
    local pbData
    pbData = CS.UnityEngine.Resources.Load("Lua/Proto/Proto.pb", typeof(CS.UnityEngine.TextAsset)).bytes
    pb.load(pbData)
    ```

5. 序列化数据

    ```lua
    local data = {
        userName = "xxx",
        userPwd = "yyyyy"
    }
    
    local chunk, _ = pb.encode("Example.CS_Login", data)
    ```

6. 反序列化数据

    ```lua
    local data = pb.decode("Example.CS_Login", chunk)
    print('decode', data.userName, data.userPwd)
    ```

### 02_PBC

使用 `pbc` 运行时编译协议

> 不推荐，仅作原理演示

1. 复制 `../Proto` 协议文件到 `Assets\Example\02_PBC\Resources\Lua\Proto`

2. 编译协议字节码

   ```lua
   local protoc = require "protoc"
   local protoFiles = { "CS_Login" }
   
   for i = 1, #protoFiles do
       local protoData = CS.UnityEngine.Resources.Load("Lua/Proto/" .. protoFiles[i], typeof(CS.UnityEngine.TextAsset)).bytes
       local pbData = protoc.new():compile(protoData)
       local success, code = pb.load(pbData)
       if success == false then
           error("pb.load fail. Lua/Proto/" .. protoFiles[i])
       end
   end
   ```

   `protoc.lua` 位置：`Assets\Example\02_PBC\Resources\Lua\protobuf`

   下载位置：`build_xlua_with_libs\build\lua-protobuf`

3. 序列化和 `01_PB` 样例 一致

### 03_MsgId

序列化增加了消息ID

1. 定义消息ID

   ```protobuf
   syntax = "proto3";
   package Example;
   
   enum MessageID {
     CS_Login = 10001;
     SC_Login = 10002;
   }
   
   message CS_Login {
     string userName = 1;
     string userPwd = 2;
   }
   ```

2. 运行 `Tools/BuildProtoLua.bat` 生成 `Proto.lua` 消息ID和消息名称的映射数据

   **生成**

   ```tex
   Assets\Example\03_MsgId\Resources\Lua\Proto\Proto.lua
   Assets\Example\01_PB\Resources\Lua\Proto\Proto.pb.bytes
   ```

   `Proto.lua` 文件内容

   ```lua
   local package = "Example"
   local p= {
   [1] = package..".CS_Login",
   [2] = package..".SC_Login"
   }
   return {
     cs = {
       id = {
   [10001] = p[1]
   },
       msg = {
   ["Login"] = p[1]
   },
     msgToId = {
   ["Login"] = 10001
   }
   },
   sc ={
       id = {
   [10002] = p[2]
   },
       msg = {
   ["Login"] = p[2]
   },
     msgToId = {
   ["Login"] = 10002
   }
   }
   }
   ```

   定制数据格式需要修改 `BuildProtobuf.exe` ，源代码位置：`BuildProtobuf`

3. 初始化，`LuaProtoBuf` 封装了 `MsgId` 的序列化

   ```lua
   Proto = require("protobuf/LuaProtoBuf")
   local proto = require("Proto/Proto")
   
   Proto:initialize({
       bytes = CS.UnityEngine.Resources.Load("Lua/Proto/Proto.pb", typeof(CS.UnityEngine.TextAsset)).bytes,
       CSCmd = proto.cs,
       SCCmd = proto.sc
   })
   ```

4. 序列化

   ```lua
   local bytes, msgId = Proto:cs("Login", { userName = "xxx", userPwd = "yyyyy" })
   ```

5. 反序列化

   ```lua
   local data = Proto:decode("cs", 10001, bytes)
   print('Proto:decode', data.userName, data.userPwd)
   ```

   正式环境中反序列化只有 `sc`

   ```lua
   local data = Proto:sc(10001, bytes)
   ```

   

