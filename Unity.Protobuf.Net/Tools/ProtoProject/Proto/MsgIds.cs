//***该文件为自动生成的***
using System;
using System.Collections.Generic;

namespace Example
{
    public class MsgIds
    {
        public class CS
        {
            public readonly static Dictionary<int, Type> IdToType;
            public readonly static Dictionary<Type, int> TypeToId;

            static CS()
            {
                IdToType = new Dictionary<int, Type>();
                TypeToId = new Dictionary<Type, int>();
                
                Register(10001, typeof(CSLogin));
           }
		   
		   
            public static void Register(int id, Type type)
            {
               IdToType[id] = type;
               TypeToId[type] = id;
            }

            public static bool TryGetType(int id, out Type type)
            {
                return IdToType.TryGetValue(id, out type);
            }

            public static bool TryGetId(Type type, out int id)
            {
                return TypeToId.TryGetValue(type, out id);
            }
	
        }

        public class SC
        {
            public readonly static Dictionary<int, Type> IdToType;
            public readonly static Dictionary<Type, int> TypeToId;

            static SC()
            {
                IdToType = new Dictionary<int, Type>();
                TypeToId = new Dictionary<Type, int>();

                Register(10002, typeof(SCLogin));
            }
		
		
            public static void Register(int id, Type type)
            {
               IdToType[id] = type;
               TypeToId[type] = id;
            }

            public static bool TryGetType(int id, out Type type)
            {
                return IdToType.TryGetValue(id, out type);
            }

            public static bool TryGetId(Type type, out int id)
            {
                return TypeToId.TryGetValue(type, out id);
            }
	
        }
    }
}
	