using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Athena.Unitop.Sure.Lib.Validator
{
    /// <summary>
    /// 规则验证器
    /// </summary>
    public class RegexValidator : IValidatorBase
    {
        public RegexValidator(string regex, string errorMessage = "", int validateIndex = 2)
        {
            this.Regex = regex;
            this.ErrorMessage = errorMessage;
            this.ValidateIndex = validateIndex;
            ID = Guid.NewGuid().ToString();
        }

        public string ID { get; set; }

        public ValiationType ValidatorType
        {
            get
            {
                return ValiationType.RegexValidator;
            }
        }

        public int ValidateIndex { get; set; }

        public object Value { get; set; }

        public string Regex { get; set; }

        public string ErrorMessage { get; set; }

        public bool Validate()
        {
            if (Value == null)
            {
                throw new Exception("请设置Value，Value不能为NULL");
            }
            if (Value.ToString().Trim() == "")
            {
                return true;
            }
            Regex regex = new Regex(Regex);
            return regex.IsMatch(Value.ToString());
        }

    }
}
