using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library.DAL
{
    public class SimpleUpdateForm <TEntity, TQueryForm>: BaseUpdateForm<TEntity, TQueryForm> where TEntity : SimpleEntity where TQueryForm : SimpleQueryForm
    {
    }
}
