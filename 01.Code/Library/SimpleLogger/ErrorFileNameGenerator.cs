using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SOAFramework.Library
{
    class ErrorFileNameGenerator : BaseFileNameGenerator
    {
        private static int _index = 0;

        public override string GetDirectory()
        {
            return "Error";
        }

        public override string GetExtension()
        {
            return ".err";
        }

        public override int GetIndex()
        {
            return _index;
        }

        public override void IncreaseIndex()
        {
            _index++;
        }

        public override void SetIndex(int index)
        {
            _index = index;
        }
    }
}
