using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Reflection;
using System.IO;

namespace SOAFramework.Library.DAL
{
    public class SQLiteHelper : BaseHelper<SQLiteConnection, SQLiteCommand, SQLiteDataAdapter, SQLiteParameter>
    {
        public SQLiteHelper(string connectionstring) :
            base(new SQLitePagingSQL())
        {
            this.ConnectionString = connectionstring;
            Assembly ass = Assembly.GetCallingAssembly();
            FileInfo file = new FileInfo(ass.Location);
            string filePath = file.Directory.FullName.TrimEnd('\\');
            string modelesPath = filePath + "\\Modules";
            FileInfo interop = new FileInfo(filePath + "\\SQLite.Interop.dll");
            bool is64bitSystem = Environment.Is64BitOperatingSystem;
            string newInterop;
            if (is64bitSystem)
            {
                newInterop = filePath + "\\SQLite.Interop64.dll";
            }
            else
            {
                newInterop = filePath + "\\SQLite.Interop32.dll";
            }
            FileInfo newInteropFile = new FileInfo(newInterop);
            if (!newInteropFile.Exists)
            {
                interop = new FileInfo(modelesPath + "\\SQLite.Interop.dll");
                if (is64bitSystem)
                {
                    newInterop = modelesPath + "\\SQLite.Interop64.dll";
                }
                else
                {
                    newInterop = modelesPath + "\\SQLite.Interop32.dll";
                }
                newInteropFile = new FileInfo(newInterop);
            }

            if (!newInteropFile.Exists)
            {
                return;
            }
            if (interop.Exists)
            {
                if (interop.Length != newInteropFile.Length)
                {
                    newInteropFile.CopyTo(interop.FullName, true);
                }
            }
            else
            {
                newInteropFile.CopyTo(interop.FullName, true);
            }
        }
    }
}
