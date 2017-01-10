using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOAFramework.Library
{
    class program
    {
        static void Main(string[] args)
        {
            string fileName = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + "\\CodeSmith.Log";
            File.AppendAllText(fileName, "begin");
            if (args.Length >= 2)
            {
                try
                {
                    File.AppendAllText(fileName, "in");
                    string templatePath = args[0];
                    StringBuilder json = new StringBuilder();
                    for (int i = 1; i < args.Length; i++)
                    {
                        json.AppendFormat("{0} ", args[i]);
                    }
                    Dictionary<string, object> dicArgs = JsonHelper.Deserialize<Dictionary<string, object>>(json.ToString());
                    File.AppendAllText(fileName, "begin generate");
                    string render = CodeSmithHelper.GenerateString(templatePath, dicArgs);
                    File.AppendAllText(fileName, "end generate");
                    Console.Write(render);
                    File.AppendAllText(fileName, "over");
                }
                catch (Exception ex)
                {
                    File.AppendAllText(fileName, "error ");
                    File.AppendAllText(fileName, string.Format("message:{0}\r\nstacktrace:{1}", ex.Message, ex.StackTrace));
                    Console.WriteLine("message:{0}\r\nstacktrace:{1}", ex.Message, ex.StackTrace);
                    Console.ReadLine();
                }
            }
        }
    }
}
