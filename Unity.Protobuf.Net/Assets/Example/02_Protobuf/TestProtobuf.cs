using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Example;
using System.IO;
using ProtoBuf;
using System;

public class TestProtobuf : MonoBehaviour
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

            Serializer.Serialize(ms, login);

            data = new byte[ms.Length];
            Buffer.BlockCopy(ms.GetBuffer(), 0, data, 0, data.Length);
        }

        DateTime dt = DateTime.Now;
        for (int i = 0; i < 10000; i++)
        {
            using (var ms = new MemoryStream(data))
            {
                CSLogin login;
              //    login = Serializer.Deserialize<CSLogin>(ms);
                login =(CSLogin) Serializer.Deserialize(typeof(CSLogin), ms);
                //Debug.Log($"userName: {login.userName}, userPwd: {login.userPwd}");
            }
        }
        Debug.Log("SSS:" + (DateTime.Now.Subtract(dt)).TotalMilliseconds);

    }

}
