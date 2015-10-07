using System;
using System.Collections.Generic;
using System.Text;

namespace Athena.Unitop.Sure.Lib.Validator
{
    public interface IValidatorBase
    {
        string ID { get; set; }

        /// <summary>
        /// 验证器类型
        /// </summary>
        ValiationType ValidatorType { get; }

        /// <summary>
        /// 需要验证的值
        /// </summary>
        object Value { get; set; }

        /// <summary>
        /// 验证失败后的错误信息
        /// </summary>
        string ErrorMessage { get; set; }

        /// <summary>
        /// 验证顺序
        /// </summary>
        int ValidateIndex { get; set; }

        /// <summary>
        /// 执行验证
        /// </summary>
        /// <returns></returns>
        bool Validate();
    }
}
