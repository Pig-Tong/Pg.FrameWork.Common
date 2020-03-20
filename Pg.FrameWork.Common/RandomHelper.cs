using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Pg.FrameWork.Common
{
    /// <summary>
    /// 随机数帮助类
    /// </summary>
    public class RandomHelper
    {
        /// <summary>
        /// 获取数字随机数
        /// </summary>
        /// <param name="minNumber">最小数</param>
        /// <param name="maxNumber">最大数</param>
        /// <returns></returns>
        public static int GetNumberRandom(int minNumber, int maxNumber)
        {
            Random random = new Random(CreateRandomSeed());
            return random.Next(minNumber, maxNumber);
        }

        /// <summary>
        /// 创建随机种子
        /// </summary>
        /// <returns></returns>
        private static int CreateRandomSeed()
        {
            byte[] array = new byte[4];
            RNGCryptoServiceProvider rNgCryptoServiceProvider = new RNGCryptoServiceProvider();
            rNgCryptoServiceProvider.GetBytes(array);
            return BitConverter.ToInt32(array, 0);
        }

        ///<summary>
        ///生成随机字符串 
        ///</summary>
        ///<param name="length">目标字符串的长度</param>
        ///<param name="useNum">是否包含数字，1=包含，默认为包含</param>
        ///<param name="useLow">是否包含小写字母，1=包含，默认为不包含</param>
        ///<param name="useUpp">是否包含大写字母，1=包含，默认为不包含</param>
        ///<param name="useSpe">是否包含特殊字符，1=包含，默认为不包含</param>
        ///<param name="custom">要包含的自定义字符，直接输入要包含的字符列表</param>
        ///<returns>指定长度的随机字符串</returns>
        public static string GetRandomString(int length, bool useNum = true, bool useLow = false, bool useUpp = false, bool useSpe = false, string custom = "")
        {
            Random r = new Random(CreateRandomSeed());
            string s = null, str = custom;
            if (useNum) { str += "0123456789"; }
            if (useLow) { str += "abcdefghijklmnopqrstuvwxyz"; }
            if (useUpp) { str += "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; }
            if (useSpe) { str += "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~"; }
            for (int i = 0; i < length; i++)
            {
                s += str.Substring(r.Next(0, str.Length - 1), 1);
            }
            return s;
        }
    }
}
