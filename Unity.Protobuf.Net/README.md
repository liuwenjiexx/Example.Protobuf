# Protobuf-net

[Protobuf-net 仓库](https://github.com/protobuf-net/protobuf-net) 

## 环境

### 下载 Protobuf-net 运行时

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





## **样例工程**

`Unity.Protobuf.Net`

### 手动定义协议

样例位置 `Assets/Example/01_Manual`

**定义协议**

```c#
[ProtoContract]
class CS_Login
{
    [ProtoMember(1)]
    public string userName;

    [ProtoMember(2)]
    public string userPwd;
}
```
**序列化**

```c#
byte[] data;
using (var ms = new MemoryStream())
{
    CS_Login login = new CS_Login();
    login.userName = "name";
    login.userPwd = "password";

    Serializer.Serialize(ms, login);

    data = new byte[ms.Length];
    Buffer.BlockCopy(ms.GetBuffer(), 0, data, 0, data.Length);
}
```

**反序列化**

```c#
using (var ms = new MemoryStream(data))
{
    CS_Login login;
    login = Serializer.Deserialize<CS_Login>(ms);
    Debug.Log($"userName: {login.userName}, userPwd: {login.userPwd}");
}
```

### 自动生成协议

 样例位置 `Assets/Example/02_Protobuf`

1. **定义 `.proto` 协议文件**
    位置 `../Proto`
    `CS_Login.proto`

   ```protobuf
   syntax = "proto3";
   package Example;
   message CS_Login {
        string userName=1;
        string userPwd=2;
   }
   ```

2. 生成 `Assets/Plugins/Proto.dll`，运行 `Tools/Build_Proto_CSharp.bat` 命令

   命令生成协议代码，位置 `Tools/ProtoProject/Proto`

   `CS_Login.cs` 协议代码，下划线自动被去掉

   ```c#
   [ProtoContract]
   class CSLogin
   {
       [ProtoMember(1)]
       public string userName;
   
       [ProtoMember(2)]
       public string userPwd;
   }
   ```

3. 运行 `02_Protobuf` 测试场景

