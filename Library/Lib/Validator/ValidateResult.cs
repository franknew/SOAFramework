using System;
using System.Collections.Generic;
using System.Text;

namespace Athena.Unitop.Sure.Lib.Validator
{
    public class ValidateResult
    {
        public bool IsValid { get; set; }

        public string ErrorMessage { get; set; }

        public string ControlID { get; set; }
    }
}
