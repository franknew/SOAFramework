using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Http;

namespace SOAFramework.Library
{
    public class ServiceModel: IResolver
    {
        private MethodInfo _self;
        private string idName;
        private IIDGenerator _generator = IDGeneratorFactory.Create(GeneratorType.SnowFlak);

        public ServiceModel(MethodInfo self)
        {
            _self = self;
            ID = _generator.Generate();
            this.Resolve();
        }

        public ServiceModel()
        {
            ID = _generator.Generate();
        }
        /// <summary>
        /// 方法名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 返回值信息
        /// </summary>
        public TypeModel Return { get; set; }
        /// <summary>
        /// 参数信息
        /// </summary>
        public List<TypeModel> Args { get; set; }
        /// <summary>
        /// 备注描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string TypeName { get; set; }
        /// <summary>
        /// 控制器
        /// </summary>
        public string Category { get; set; }
        /// <summary>
        /// 访问路由
        /// </summary>
        public string Route { get; set; }
        /// <summary>
        /// ID
        /// </summary>
        public string ID { get; private set; }

        /// <summary>
        /// 用于标识的服务名称
        /// </summary>
        public string FriendlyID { get; set; }
        /// <summary>
        /// http method(GET,POST,PUT,DELETE)
        /// </summary>
        public string HttpMethod { get; set; }

        public void Resolve()
        {
            if (_self == null) return;
            idName = GetFullName(_self);
            var desc = ResolverHelper.GetMemberFromCache(_self.DeclaringType, t =>
            {
                return t.Name.StartsWith("M:" + idName);
            });

            var args = _self.GetParameters();
            Args = new List<TypeModel>();
            for (int i = 0; i < args.Length; i++)
            {
                var arg = args[i];
                TypeModel argmodel = ModelFactory.CreateTypeModel(arg.ParameterType);
                Args.Add(argmodel);
            }
            Description = desc?.Summary?.Trim();
            Name = _self.Name;
            TypeName = _self.DeclaringType.FullName;
            Return = ModelFactory.CreateTypeModel(_self.ReturnType);
            Return.Description = desc?.Returns?.Trim();
        }

        public static string GetFullName(MethodInfo method)
        {
            return method.DeclaringType.FullName + "." + method.Name;
        }
    }
}
