using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library.Validator
{
    public class RegularExpressions
    {
        /// <summary>
        /// 验证数字
        /// </summary>
        public const string NumberOnly = @"^[0-9]+$";

        /// <summary>
        /// 验证大写英文与数字
        /// </summary>
        public const string NumberOrUpperEnglish = @"^[A-Z0-9]+$";
        /// <summary>
        /// 验证英文
        /// </summary>
        public const string EnglishOny = @"^[A-Za-z]+$";

        /// <summary>
        /// 验证大写英文
        /// </summary>
        public const string UpperEnglishOny = @"^[A-Z]+$";

        /// <summary>
        /// 验证英文和数字
        /// </summary>
        public const string EnglishOrNumber = @"^[A-Za-z0-9]+$";

        /// <summary>
        /// 验证中文
        /// </summary>
        public const string ChineseOnly = @"^[\u4E00-\u9FFF]+$";

        /// <summary>
        /// 验证身份证15或18位
        /// </summary>
        public const string ID15Or18 = @"^\d{15}|\d{}18$";

        /// <summary>
        /// 验证EMAIL
        /// </summary>
        public const string Email = @"^\w+[-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";

        /// <summary>
        /// 验证密码
        /// 以字母开头，长度在6-18之间，只能包含字符、数字和下划线
        /// </summary>
        public const string Password = @"^[a-zA-Z]\w{5,17}$";

        /// <summary>
        /// 验证URL
        /// </summary>
        public const string URL = @"^http://([\w-]+\.)+[\w-]+(/[\w-./?%&=]*)?$ ；^[a-zA-z]+://(w+(-w+)*)(.(w+(-w+)*))*(?S*)?$";

        /// <summary>
        /// 验证电话号码
        /// 正确格式为：XXXX-XXXXXXX，XXXX-XXXXXXXX，XXX-XXXXXXX，XXX-XXXXXXXX，XXXXXXX，XXXXXXXX
        /// </summary>
        public const string Phone = @"^(\(\d{3,4}\)|\d{3,4}-)?\d{7,8}$";

        /// <summary>
        /// 正小数
        /// </summary>
        public const string DigitOnly = @"[\-\+]?[0-9]*(\.[0-9]+)?";

        /// <summary>
        /// 手机号码，11位
        /// </summary>
        public const string Mobile = @"^(13[0-9]|15[^4]|18[6|8|9])\d{8}$";

        /// <summary>
        /// 日期0-31
        /// </summary>
        public const string Days = @"\d|3[01]";

        public const string ALPHA = "[^a-zA-Z]";
        public const string ALPHA_NUMERIC = "[^a-zA-Z0-9]";
        public const string ALPHA_NUMERIC_SPACE = @"[^a-zA-Z0-9\s]";
        public const string CREDIT_CARD_AMERICAN_EXPRESS = @"^(?:(?:[3][4|7])(?:\d{13}))$";
        public const string CREDIT_CARD_CARTE_BLANCHE = @"^(?:(?:[3](?:[0][0-5]|[6|8]))(?:\d{11,12}))$";
        public const string CREDIT_CARD_DINERS_CLUB = @"^(?:(?:[3](?:[0][0-5]|[6|8]))(?:\d{11,12}))$";
        public const string CREDIT_CARD_DISCOVER = @"^(?:(?:6011)(?:\d{12}))$";
        public const string CREDIT_CARD_EN_ROUTE = @"^(?:(?:[2](?:014|149))(?:\d{11}))$";
        public const string CREDIT_CARD_JCB = @"^(?:(?:(?:2131|1800)(?:\d{11}))$|^(?:(?:3)(?:\d{15})))$";
        public const string CREDIT_CARD_MASTER_CARD = @"^(?:(?:[5][1-5])(?:\d{14}))$";
        public const string CREDIT_CARD_STRIP_NON_NUMERIC = @"(\-|\s|\D)*";
        public const string CREDIT_CARD_VISA = @"^(?:(?:[4])(?:\d{12}|\d{15}))$";
        public const string EMAIL = @"^([0-9a-zA-Z]+[-._+&])*[0-9a-zA-Z]+@([-0-9a-zA-Z]+[.])+[a-zA-Z]{2,6}$";
        public const string EMBEDDED_CLASS_NAME_MATCH = "(?<=^_).*?(?=_)";
        public const string EMBEDDED_CLASS_NAME_REPLACE = "^_.*?_";
        public const string EMBEDDED_CLASS_NAME_UNDERSCORE_MATCH = "(?<=^UNDERSCORE).*?(?=UNDERSCORE)";
        public const string EMBEDDED_CLASS_NAME_UNDERSCORE_REPLACE = "^UNDERSCORE.*?UNDERSCORE";
        public const string GUID = "[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}";
        public const string IP_ADDRESS = @"^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$";
        public const string LOWER_CASE = @"^[a-z]+$";
        public const string NUMERIC = "[^0-9]";
        public const string SOCIAL_SECURITY = @"^\d{3}[-]?\d{2}[-]?\d{4}$";
        public const string SQL_EQUAL = @"\=";
        public const string SQL_GREATER = @"\>";
        public const string SQL_GREATER_OR_EQUAL = @"\>.*\=";
        public const string SQL_IS = @"\x20is\x20";
        public const string SQL_IS_NOT = @"\x20is\x20not\x20";
        public const string SQL_LESS = @"\<";
        public const string SQL_LESS_OR_EQUAL = @"\<.*\=";
        public const string SQL_LIKE = @"\x20like\x20";
        public const string SQL_NOT_EQUAL = @"\<.*\>";
        public const string SQL_NOT_LIKE = @"\x20not\x20like\x20";

        public const string STRONG_PASSWORD =
            @"(?=^.{8,255}$)((?=.*\d)(?=.*[A-Z])(?=.*[a-z])|(?=.*\d)(?=.*[^A-Za-z0-9])(?=.*[a-z])|(?=.*[^A-Za-z0-9])(?=.*[A-Z])(?=.*[a-z])|(?=.*\d)(?=.*[A-Z])(?=.*[^A-Za-z0-9]))^.*";

        public const string UPPER_CASE = @"^[A-Z]+$";
        public const string US_CURRENCY = @"^\$(([1-9]\d*|([1-9]\d{0,2}(\,\d{3})*))(\.\d{1,2})?|(\.\d{1,2}))$|^\$[0](.00)?$";
        public const string US_TELEPHONE = @"^[01]?[- .]?(\([2-9]\d{2}\)|[2-9]\d{2})[- .]?\d{3}[- .]?\d{4}$";
        public const string US_ZIPCODE = @"^\d{5}$";
        public const string US_ZIPCODE_PLUS_FOUR = @"^\d{5}((-|\s)?\d{4})$";
        public const string US_ZIPCODE_PLUS_FOUR_OPTIONAL = @"^\d{5}((-|\s)?\d{4})?$";
    }
}
