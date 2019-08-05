using System;
using System.IO;
using ICSharpCode.SharpZipLib.Checksum;
using ICSharpCode.SharpZipLib.Zip;

namespace Pg.FrameWork.Common.Compression
{
    /// <summary>
    /// Zip解压缩
    /// </summary>
    public class ZipDecompress
    {
        /// <summary>
        /// 压缩字典
        /// </summary>
        /// <param name="folderToZip"></param>
        /// <param name="zipStream"></param>
        /// <param name="parentFolderName"></param>
        /// <returns></returns>
        private static bool ZipDirectory(string folderToZip, ZipOutputStream zipStream, string parentFolderName)
        {
            //IL_0007: Unknown result type (might be due to invalid IL or missing references)
            //IL_000e: Expected O, but got Unknown
            //IL_0024: Unknown result type (might be due to invalid IL or missing references)
            //IL_002a: Expected O, but got Unknown
            //IL_0094: Unknown result type (might be due to invalid IL or missing references)
            //IL_009a: Expected O, but got Unknown
            bool result = true;
            FileStream fileStream = null;
            Crc32 val = new Crc32();
            try
            {
                ZipEntry val2 = new ZipEntry(Path.Combine(parentFolderName, Path.GetFileName(folderToZip) + "/"));
                zipStream.PutNextEntry(val2);
                ((Stream)zipStream).Flush();
                string[] files = Directory.GetFiles(folderToZip);
                string[] array = files;
                foreach (string path in array)
                {
                    fileStream = File.OpenRead(path);
                    byte[] array2 = new byte[fileStream.Length];
                    fileStream.Read(array2, 0, array2.Length);
                    val2 = new ZipEntry(Path.Combine(parentFolderName, Path.GetFileName(folderToZip) + "/" + Path.GetFileName(path)))
                    {
                        DateTime = DateTime.Now,
                        Size = fileStream.Length,
                        Crc = val.Value
                    };
                    fileStream.Close();
                    val.Reset();
                    val.Update(array2);
                    zipStream.PutNextEntry(val2);
                    ((Stream)zipStream).Write(array2, 0, array2.Length);
                }
            }
            catch
            {
                result = false;
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Close();
                    fileStream.Dispose();
                }
                GC.Collect();
                GC.Collect(1);
            }
            string[] directories = Directory.GetDirectories(folderToZip);
            string[] array3 = directories;
            foreach (string folderToZip2 in array3)
            {
                if (!ZipDirectory(folderToZip2, zipStream, folderToZip))
                {
                    return false;
                }
            }
            return result;
        }

        /// <summary>
        /// 压缩字典
        /// </summary>
        /// <param name="folderToZip"></param>
        /// <param name="zipedFile"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool ZipDirectory(string folderToZip, string zipedFile, string password)
        {
            //IL_0012: Unknown result type (might be due to invalid IL or missing references)
            //IL_0018: Expected O, but got Unknown
            bool result = false;
            if (!Directory.Exists(folderToZip))
            {
                return result;
            }
            ZipOutputStream val = new ZipOutputStream((Stream)File.Create(zipedFile));
            val.SetLevel(6);
            if (!string.IsNullOrEmpty(password))
            {
                val.Password = password;
            }
            result = ZipDirectory(folderToZip, val, "");
            val.Finish();
            ((Stream)val).Close();
            return result;
        }

        /// <summary>
        /// 压缩字典
        /// </summary>
        /// <param name="folderToZip"></param>
        /// <param name="zipedFile"></param>
        /// <returns></returns>
        public static bool ZipDirectory(string folderToZip, string zipedFile)
        {
            return ZipDirectory(folderToZip, zipedFile, null);
        }

        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="fileToZip"></param>
        /// <param name="zipedFile"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool ZipFile(string fileToZip, string zipedFile, string password)
        {
            //IL_0043: Unknown result type (might be due to invalid IL or missing references)
            //IL_0049: Expected O, but got Unknown
            //IL_005e: Unknown result type (might be due to invalid IL or missing references)
            //IL_0064: Expected O, but got Unknown
            bool result = true;
            ZipOutputStream val = null;
            FileStream fileStream = null;
            if (!File.Exists(fileToZip))
            {
                return false;
            }
            try
            {
                fileStream = File.OpenRead(fileToZip);
                byte[] array = new byte[fileStream.Length];
                fileStream.Read(array, 0, array.Length);
                fileStream.Close();
                fileStream = File.Create(zipedFile);
                val = new ZipOutputStream((Stream)fileStream);
                if (!string.IsNullOrEmpty(password))
                {
                    val.Password = password;
                }
                ZipEntry val2 = new ZipEntry(Path.GetFileName(fileToZip));
                val.PutNextEntry(val2);
                val.SetLevel(6);
                ((Stream)val).Write(array, 0, array.Length);
            }
            catch
            {
                result = false;
            }
            finally
            {
                if (val != null)
                {
                    val.Finish();
                    ((Stream)val).Close();
                }
                if (fileStream != null)
                {
                    fileStream.Close();
                    fileStream.Dispose();
                }
            }
            GC.Collect();
            GC.Collect(1);
            return result;
        }

        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="fileToZip"></param>
        /// <param name="zipedFile"></param>
        /// <returns></returns>
        public static bool ZipFile(string fileToZip, string zipedFile)
        {
            return ZipFile(fileToZip, zipedFile, null);
        }

        /// <summary>
        /// 压缩
        /// </summary>
        /// <param name="fileToZip"></param>
        /// <param name="zipedFile"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool Zip(string fileToZip, string zipedFile, string password)
        {
            bool result = false;
            if (Directory.Exists(fileToZip))
            {
                result = ZipDirectory(fileToZip, zipedFile, password);
            }
            else if (File.Exists(fileToZip))
            {
                result = ZipFile(fileToZip, zipedFile, password);
            }
            return result;
        }

        /// <summary>
        /// 压缩
        /// </summary>
        /// <param name="fileToZip"></param>
        /// <param name="zipedFile"></param>
        /// <returns></returns>
        public static bool Zip(string fileToZip, string zipedFile)
        {
            return Zip(fileToZip, zipedFile, null);
        }

        /// <summary>
        /// 解压缩
        /// </summary>
        /// <param name="fileToUnZip"></param>
        /// <param name="zipedFolder"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool UnZip(string fileToUnZip, string zipedFolder, string password)
        {
            //IL_0027: Unknown result type (might be due to invalid IL or missing references)
            //IL_002d: Expected O, but got Unknown
            bool result = true;
            FileStream fileStream = null;
            ZipInputStream val = null;
            if (!File.Exists(fileToUnZip))
            {
                return false;
            }
            if (!Directory.Exists(zipedFolder))
            {
                Directory.CreateDirectory(zipedFolder);
            }
            try
            {
                val = new ZipInputStream((Stream)File.OpenRead(fileToUnZip));
                if (!string.IsNullOrEmpty(password))
                {
                    val.Password = password;
                }
                ZipEntry nextEntry;
                while ((nextEntry = val.GetNextEntry()) != null)
                {
                    if (!string.IsNullOrEmpty(nextEntry.Name))
                    {
                        string text = Path.Combine(zipedFolder, nextEntry.Name);
                        text = text.Replace('/', '\\');
                        if (text.EndsWith("\\"))
                        {
                            Directory.CreateDirectory(text);
                        }
                        else
                        {
                            fileStream = File.Create(text);
                            int num = 2048;
                            byte[] array = new byte[num];
                            while (true)
                            {
                                num = fileStream.Read(array, 0, array.Length);
                                if (num <= 0)
                                {
                                    break;
                                }
                                fileStream.Write(array, 0, array.Length);
                            }
                        }
                    }
                }
                return result;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Close();
                    fileStream.Dispose();
                }
                if (val != null)
                {
                    ((Stream)val).Close();
                    ((Stream)val).Dispose();
                }
                GC.Collect();
                GC.Collect(1);
            }
        }

        /// <summary>
        /// 解压缩
        /// </summary>
        /// <param name="fileToUnZip"></param>
        /// <param name="zipedFolder"></param>
        /// <returns></returns>
        public static bool UnZip(string fileToUnZip, string zipedFolder)
        {
            return UnZip(fileToUnZip, zipedFolder, null);
        }
    }
}
