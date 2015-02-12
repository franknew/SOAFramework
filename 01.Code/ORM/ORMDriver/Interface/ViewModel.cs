using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.Interface
{
    public class ViewModel<T> : ModelBase<T>, ITableInterface<T>
    {
    }
}
