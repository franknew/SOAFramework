using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web.Services.Description;
using Microsoft.CSharp;

namespace SOAFramework.Library
{
    public class DynamicService
    {
        public DynamicService(string url, string nameSpace = null, SoapHeader header = null)
        {
            this.url = url;
            this.header = header;
            if (!string.IsNullOrEmpty(nameSpace)) this.nameSpace = nameSpace;
        } 

        private string url;
        private SoapHeader header;
        private string nameSpace = "EnterpriseServerBase.WebService.DynamicWebService";

        public string Url
        {
            get
            {
                return url;
            }
        }

        public SoapHeader Header
        {
            get
            {
                return header;
            }
        }

        public string NameSpace
        {
            get
            {
                return nameSpace;
            }
        }

        public object Invoke(string action, object[] args)
        {
            WebClient wc = new WebClient();
            using (Stream stream = wc.OpenRead(url + "?WSDL"))
            {
                ServiceDescription sd = ServiceDescription.Read(stream);
                ServiceDescriptionImporter sdi = new ServiceDescriptionImporter();
                sdi.AddServiceDescription(sd, "", "");
                CodeNamespace cn = new CodeNamespace(nameSpace);

                string typeName = "";
                string[] parts = url.Split('/');
                string[] pps = parts[parts.Length - 1].Split('.');

                typeName = pps[0];

                //生成客户端代理类代码
                CodeCompileUnit ccu = new CodeCompileUnit();
                ccu.Namespaces.Add(cn);
                sdi.Import(cn, ccu);
                CSharpCodeProvider csc = new CSharpCodeProvider();
                ICodeCompiler icc = csc.CreateCompiler();

                //设定编译参数
                CompilerParameters cplist = new CompilerParameters();
                cplist.GenerateExecutable = false;
                cplist.GenerateInMemory = true;
                cplist.ReferencedAssemblies.Add("System.dll");
                cplist.ReferencedAssemblies.Add("System.XML.dll");
                cplist.ReferencedAssemblies.Add("System.Web.Services.dll");
                cplist.ReferencedAssemblies.Add("System.Data.dll");

                //编译代理类
                CompilerResults cr = icc.CompileAssemblyFromDom(cplist, ccu);
                if (true == cr.Errors.HasErrors)
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    foreach (System.CodeDom.Compiler.CompilerError ce in cr.Errors)
                    {
                        sb.Append(ce.ToString());
                        sb.Append(System.Environment.NewLine);
                    }
                    throw new Exception(sb.ToString());
                }

                //生成代理实例，并调用方法
                System.Reflection.Assembly assembly = cr.CompiledAssembly;
                Type t = assembly.GetType(nameSpace + "." + typeName, true, true);
                object obj = Activator.CreateInstance(t);


                #region soapheader信息
                FieldInfo[] arry = t.GetFields();

                FieldInfo fieldHeader = null;
                //soapheader 对象值
                object objHeader = null;
                if (header != null)
                {
                    fieldHeader = t.GetField(header.ClassName + "Value");

                    Type tHeader = assembly.GetType(nameSpace + "." + header.ClassName);
                    objHeader = Activator.CreateInstance(tHeader);

                    foreach (KeyValuePair<string, object> property in header.Properties)
                    {
                        FieldInfo[] arry1 = tHeader.GetFields();
                        int ts = arry1.Count();
                        FieldInfo f = tHeader.GetField(property.Key);
                        if (f != null)
                        {
                            f.SetValue(objHeader, property.Value);
                        }
                    }
                }

                if (header != null)
                {
                    //设置Soap头
                    fieldHeader.SetValue(obj, objHeader);
                }

                #endregion


                System.Reflection.MethodInfo mi = t.GetMethod(action);
                return mi.Invoke(obj, args);
            }
        }
    }

    /// <summary>
    /// SOAP头
    /// </summary>
    public class SoapHeader
    {
        /// <summary>
        /// 构造一个SOAP头
        /// </summary>
        public SoapHeader()
        {
            this.Properties = new Dictionary<string, object>();
        }

        /// <summary>
        /// 构造一个SOAP头
        /// </summary>
        /// <param name="className">SOAP头的类名</param>
        public SoapHeader(string className)
        {
            this.ClassName = className;
            this.Properties = new Dictionary<string, object>();
        }

        /// <summary>
        /// 构造一个SOAP头
        /// </summary>
        /// <param name="className">SOAP头的类名</param>
        /// <param name="properties">SOAP头的类属性名及属性值</param>
        public SoapHeader(string className, Dictionary<string, object> properties)
        {
            this.ClassName = className;
            this.Properties = properties;
        }

        /// <summary>
        /// SOAP头的类名
        /// </summary>
        public string ClassName
        {
            get;
            set;
        }

        /// <summary>
        /// SOAP头的类属性名及属性值
        /// </summary>
        public Dictionary<string, object> Properties
        {
            get;
            set;
        }

        /// <summary>
        /// 为SOAP头增加一个属性及值
        /// </summary>
        /// <param name="name">SOAP头的类属性名</param>
        /// <param name="value">SOAP头的类属性值</param>
        public void AddProperty(string name, object value)
        {
            if (this.Properties == null)
            {
                this.Properties = new Dictionary<string, object>();
            }
            Properties.Add(name, value);
        }
    }
}
