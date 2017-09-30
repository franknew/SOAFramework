using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SOAFramework.Library
{
    public interface IIDGenerator
    {
        string Generate();
    }

    public class IDGeneratorFactory
    {
        public static IIDGenerator Create(GeneratorType type)
        {
            IIDGenerator generator = null;
            switch (type)
            {
                case GeneratorType.GUID:
                    generator = new GUIDGenerator();
                    break;
                case GeneratorType.SnowFlak:
                    generator = new SnowFlakGenerator();
                    break;
            }

            return generator;
        }
    }

    public enum GeneratorType
    {
        GUID,
        SnowFlak
    }
}
