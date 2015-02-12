using System;
using System.Collections.Generic;
using System.Text;

namespace Athena.Unitop.Sure.Lib.Validator
{
    /// <summary>
    /// 非空验证器
    /// </summary>
    public class EmptyValidator : IValidatorBase
    {
        public EmptyValidator(string errorMessage = "", int validateIndex = 0)
        {
            ErrorMessage = errorMessage;
            ValidateIndex = validateIndex;
            ID = Guid.NewGuid().ToString();
        }

        public ValiationType ValidatorType
        {
            get
            {
                return ValiationType.EmptyValidator;
            }
        }

        public string ID { get; set; }

        public object Value { get; set; }

        public string ErrorMessage { get; set; }

        public int ValidateIndex { get; set; }

        public bool Validate()
        {
            string value = null;
            if (Value != null && Value != DBNull.Value)
            {
                value = (string)Value;
            }
            return !string.IsNullOrEmpty(value);
        }
    }
}
