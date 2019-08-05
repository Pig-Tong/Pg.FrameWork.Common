using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.GZip;

namespace Pg.FrameWork.Common.Compression
{
    /// <summary>
    /// GZip解压缩
    /// </summary>
    public class GZipDecompress
    {
        public static bool GZipFile(string sourceFileName, string zipFileName)
        {
            //IL_0018: Unknown result type (might be due to invalid IL or missing references)
            //IL_001e: Expected O, but got Unknown
            if (!File.Exists(sourceFileName))
            {
                return false;
            }
            FileStream fileStream = File.OpenRead(sourceFileName);
            GZipOutputStream val = new GZipOutputStream((Stream)File.Open(zipFileName, FileMode.Create));
            bool result;
            try
            {
                byte[] array = new byte[fileStream.Length];
                fileStream.Read(array, 0, (int)fileStream.Length);
                ((Stream)val).Write(array, 0, array.Length);
                result = true;
            }
            catch
            {
                result = false;
            }
            fileStream.Close();
            ((Stream)val).Close();
            return result;
        }

        public static bool UnGzipFile(string zipFileName, string unzipFileName)
        {
            //IL_0010: Unknown result type (might be due to invalid IL or missing references)
            //IL_0016: Expected O, but got Unknown
            if (!File.Exists(zipFileName))
            {
                return false;
            }
            GZipInputStream val = new GZipInputStream((Stream)File.OpenRead(zipFileName));
            FileStream fileStream = File.Open(unzipFileName, FileMode.Create);
            bool result;
            try
            {
                int num = 2048;
                byte[] buffer = new byte[num];
                while (num > 0)
                {
                    num = ((Stream)val).Read(buffer, 0, num);
                    fileStream.Write(buffer, 0, num);
                }
                result = true;
            }
            catch
            {
                result = false;
            }
            fileStream.Close();
            ((Stream)val).Close();
            return result;
        }

        public static string CompressToBase64(string rawData)
        {
            return Convert.ToBase64String(Compress(rawData));
        }

        public static byte[] Compress(string rawData)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(rawData);
            return Compress(bytes);
        }

        public static byte[] Compress(byte[] rawData)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true);
                gZipStream.Write(rawData, 0, rawData.Length);
                gZipStream.Close();
                return memoryStream.ToArray();
            }
        }

        public static string DecompressFromBase64(string result)
        {
            byte[] result2 = Convert.FromBase64String(result);
            return Decompress(result2);
        }

        public static string Decompress(byte[] result)
        {
            using (MemoryStream stream = new MemoryStream(result))
            {
                GZipStream stream2 = new GZipStream(stream, CompressionMode.Decompress);
                StreamReader streamReader = new StreamReader(stream2);
                string result2 = streamReader.ReadToEnd();
                streamReader.Close();
                return result2;
            }
        }

        public static byte[] StreamToBytes(Stream stream)
        {
            byte[] array = new byte[stream.Length];
            stream.Read(array, 0, array.Length);
            stream.Seek(0L, SeekOrigin.Begin);
            return array;
        }
    }

}
