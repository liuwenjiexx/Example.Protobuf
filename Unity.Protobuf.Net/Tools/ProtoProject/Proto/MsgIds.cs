//***该文件为自动生成的***
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
	