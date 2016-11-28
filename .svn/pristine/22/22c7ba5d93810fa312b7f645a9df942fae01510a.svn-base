using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.ORM.Mapping
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RelationMapping : Attribute
    {

        public static RelationMapping GetMappingAttribute(object Relationship)
        {
            return GetMappingAttribute(Relationship.GetType());
        }

        public static RelationMapping GetMappingAttribute(Type Type)
        {
            RelationMapping mapping = null;
            object[] attributes = Type.GetCustomAttributes(typeof(RelationMapping), false);
            if (attributes != null && attributes.Length > 0)
            {
                mapping = attributes[0] as RelationMapping;
            }
            return mapping;
        }
    }
}
