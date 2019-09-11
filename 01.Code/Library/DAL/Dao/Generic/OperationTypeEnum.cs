using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library.DAL.Generic
{
    public enum OperationTypeEnum
    {
        Equals,
        LargeThan,
        LargeEqual,
        LessThan,
        LessEqual,
        In,
        Like,
        NotIn,
        NotLike,
    }

    public class OperationTypeConverter
    {
        public static string Convert(OperationTypeEnum type)
        {
            string opertion = "";
            switch (type)
            {
                case OperationTypeEnum.Equals:
                    opertion = "=";
                    break;
                case OperationTypeEnum.In:
                    opertion = "in";
                    break;
                case OperationTypeEnum.LargeEqual:
                    opertion = ">=";
                    break;
                case OperationTypeEnum.LargeThan:
                    opertion = ">";
                    break;
                case OperationTypeEnum.LessEqual:
                    opertion = "<";
                    break;
                case OperationTypeEnum.LessThan:
                    opertion = "<=";
                    break;
                case OperationTypeEnum.Like:
                    opertion = "like";
                    break;
                case OperationTypeEnum.NotIn:
                    opertion = "not in";
                    break;
                case OperationTypeEnum.NotLike:
                    opertion = "not like";
                    break;
            }

            return opertion;
        }
    }
}
