using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library.SDK.Domain
{
    /// <summary>
    /// 
    /// </summary>
    public class ACLInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public int? CreateIndex { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? ModifyIndex { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Rules { get; set; }
    }
}
