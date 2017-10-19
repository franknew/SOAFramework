using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting.Proxies;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;

namespace SOAFramework.Library.AOP
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AOPClassAttribute : Attribute
    {
        private AOPAttributeArea enmu_AttrArea;

        public AOPAttributeArea AttributeArea
        {
            set { enmu_AttrArea = value; }
            get { return enmu_AttrArea; }
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class AOPMethodAttribute : Attribute
    {
        private string mStr_CustomBeforeMethodName;
        private string mStr_CustomAfterMethodName;
        private string mStr_CustomOnExceptionMethodName;
        private string mStr_CustomAspectMethoNameSpace;
        private string mStr_CustomDllFullFileName;
        private bool mBl_PassArgs;

        public string CustomBeforeMethodName
        {
            set { mStr_CustomBeforeMethodName = value; }
            get { return mStr_CustomBeforeMethodName; }
        }

        public string CustomAfterMethodName
        {
            set { mStr_CustomAfterMethodName = value; }
            get { return mStr_CustomAfterMethodName; }
        }

        public string CustomOnExceptionMethodName
        {
            set { mStr_CustomOnExceptionMethodName = value; }
            get { return mStr_CustomOnExceptionMethodName; }
        }

        public string CustomAspectMethoNameSpace
        {
            set { mStr_CustomAspectMethoNameSpace = value; }
            get { return mStr_CustomAspectMethoNameSpace; }
        }

        public string CustomDllFullFileName
        {
            set { mStr_CustomDllFullFileName = value; }
            get { return mStr_CustomDllFullFileName; }
        }

        public bool PassArgs
        {
            set { mBl_PassArgs = value; }
            get { return mBl_PassArgs; }
        }
    }


    [AttributeUsage(AttributeTargets.Class)]
    public class AspectAttribute : ContextAttribute
    {
        public AspectAttribute()
            : base("AspectAttribute") 
        {
            //Console.WriteLine("AspectAttribute contructor");
        }

        public override void GetPropertiesForNewContext(IConstructionCallMessage ccm)
        {
            //植入截获类,用于截获方法
            //Console.WriteLine("GetPropertiesForNewContext");
            ccm.ContextProperties.Add(new AOPProperty());
        }
    }
}
