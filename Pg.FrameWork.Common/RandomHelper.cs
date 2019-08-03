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
    }
}
