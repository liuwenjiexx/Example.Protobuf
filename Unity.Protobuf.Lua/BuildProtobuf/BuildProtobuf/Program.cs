using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace BuildProtobuf
{
    public class Program
    {
        public static string SourceDir = "./";
        public static string Extension = ".proto";
        public static string OutputLuaDir;
        /// <summary>
        ///  protoc.exe
        /// </summary>
        public static string ProtocPath;
        public static string OutputPbFile;
        public static string DataFile = "proto.txt";
        //public static string ClientToServerDir = "CS";
        //public static string ServerToClientDir = "SC";
        public static string LuaFile = "Proto.lua";
        public static bool ResetID = false;
        public static int AutoID = 10000;
        //public static string MsgCSTypePattern = "^CS_.+";
        //public static string MsgSCTypePattern = "^SC_.+";

        /// <summary>
        /// 正则表达式
        /// 参数：cs: 客户端到服务端消息，sc：服务端到客户端消息，name：使用的消息名称
        /// 无命名空间："([^\\.]+\\.)?(?<name>.*)$"
        /// CS_msg, SC_msg: "(^|\\.)(((?<cs>CS)|(?<sc>SC))_)?(?<name>[^\\.]+)$"
        /// msgRequest, msgResponse: "(^|\\.)(?<name>[^\\.]+?)((?<cs>Request)|(?<sc>Response))?$"
        /// </summary>
        public static string MsgPattern = "(^|\\.)(((?<cs>CS)|(?<sc>SC))_)?(?<name>[^\\.]+)$";


        static void Main(string[] args)
        {
            try
            {
                var dic = ParseArgs(args);

                TryGetArg(dic, "-source", ref SourceDir);
                TryGetArg(dic, "-extension", ref Extension);
                TryGetArg(dic, "-protoc", ref ProtocPath);
                //TryGetArg(dic, "-SC", ref ServerToClientDir);
                //TryGetArg(dic, "-CS", ref ClientToServerDir);
                TryGetArg(dic, "-msg", ref MsgPattern);
                // TryGetArg(dic, "-Lua", ref LuaFile);

                string str = null;
                if (TryGetArg(dic, "-AutoID", ref str))
                {
                    AutoID = int.Parse(str);
                }
                if (TryGetArg(dic, "-ResetID", ref str))
                {
                    ResetID = bool.Parse(str);
                }
                string fullSrcDir = Path.GetFullPath(SourceDir);

                if (!Directory.Exists(fullSrcDir))
                {
                    throw new System.Exception("Directory not exists. dir: " + fullSrcDir);
                }

                var messages = LoadMessages(fullSrcDir);

                foreach (var file in messages.Select(o => o.Path).Distinct())
                {
                    Console.WriteLine(file);
                }

                if (TryGetArg(dic, "-pb", ref OutputPbFile))
                {
                    BuildProtoPB(messages.Select(o => o.Path).Distinct(), fullSrcDir, OutputPbFile);
                }

                if (TryGetArg(dic, "-lua", ref LuaFile))
                {
                    BuildProtoLua(messages, LuaFile);
                    //BuildProtoLua(messages, Path.Combine(OutputLuaDir, CSLuaFile));

                    //BuildProtoLua(messages.Where(o => !o.IsClientToServer), Path.Combine(OutputLuaDir, SCLuaFile));

                }

                StringBuilder sb = new StringBuilder();
                foreach (var msg in messages)
                {
                    sb.Append(msg.FullName)
                        .Append("=")
                        .Append(msg.id)
                        .AppendLine();
                }
                sb.AppendLine();
                File.WriteAllText(DataFile, sb.ToString(), Encoding.UTF8);
                Console.WriteLine("Build success");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.ReadKey();
        }

        static bool TryGetArg(Dictionary<string, string> args, string key, ref string value)
        {
            if (args.ContainsKey(key))
            {
                value = args[key];
                return true;
            }
            return false;
        }

        static Dictionary<string, string> ParseArgs(string[] args)
        {
            Dictionary<string, string> values = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            foreach (var arg in args)
            {
                var parts = arg.Split('=');
                string key, value = null;
                key = parts[0];
                if (parts.Length > 1)
                {
                    value = parts[1];
                }
                values[key] = value;
            }
            return values;
        }
        static List<ProtoMessageInfo> LoadMessages(string dir)
        {
            string filter = "*" + Extension;

            Dictionary<string, int> oldIds = new Dictionary<string, int>();

            if (!ResetID && File.Exists(DataFile))
            {
                foreach (string line in File.ReadAllLines(DataFile))
                {
                    if (string.IsNullOrEmpty(line))
                        break;
                    var parts = line.Split('=');
                    if (parts.Length == 1)
                        break;
                    string msgName = parts[0];
                    int msgId = 0;
                    int.TryParse(parts[1], out msgId);
                    oldIds[msgName] = msgId;
                }

            }

            List<ProtoMessageInfo> messages = new List<ProtoMessageInfo>();
            foreach (var msg in FindProtoFiles(dir, filter))
            {
                //msg.IsClientToServer = true;
                messages.Add(msg);
            }

            //foreach (var msg in FindProtoFiles(Path.Combine(dir, ServerToClientDir), filter))
            //{
            //    msg.IsClientToServer = false;
            //    messages.Add(msg);
            //}

            if (oldIds.Count > 0)
            {
                foreach (var msg in messages)
                {
                    if (!oldIds.ContainsKey(msg.FullName))
                    {
                        oldIds.Remove(msg.FullName);
                    }
                }
            }

            messages = messages.OrderBy(o => o.FullName).ToList();

            int index = 0;
            int nextId = AutoID;
            foreach (var msg in messages)
            {
                msg.index = index;

                if (oldIds.TryGetValue(msg.FullName, out var id))
                {
                    msg.id = id;
                }
                else
                {

                    while (true)
                    {
                        nextId++;
                        if (!oldIds.ContainsValue(nextId))
                            break;
                    }
                    msg.id = nextId;
                }
                index++;
            }


            return messages;
        }
        static IEnumerable<ProtoMessageInfo> FindProtoFiles(string dir, string filter)
        {
            string fullDir = Path.GetFullPath(dir);

            if (Directory.Exists(fullDir))
            {
                foreach (var file in Directory.GetFiles(fullDir, filter, SearchOption.AllDirectories))
                {
                    foreach (var msg in ProtoMessageInfo.Parse(File.ReadAllText(file, Encoding.UTF8)))
                    {
                        msg.Path = file;
                        yield return msg;
                    }
                }
            }
        }

        public static string FindPath(string file)
        {
            string dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (File.Exists(Path.Combine(dir, file)))
                return Path.Combine(dir, file);
            return null;
        }

        public static void BuildProtoPB(IEnumerable<string> protoFiles, string rootDir, string outputPath)
        {
            Console.WriteLine("Build pb start");
            string pbcPath = null;
            if (!string.IsNullOrEmpty(ProtocPath))
            {
                if (File.Exists(ProtocPath))
                    pbcPath = ProtocPath;
            }
            else
            {
                pbcPath = FindPath("protoc.exe");
            }

            if (string.IsNullOrEmpty(pbcPath))
                throw new Exception("Protoc file not exists.");
            Console.WriteLine(pbcPath);

            outputPath = Path.GetFullPath(outputPath);

            StringBuilder cmdText = new StringBuilder();
            cmdText.Append("-o \"")
                .Append(outputPath)
                .Append("\"");

            int index;
            if (rootDir.EndsWith("\\") || rootDir.EndsWith("/"))
                index = rootDir.Length;
            else
                index = rootDir.Length + 1;
            foreach (var file in protoFiles)
            {
                cmdText.Append(" \"")
                   .Append(file.Substring(index))
                    .Append("\"");
            }
            using (var proc = new System.Diagnostics.Process())
            {
                proc.StartInfo = new System.Diagnostics.ProcessStartInfo()
                {
                    WorkingDirectory = rootDir,
                    FileName = pbcPath,
                    Arguments = cmdText.ToString(),
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                };
                StringBuilder error = new StringBuilder();
                proc.OutputDataReceived += (o, e) =>
                {
                    error.AppendLine(e.Data);
                };
                proc.ErrorDataReceived += (o, e) =>
                {
                    error.AppendLine(e.Data);
                };

                proc.Start();
                proc.WaitForExit();
                if (error.Length > 0)
                {
                    throw new Exception(error.ToString());
                }
            }

            Console.WriteLine("done");
            Console.WriteLine($"{outputPath}");
        }


        static void BuildProtoLua(IEnumerable<ProtoMessageInfo> messages, string outputPath)
        {
            StringBuilder sb = new StringBuilder();
            int index = 0;
            outputPath = Path.GetFullPath(outputPath);

            int count = messages.Count();
            var mapIndex = new Dictionary<ProtoMessageInfo, int>();

            if (count > 0)
            {
                string packageName;

                var first = messages.First();
                packageName = first.PackageName;
                sb.AppendLine("-- ***该文件为自动生成的***");

                sb.AppendLine($"local package = \"{packageName}\"");
                sb.AppendLine("local p= {");

                index = 0;
                foreach (var msg in messages)
                {
                    sb.Append($"[{(index + 1)}] = ");
                    if (string.IsNullOrEmpty(packageName))
                    {
                        sb.Append($"\"{msg.Name}\"");
                    }
                    else
                    {
                        sb.Append($"package..\".{msg.Name}\"");
                    }
                    if (index < count - 1)
                        sb.Append(",");
                    sb.AppendLine();
                    mapIndex[msg] = index;
                    index++;
                }
                sb.AppendLine("}");
            }

            sb.AppendLine("return {");
            sb.AppendLine("  cs = {");
            Build(messages.Where(o => o.IsClientToServer), sb, mapIndex);
            sb.AppendLine("},")
                .AppendLine("sc ={");
            Build(messages.Where(o => !o.IsClientToServer), sb, mapIndex);
            sb.AppendLine("}");



            sb.AppendLine("}");
            if (!Directory.Exists(Path.GetDirectoryName(outputPath)))
                Directory.CreateDirectory(Path.GetDirectoryName(outputPath));
            Encoding encoding = new UTF8Encoding(false);
            File.WriteAllText(outputPath, sb.ToString(), encoding);

            Console.WriteLine($"done");
            Console.WriteLine($"{outputPath}");
        }


        static void Build(IEnumerable<ProtoMessageInfo> items, StringBuilder sb, Dictionary<ProtoMessageInfo, int> mapIndex)
        {
            int count2 = items.Count();
            int index;
            sb.AppendLine("    id = {");
            index = 0;
            foreach (var msg in items)
            {
                sb.Append($"[{msg.id}] = p[{(mapIndex[msg] + 1)}]");
                if (index < count2 - 1)
                    sb.Append(",");
                sb.AppendLine();
                index++;
            }
            sb.AppendLine("},");

            sb.AppendLine("    msg = {");
            index = 0;
            foreach (var msg in items)
            {
                sb.Append($"[\"{msg.UsedName}\"] = p[{(mapIndex[msg] + 1)}]");
                if (index < count2 - 1)
                    sb.Append(",");
                sb.AppendLine();
                index++;
            }
            sb.AppendLine("},");

            sb.AppendLine("  msgToId = {");
            index = 0;
            foreach (var msg in items)
            {
                sb.Append($"[\"{msg.UsedName}\"] = {msg.id}");
                if (index < count2 - 1)
                    sb.Append(",");
                sb.AppendLine();
                index++;
            }
            sb.AppendLine("}");
        }

    }


    class ProtoMessageInfo
    {
        public string PackageName;
        public string Name;
        public string FullName;
        public string UsedName;
        public int Version;
        public int id;
        public int index;
        public bool IsClientToServer;
        public string Path;

        static Regex regexVersion = new Regex("syntax\\s*=\\s*\"proto(?<Version>[^\"]+)");
        static Regex regexPackage = new Regex("package\\s*(?<Name>[^\\s;]+)");
        static Regex regexMessage = new Regex("^\\s*message\\s+(?<Name>[^\\s\\{]+)", RegexOptions.Multiline);

        public static IEnumerable<ProtoMessageInfo> Parse(string text)
        {
            int version = 3;
            string packageName = "";

            var m = regexVersion.Match(text);

            if (m.Success)
            {
                if (int.TryParse(m.Groups["Version"].Value, out var n))
                {
                    version = n;
                }
            }

            m = regexPackage.Match(text);

            if (m.Success)
            {
                packageName = m.Groups["Name"].Value;
            }
            Regex msgRegex = null;
            if (string.IsNullOrEmpty(Program.MsgPattern))
                throw new Exception($"{nameof(Program.MsgPattern)} empty");
            msgRegex = new Regex(Program.MsgPattern);

            foreach (Match m1 in regexMessage.Matches(text))
            {

                ProtoMessageInfo msg = new ProtoMessageInfo()
                {
                    Name = m1.Groups["Name"].Value,
                    PackageName = packageName,
                    Version = version,

                };
                if (!string.IsNullOrEmpty(packageName))
                {
                    msg.FullName = msg.PackageName + "." + msg.Name;
                }
                else
                {
                    msg.FullName = msg.Name;
                }

                var m2 = msgRegex.Match(msg.FullName);
                if (m2.Groups["cs"].Success)
                {
                    msg.IsClientToServer = true;
                }
                else if (m2.Groups["sc"].Success)
                {
                    msg.IsClientToServer = false;
                }

                msg.UsedName = m2.Groups["name"].Value;
                if (string.IsNullOrEmpty(msg.UsedName))
                    msg.UsedName = msg.FullName;



                yield return msg;

            }
        }
    }
}
