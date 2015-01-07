using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SOAFramework.Service.Model
{
    /// <summary>
    /// 服务信息实体
    /// </summary>
    [DataContract]
    public class ServiceInfo
    {
        /// <summary>
        /// 接口名
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string InterfaceName { get; set; }

        /// <summary>
        /// 接口描述
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string Description { get; set; }

        /// <summary>
        /// 参数描述信息
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public List<ServiceParameter> Parameters { get; set; }

        /// <summary>
        /// 返回值描述
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string ReturnDesc { get; set; }
    }

    [DataContract]
    public class ServiceParameter
    {
        [DataMember(EmitDefaultValue = false)]
        public string Name { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string TypeName { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int Index { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Description { get; set; }
    }
}
