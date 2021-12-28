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

