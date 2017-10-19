using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Athena.Unitop.Sure.Lib.Validator
{
    /// <summary>
    /// 长度验证器
    /// </summary>
    public class LengthValidator : IValidatorBase
    {
        public LengthValidator(int minLength = 0, int maxLength = 0, string errorMessage = "", int validateIndex = 1)
        {
            MinLength = minLength;
            MaxLength = maxLength;
            ErrorMessage = errorMessage;
            ValidateIndex = validateIndex;
            ID = Guid.NewGuid().ToString();
        }

        public ValiationType ValidatorType
        {
            get
            {
                return ValiationType.LengthValidator;
            }
        }

        public string ID { get; set; }

        public string ErrorMessage { get; set; }

        public object Value { get; set; }

        public int ValidateIndex { get; set; }

        /// <summary>
        /// 最小长度
        /// </summary>
        public int MinLength {get;set;}

        /// <summary>
        /// 最大长度
        /// </summary>
        public int MaxLength { get; set; }

        public bool Validate()
        {
            string value = Value.ToString();
            if (MaxLength > 0 && MinLength <= 0)
            {
                return MaxLength >= value.Length;
            }
            else if (MinLength > 0 && MaxLength <= 0)
            {
                return value.Length >= MinLength;
            }
            else
            {
                return MaxLength >= value.Length && value.Length >= MinLength;
            }
        }
    }
}
