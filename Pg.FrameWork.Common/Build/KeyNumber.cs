using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Pg.FrameWork.Common.Build
{
    /// <summary>
    /// 主键数字生成
    /// </summary>
    public class KeyNumber
    {
        /// <summary>
        /// 生成业务Id
        /// </summary>
        /// <returns></returns>
        public static string BuildBusinessId()
        {
            return BulidNumber(ServiceCode.Business);
        }

        /// <summary>
        /// 生成号码
        /// </summary>
        /// <param name="serviceCode">服务code</param>
        /// <returns></returns>
        private static string BulidNumber(string serviceCode)
        {
            string arg = DateTime.Now.ToString("yyMMddHHmmssfff");
            Random random = new Random(CreateRandomSeed());
            int num = random.Next(10000, 99999);
            return string.Format("{0}{1}{2}", arg, serviceCode, num);
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
