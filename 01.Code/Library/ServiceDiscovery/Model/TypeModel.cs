using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SOAFramework.Library
{
    public class TypeModel
    {
        private IIDGenerator _generator = IDGeneratorFactory.Create(GeneratorType.SnowFlak);
        private Type _self;

        public TypeModel()
        {
            ID = _generator.Generate();
        }

        /// <summary>
        /// ID
        /// </summary>
        [JsonProperty("id")]
        public string ID { get; set; }
        /// <summary>
        /// 成员名称
        /// </summary>
        [JsonProperty("memberName")]
        public string MemberName { get; set; }

        /// <summary>
        /// 是否数组
        /// </summary>
        [JsonProperty("isArray")]
        public bool IsArray { get; set; }
        /// <summary>
        /// 类型名称
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
        /// <summary>
        /// 类型全名
        /// </summary>
        [JsonProperty("fullName")]
        public string FullName { get; set; }
        /// <summary>
        /// 备注描述
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }
        /// <summary>
        /// 命名空间
        /// </summary>
        [JsonProperty("nameSpace")]
        public string NameSpace { get; set; }
        /// <summary>
        /// 泛型
        /// </summary>
        [JsonProperty("genericArguments")]
        public List<TypeModel> GenericArguments { get; set; }
        /// <summary>
        /// 属性
        /// </summary>
        [JsonProperty("properties")]
        public List<TypeModel> Properties { get; set; }
        /// <summary>
        /// 是否类
        /// </summary>
        [JsonProperty("isClass")]
        public bool IsClass { get; set; }

        [JsonIgnore]
        public Type Type { get; internal set; }

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
