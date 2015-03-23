using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace CMS.CommonLib.Utils
{
    public class FileHandler
    {
        private const string encoder = "utf-8";

        /// <summary>
        /// ����������ļ���·���Ƿ���ڣ��粻���ڣ��������ļ��С�
        /// </summary>
        /// <param name="filepath"></param>
        public static bool CheckDirectory(string filepath)
        {
            bool flag = true;
            if (!Directory.Exists(filepath))
            {
                flag = false;
                Directory.CreateDirectory(filepath);
            }
            return flag;
        }

        public static void Write(string str, string fileName)
        {
            StreamWriter sw = new StreamWriter(fileName, false, System.Text.Encoding.GetEncoding(encoder));
            sw.Write(str);
            sw.Flush();
            sw.Close();
        }

        public static void Write(string str, string fileName,Encoding encode)
        {
            FileStream fs = File.Open(fileName, FileMode.Create, FileAccess.Write, FileShare.Read);
            StreamWriter writer = new StreamWriter(fs,encode);
            writer.Write(str);
            writer.Close();
            fs.Close();
        }


        public static void Append(string str, string fileName)
        {
            StreamWriter sw = new StreamWriter(fileName, true, System.Text.Encoding.GetEncoding(encoder));
            sw.Write(str);
            sw.Flush();
            sw.Close();
        }

        public static string Read(string fileName)
        {
            string str = "";
            if (File.Exists(fileName))
            {
                StreamReader sd = new StreamReader(fileName, System.Text.Encoding.GetEncoding(encoder));
                str = sd.ReadToEnd();
                sd.Close();
            }
            return str;
        }
    }
}
