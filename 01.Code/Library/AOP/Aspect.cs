using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Activation;


namespace SOAFramework.Library.AOP
{
    //AOP类
    internal class AOPAspect : IMessageSink
    {
        private IMessageSink mIM_next;
        //当前方法的信息
        private MarshalByRefObject mMro_ParentMethod;
        //AOP的范围
        private AOPAttributeArea mEnmu_AttrArea;
        //自定义在方法前运行的方法名
        private string mStr_BeforeMethodName;
        //自定义在方法后运行的方法名
        private string mStr_AfterMethodName;
        private string mStr_OnExceptionName;
        private string mStr_CustomAspectMethodNameSpace;
        private string mStr_CustomDllFullFileName;
        private string mStr_ClassName;
        private bool mBl_HasMethodAttr;
        private bool mBl_PassArgs;

        private const string _strDefaultBeforeMethodName = "BeforeMethodFunction";
        private const string _strDefaultAfterMethodName = "AfterMethodFunction";
        private const string _strDefaultOnExceptionMethodName = "OnExceptionFunction";
        private const string _strClassName = "AOP";
        private const string _strNameSpace = "AOP.DefaultAOPMethod";
        /// <summary>
        /// AOP构造函数
        /// </summary>
        /// <param name="Next"></param>
        /// <param name="Method">截获的方法对象</param>
        internal AOPAspect(IMessageSink Next, MarshalByRefObject Method)
        {
            //Console.WriteLine("in AOPAspect contructor");
            this.mIM_next = Next;
            this.mMro_ParentMethod = Method;
        }

        #region IMessageSink implementation
        public IMessageSink NextSink
        {
            get { return mIM_next; }
        }
        /// <summary>
        /// 同步处理消息
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public IMessage SyncProcessMessage(IMessage msg)
        {
            GetAttributeSettings(msg);
            try
            {
                IMethodCallMessage call = msg as IMethodCallMessage;
                //调用方法之前
                BeforeMethod();
                //Console.WriteLine("before method");
                //截获方法
                IMessage returnMethod = mIM_next.SyncProcessMessage(msg);
                //调用方法之后
                AfterMethod();
                //Console.WriteLine("after method");

                return returnMethod;
            }
            catch//抛出不经过再次封装的异常,如果throw ex,则异常将会被封装一次
            {
                OnExcetpion();
                throw ;
            }
        }
        /// <summary>
        /// 异步处理消息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="replySink"></param>
        /// <returns></returns>
        public IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink)
        {
            throw new InvalidOperationException();
        }

        #endregion 
        //private void Preprocess(IMessage msg)
        //{
        //    // We only want to process method calls
        //    if (!(msg is IMethodMessage)) return;
        //    IMethodMessage call = msg as IMethodMessage;
        //    Type type = Type.GetType(call.TypeName);
        //    string callStr = type.Name + "." + call.MethodName;
        //}

        #region 截取消息后进行处理的方法
        /// <summary>
        /// 在方法执行之前执行
        /// </summary>
        private bool BeforeMethod()
        {
            if (mEnmu_AttrArea != AOPAttributeArea.Class && string.IsNullOrEmpty(mStr_BeforeMethodName) && !mBl_HasMethodAttr)
            {
                return true;
            }
            bool blReturn = false;
            string strBeforeMethodName = string.IsNullOrEmpty(mStr_BeforeMethodName) ? _strDefaultBeforeMethodName : mStr_BeforeMethodName;
            string strClassName = string.IsNullOrEmpty(mStr_BeforeMethodName) ? _strClassName : mStr_ClassName;
            string strTypeName = string.IsNullOrEmpty(mStr_BeforeMethodName) ? _strNameSpace : mStr_CustomAspectMethodNameSpace;
            object[] objArgs = new object[2];
            objArgs[0] = mIM_next;
            objArgs[1] = mMro_ParentMethod;
            object objReturn = Reflection.CallMethod(MethodName: strBeforeMethodName, DllFilePath: mStr_CustomDllFullFileName, AssemblyName: strClassName, ClassTypeName: strTypeName, Args: objArgs);
            try
            {
                blReturn = Convert.ToBoolean(objReturn);
            }
            catch
            {
            }
            return blReturn;
        }

        private bool AfterMethod()
        {
            if (mEnmu_AttrArea != AOPAttributeArea.Class && string.IsNullOrEmpty(mStr_AfterMethodName) && !mBl_HasMethodAttr)
            {
                return true;
            } 
            string strAfterMethodName = string.IsNullOrEmpty(mStr_AfterMethodName) ? _strDefaultAfterMethodName : mStr_AfterMethodName;
            string strClassName = string.IsNullOrEmpty(mStr_AfterMethodName) ? _strClassName : mStr_ClassName;
            string strTypeName = string.IsNullOrEmpty(mStr_AfterMethodName) ? _strNameSpace : mStr_CustomAspectMethodNameSpace;
            object[] objArgs = new object[2];
            objArgs[0] = mIM_next;
            objArgs[1] = mMro_ParentMethod;
            object objReturn = Reflection.CallMethod(MethodName: strAfterMethodName, DllFilePath: mStr_CustomDllFullFileName, AssemblyName: strClassName, ClassTypeName: strTypeName, Args: objArgs);
            bool blReturn = false;
            try
            {
                blReturn = Convert.ToBoolean(objReturn);
            }
            catch
            {
            }
            return blReturn;
        }

        private bool OnExcetpion()
        {
            if (mEnmu_AttrArea != AOPAttributeArea.Class && string.IsNullOrEmpty(mStr_OnExceptionName) && !mBl_HasMethodAttr)
            {
                return true;
            }
            string strOnExcetpionMethodName = string.IsNullOrEmpty(mStr_OnExceptionName) ? _strDefaultOnExceptionMethodName : mStr_OnExceptionName;
            string strClassName = string.IsNullOrEmpty(mStr_OnExceptionName) ? _strClassName : mStr_ClassName;
            string strTypeName = string.IsNullOrEmpty(mStr_OnExceptionName) ? _strNameSpace : mStr_CustomAspectMethodNameSpace;
            object[] objArgs = new object[2];
            objArgs[0] = mIM_next;
            objArgs[1] = mMro_ParentMethod;
            object objReturn = Reflection.CallMethod(MethodName: strOnExcetpionMethodName, DllFilePath: mStr_CustomDllFullFileName, AssemblyName: strClassName, ClassTypeName: strTypeName, Args: objArgs);
            bool blReturn = false;
            try
            {
                blReturn = Convert.ToBoolean(objReturn);
            }
            catch
            {
            }
            return blReturn;
        }

        /// <summary>
        /// 获得类和方法里面的属性
        /// </summary>
        /// <param name="msg"></param>
        private void GetAttributeSettings(IMessage msg)
        {
            //获得类属性
            object[] objClassAttribute = this.mMro_ParentMethod.GetType().GetCustomAttributes(typeof(AOPClassAttribute), true);
            if (objClassAttribute == null || objClassAttribute.Length == 0)
            {
                return;
            }
            PropertyInfo piAttrArea = objClassAttribute[0].GetType().GetProperty("AttributeArea");
            mEnmu_AttrArea = (AOPAttributeArea)piAttrArea.GetValue(objClassAttribute[0], null);
            //转换成callmessage获得运行的方法名
            IMethodCallMessage imcmMessage = msg as IMethodCallMessage;
            //获得该方法上的AOP属性
            object[] objMethodAttribute = mMro_ParentMethod.GetType().GetMethod(imcmMessage.MethodName).GetCustomAttributes(typeof(AOPMethodAttribute), true);
            if (objMethodAttribute == null || objMethodAttribute.Length == 0)
            {
                mBl_HasMethodAttr = false;
                return;
            }
            mBl_HasMethodAttr = true;
            PropertyInfo[] arrProperties = objMethodAttribute[0].GetType().GetProperties();
            foreach (PropertyInfo piTemp in arrProperties)
            {
                object objValue = piTemp.GetValue(objMethodAttribute[0], null);
                string strValue = objValue == null ? "" : objValue.ToString();
                switch (piTemp.Name.ToLower())
                {
                    case "custombeforemethodname":
                        mStr_BeforeMethodName = strValue;
                        break;
                    case "customaftermethodname":
                        mStr_AfterMethodName = strValue;
                        break;
                    case "customonexceptionmethodname":
                        mStr_OnExceptionName = strValue;
                        break;
                    case "customaspectmethonamespace":
                        mStr_CustomAspectMethodNameSpace = strValue;
                        if (!string.IsNullOrEmpty(mStr_CustomAspectMethodNameSpace))
                        {
                            mStr_ClassName = mStr_CustomAspectMethodNameSpace.Substring(0, mStr_CustomAspectMethodNameSpace.IndexOf("."));
                        }
                        else
                        {
                            mStr_ClassName = mMro_ParentMethod.GetType().Namespace;
                        }
                        break;
                    case "customdllfullfilename":
                        mStr_CustomDllFullFileName = strValue;
                        break;
                    case "passargs":
                        mBl_HasMethodAttr = objValue == null ? false : Convert.ToBoolean(objValue);
                        break;
                }
            }
            if (string.IsNullOrEmpty(mStr_BeforeMethodName))
            {

            }
        }
        #endregion

        //private void AfterProcess(IMessage msg)
        //{
        //    if (!(msg is IMethodMessage)) return;
        //}
    }
    /// <summary>
    /// AOP属性
    /// </summary>
    public class AOPProperty : IContextProperty, IContributeObjectSink
    {
        #region IContributeObjectSink implementation
        public string mstr_Name = "AOPProperty";
        public IMessageSink GetObjectSink(MarshalByRefObject o, IMessageSink next)
        {
            Console.WriteLine("GetObjectSink");
            return new AOPAspect(next, o);
        }
        #endregion

        #region IContextProperty implementation
        /// <summary>
        /// 默认空实现
        /// </summary>
        /// <param name="a"></param>
        public void Freeze(Context a)
        {
            Console.WriteLine("Freeze");
        }
        /// <summary>
        /// 判断当前上下文是否可用,默认空实现,返回true
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public bool IsNewContextOK(Context a)
        {
            Console.WriteLine("IsNewContextOK");
            return true;
        }
        public string Name
        {
            get { return mstr_Name; }
        }
        #endregion 
    }

    //继承AOPClass即可实现AOP
    [Aspect()]
    public class AOPClass : ContextBoundObject
    {
    }
}
