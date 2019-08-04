using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pg.FrameWork.Common
{
    /// <summary>
    /// 文本帮助类
    /// </summary>
    public class TextHelper
    {
        /// <summary>
        /// 比较三个值中的最小值
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <param name="third"></param>
        /// <returns></returns>
        private static int LowerOfThree(int first, int second, int third)
        {
            int num = first;
            if (second < num)
            {
                num = second;
            }
            if (third < num)
            {
                num = third;
            }
            return num;
        }

        /// <summary>
        /// 计算两个字符串
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <returns></returns>
        private static int Distance(string str1, string str2)
        {
            int length = str1.Length;
            int length2 = str2.Length;
            if (length == 0)
            {
                return length2;
            }
            if (length2 == 0)
            {
                return length;
            }
            int[,] array = new int[length + 1, length2 + 1];
            for (int i = 0; i <= length; i++)
            {
                array[i, 0] = i;
            }
            for (int j = 0; j <= length2; j++)
            {
                array[0, j] = j;
            }
            for (int i = 1; i <= length; i++)
            {
                char c = str1[i - 1];
                for (int j = 1; j <= length2; j++)
                {
                    char obj = str2[j - 1];
                    int num = (!c.Equals(obj)) ? 1 : 0;
                    array[i, j] = LowerOfThree(array[i - 1, j] + 1, array[i, j - 1] + 1, array[i - 1, j - 1] + num);
                }
            }
            return array[length, length2];
        }

        /// <summary>
        /// 比较两个字符串
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <returns></returns>
        public static decimal Compare(string str1, string str2)
        {
            int value = (str1.Length > str2.Length) ? str1.Length : str2.Length;
            int value2 = Distance(str1, str2);
            decimal d = 1m - (decimal)value2 / (decimal)value;
            return Math.Round(d, 2);
        }
    }
}
