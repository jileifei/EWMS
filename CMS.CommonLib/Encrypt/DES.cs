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
        /// ���캯����ʹ��ϵͳĬ�ϵ���Կ�ͳ�ʼ������
        /// </summary>
        public DES()
        {
        }

        //��ʼ������������ģ��ǰһ���顣       
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

        //���ڼ��ܽ��ܵ���Կ
        //��ʼ�����룬�����ָ�����룬�����ø����룬���볤�ȱ�����8λ�������Զ���ȡ��ǰ8λ��
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
        /// �����ַ���
        /// </summary>
        /// <param name="input">��Ҫ���ܵ��ַ���</param>
        /// <returns>���ܺ���ַ���</returns>
        public static string Encrypt(string input)
        {            
            byte[] Key = Encoding.UTF8.GetBytes(strKey);
            byte[] IV = Encoding.UTF8.GetBytes(strIV);     
            byte[] byt = Encoding.UTF8.GetBytes(input); //���� UTF-8 ������ַ�������ת���� byte ����
            ICryptoTransform ct = desCSP.CreateEncryptor(Key, IV);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
            cs.Write(byt, 0, byt.Length);
            cs.FlushFinalBlock();
            cs.Close();
            return Convert.ToBase64String(ms.ToArray()); // �����ܵ� byte �������� Base64 ����ת�����ַ���
        }

        /// <summary>
        /// �����ַ���
        /// </summary>
        /// <param name="input">��Ҫ���ܵ��ַ���</param>
        /// <returns>���ܺ���ַ���</returns>
        public static string Decrypt(string input)
        {
            byte[] Key = Encoding.UTF8.GetBytes(strKey);
            byte[] IV = Encoding.UTF8.GetBytes(strIV);
            byte[] byt = Convert.FromBase64String(input); // �� ���� �� Base64 ����ת���� byte ����
            ICryptoTransform ct = desCSP.CreateDecryptor(Key, IV);//ע��˴�����
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
            cs.Write(byt, 0, byt.Length);
            cs.FlushFinalBlock();
            cs.Close();
            return Encoding.UTF8.GetString(ms.ToArray()); // �� ���� �� UTF8 ����ת�����ַ���
        }

        /// <summary>
        /// �����ļ�
        /// </summary>
        /// <param name="inputFile">��Ҫ���ܵ��ļ���</param>
        /// <param name="outputFile">���ܻ�õ��ļ���</param>   
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
        /// �����ļ�
        /// </summary>
        /// <param name="inputFile">��Ҫ���ܵ��ļ���</param>
        /// <param name="outputFile">���ܻ�õ��ļ���</param>        
        public void Decrypt(string inputFile, string outputFile)
        {
            byte[] Key = Encoding.UTF8.GetBytes(strKey);
            byte[] IV = Encoding.UTF8.GetBytes(strIV);

            FileStream fin = new FileStream(inputFile, FileMode.Open, FileAccess.Read);
            FileStream fout = new FileStream(outputFile, FileMode.OpenOrCreate, FileAccess.Write);
            fout.SetLength(0);

            ICryptoTransform ct = desCSP.CreateDecryptor(Key, IV);//ע��˴�����
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
