using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Xsl;

namespace BuildProtobuf
{
    public partial class Program
    {

        static void BuildProtoNetCSharp(IEnumerable<ProtoMessageInfo> messages, string outputPath)
        {
            StringBuilder builder = new StringBuilder();
            int index = 0;
            outputPath = Path.GetFullPath(outputPath);

            int count = messages.Count();
            var mapIndex = new Dictionary<ProtoMessageInfo, int>();

            if (count > 0)
            {
                string packageName;

                var first = messages.First();
                packageName = first.PackageInfo.PackageName;
                //    builder.AppendLine("-- ***该文件为自动生成的***");

                //    builder.AppendLine("public class MsgIDs")
                //        .AppendLine("{")
                //        .AppendLine();
                //    {
                //        builder.AppendLine("public class CS")
                //            .AppendLine("{");
                //        {
                //            builder.AppendLine("public readonly static Dictionary<int, Type> IdToType;")
                //                .AppendLine("public readonly static Dictionary<Type, int> TypeToId;")
                //                .AppendLine("static CS()")
                //                .AppendLine("{");
                //            {
                //                builder.AppendLine("IdToType = new Dictionary<int, Type>();")
                //                    .AppendLine("TypeToId = new Dictionary<Type, int>(); ");
                //            }
                //            builder.AppendLine("}");

                //            builder.AppendLine("public static void Register(int id, Type type)")
                //                .AppendLine("{")
                //                .AppendLine("IdToType[id] = type; ")
                //                .AppendLine("TypeToId[type] = id; ")
                //                .AppendLine("}");


                //            builder.AppendLine("")
                //            public static bool TryGetType(int id, out Type type)
                //            {
                //                return IdToType.TryGetValue(id, out type);
                //            }

                //            public static bool TryGetId(Type type, out int id)
                //            {
                //                return TypeToId.TryGetValue(type, out id);
                //            }
                //            ")
                //        }
                //        builder.AppendLine("}");
                //    }
                //    builder.AppendLine("}");

                //    builder.AppendLine($"local package = \"{packageName}\"");
                //    builder.AppendLine("local p= {");

                //    index = 0;
                //    foreach (var msg in messages)
                //    {
                //        builder.Append($"[{(index + 1)}] = ");
                //        if (string.IsNullOrEmpty(packageName))
                //        {
                //            builder.Append($"\"{msg.Name}\"");
                //        }
                //        else
                //        {
                //            builder.Append($"package..\".{msg.Name}\"");
                //        }
                //        if (index < count - 1)
                //            builder.Append(",");
                //        builder.AppendLine();
                //        mapIndex[msg] = index;
                //        index++;
                //    }
                //    builder.AppendLine("}");
                //}

                //builder.AppendLine("return {");
                //builder.AppendLine("  cs = {");
                //Build(messages.Where(o => o.IsClientToServer), builder, mapIndex);
                //builder.AppendLine("},")
                //    .AppendLine("sc ={");
                //Build(messages.Where(o => !o.IsClientToServer), builder, mapIndex);
                //builder.AppendLine("}");


                XsltArgumentList args = new XsltArgumentList();

                foreach (var msg in messages)
                {
                    msg.TypeName = msg.Name.Replace("_", "");
                }
                args.AddParam("ClassName", "", CSharpMsgIdsClassName);
                var aaa= typeof(Program).Assembly.GetManifestResourceNames();
                var manifestStream = typeof(Program).Assembly.GetManifestResourceStream("BuildProtobuf.xslt.netcsharp.MsgIds.xslt");
                using (StreamReader sr = new StreamReader(manifestStream))
                {
                    TransformXsl(messages, sr.ReadToEnd(), args, outputPath);
                }
                //TransformXsl(messages, Resource1.MsgIds, args, outputPath);
                //Encoding encoding = new UTF8Encoding(false);
                //File.WriteAllText(outputPath, builder.ToString(), encoding);


                //builder.AppendLine("}");


                Console.WriteLine($"done");
                Console.WriteLine($"{outputPath}");
            }

        }
    }
}
