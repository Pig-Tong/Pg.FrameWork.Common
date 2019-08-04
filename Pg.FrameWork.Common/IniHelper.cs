using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pg.FrameWork.Common
{
    public class IniHelper
    {
        public static void Write(string path, string iniName, string context)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string path2 = path + "\\" + iniName;
                if (!File.Exists(path2))
                {
                    FileStream fileStream = File.Create(path2);
                    fileStream.Close();
                }
                FileStream fileStream2 = new FileStream(path2, FileMode.Create, FileAccess.Write);
                StreamWriter streamWriter = new StreamWriter(fileStream2, Encoding.Default);
                streamWriter.Write(context);
                streamWriter.Flush();
                streamWriter.Close();
                fileStream2.Close();
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public static T Read<T>(string path)
        {
            if (File.Exists(path))
            {
                try
                {
                    FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
                    StreamReader streamReader = new StreamReader(fileStream, Encoding.Default);
                    string value = streamReader.ReadToEnd();
                    streamReader.Close();
                    fileStream.Close();
                    return (T)Convert.ChangeType(value, typeof(T));
                }
                catch
                {
                    throw;
                }
            }
            return default(T);
        }
    }

}
