using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library.Validator
{
    public enum ValiationType
    {
        EmptyValidator = 0,
        LengthValidator = 1,
        RegexValidator = 2,
        FunctionValidator,
    }
}
