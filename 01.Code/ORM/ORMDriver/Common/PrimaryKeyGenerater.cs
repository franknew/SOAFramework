using System;
using System.Collections.Generic;
using System.Text;

namespace SOAFramework.ORM.Common
{
    public class PrimaryKeyGenerater
    {
        private static int _intCount = 0;
        public static string GeneratePrimaryKey(PKGenerationType GenerationType)
        {
            string strPrimaryKeyValue = null;
            switch (GenerationType)
            {
                case PKGenerationType.Date:
                    strPrimaryKeyValue = DateTime.Now.ToString("yyyyMMdd");
                    break;
                case PKGenerationType.DateTime:
                    strPrimaryKeyValue = DateTime.Now.ToString("yyyyMMddHHmmss");
                    break;
                case PKGenerationType.DateTimeAndNumber:
                    if (_intCount >= 10000)
                    {
                        _intCount = 0;
                    }
                    _intCount++;
                    strPrimaryKeyValue = DateTime.Now.ToString("yyyyMMddHHmmss") + _intCount.ToString();
                    break;
                case PKGenerationType.GUID:
                    strPrimaryKeyValue = Guid.NewGuid().ToString();
                    break;
                case PKGenerationType.Time:
                    strPrimaryKeyValue = DateTime.Now.ToString("HHmmss");
                    break;
                case PKGenerationType.None:
                    break;
                case PKGenerationType.AutoIncrease:
                    break;
                default:
                    break;
            }
            return strPrimaryKeyValue;
        }
    }

    public enum PKGenerationType
    {
        DateTime,
        Date,
        Time,
        DateTimeAndNumber,
        GUID,
        AutoIncrease,
        None
    }
}
