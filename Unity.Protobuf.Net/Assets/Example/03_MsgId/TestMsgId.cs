using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Example;
using System.IO;
using ProtoBuf;
using System;
using System.Net;

public class TestMsgId : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"Run test '{GetType().Name}'");

        Debug.Log("run Tools/Build_Proto_CSharp.bat generate [Proto.dll]");

        byte[] data;

        using (var ms = new MemoryStream())
        {
            CSLogin login = new CSLogin();
            login.userName = "name";
            login.userPwd = "password";

            //获取消息Id
            MsgIds.CS.TryGetId(typeof(CSLogin), out var id);
            //写入消息Id
            WriteInt32(ms, id);
            Debug.Log($"Write MsgId: {id}");

            Serializer.Serialize(ms, (object)login);

            data = new byte[ms.Length];
            Buffer.BlockCopy(ms.GetBuffer(), 0, data, 0, data.Length);
        }


        using (var ms = new MemoryStream(data))
        {
            //读取消息Id
            int id = ReadInt32(ms);
            Debug.Log($"Read MsgId: {id}");
            //获取消息类型
            MsgIds.CS.TryGetType(id, out var type);

            object obj = Serializer.Deserialize(type, ms);

            CSLogin login = (CSLogin)obj;
            Debug.Log($"userName: {login.userName}, userPwd: {login.userPwd}");
        }


    }

    byte[] bytes4 = new byte[4];
    public int ReadInt32(Stream stream)
    {
        stream.Read(bytes4, 0, 4);
        int value = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(bytes4, 0));
        return value;
    }

    public void WriteInt32(Stream stream, int value)
    {
        byte[] bytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(value));
        stream.Write(bytes, 0, 4);
    }


}

//public class MsgIDs
//{

//    public class CS
//    {

//        public readonly static Dictionary<int, Type> IdToType;
//        public readonly static Dictionary<Type, int> TypeToId;

//        static CS()
//        {
//            IdToType = new Dictionary<int, Type>();
//            TypeToId = new Dictionary<Type, int>();

//            Register(1001, typeof(CSLogin));
//        }

//        public static void Register(int id, Type type)
//        {
//            IdToType[id] = type;
//            TypeToId[type] = id;
//        }



//        public static bool TryGetType(int id, out Type type)
//        {
//            return IdToType.TryGetValue(id, out type);
//        }

//        public static bool TryGetId(Type type, out int id)
//        {
//            return TypeToId.TryGetValue(type, out id);
//        }

//    }


//    public class SC
//    {

//    }





//}