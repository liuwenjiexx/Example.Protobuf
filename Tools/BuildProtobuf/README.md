# BuildProtobuf

## 参数

- -source

  协议源文件，文件扩展名 `.proto`

- -extension

  默认 `.proto`，定制协议源文件扩展名

- -protoc

  `protoc.exe` 命令路径，用于生成 `pb` 文件和 `google` 代码文件

- -pb

  `pb` 文件，`Proto.pb.bytes` 路径

- -lua

  `Proto.lua` 文件路径

- -msg_id_enum

  默认 `MessageID`，正则表达式格式，消息ID枚举名称

- -msg

  消息名称格式，正则表达式格式，正则返回值：`name` 为程序使用的名称，`cs` 为客户端到服务端消息，`sc` 为服务端到客户端消息

  **样例**

  `CS_XXX`，`SC_XXX`，使用名称为 `XXX`

  ```tex
  (^|\.)(((?<cs>CS)|(?<sc>SC))_)?(?<name>[^\.]+)$
  ```

  `XXXRequest`, `XXXResponse`，使用名称为 `XXX`

  ```tex
  (^|\.)(?<name>[^\.]+?)((?<cs>Request)|(?<sc>Response))?$
  ```

  `package.XXX`，使用名称为 `XXX`

  ```tex
  ([^\.]+\.)?(?<name>.*)$
  ```

- -netcsharp

  生成 `protobuf-net` `C#` 代码



## 样例

### 协议源文件

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

message SC_Login {
  int32 errorCode = 1;
  string accessToken = 2;
}
```



### Proto.lua

`-lua` 参数生成

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

### MsgIds.cs

`-netcsharp` 参数生成

```c#
using System;
using System.Collections.Generic;

namespace Example
{
    public class MsgIds
    {
        public Dictionary<int, Type> IdToType { get; private set; } = new Dictionary<int, Type>();
        public Dictionary<Type, int> TypeToId { get; private set; } = new Dictionary<Type, int>();
        public Dictionary<string, int> NameToId { get; private set; } = new Dictionary<string, int>();
        public Dictionary<int, string> IdToName { get; private set; } = new Dictionary<int, string>();

        public static MsgIds CS { get; private set; }
        public static MsgIds SC { get; private set; }

        static MsgIds()
        {
            CS = new MsgIds();            
            CS.Register(10001, typeof(CSLogin), "Login");
            SC = new MsgIds();            
            SC.Register(10002, typeof(SCLogin), "Login");
        }

        public void Register(int id, Type type, string name)
        {
            IdToType[id] = type;
            TypeToId[type] = id;
            NameToId[name] = id;
            IdToName[id] = name;
        }
        
        public bool TryGetId(string name, out int id)
        {
            return NameToId.TryGetValue(name, out id);
        }
        
        public bool TryGetId(Type type, out int id)
        {
            return TypeToId.TryGetValue(type, out id);
        }
        public bool TryGetName(int id, out string name)
        {
            return IdToName.TryGetValue(id, out name);
        }
        public bool TryGetType(int id, out Type type)
        {
            return IdToType.TryGetValue(id, out type);
        }
    }
}
```

