using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library.DAL
{
    public class BaseUpdateForm<TEntity> where TEntity : BaseEntity
    {
        private TEntity entity = default(TEntity);
        public TEntity Entity
        {
            get { return entity; }
            set { entity = value; }
        }
    }
}
