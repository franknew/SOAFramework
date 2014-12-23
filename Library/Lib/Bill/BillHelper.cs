using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using SOAFramework.Library.Validator;

namespace SOAFramework.Library
{
    public class BillHelper
    {
        /// <summary>
        /// --速尔单号规则校验,根据单号返回单号类型;
        ///--返回类型为数字: 0(运单) or 1(子单) or 2(货单) or 3(无效单号) or 4(袋号) or 5(到付单、内部单) or
        ///          --6(同城件) or 7(回单) or 8(香港件) or 9(车号) or 10(笼号) or 11(包号) or 12(市内件)
        /// </summary>
        /// <param name="code">返回类型为数字: 0(运单) or 1(子单) or 2(货单) or 3(无效单号) or 4(袋号) or 5(到付单、内部单) or 6(同城件) or 7(回单) or 8(香港件) or 9(车号) or 10(笼号) or 11(包号) or 12(市内件)</param>
        /// <returns>返回类型为数字: 0(运单) or 1(子单) or 2(货单) or 3(无效单号) or 4(袋号) or 5(到付单、内部单) or 6(同城件) or 7(回单) or 8(香港件) or 9(车号) or 10(笼号) or 11(包号) or 12(市内件)</returns>
        public static int CheckSureBillCode(string code)
        {  int type = 3;
        if (string.IsNullOrEmpty(code)) return type;
          
            int length = code.Length;
            if (length == 12 && new Regex(RegularExpressions.NumberOrUpperEnglish).IsMatch(code))
            {
                type = 0;
            }
            else if (length == 10 && new Regex(RegularExpressions.NumberOnly).IsMatch(code))
            {
                type = 0;
            }
            else if (length == 8 && new Regex(RegularExpressions.NumberOnly).IsMatch(code))
            {
                type = 1;
            }
            else if (length == 8 && new Regex(RegularExpressions.UpperEnglishOny).IsMatch(code.Substring(0, 3))
                && new Regex(RegularExpressions.NumberOrUpperEnglish).IsMatch(code.Substring(3, 1))
                && new Regex(RegularExpressions.NumberOnly).IsMatch(code.Substring(4, 4)))
            {
                type = 1;
            }
            else if (length == 10 && code.Substring(0, 1) == "P" && new Regex(RegularExpressions.NumberOnly).IsMatch(code.Substring(1, 9)))
            {
                type = 1;
            }
            else if (length == 9 && new Regex(RegularExpressions.NumberOrUpperEnglish).IsMatch(code))
            {
                type = 1;
            }
            else if (length == 8 && code.Substring(0, 1) == "S" && new Regex(RegularExpressions.NumberOnly).IsMatch(code.Substring(1, 7)))
            {
                type = 2;
            }
            else if (length == 10 && code.Substring(0, 1) == "S" && new Regex(RegularExpressions.NumberOnly).IsMatch(code.Substring(1, 9)))
            {
                type = 2;
            }
            else if (length == 10 && code.Substring(0, 1) == "C" && new Regex(RegularExpressions.NumberOnly).IsMatch(code.Substring(1, 9)))
            {
                type = 4;
            }
            else if (length == 8 && code.Substring(0, 1) == "C" && new Regex(RegularExpressions.NumberOnly).IsMatch(code.Substring(1, 7)))
            {
                type = 4;
            }
            else
            {
                type = 3;
            }
            return type;
        }
    }
}
