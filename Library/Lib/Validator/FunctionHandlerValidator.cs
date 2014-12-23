using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library.Validator
{
    public delegate string FunctionHandlerDelegate(Dictionary<string, object> args);

    /// <summary>
    /// 自定义函数验证器
    /// </summary>
    public class FunctionHandlerValidator : IValidatorBase
    {
        public FunctionHandlerValidator(Dictionary<string, object> args = null, int validateIndex = 3)
        {
            this.Args = args;
            this.ValidateIndex = validateIndex;
            ID = Guid.NewGuid().ToString();
        }

        public string ID { get; set; }

        public ValiationType ValidatorType
        {
            get { return ValiationType.FunctionValidator; }
        }

        public Dictionary<string, object> Args { get; set; }

        public int ValidateIndex { get; set; }

        public object Value { get; set; }

        public FunctionHandlerDelegate FunctionHandler { get; set; }

        public string ErrorMessage { get; set; }

        public bool Validate()
        {
            if (Value == null)
            {
                throw new Exception("请设置Value，Value不能为NULL");
            }
            if (FunctionHandler == null)
            {
                throw new Exception("请设置FunctionHandler，FunctionHandler不能为NULL");
            }
            else
            {
                if (Args == null)
                {
                    Args = new Dictionary<string, object>();
                }
                Args["_value"] = Value;
                string errorMsg = FunctionHandler.Invoke(Args);
                if (string.IsNullOrEmpty(errorMsg))
                {
                    return true;
                }
                else
                {
                    this.ErrorMessage = errorMsg;
                    return false;
                }
            }
        }
    }
}
