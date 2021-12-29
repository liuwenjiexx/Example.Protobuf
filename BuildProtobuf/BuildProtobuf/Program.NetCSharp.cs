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
            StringBuilder cmdText = new StringBuilder();

            outputPath = Path.GetFullPath(outputPath);

            cmdText.Append($"--csharp_out=\"{outputPath}\" ");

            foreach (var file in ProtoFiles)
            {
                cmdText.Append(" \"").Append(file).Append("\"");
            }

            RunCmd(FullSourceDir, "protogen", cmdText.ToString());

            string msgIdFilePath = Path.Combine(outputPath, CSharpMsgIdsClassName + ".cs");

            int count = messages.Count();

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

                var manifestStream = typeof(Program).Assembly.GetManifestResourceStream("BuildProtobuf.xslt.netcsharp.MsgId.xslt");
                using (StreamReader sr = new StreamReader(manifestStream))
                {
                    TransformXsl(messages, sr.ReadToEnd(), args, msgIdFilePath);
                }

                Console.WriteLine($"done");
                Console.WriteLine($"{msgIdFilePath}");
            }

        }
    }
}
