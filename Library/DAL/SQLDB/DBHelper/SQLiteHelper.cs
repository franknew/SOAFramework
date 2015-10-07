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
            FileInfo interop = new FileInfo(file.Directory.FullName.TrimEnd('\\') + "\\SQLite.Interop.dll");
            bool is64bitSystem = Environment.Is64BitOperatingSystem;
            string newInterop;
            if (is64bitSystem)
            {
                newInterop = file.Directory.FullName.TrimEnd('\\') + "\\SQLite.Interop64.dll";
            }
            else
            {
                newInterop = file.Directory.FullName.TrimEnd('\\') + "\\SQLite.Interop32.dll";
            }
            FileInfo newInteropFile = new FileInfo(newInterop);
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
