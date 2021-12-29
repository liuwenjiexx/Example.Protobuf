using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Xsl;

namespace BuildProtobuf
{
    public partial class Program
    {

        static void BuildProtoLua(IEnumerable<ProtoMessageInfo> messages, string outputPath)
        {
            StringBuilder sb = new StringBuilder();

            outputPath = Path.GetFullPath(outputPath);

            XsltArgumentList args = new XsltArgumentList();

            var manifestStream = typeof(Program).Assembly.GetManifestResourceStream("BuildProtobuf.xslt.lua.MsgId.xslt");
            using (StreamReader sr = new StreamReader(manifestStream))
            {
                TransformXsl(messages, sr.ReadToEnd(), args, outputPath);
            }

            Console.WriteLine($"done");
            Console.WriteLine($"{outputPath}");
        }

    }
}
