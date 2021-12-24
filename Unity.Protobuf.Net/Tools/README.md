# Protobuf-NET

[protobuf-net 仓库](https://github.com/protobuf-net/protobuf-net)



## ProtoProject 工程设置

工程位置 `ProtoProject/ProtoProject.sln`

设置工程 `Protobuf-net` 运行时

1. `VS` 创建 `.NET Framework 4` `空项目`
2. 右键工程菜单选择 `管理 NuGet 程序包` 
3. 点击 `浏览` 页签，搜索 `Protobuf-net`  
4. 选择 `protobuf-net.Core` 版本，点击 `安装` 按钮
5. 复制 `packages\protobuf-net\lib\net461` 和依赖目录下 `dll` 到 Unity 工程 `Assets/Plugins`



### 安装生成协议命令

```sh
dotnet tool install --global protobuf-net.Protogen --version 3.0.101
```

安装 `protogen` 命令

`dotnet` 命令位置 `C:\Program Files\dotnet\dotnet.exe`

```sh
protogen -help
```

查看命令使用方法



## 命令

### 生成程序集

运行 `Build_Proto_CSharp.bat` 命令

**参数**

- MSBuild

  设置 `VisualStudio` 系统环境变量

- PROTO_DST

  协议 `.dll` 程序集输出位置

### 只生成代码

运行 `Build_Proto_CSharp_Code.bat` 命令

**参数**

- PROTO_SRC

`.proto` 协议源文件位置

- CSHARP_OUT

  `.cs` 文件输出位置

