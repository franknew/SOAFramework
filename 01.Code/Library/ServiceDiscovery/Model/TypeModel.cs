using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SOAFramework.Library
{
    public class TypeModel : IResolver
    {
        private IIDGenerator _generator = IDGeneratorFactory.Create(GeneratorType.SnowFlak);
        private Type _self;
        public TypeModel(Type self)
        {
            _self = self;
            this.Resolve();
        }

        public TypeModel() { }
        public string ID { get; set; }
        public string MemberName { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string Description { get; set; }
        public string NameSpace { get; set; }
        public List<TypeModel> GenericArguments { get; set; }
        public List<TypeModel> Properties { get; set; }
        public bool IsClass { get; set; }

        public IResolver Resolve()
        {
            ID = _generator.Generate();
            Name = _self.Name;
            FullName = _self.FullName;
            GenericArguments = new List<TypeModel>();
            Properties = new List<TypeModel>();
            IsClass = _self.IsClass;
            if (_self.IsValueType || _self.Equals(typeof(string)) || _self.Equals(typeof(Type)))
            {
                TypeModelCacheManager.Set(this.ID, this);
                return this;
            }
            else if (_self.IsGenericType)
            {
                var arguments = _self.GetGenericArguments();
                foreach (var arg in arguments)
                {
                    TypeModel model = ResolverHelper.GetTypeFromCache(arg);
                    GenericArguments.Add(model);
                }
            }
            else if (_self.IsArray)
            {
                var elementType = _self.GetElementType();
                TypeModel model = ResolverHelper.GetTypeFromCache(elementType);
                GenericArguments.Add(model);
            }
            else
            {
                var properties = _self.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                foreach (var p in properties)
                {
                    TypeModel model = ResolverHelper.GetTypeFromCache(p.PropertyType);
                    var desc = ResolverHelper.GetMemberFromCache(p.DeclaringType, t =>
                    {
                        return t.Name.StartsWith("P:" + p.DeclaringType.FullName + "." + p.Name);
                    });
                    model.Name = p.PropertyType.Name;
                    model.Description = desc?.Summary?.Trim();
                    model.MemberName = p.Name;
                    Properties.Add(model);
                }
            }
            return this;
        }


        public TypeModel Copy()
        {
            TypeModel model = new TypeModel
            {
                FullName = this.FullName,
                Name = this.Name,
                Properties = this.Properties,
                GenericArguments = this.GenericArguments,
                MemberName = this.MemberName,
            };
            return model;
        }
    }
}
