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
             
                XsltArgumentList args = new XsltArgumentList();

                foreach (var msg in messages)
                {
                    msg.TypeName = msg.Name.Replace("_", "");
                }
                args.AddParam("ClassName", "", CSharpMsgIdsClassName);

                var manifestStream = typeof(Program).Assembly.GetManifestResourceStream("BuildProtobuf.xslt.netcsharp.MsgIds.xslt");
                using (StreamReader sr = new StreamReader(manifestStream))
                {
                    TransformXsl(messages, sr.ReadToEnd(), args, outputPath);
                }
    
                Console.WriteLine($"done");
                Console.WriteLine($"{outputPath}");
            }

        }
    }
}
