using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscoverServiceWeb.Models
{
    public class PropertyDescription
    {
        public string PropertyName { get; set; }
        private TypeInfo propertyTypeInfo = new TypeInfo();

        public TypeInfo PropertyTypeInfo
        {
            get { return propertyTypeInfo; }
            set { propertyTypeInfo = value; }
        }
    }
}