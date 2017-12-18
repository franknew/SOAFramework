using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library.DAL
{
    public class BaseUpdateForm<TEntity, TQueryForm> : IUpdateForm where TEntity : BaseEntity where TQueryForm : BaseQueryForm
    {
        public TEntity Entity { get; set; }
        public TQueryForm QueryForm { get; set; }
    }
}
