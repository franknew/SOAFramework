using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SOAFramework.Library
{
    public class ServiceModel: IResolver
    {
        private MethodInfo _self;
        private IIDGenerator _generator = IDGeneratorFactory.Create(GeneratorType.SnowFlak);
        public ServiceModel(MethodInfo self)
        {
            _self = self;
            this.Resolve();
        }
        
        public string Name { get; set; }
        public string FullName { get; set; }
        public TypeModel Return { get; set; }
        public List<ArgModel> Args { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string FullAction { get; set; }
        public string ID { get; set; }

        public IResolver Resolve()
        {
            ID = _generator.Generate();
            FullName = _self.DeclaringType.FullName + "." + _self.Name;
            var desc = ResolverHelper.GetMemberFromCache(_self.DeclaringType, t =>
            {
                return t.Name.StartsWith("M:" + FullName);
            });
            Description = desc?.Summary?.Trim();
            Name = _self.Name;
            Type = _self.DeclaringType.FullName;
            Return = new TypeModel(_self.ReturnType);
            Return.Description = desc?.Returns?.Trim();
            Args = new List<ArgModel>();

            var args = _self.GetParameters();
            int i = 0;
            foreach (var arg in args)
            {
                var argmodel = new ArgModel(arg.ParameterType);
                argmodel.MemberName = arg.Name;
                argmodel.Description = desc?.Params?.FirstOrDefault(t => t.Name.Equals(arg.Name))?.Description?.Trim();
                argmodel.Index = i;
                Args.Add(argmodel);
                i++;
            }
            
            return this;
        }
    }
}
