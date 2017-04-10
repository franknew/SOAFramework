using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SOAFramework.Library
{
    public static class DirectoryExtension
    {
        public static void Copy(this DirectoryInfo directory, string path, bool copyDirs = true)
        {
            if (!directory.Exists) directory.Create();
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            var files = directory.GetFiles();
            foreach (var file in files)
            {
                string destFile = Path.Combine(path, file.Name);
                file.CopyTo(destFile, true);
            }
            if (!copyDirs) return;
            var dirs = directory.GetDirectories();
            foreach (DirectoryInfo subdir in dirs)
            {
                string temppath = Path.Combine(path, subdir.Name);
                subdir.Copy(temppath, copyDirs);
            }
        }
    }
}
