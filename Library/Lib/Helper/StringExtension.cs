using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Athena.Unitop.Sure.Lib
{
    public static class StringExtension
    {
        /// <summary>
        /// 获得字符串内相关字符的所有索引
        /// </summary>
        /// <param name="text"></param>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public static List<int> IndexOfAll(this string text, string symbol)
        {
            List<int> listIndex = new List<int>();
            int index = 0;
            while (true)
            {
                index = text.IndexOf(symbol, index);
                if (index > -1)
                {
                    listIndex.Add(index);
                    index++;
                }
                else
                {
                    break;
                }
            }
            listIndex.Sort();
            return listIndex;
        }

        /// <summary>
        /// 获得字符串内相关字符的所有索引
        /// </summary>
        /// <param name="text"></param>
        /// <param name="symbols"></param>
        /// <returns></returns>
        public static List<int> IndexOfAll(this string text, string[] symbols)
        {
            List<int> listIndex = new List<int>();
            foreach (var symbol in symbols)
            {
                listIndex.AddRange(text.IndexOfAll(symbol));
            }
            listIndex.Sort();
            return listIndex;
        }
    }
}
