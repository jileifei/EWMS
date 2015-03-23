using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Net;
using System.Net.Mail;

namespace CMS.CommonLib.Utils
{
	/// <summary>
	/// ������
	/// </summary>
	public partial class Utils
	{
        /// <summary>
        /// �������ջ�ȡ����
        /// </summary>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        public static string GetConstellationByBirthDay(int month, int day)
        {
            string strconstellation = "";
            string[] constellation = {"ˮƿ��", "˫����", "������", "��ţ��", "˫����", "��з��", "ʨ����", "��Ů��", "�����", "��Ы��", "������", "Ħ����" };

            if (day <= 22)
            {
                if (1 != month)
                {
                    strconstellation = constellation[month - 2];
                }
                else
                {
                    strconstellation = constellation[11];
                }
            }
            else
            {
                strconstellation = constellation[month - 1];
            }

            return strconstellation;
        }

		/// <summary>
		/// �����ַ�����ʵ����, 1�����ֳ���Ϊ2
		/// </summary>
		/// <returns></returns>
		public static int GetStringLength(string str)
		{
			return Encoding.Default.GetBytes(str).Length;
		}
		
		public static bool IsCompriseStr(string str, string stringarray, string strsplit)
		{
			str = str.ToLower();
			string[] stringArray = Utils.SplitString(stringarray, strsplit);
			for (int i = 0; i < stringArray.Length; i++)
			{
				if(stringArray[i].ToLower().IndexOf(str) != -1)
				{
					return true;
				}
			}
			return false;
		}

        # region ��ȡ�ļ�����

        /// <summary>
        /// ��ȡ�ļ�����
        /// </summary>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static Encoding GetEncodingByEncode(string encode)
        {
            Encoding encoding;
            switch (encode.ToLower())
            {
                case "gb2312":
                    encoding = Encoding.Default;
                    break;
                case "utf-8":
                    encoding = new UTF8Encoding(false);
                    break;
                default:
                    encoding = Encoding.Default;
                    break;
            }
            return encoding;
        }

        # endregion

		/// <summary>
		/// �ж�ָ���ַ�����ָ���ַ��������е�λ��
		/// </summary>
		/// <param name="strSearch">�ַ���</param>
		/// <param name="stringArray">�ַ�������</param>
		/// <param name="caseInsensetive">�Ƿ����ִ�Сд, trueΪ������, falseΪ����</param>
		/// <returns>�ַ�����ָ���ַ��������е�λ��, �粻�����򷵻�-1</returns>
		public static int GetInArrayID(string strSearch, string[] stringArray, bool caseInsensetive)
		{
			for (int i = 0; i < stringArray.Length; i++)
			{
				if (caseInsensetive)
				{
					if(strSearch.ToLower() == stringArray[i].ToLower())
					{
						return i;
					}
				}
				else
				{
					if(strSearch == stringArray[i])
					{
						return i;
					}
				}				
				
			}
			return -1;
		}

		
		/// <summary>
		/// �ж�ָ���ַ�����ָ���ַ��������е�λ��
		/// </summary>
		/// <param name="strSearch">�ַ���</param>
		/// <param name="stringArray">�ַ�������</param>
		/// <returns>�ַ�����ָ���ַ��������е�λ��, �粻�����򷵻�-1</returns>		
		public static int GetInArrayID(string strSearch, string[] stringArray)
		{
			return GetInArrayID(strSearch, stringArray, true);
		}
		
		/// <summary>
		/// �ж�ָ���ַ����Ƿ�����ָ���ַ��������е�һ��Ԫ��
		/// </summary>
		/// <param name="strSearch">�ַ���</param>
		/// <param name="stringArray">�ַ�������</param>
		/// <param name="caseInsensetive">�Ƿ����ִ�Сд, trueΪ������, falseΪ����</param>
		/// <returns>�жϽ��</returns>
		public static bool InArray(string strSearch, string[] stringArray, bool caseInsensetive)
		{
			return GetInArrayID(strSearch, stringArray, caseInsensetive) >= 0;
		}

		/// <summary>
		/// �ж�ָ���ַ����Ƿ�����ָ���ַ��������е�һ��Ԫ��
		/// </summary>
		/// <param name="str">�ַ���</param>
		/// <param name="stringarray">�ַ�������</param>
		/// <returns>�жϽ��</returns>
		public static bool InArray(string str, string[] stringarray)
		{
			return InArray(str, stringarray, false);
		}

		/// <summary>
		/// �ж�ָ���ַ����Ƿ�����ָ���ַ��������е�һ��Ԫ��
		/// </summary>
		/// <param name="str">�ַ���</param>
		/// <param name="stringarray">�ڲ��Զ��ŷָ�ʵ��ַ���</param>
		/// <returns>�жϽ��</returns>
		public static bool InArray(string str, string stringarray)
		{
			return InArray(str, SplitString(stringarray, ","), false);
		}

		/// <summary>
		/// �ж�ָ���ַ����Ƿ�����ָ���ַ��������е�һ��Ԫ��
		/// </summary>
		/// <param name="str">�ַ���</param>
		/// <param name="stringarray">�ڲ��Զ��ŷָ�ʵ��ַ���</param>
		/// <param name="strsplit">�ָ��ַ���</param>
		/// <returns>�жϽ��</returns>
		public static bool InArray(string str, string stringarray, string strsplit)
		{
			return InArray(str, SplitString(stringarray, strsplit), false);
		}
		
		/// <summary>
		/// �ж�ָ���ַ����Ƿ�����ָ���ַ��������е�һ��Ԫ��
		/// </summary>
		/// <param name="str">�ַ���</param>
		/// <param name="stringarray">�ڲ��Զ��ŷָ�ʵ��ַ���</param>
		/// <param name="strsplit">�ָ��ַ���</param>
		/// <param name="caseInsensetive">�Ƿ����ִ�Сд, trueΪ������, falseΪ����</param>
		/// <returns>�жϽ��</returns>
		public static bool InArray(string str, string stringarray, string strsplit, bool caseInsensetive)
		{
			return InArray(str, SplitString(stringarray, strsplit), caseInsensetive);
		}

		/// <summary>
		/// string��ת��Ϊint��
		/// </summary>
		/// <param name="strValue">Ҫת�����ַ���</param>
		/// <param name="defValue">ȱʡֵ</param>
		/// <returns>ת�����int���ͽ��</returns>
		public static int StrToInt(object strValue, int defValue)
		{
			if ((strValue == null)||(strValue.ToString().Length > 9))
			{
				return defValue;
			}
			int intValue = defValue;
			if (strValue != null)
			{
				bool IsInt = new Regex(@"^([-]|[0-9])[0-9]*$").IsMatch(strValue.ToString());
				if (IsInt)
				{
					intValue = Convert.ToInt32(strValue);
				}
			}

			return intValue;
		}

		/// <summary>
		/// string��ת��Ϊfloat��
		/// </summary>
		/// <param name="strValue">Ҫת�����ַ���</param>
		/// <param name="defValue">ȱʡֵ</param>
		/// <returns>ת�����int���ͽ��</returns>
		public static float StrToFloat(object strValue, float defValue)
		{
			if((strValue == null)||(strValue.ToString().Length > 10))
			{
				return defValue;
			}

			float intValue = defValue;
			if (strValue != null)
			{
				bool IsFloat = new Regex(@"^([-]|[0-9])[0-9]*(\.\w*)?$").IsMatch(strValue.ToString());
				if (IsFloat)
				{
					intValue = Convert.ToSingle(strValue);
				}
			}
			return intValue;
		}



		/// <summary>
		/// �жϸ������ַ���(strNumber)�Ƿ�����ֵ��
		/// </summary>
		/// <param name="strNumber">Ҫȷ�ϵ��ַ���</param>
		/// <returns>���򷵼�true �����򷵻� false</returns>
		public static bool IsNumber(string strNumber)
		{
			return new Regex(@"^([0-9])[0-9]*(\.\w*)?$").IsMatch(strNumber);
		}
		

		/// <summary>
		/// �жϸ������ַ�������(strNumber)�е������ǲ��Ƕ�Ϊ��ֵ��
		/// </summary>
		/// <param name="strNumber">Ҫȷ�ϵ��ַ�������</param>
		/// <returns>���򷵼�true �����򷵻� false</returns>
		public static bool IsNumberArray(string[] strNumber)
		{
			if (strNumber == null)
			{
				return false;
			}
			if (strNumber.Length<1)
			{
				return false;
			}
			foreach (string id in strNumber)
			{
				if (!IsNumber(id))
				{
					return false;
				}
			}
			return true;

		}

		
		/// <summary>
		/// ɾ���ַ���β���Ļس�/����/�ո�
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static string RTrim(string str)
		{
			for(int i = str.Length; i >= 0; i--)
			{
				if (str[i].Equals(" ") || str[i].Equals("\r") || str[i].Equals("\n"))
				{
					str.Remove(i, 1);
				}
			}
			return str;
		}


		/// <summary>
		/// ��������ַ����еĻس������з�
		/// </summary>
		/// <param name="str">Ҫ������ַ���</param>
		/// <returns>����󷵻ص��ַ���</returns>
		public static string ClearBR(string str)
		{
			Regex r = null;
			Match m = null;

			r = new Regex(@"(\r\n)",RegexOptions.IgnoreCase);
			for (m = r.Match(str); m.Success; m = m.NextMatch()) 
			{
				str = str.Replace(m.Groups[0].ToString(),"");
			}


			return str;
		}
		/// <summary>
		/// ���ַ�����ָ��λ�ý�ȡָ�����ȵ����ַ���
		/// </summary>
		/// <param name="str">ԭ�ַ���</param>
		/// <param name="startIndex">���ַ�������ʼλ��</param>
		/// <param name="length">���ַ����ĳ���</param>
		/// <returns>���ַ���</returns>
		public static string CutString(string str,int startIndex,int length)
		{
			if (startIndex >= 0)
			{
				if (length < 0)
				{
					length = length * -1;
					if (startIndex - length<0)
					{
						length = startIndex;
						startIndex = 0;
					}
					else
					{
						startIndex = startIndex - length;
					}
				}


				if (startIndex > str.Length)
				{
					return "";
				}


			}
			else
			{
				if (length < 0)
				{
					return "";
				}
				else
				{
					if (length + startIndex > 0)
					{
						length = length + startIndex;
						startIndex = 0;
					}
					else
					{
						return "";
					}
				}
			}

			if (str.Length - startIndex < length)
			{
				length = str.Length - startIndex;
			}

			try
			{
				return str.Substring(startIndex,length);
			}
			catch
			{
				return str;
			}
		}

		/// <summary>
		/// ���ַ�����ָ��λ�ÿ�ʼ��ȡ���ַ�����β���˷���
		/// </summary>
		/// <param name="str">ԭ�ַ���</param>
		/// <param name="startIndex">���ַ�������ʼλ��</param>
		/// <returns>���ַ���</returns>
		public static string CutString(string str,int startIndex)
		{
			return CutString(str,startIndex,str.Length);
		}
		


		/// <summary>
		/// ��õ�ǰ����·��
		/// </summary>
		/// <param name="strPath">ָ����·��</param>
		/// <returns>����·��</returns>
		public static string GetMapPath(string strPath)
		{
			return HttpContext.Current.Server.MapPath(strPath);
		}



		/// <summary>
		/// �����ļ��Ƿ����
		/// </summary>
		/// <param name="filename">�ļ���</param>
		/// <returns>�Ƿ����</returns>
		public static bool FileExists(string filename)
		{
			return File.Exists(filename);
		}



		/// <summary>
		/// ��ָ����ContentType���ָ���ļ��ļ�
		/// </summary>
		/// <param name="filepath">�ļ�·��</param>
		/// <param name="filename">������ļ���</param>
		/// <param name="filetype">���ļ����ʱ���õ�ContentType</param>
		public static void ResponseFile(string filepath, string  filename, string filetype)
		{
			Stream iStream = null;

			// ������Ϊ10k
			byte[] buffer = new Byte[10000];

			// �ļ�����
			int length;

			// ��Ҫ�������ݳ���
			long dataToRead;

			try
			{
				// ���ļ�
				iStream = new FileStream(filepath, FileMode.Open, FileAccess.Read,FileShare.Read);


				// ��Ҫ�������ݳ���
				dataToRead = iStream.Length;

				HttpContext.Current.Response.ContentType = filetype;
				HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + Utils.UrlEncode(filename.Trim()));

				while (dataToRead > 0)
				{
					// ���ͻ����Ƿ񻹴�������״̬
					if (HttpContext.Current.Response.IsClientConnected)
					{
						length = iStream.Read(buffer, 0, 10000);
						HttpContext.Current.Response.OutputStream.Write(buffer, 0, length);
						HttpContext.Current.Response.Flush();
						buffer = new Byte[10000];
						dataToRead = dataToRead - length;
					}
					else
					{
						// �������������������ѭ��
						dataToRead = -1;
					}
				}
			}
			catch (Exception ex)
			{
				HttpContext.Current.Response.Write("Error : " + ex.Message);
			}
			finally
			{
				if (iStream != null)
				{
					// �ر��ļ�
					iStream.Close();
				}
			}
			HttpContext.Current.Response.End();
		}

		/// <summary>
		/// �ж��ļ����Ƿ�Ϊ���������ֱ����ʾ��ͼƬ�ļ���
		/// </summary>
		/// <param name="filename">�ļ���</param>
		/// <returns>�Ƿ����ֱ����ʾ</returns>
		public static bool IsImgFilename(string filename)
		{
			if (filename.EndsWith(".") || filename.IndexOf(".") == -1)
			{
				return false;
			}
			string extname = filename.Substring(filename.LastIndexOf(".") + 1).ToLower();
			return (extname == "jpg" || extname == "jpeg" || extname == "png" || extname == "bmp" || extname == "gif");
		}			
		

		/// <summary>
		/// int��ת��Ϊstring��
		/// </summary>
		/// <returns>ת�����string���ͽ��</returns>
		public static string IntToStr(int intValue)
		{
			//
			return Convert.ToString(intValue);
		}
		/// <summary>
		/// MD5����
		/// </summary>
		/// <param name="str">ԭʼ�ַ���</param>
		/// <returns>MD5���</returns>
		public static string MD5(string str)
		{
			byte[] b = Encoding.Default.GetBytes(str);
			b = new MD5CryptoServiceProvider().ComputeHash(b);
			string ret = "";
			for(int i = 0; i < b.Length; i++)
				ret += b[i].ToString("x").PadLeft(2,'0');
			return ret;
		}

		/// <summary>
		/// SHA256����
		/// </summary>
		/// /// <param name="str">ԭʼ�ַ���</param>
		/// <returns>SHA256���</returns>
		public static string SHA256(string str)
		{
			byte[] SHA256Data = Encoding.UTF8.GetBytes(str);
			SHA256Managed Sha256 = new SHA256Managed();
			byte[] Result = Sha256.ComputeHash(SHA256Data);
			return Convert.ToBase64String(Result);  //���س���Ϊ44�ֽڵ��ַ���
		}


		/// <summary>
		/// �ַ�������ٹ�ָ�������򽫳����Ĳ�����ָ���ַ�������
		/// </summary>
		/// <param name="p_SrcString">Ҫ�����ַ���</param>
		/// <param name="p_Length">ָ������</param>
		/// <param name="p_TailString">�����滻���ַ���</param>
		/// <returns>��ȡ����ַ���</returns>
		public static string GetSubString(string p_SrcString, int p_Length, string p_TailString) 
		{ 
			string myResult = p_SrcString; 

			if (p_Length >= 0) 
			{ 
				byte[] bsSrcString = Encoding.Default.GetBytes(p_SrcString); 

				if (bsSrcString.Length > p_Length) 
				{ 
					int nRealLength = p_Length; 
					int[] anResultFlag = new int[p_Length]; 
					byte[] bsResult = null; 

					int nFlag = 0; 
					for (int i = 0; i < p_Length; i++) 
					{ 

						if (bsSrcString[i] > 127) 
						{ 
							nFlag++; 
							if (nFlag == 3) 
							{ 
								nFlag = 1; 
							} 
						} 
						else 
						{ 
							nFlag = 0; 
						} 

						anResultFlag[i] = nFlag; 
					} 

					if ((bsSrcString[p_Length - 1] > 127) && (anResultFlag[p_Length - 1] == 1)) 
					{ 
						nRealLength = p_Length + 1; 
					} 

					bsResult = new byte[nRealLength]; 

					Array.Copy(bsSrcString, bsResult, nRealLength); 

					myResult = Encoding.Default.GetString(bsResult); 

					myResult = myResult + p_TailString; 
				} 

			} 

			return myResult; 
		} 

		/// <summary>
		/// �Զ�����滻�ַ�������
		/// </summary>
		public static string ReplaceString(string SourceString, string SearchString,string ReplaceString, bool IsCaseInsensetive)
		{
			return Regex.Replace(SourceString, Regex.Escape(SearchString), ReplaceString, (IsCaseInsensetive==true)?RegexOptions.IgnoreCase:RegexOptions.None);
		}

		/// <summary>
		/// ����ָ��������html�ո����
		/// </summary>
		public static string Spaces(int nSpaces)
		{
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < nSpaces; i++)
			{
				sb.Append(" &nbsp;&nbsp;");
			}
			return sb.ToString();
		}

		/// <summary>
		/// ����Ƿ����email��ʽ
		/// </summary>
		/// <param name="strEmail">Ҫ�жϵ�email�ַ���</param>
		/// <returns>�жϽ��</returns>
		public static bool IsValidEmail(string strEmail)
		{
			return Regex.IsMatch(strEmail, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
		}

        /// <summary>
        /// ��ȡEMAIL��ַ������
        /// </summary>
        /// <param name="strEmail"></param>
        /// <returns></returns>
		public static string GetEmailHostName(string strEmail)
		{
			if (strEmail.IndexOf("@") < 0)
			{
				return "";
			}
			return strEmail.Substring(strEmail.LastIndexOf("@")).ToLower();
		}

        /// <summary>
        /// ����Ƿ����Url��ʽ
        /// </summary>
        /// <param name="strEmail">Ҫ�жϵ�Url�ַ���</param>
        /// <returns>�жϽ��</returns>
        public static bool IsValidUrl(string strUrl)
        {
            return Regex.IsMatch(strUrl, @"^(http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?)$");
        }

		/// <summary>
		/// �ж��Ƿ�Ϊbase64�ַ���
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static bool IsBase64String(string str)
		{
			//A-Z, a-z, 0-9, +, /, =
			return Regex.IsMatch(str, @"[A-Za-z0-9\+\/\=]"); 
		}
		/// <summary>
		/// ����Ƿ���SqlΣ���ַ�
		/// </summary>
		/// <param name="str">Ҫ�ж��ַ���</param>
		/// <returns>�жϽ��</returns>
		public static bool IsSafeSqlString(string str)
		{

			return !Regex.IsMatch(str, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']"); 
		}
		/// <summary>
		/// �����ַ���
		/// </summary>
		public static string CleanInput(string strIn)
		{
			return Regex.Replace(strIn.Trim(), @"[^\w\.@-]", ""); 
		}

		/// <summary>
		/// ����URL�н�β���ļ���
		/// </summary>		
		public static string GetFilename(string url)
		{
			if (url == null)
			{
				return "";
			}
			string[] strs1 = url.Split(new char[]{'/'});
			return strs1[strs1.Length - 1].Split(new char[]{'?'})[0];
		}

		/// <summary>
		/// ���ݰ��������ַ����·ݵ�����(�ɸ���Ϊĳ������)
		/// </summary>	
		public static string[] Monthes
		{
			get
			{
				return new string[]{"January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"};
			}
		}

		/// <summary>
		/// �滻�س����з�Ϊhtml���з�
		/// </summary>
		public static string StrFormat(string str)
		{
			string str2;

			if (str == null)
			{
				str2 = "";
			}
			else
			{
				str = str.Replace("\r\n", "<br />");
				str = str.Replace("\n", "<br />");
				str2 = str;
			}
			return str2;
		}

		/// <summary>
		/// ���ر�׼���ڸ�ʽstring
		/// </summary>
		public static string GetDate()
		{
			return DateTime.Now.ToString("yyyy-MM-dd");
		}

        /// <summary>
        /// ���ر�׼���ڸ�ʽint32
        /// </summary>
        public static int GetIntDate()
        {
            return Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd"));
        }

		/// <summary>
		/// ����ָ�����ڸ�ʽ
		/// </summary>
		public static string GetDate(string datetimestr,string replacestr)
		{
			if (datetimestr == null)
			{
				return replacestr;
			}

			if (datetimestr.Equals(""))
			{
				return replacestr;
			}

			try
			{
				datetimestr = Convert.ToDateTime(datetimestr).ToString("yyyy-MM-dd").Replace("1900-01-01",replacestr);
			}
			catch
			{
				return replacestr;
			}
			return datetimestr;
			
		}


		/// <summary>
		/// ���ر�׼ʱ���ʽstring
		/// </summary>
		public static string GetTime()
		{
			return DateTime.Now.ToString("HH:mm:ss");
		}

		/// <summary>
		/// ���ر�׼ʱ���ʽstring
		/// </summary>
		public static string GetDateTime()
		{
			return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		}
		/// <summary>
		/// ���ر�׼ʱ���ʽstring
		/// </summary>
		public static string GetDateTimeF()
		{
			return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fffffff");
		}
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public static bool IsTime(string timeval)
		{
			return Regex.IsMatch(timeval, @"^((([0-1]?[0-9])|(2[0-3])):([0-5]?[0-9])(:[0-5]?[0-9])?)$"); 
		}


        /// <summary>
        /// ��õ�ǰҳ��ͻ��˵�IP
        /// </summary>
        /// <returns>��ǰҳ��ͻ��˵�IP</returns>
        public static string GetRealIP()
        {
            string result = String.Empty;

            result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (null == result || result == String.Empty)
            {
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            if (null == result || result == String.Empty)
            {
                result = HttpContext.Current.Request.UserHostAddress;
            }

            if (null == result || result == String.Empty)
            {
                return "0.0.0.0";
            }

            return result;

        }

		/// <summary>
		/// ����sql����е�ת���ַ�
		/// </summary>
		public static string mashSQL(string str)
		{
			string str2;

			if (str == null)
			{
				str2 = "";
			}
			else
			{
				str = str.Replace("\'", "'");
				str2 = str;
			}
			return str2;
		}

		/// <summary>
		/// �滻sql����е����������
		/// </summary>
		public static string ChkSQL(string str)
		{
			string str2;

			if (str == null)
			{
				str2 = "";
			}
			else
			{
				str = str.Replace("'", "''");
				str2 = str;
			}
			return str2;
		}


		/// <summary>
		/// ת��Ϊ��̬html
		/// </summary>
		public void transHtml(string path,string outpath)        
		{
			Page page = new Page();
			StringWriter writer = new StringWriter();
			page.Server.Execute(path, writer);
			FileStream fs;
			if(File.Exists(page.Server.MapPath("") + "\\" + outpath))              
			{
				File.Delete(page.Server.MapPath("") + "\\" + outpath);
				fs = File.Create(page.Server.MapPath("") + "\\" + outpath);
			}
			else            
			{
				fs = File.Create(page.Server.MapPath("") + "\\" + outpath);
			}
			byte[] bt = Encoding.Default.GetBytes(writer.ToString());
			fs.Write(bt,0,bt.Length);
			fs.Close();
		}


		/// <summary>
		/// �ָ��ַ���
		/// </summary>
		public static string[] SplitString(string strContent, string strSplit)
		{
			if(strContent.IndexOf(strSplit) < 0)
			{
				string[] tmp = {strContent};
				return tmp;
			}
			return Regex.Split(strContent, @strSplit.Replace(".",@"\."), RegexOptions.IgnoreCase);
		}

		/// <summary>
		/// �滻html�ַ�
		/// </summary>
		public static string EncodeHtml(string strHtml)
		{
			if (strHtml != "")
			{
				strHtml = strHtml.Replace(",", "&def");
				strHtml = strHtml.Replace("'", "&dot");
				strHtml = strHtml.Replace(";", "&dec");
				return strHtml;
			}
			return "";
		}



		public static string ClearHtml(string strHtml)
		{
			if (strHtml != "")
			{
				Regex r = null;
				Match m = null;

				r = new Regex(@"<\/?[^>]*>",RegexOptions.IgnoreCase);
				for (m = r.Match(strHtml); m.Success; m = m.NextMatch()) 
				{
					strHtml = strHtml.Replace(m.Groups[0].ToString(),"");
				}
			}
			return strHtml;
		}


		/// <summary>
		/// ����ָ�����滻(���ֹ���)
		/// </summary>
		public static string StrFilter(string str, string bantext)
		{
			string text1 = "";
			string text2 = "";
			string[] textArray1 = SplitString(bantext, "\r\n");
			for (int num1 = 0; num1 < textArray1.Length; num1++)
			{
				text1 = textArray1[num1].Substring(0, textArray1[num1].IndexOf("="));
				text2 = textArray1[num1].Substring(textArray1[num1].IndexOf("=") + 1);
				str = str.Replace(text1, text2);
			}
			return str;
		}

		
		
		/// <summary>
		/// ���α��̬ҳ����ʾ����
		/// </summary>
		/// <param name="curPage">��ǰҳ��</param>
		/// <param name="countPage">��ҳ��</param>
		/// <param name="url">�������ӵ�ַ</param>
		/// <param name="extendPage">�ܱ�ҳ����ʾ��������</param>
		/// <returns>ҳ��html</returns>
		public static string GetStaticPageNumbers(int curPage, int countPage, string url, string expname, int extendPage)
		{
			int startPage = 1;
			int endPage = 1;

			string t1 = "<a href=\"" + url + "-1" + expname + "\">&laquo;</a>&nbsp;";
			string t2 = "<a href=\"" + url + "-" + countPage + expname + "\">&raquo;</a>&nbsp;";

			if(countPage < 1) countPage = 1;
			if(extendPage < 3) extendPage = 2;
			
			if(countPage > extendPage)
			{
				if(curPage - (extendPage / 2) > 0)
				{
					if(curPage + (extendPage / 2) < countPage)
					{
						startPage = curPage - (extendPage / 2);
						endPage = startPage + extendPage - 1;
					}
					else
					{
						endPage = countPage;
						startPage = endPage - extendPage + 1;
						t2 = "";
					}
				}
				else
				{
					endPage = extendPage;
					t1 = "";
				}
			}
			else
			{
				startPage = 1;
				endPage = countPage;
				t1 = "";
				t2 = "";
			}
			
			StringBuilder s = new StringBuilder("");
			
			s.Append(t1);
			for (int i = startPage; i <= endPage; i++)
			{
				if (i == curPage)
				{
					s.Append("&nbsp;");
					s.Append(i);
					s.Append("&nbsp;");
				}
				else
				{
					s.Append("&nbsp;<a href=\"");
					s.Append(url);
					s.Append("-");
					s.Append(i);
					s.Append(expname); 
					s.Append("\">");
					s.Append(i);
					s.Append("</a>&nbsp;");
				}
			}
			s.Append(t2);

			return s.ToString();
		}


		/// <summary>
		/// ������ӵ�α��̬ҳ����ʾ����
		/// </summary>
		/// <param name="expname"></param>
		/// <param name="countPage">��ҳ��</param>
		/// <param name="url">�������ӵ�ַ</param>
		/// <param name="extendPage">�ܱ�ҳ����ʾ��������</param>
		/// <returns>ҳ��html</returns>
		public static string GetPostPageNumbers(int countPage, string url, string expname, int extendPage)
		{
			int startPage = 1;
			int endPage = 1;
			int curPage = 1;

			string t1 = "<a href=\"" + url + "-1" + expname + "\">&laquo;</a>&nbsp;";
			string t2 = "<a href=\"" + url + "-" + countPage + expname + "\">&raquo;</a>&nbsp;";

			if(countPage < 1) countPage = 1;
			if(extendPage < 3) extendPage = 2;
			
			if(countPage > extendPage)
			{
				if(curPage - (extendPage / 2) > 0)
				{
					if(curPage + (extendPage / 2) < countPage)
					{
						startPage = curPage - (extendPage / 2);
						endPage = startPage + extendPage - 1;
					}
					else
					{
						endPage = countPage;
						startPage = endPage - extendPage + 1;
						t2 = "";
					}
				}
				else
				{
					endPage = extendPage;
					t1 = "";
				}
			}
			else
			{
				startPage = 1;
				endPage = countPage;
				t1 = "";
				t2 = "";
			}
			
			StringBuilder s = new StringBuilder("");
			
			s.Append(t1);
			for(int i = startPage; i <= endPage; i++)
			{
				s.Append("&nbsp;<a href=\"");
				s.Append(url);
				s.Append("-");
				s.Append(i);
				s.Append(expname); 
				s.Append("\">");
				s.Append(i);
				s.Append("</a>&nbsp;");
			}
			s.Append(t2);

			return s.ToString();
		}
		



		/// <summary>
		/// ���ҳ����ʾ����
		/// </summary>
		/// <param name="curPage">��ǰҳ��</param>
		/// <param name="countPage">��ҳ��</param>
		/// <param name="url">�������ӵ�ַ</param>
		/// <param name="extendPage">�ܱ�ҳ����ʾ��������</param>
		/// <returns>ҳ��html</returns>
		public static string GetPageNumbers(int curPage, int countPage, string url, int extendPage)
		{
			int startPage = 1;
			int endPage = 1;

            string t1 = "<a href=\"" + RegularUrl("page", "1", url) + "\">&laquo;</a>&nbsp;";
            string t2 = "<a href=\"" + RegularUrl("page", countPage.ToString(), url) + "\">&raquo;</a>&nbsp;";

			if(countPage < 1) countPage = 1;
			if(extendPage < 3) extendPage = 2;
			
			if(countPage > extendPage)
			{
				if(curPage - (extendPage / 2) > 0)
				{
					if(curPage + (extendPage / 2) < countPage)
					{
						startPage = curPage - (extendPage / 2);
						endPage = startPage + extendPage - 1;
					}
					else
					{
						endPage = countPage;
						startPage = endPage - extendPage + 1;
						t2 = "";
					}
				}
				else
				{
					endPage = extendPage;
					t1 = "";
				}
			}
			else
			{
				startPage = 1;
				endPage = countPage;
				t1 = "";
				t2 = "";
			}
			
			StringBuilder s = new StringBuilder("");
			
			s.Append(t1);
			for (int i = startPage; i <= endPage; i++)
			{
				if (i == curPage)
				{
					s.Append("&nbsp;");
					s.Append(i);
					s.Append("&nbsp;");
				}
				else
				{
					s.Append("&nbsp;<a href=\"");
                    s.Append(RegularUrl("page", i.ToString(), url));
					s.Append("\">");
					s.Append(i);
					s.Append("</a>&nbsp;");
				}
			}
			s.Append(t2);

			return s.ToString();
		}

		/// <summary>
		/// ���� HTML �ַ����ı�����
		/// </summary>
		/// <param name="str">�ַ���</param>
		/// <returns>������</returns>
		public static string HtmlEncode(string str)
		{
			return HttpUtility.HtmlEncode(str);
		}

		/// <summary>
		/// ���� HTML �ַ����Ľ�����
		/// </summary>
		/// <param name="str">�ַ���</param>
		/// <returns>������</returns>
		public static string HtmlDecode(string str)
		{
			return HttpUtility.HtmlDecode(str);
		}

		/// <summary>
		/// ���� URL �ַ����ı�����
		/// </summary>
		/// <param name="str">�ַ���</param>
		/// <returns>������</returns>
		public static string UrlEncode(string str)
		{
			return HttpUtility.UrlEncode(str);
		}

		/// <summary>
		/// ���� URL �ַ����ı�����
		/// </summary>
		/// <param name="str">�ַ���</param>
		/// <returns>������</returns>
		public static string UrlDecode(string str)
		{
			return HttpUtility.UrlDecode(str);
		}
	
	
		/// <summary>
		/// ����ָ��Ŀ¼�µķ� UTF8 �ַ����ļ�
		/// </summary>
		/// <param name="Path">·��</param>
		/// <returns>�ļ������ַ�������</returns>
		public static string[] FindNoUTF8File(string Path) 
		{ 
			//System.IO.StreamReader reader = null;
			StringBuilder filelist = new StringBuilder();   
			DirectoryInfo Folder = new DirectoryInfo(Path); 
			//System.IO.DirectoryInfo[] subFolders = Folder.GetDirectories(); 
			/*
			for (int i=0;i<subFolders.Length;i++) 
			{ 
				FindNoUTF8File(subFolders[i].FullName); 
			}
			*/
			FileInfo[] subFiles = Folder.GetFiles(); 
			for (int j = 0; j < subFiles.Length; j++) 
			{ 
				if (subFiles[j].Extension.ToLower().Equals(".htm")) 
				{           
					FileStream fs = new FileStream(subFiles[j].FullName, FileMode.Open,FileAccess.Read); 
					bool bUtf8 = IsUTF8(fs);
					fs.Close();
					if (!bUtf8)
					{
						filelist.Append(subFiles[j].FullName);
						filelist.Append("\r\n");
					}       
				} 
			}
			return Utils.SplitString(filelist.ToString(), "\r\n");
     
		} 
   
		//0000 0000-0000 007F - 0xxxxxxx  (ascii converts to 1 octet!)
		//0000 0080-0000 07FF - 110xxxxx 10xxxxxx    ( 2 octet format)
		//0000 0800-0000 FFFF - 1110xxxx 10xxxxxx 10xxxxxx (3 octet format)

		/// <summary>
		/// �ж��ļ����Ƿ�ΪUTF8�ַ���
		/// </summary>
		/// <param name="sbInputStream">�ļ���</param>
		/// <returns>�жϽ��</returns>
		private static bool IsUTF8(FileStream sbInputStream) 
		{ 
			int   i; 
			byte cOctets;  // octets to go in this UTF-8 encoded character 
			byte chr; 
			bool  bAllAscii= true; 
			long iLen = sbInputStream.Length; 

			cOctets = 0; 
			for (i = 0; i < iLen; i++)  
			{ 
				chr = (byte)sbInputStream.ReadByte(); 

				if ( (chr & 0x80) != 0 ) bAllAscii= false; 

				if ( cOctets == 0 )   
				{ 
					if( chr >= 0x80 )  
					{   
						do  
						{ 
							chr <<= 1; 
							cOctets++; 
						} 
						while ((chr & 0x80) != 0); 

						cOctets--;                         
						if (cOctets == 0) return false;   
					} 
				} 
				else  
				{ 
					if ((chr & 0xC0) != 0x80)  
					{ 
						return false; 
					} 
					cOctets--;                        
				} 
			} 

			if (cOctets > 0)  
			{   
				return false; 
			} 

			if (bAllAscii)  
			{     
				return false; 
			} 

			return true; 

		}

		public static string FormatBytesStr(int bytes)
		{
			if (bytes > 1073741824)
			{
				return ((double)(bytes / 1073741824)).ToString("0") + "G";
			}
			if (bytes > 1048576)
			{
				return ((double)(bytes / 1048576)).ToString("0") + "M";
			}
			if (bytes > 1024)
			{
				return ((double)(bytes / 1024)).ToString("0") + "K";
			}
			return bytes.ToString() + "Bytes";
		}

		/// <summary>
		/// ������������
		/// </summary>
		/// <param name="Time"></param>
		/// <param name="Sec"></param>
		/// <returns></returns>
		public static int StrDateDiffSeconds(string Time, int Sec)
		{
			TimeSpan ts = DateTime.Now - DateTime.Parse(Time).AddSeconds(Sec);
			if (ts.TotalSeconds > int.MaxValue)
			{
				return int.MaxValue;
			}
			else if (ts.TotalSeconds < int.MinValue)
			{
				return int.MinValue;
			}
			return (int)ts.TotalSeconds;
		}

		/// <summary>
		/// �������ķ�����
		/// </summary>
		/// <param name="Time"></param>
		/// <param name="Minutes"></param>
		/// <returns></returns>
		public static int StrDateDiffMinutes(string Time, int Minutes)
		{
			TimeSpan ts = DateTime.Now - DateTime.Parse(Time).AddMinutes(Minutes);
			if (ts.TotalMinutes > int.MaxValue)
			{
				return int.MaxValue;
			}
			else if (ts.TotalMinutes < int.MinValue)
			{
				return int.MinValue;
			}
			return (int)ts.TotalMinutes;
		}

		/// <summary>
		/// ��������Сʱ��
		/// </summary>
		/// <param name="Time"></param>
		/// <param name="Hours"></param>
		/// <returns></returns>
		public static int StrDateDiffHours(string Time, int Hours)
		{
			TimeSpan ts = DateTime.Now - DateTime.Parse(Time).AddHours(Hours);
			if (ts.TotalHours > int.MaxValue)
			{
				return int.MaxValue;
			}
			else if (ts.TotalHours < int.MinValue)
			{
				return int.MinValue;
			}
			return (int)ts.TotalHours;
		}
		
		
		public static bool CreateDir(string name)
		{
			return Utils.MakeSureDirectoryPathExists(name);
		}

		public static string ReplaceStrToScript(string str)
		{
			str = str.Replace("\\","\\\\");
			str = str.Replace("'","\\'");
			str = str.Replace("\"","\\\"");
			return str;
		}

		public static bool IsIP(string ip)
		{
			return new Regex(@"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$").IsMatch(ip);
		}

		/// <summary>
		/// ����ָ��IP�Ƿ���ָ����IP�������޶��ķ�Χ��, IP�����ڵ�IP��ַ����ʹ��*��ʾ��IP������, ����192.168.1.*
		/// </summary>
		/// <param name="ip"></param>
		/// <param name="iparray"></param>
		/// <returns></returns>
		public static bool InIPArray(string ip, string[] iparray)
		{
			
			string[] userip = Utils.SplitString(ip, @".");
			for (int ipIndex = 0; ipIndex < iparray.Length; ipIndex++)
			{
				string[] tmpip = Utils.SplitString(iparray[ipIndex],@".");
				int r = 0;
				for (int i = 0; i < tmpip.Length; i++)
				{
					if (tmpip[i] == "*")
					{
						return true;
					}

					if (userip.Length > i)
					{
						if (tmpip[i] == userip[i])
						{
							r ++;
						}
						else
						{
							break;
						}
					}
					else
					{
						break;
					}

				}
				if (r == 4)
				{
					return true;
				}
				

			}
			return false;

		}

		/// <summary>
		/// ���Assembly�汾��
		/// </summary>
		/// <returns></returns>
		public static string GetAssemblyVersion()
		{
			Assembly myAssembly = Assembly.GetExecutingAssembly();
			FileVersionInfo myFileVersion = FileVersionInfo.GetVersionInfo(myAssembly.Location);
			return string.Format("{0}.{1}.{2}",myFileVersion.FileMajorPart, myFileVersion.FileMinorPart, myFileVersion.FileBuildPart);
		}

		/// <summary>
		/// ���Assembly��Ʒ����
		/// </summary>
		/// <returns></returns>
		public static string GetAssemblyProductName()
		{
			Assembly myAssembly = Assembly.GetExecutingAssembly();
			FileVersionInfo myFileVersion = FileVersionInfo.GetVersionInfo(myAssembly.Location);
			return myFileVersion.ProductName;
		}

		/// <summary>
		/// ����Ŀ¼
		/// </summary>
		/// <param name="name">����</param>
		/// <returns>�����Ƿ�ɹ�</returns>
		[DllImport("dbgHelp", SetLastError=true)]
		private static extern bool MakeSureDirectoryPathExists(string name);


		/// <summary>
		/// дcookieֵ
		/// </summary>
		/// <param name="strName">����</param>
		/// <param name="strValue">ֵ</param>
		public static void WriteCookie(string strName, string strValue)
		{
			HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
			if (cookie == null)
			{
				cookie = new HttpCookie(strName);
			}
			cookie.Value = strValue;
			HttpContext.Current.Response.AppendCookie(cookie);

		}
		/// <summary>
		/// дcookieֵ
		/// </summary>
		/// <param name="strName">����</param>
		/// <param name="strValue">ֵ</param>
		public static void WriteCookie(string strName, string strValue, int expires)
		{
			HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
			if (cookie == null)
			{
				cookie = new HttpCookie(strName);
			}
			cookie.Value = strValue;
			cookie.Expires = DateTime.Now.AddMinutes(expires);
			HttpContext.Current.Response.AppendCookie(cookie);

		}

		/// <summary>
		/// ��cookieֵ
		/// </summary>
		/// <param name="strName">����</param>
		/// <returns>cookieֵ</returns>
		public static string GetCookie(string strName)
		{
			if (HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies[strName] != null)
			{
				return HttpContext.Current.Request.Cookies[strName].Value.ToString();
			}

			return "";
		}

        /// <summary>
        /// ����XML�ļ������֣����������λ����
        /// </summary>
        /// <param name="fileName">�����͵��ļ���</param>
        /// <param name="fileNameLen">�ļ�������</param>
        /// <returns></returns>
        public static string ConstructFileName(string fileName,int fileNameLen)
        {
            if (fileName.Length < fileNameLen)
            {
                int length = fileNameLen - fileName.Length;

                for (int i = 0; i < length; i++)
                {
                    fileName = "0" + fileName;
                }
            }

            return fileName + ".xml";
        }

        /// <summary>
        /// ����XML�ļ������֣����������λ����
        /// </summary>
        /// <param name="fileName">�����͵��ļ���</param>
        /// <param name="fileNameLen">�ļ�������</param>
        /// <returns></returns>
        public static string ConstructFileName(int fileName, int fileNameLen)
        {
            string strFileName = fileName.ToString();

            if (strFileName.Length < fileNameLen)
            {
                int length = fileNameLen - strFileName.Length;

                for (int i = 0; i < length; i++)
                {
                    strFileName = "0" + strFileName;
                }
            }

            return strFileName + ".xml";
        }

        # region ��ȡָ���ߴ��ͼƬUrl

        /// <summary>
        /// ��ȡָ���ߴ��ͼƬURL
        /// </summary>
        /// <param name="photoUrl">��ʵͼƬ��URL</param>
        /// <param name="photoSize">ͼƬ�ߴ� 1=t 2=s 3=m 4=b</param>
        /// <returns></returns>
        public static string GetPhotoSizeUrl(string photoUrl, int photoSize)
        {
            # region OLD METHOD

            //string extName = photoUrl.Substring(photoUrl.LastIndexOf("."));
            //string photoSizeName = "";
            //switch (photoSize)
            //{
            //    case 1:
            //        photoSizeName = "_t";
            //        break;
            //    case 2:
            //        photoSizeName = "_s";
            //        break;
            //    case 3:
            //        photoSizeName = "_m";
            //        break;
            //    case 4:
            //        photoSizeName = "_b";
            //        break;
            //    default:
            //        photoSizeName = "";
            //        break;
            //}

            //return photoUrl.Substring(0, photoUrl.LastIndexOf(".")) + photoSizeName + extName;

            # endregion

            string photoSizeName = "";

            switch (photoSize)
            {
                case 1:
                    photoSizeName = "t_";
                    break;
                case 2:
                    photoSizeName = "s_";
                    break;
                case 3:
                    photoSizeName = "m_";
                    break;
                case 4:
                    photoSizeName = "b_";
                    break;
                default:
                    photoSizeName = "";
                    break;
            }

            photoUrl = photoUrl.Insert(photoUrl.LastIndexOf('/') + 1, photoSizeName);

            return photoUrl;
        }

        # endregion


        public static string GetPager(int CountNumber, int pageSize, int CurrentPage, int ExtPage)
        {
            StringBuilder html = new StringBuilder();
            html.Append("<a href='?page=1'>��ҳ</a>");
            int PageCount = 0;
            if (CountNumber % pageSize == 0)
                PageCount = CountNumber / pageSize;
            else
                PageCount = (CountNumber / pageSize) + 1;

            int StartPage = 0, EndPage = 10;
            if (CurrentPage <= PageCount)
            {
                if (CurrentPage / ExtPage >= 1)
                {
                    int extPage = CurrentPage / ExtPage;
                    StartPage = extPage * ExtPage - 1;
                    EndPage = StartPage + ExtPage + 1;
                }
                else
                {
                    EndPage = StartPage + ExtPage;
                }
            }
            if (EndPage > PageCount)
                EndPage = PageCount;
            if (StartPage < 1)
                StartPage = 1;
            if (CurrentPage > 1)
                html.Append(" <a href='?page=" + (CurrentPage - 1) + "'>��ҳ</a>");
            else
                html.Append(" <a href='javascript:void(0)'>��ҳ</a> ");
            for (int i = StartPage; i <= EndPage; i++)
            {
                if (i == CurrentPage)
                    html.Append("<span>[" + i + "]</span>");
                else
                    html.Append("&nbsp;<a href='?page=" + i + "'>[" + i + "]</a>");
            }
            if (CurrentPage < PageCount)
                html.Append("<a href='?page=" + (CurrentPage + 1) + "'>��ҳ</a>");
            else
                html.Append("&nbsp;<a href='javascript:void(0)'>��ҳ</a>");
            html.Append("&nbsp;<a href='?page=" + PageCount + "'>ĩҳ</a>");
            html.Append("&nbsp;<a>��" + CurrentPage + "/" + PageCount + "ҳ</a>");
            return html.ToString();
        }

        public static Int64 GetNewFileID()
        {
            Random ra = new Random();
            string newfileid = DateTime.Now.Year + DateTime.Now.Month.ToString(CultureInfo.InvariantCulture)
            + DateTime.Now.Day
            + ra.Next(1000);
            return Convert.ToInt64(newfileid);
        }

        public static string SendMails(string strHost, string strAccount, string strPwd, string emailFrom, string emailTo, string body, string mailSubject)
        {
            //string body = "";      
            //if (url.Length > 0)
            //{
            //    WebRequest request = WebRequest.Create(url);
            //    request.Credentials = CredentialCache.DefaultCredentials;
            //    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            //    Stream dataStream = response.GetResponseStream();
            //    StreamReader reader = new StreamReader(dataStream, Encoding.UTF8);
            //    body = reader.ReadToEnd();
            //}
            try
            {
                if (emailTo != "")
                {
                    MailMessage mail = new MailMessage(emailFrom, emailTo);
                    mail.Subject = mailSubject;//����
                    mail.Body = body; //�ʼ�����
                    mail.IsBodyHtml = true;//�Ƿ���HTML��ʽ
                    mail.SubjectEncoding = Encoding.UTF8;
                    mail.BodyEncoding = Encoding.UTF8;//�������
                    mail.Priority = MailPriority.High;   //����
                    SmtpClient smtpClient = new SmtpClient();  //�����ʼ�
                    smtpClient.Host = strHost;     //SMTP������ַ
                    smtpClient.Credentials = new NetworkCredential(strAccount, strPwd);//��Ȩ
                    smtpClient.Send(mail);
                    return "success";
                }
                return "error";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// ��Url��key��ֵ�滻Ϊvalue�����������key��׷��
        public static string RegularUrl(string key, string value, string url)
        {
            int fragPos = url.LastIndexOf("#");
            string fragment = "";
            if (fragPos > -1)
            {
                fragment = url.Substring(fragPos);
                url = url.Substring(0, fragPos);
            }
            int querystart = url.IndexOf("?");
            if (querystart < 0)
            {
                url += "?" + key + "=" + value;
            }
            else if (querystart == url.Length - 1)
            {
                url += key + "=" + value;
            }
            else
            {
                Regex Re = new Regex(key + "=[^\\s&#]*", RegexOptions.IgnoreCase);
                url = (Re.IsMatch(url)) ? Re.Replace(url, key + "=" + value) : url + "&" + key + "=" + value;
            }
            return url + fragment;
        }
	} 
}
