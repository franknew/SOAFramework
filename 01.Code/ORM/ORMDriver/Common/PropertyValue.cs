using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.ORM.Common
{
    public class PropertyValue<T>
    {
        #region attribute
        private string _str_Name;
        private T _t_Value;
        #endregion

        #region property
        public string Name
        {
            get { return _str_Name; }
            set { _str_Name = value; }
        }

        public T Value
        {
            get { return _t_Value; }
            set { _t_Value = value; }
        }

        //public T this[string Name]
        //{
        //    set
        //    {
        //        if (_str_Name.ToLower().Equals(Name.ToLower()))
        //        {
        //            this[Name] = value;
        //        }
        //    }
        //    get
        //    {
        //        return this[Name];
        //    }
        //}
        #endregion
    }
}
