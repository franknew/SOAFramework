using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Frank.Common.DAL;

namespace SOAFramework.ORM.ModelDriver
{
    partial class ModelBase<T> where T : new()
    {
        public static void RollBack()
        {
            if (_DBHelper != null)
            {
                _DBHelper.RollBack();
            }
        }
    }
}