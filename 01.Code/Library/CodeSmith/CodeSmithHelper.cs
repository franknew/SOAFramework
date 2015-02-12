using CodeSmith.Engine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SOAFramework.Library
{
    public class CodeSmithHelper
    {
        public static string GenerateString(string fullTemplateFileName, Dictionary<string, object> args)
        {
            string code = "";
            DefaultEngineHost host = new DefaultEngineHost(Path.GetDirectoryName(fullTemplateFileName));
            TemplateEngine engine = new TemplateEngine(System.IO.Path.GetDirectoryName(fullTemplateFileName));
            CompileTemplateResult result = engine.Compile(fullTemplateFileName);
            if (!result.Errors.HasErrors)
            {
                CodeTemplate template = result.CreateTemplateInstance();
                foreach (var key in args.Keys)
                {
                    PropertyInfo info = template.GetPropertyInfo(key, false);
                    object value = args[key];
                    if (info.PropertyType != value.GetType())
                    {
                        string json = JsonHelper.Serialize(value);
                        value = JsonHelper.Deserialize(json, info.PropertyType);
                    }
                    template.SetProperty(key, value);
                }
                code = template.RenderToString();
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                foreach (var e in result.Errors)
                {
                    if (e.IsError)
                    {
                        sb.AppendFormat("{0}\r\n", e.Description);
                    }
                }
                if (sb.Length > 0)
                {
                    throw new Exception(string.Format("编译模板时发生错误。\r\n{0}", sb.ToString()));
                }
            }
            return code;
        }

        public static string GenerateStringByShell(string fullTemplateFileName, Dictionary<string, object> args)
        {
            string data = "";
            string shellFileName = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + "\\SOAFramework.Library.CodeSmithConsole.exe";
            Process process = new Process();
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.FileName = shellFileName;
            process.StartInfo.Arguments = fullTemplateFileName + " " + JsonHelper.Serialize(args).Replace("\"", "\\\"");
            process.Start();
            process.WaitForExit();
            data = process.StandardOutput.ReadToEnd();
            return data;
        }
    }
}
