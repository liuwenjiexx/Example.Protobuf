# Tools

## BuildProtobuf

`../BuildProtobuf/BuildProtobuf.sln` 生成

[文档说明](BuildProtobuf/README.md)

## protoc

google protobuf 工具

[protoc.exe 下载](https://github.com/protocolbuffers/protobuf/releases)

- 生成 `lua` 运行时 `pb.load` 加载该字节码文件

  ```bash
  protoc -o <filename.pb> <filename.proto> [<filename2.proto> ...]
  ```

- 生成 `C#` 代码文件

  ```bash
  protoc -I=<SRC_DIR> --csharp_out=<DST_DIR> <filename.proto>
  ```

  

## protogen

生成 [protobuf-net](https://www.nuget.org/packages/protobuf-net.Protogen) 代码工具

### 安装

```sh
dotnet tool install --global protobuf-net.Protogen --version 3.0.101
```

`dotnet` 命令位置 `C:\Program Files\dotnet\dotnet.exe`

### 使用

- 查看命令使用方法

```sh
protogen -help
```

- 生成 `C#` 代码文件

```bash
protogen --csharp_out=<CSHARP_OUT> <PROTO_FILES>
```

