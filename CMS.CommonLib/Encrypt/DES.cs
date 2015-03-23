using System;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace CMS.CommonLib.Encrypt
{
    public class DES
    {
        private static DESCryptoServiceProvider desCSP = new DESCryptoServiceProvider();
        
        /// <summary>
        /// 构造函数，使用系统默认的密钥和初始化向量
        /// </summary>
        public DES()
        {
        }

        //初始化向量，用于模拟前一个块。       
        private static string strIV = "12345678";
        public static string IVS
        {
            get { return strIV; }
            set
            {
                if (value.Length > 8)
                {
                    strIV = value.Substring(0, 8);
                }
                else
                {
                    if (value.Length < 8)
                    {
                        string newiv = value + "00000000";
                        strIV = newiv.Substring(0, 8);
                    }
                    else
                    {
                        strIV = value;
                    }
                }

            }
        }

        //用于加密解密的密钥
        //初始化密码，如果不指定密码，将采用该密码，密码长度必须是8位，否则将自动截取至前8位。
        private static string strKey = "password";
        public static string Keys
        {
            get { return strKey; }
            set
            {
                if (value.Length > 8)
                {
                    strKey = value.Substring(0, 8);
                }
                else
                {
                    if (value.Length < 8)
                    {
                        string newkey = value + "password";
                        strKey = newkey.Substring(0, 8);
                    }
                    else
                    {
                        strKey = value;
                    }
                }

            }
        }
        
        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="input">需要加密的字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string Encrypt(string input)
        {            
            byte[] Key = Encoding.UTF8.GetBytes(strKey);
            byte[] IV = Encoding.UTF8.GetBytes(strIV);     
            byte[] byt = Encoding.UTF8.GetBytes(input); //根据 UTF-8 编码对字符串处理，转换成 byte 数组
            ICryptoTransform ct = desCSP.CreateEncryptor(Key, IV);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
            cs.Write(byt, 0, byt.Length);
            cs.FlushFinalBlock();
            cs.Close();
            return Convert.ToBase64String(ms.ToArray()); // 将加密的 byte 数组依照 Base64 编码转换成字符串
        }

        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="input">需要解密的字符串</param>
        /// <returns>解密后的字符串</returns>
        public static string Decrypt(string input)
        {
            byte[] Key = Encoding.UTF8.GetBytes(strKey);
            byte[] IV = Encoding.UTF8.GetBytes(strIV);
            byte[] byt = Convert.FromBase64String(input); // 将 密文 以 Base64 编码转换成 byte 数组
            ICryptoTransform ct = desCSP.CreateDecryptor(Key, IV);//注意此处区别。
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
            cs.Write(byt, 0, byt.Length);
            cs.FlushFinalBlock();
            cs.Close();
            return Encoding.UTF8.GetString(ms.ToArray()); // 将 明文 以 UTF8 编码转换成字符串
        }

        /// <summary>
        /// 加密文件
        /// </summary>
        /// <param name="inputFile">需要加密的文件名</param>
        /// <param name="outputFile">加密获得的文件名</param>   
        public void EncryptFile(string inputFile, string outputFile)
        {
            byte[] Key = Encoding.UTF8.GetBytes(strKey);
            byte[] IV = Encoding.UTF8.GetBytes(strIV);

            FileStream fin = new FileStream(inputFile, FileMode.Open, FileAccess.Read);
            FileStream fout = new FileStream(outputFile, FileMode.OpenOrCreate, FileAccess.Write);
            fout.SetLength(0);

            ICryptoTransform ct = desCSP.CreateEncryptor(Key, IV);
            CryptoStream cs = new CryptoStream(fout, ct, CryptoStreamMode.Write);

            byte[] bin = new byte[100];
            long rdlen = 0;
            long totlen = fin.Length;
            int len; 
            while (rdlen < totlen)
            {
                len = fin.Read(bin, 0, 100);
                cs.Write(bin, 0, len);
                rdlen = rdlen + len;                
            }
            cs.Close();
            fout.Close();
            fin.Close();           
           
        }

        /// <summary>
        /// 解密文件
        /// </summary>
        /// <param name="inputFile">需要加密的文件名</param>
        /// <param name="outputFile">加密获得的文件名</param>        
        public void Decrypt(string inputFile, string outputFile)
        {
            byte[] Key = Encoding.UTF8.GetBytes(strKey);
            byte[] IV = Encoding.UTF8.GetBytes(strIV);

            FileStream fin = new FileStream(inputFile, FileMode.Open, FileAccess.Read);
            FileStream fout = new FileStream(outputFile, FileMode.OpenOrCreate, FileAccess.Write);
            fout.SetLength(0);

            ICryptoTransform ct = desCSP.CreateDecryptor(Key, IV);//注意此处区别。
            CryptoStream cs = new CryptoStream(fout, ct, CryptoStreamMode.Write);

            byte[] bin = new byte[100];
            long rdlen = 0;
            long totlen = fin.Length;
            int len;
            while (rdlen < totlen)
            {
                len = fin.Read(bin, 0, 100);
                cs.Write(bin, 0, len);
                rdlen = rdlen + len;
            }
            cs.Close();
            fout.Close();
            fin.Close();   
        }
        
    }
}
