using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library
{
    public interface IFileNameGenerator
    {
        string GetFileName(string fileNameFormat, long fileSize);
    }

    public class FileNameGeneratorFactory
    {
        public static IFileNameGenerator Create(FileTypeEnum type)
        {
            IFileNameGenerator generator = null;
            switch (type)
            {
                case FileTypeEnum.Log:
                    generator = new LogFileNameGenerator();
                    break;
                case FileTypeEnum.Debug:
                    generator = new DebugFileNameGanerator();
                    break;
                case FileTypeEnum.Error:
                    generator = new ErrorFileNameGenerator();
                    break;
            }
            return generator;
        }
    }



    public enum FileTypeEnum
    {
        Log,
        Debug,
        Error
    }
}
