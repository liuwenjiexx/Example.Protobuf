using ProtoBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TestManual : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"Run test '{GetType().Name}'");

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

        using (var ms = new MemoryStream(data))
        {
            CS_Login login;
            login = Serializer.Deserialize<CS_Login>(ms);
            Debug.Log($"userName: {login.userName}, userPwd: {login.userPwd}");
        }

    }

    [ProtoContract]
    class CS_Login
    {
        [ProtoMember(1)]
        public string userName;

        [ProtoMember(2)]
        public string userPwd;
    }

}
