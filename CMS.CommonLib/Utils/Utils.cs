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
	/// 工具类
	/// </summary>
	public partial class Utils
	{
        /// <summary>
        /// 根据生日获取星座
        /// </summary>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        public static string GetConstellationByBirthDay(int month, int day)
        {
            string strconstellation = "";
            string[] constellation = {"水瓶座", "双鱼座", "白羊座", "金牛座", "双子座", "巨蟹座", "狮子座", "处女座", "天秤座", "天蝎座", "射手座", "摩羯座" };

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
		/// 返回字符串真实长度, 1个汉字长度为2
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

        # region 获取文件编码

        /// <summary>
        /// 获取文件编码
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
		/// 判断指定字符串在指定字符串数组中的位置
		/// </summary>
		/// <param name="strSearch">字符串</param>
		/// <param name="stringArray">字符串数组</param>
		/// <param name="caseInsensetive">是否不区分大小写, true为不区分, false为区分</param>
		/// <returns>字符串在指定字符串数组中的位置, 如不存在则返回-1</returns>
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
		/// 判断指定字符串在指定字符串数组中的位置
		/// </summary>
		/// <param name="strSearch">字符串</param>
		/// <param name="stringArray">字符串数组</param>
		/// <returns>字符串在指定字符串数组中的位置, 如不存在则返回-1</returns>		
		public static int GetInArrayID(string strSearch, string[] stringArray)
		{
			return GetInArrayID(strSearch, stringArray, true);
		}
		
		/// <summary>
		/// 判断指定字符串是否属于指定字符串数组中的一个元素
		/// </summary>
		/// <param name="strSearch">字符串</param>
		/// <param name="stringArray">字符串数组</param>
		/// <param name="caseInsensetive">是否不区分大小写, true为不区分, false为区分</param>
		/// <returns>判断结果</returns>
		public static bool InArray(string strSearch, string[] stringArray, bool caseInsensetive)
		{
			return GetInArrayID(strSearch, stringArray, caseInsensetive) >= 0;
		}

		/// <summary>
		/// 判断指定字符串是否属于指定字符串数组中的一个元素
		/// </summary>
		/// <param name="str">字符串</param>
		/// <param name="stringarray">字符串数组</param>
		/// <returns>判断结果</returns>
		public static bool InArray(string str, string[] stringarray)
		{
			return InArray(str, stringarray, false);
		}

		/// <summary>
		/// 判断指定字符串是否属于指定字符串数组中的一个元素
		/// </summary>
		/// <param name="str">字符串</param>
		/// <param name="stringarray">内部以逗号分割单词的字符串</param>
		/// <returns>判断结果</returns>
		public static bool InArray(string str, string stringarray)
		{
			return InArray(str, SplitString(stringarray, ","), false);
		}

		/// <summary>
		/// 判断指定字符串是否属于指定字符串数组中的一个元素
		/// </summary>
		/// <param name="str">字符串</param>
		/// <param name="stringarray">内部以逗号分割单词的字符串</param>
		/// <param name="strsplit">分割字符串</param>
		/// <returns>判断结果</returns>
		public static bool InArray(string str, string stringarray, string strsplit)
		{
			return InArray(str, SplitString(stringarray, strsplit), false);
		}
		
		/// <summary>
		/// 判断指定字符串是否属于指定字符串数组中的一个元素
		/// </summary>
		/// <param name="str">字符串</param>
		/// <param name="stringarray">内部以逗号分割单词的字符串</param>
		/// <param name="strsplit">分割字符串</param>
		/// <param name="caseInsensetive">是否不区分大小写, true为不区分, false为区分</param>
		/// <returns>判断结果</returns>
		public static bool InArray(string str, string stringarray, string strsplit, bool caseInsensetive)
		{
			return InArray(str, SplitString(stringarray, strsplit), caseInsensetive);
		}

		/// <summary>
		/// string型转换为int型
		/// </summary>
		/// <param name="strValue">要转换的字符串</param>
		/// <param name="defValue">缺省值</param>
		/// <returns>转换后的int类型结果</returns>
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
		/// string型转换为float型
		/// </summary>
		/// <param name="strValue">要转换的字符串</param>
		/// <param name="defValue">缺省值</param>
		/// <returns>转换后的int类型结果</returns>
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
		/// 判断给定的字符串(strNumber)是否是数值型
		/// </summary>
		/// <param name="strNumber">要确认的字符串</param>
		/// <returns>是则返加true 不是则返回 false</returns>
		public static bool IsNumber(string strNumber)
		{
			return new Regex(@"^([0-9])[0-9]*(\.\w*)?$").IsMatch(strNumber);
		}
		

		/// <summary>
		/// 判断给定的字符串数组(strNumber)中的数据是不是都为数值型
		/// </summary>
		/// <param name="strNumber">要确认的字符串数组</param>
		/// <returns>是则返加true 不是则返回 false</returns>
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
		/// 删除字符串尾部的回车/换行/空格
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
		/// 清除给定字符串中的回车及换行符
		/// </summary>
		/// <param name="str">要清除的字符串</param>
		/// <returns>清除后返回的字符串</returns>
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
		/// 从字符串的指定位置截取指定长度的子字符串
		/// </summary>
		/// <param name="str">原字符串</param>
		/// <param name="startIndex">子字符串的起始位置</param>
		/// <param name="length">子字符串的长度</param>
		/// <returns>子字符串</returns>
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
		/// 从字符串的指定位置开始截取到字符串结尾的了符串
		/// </summary>
		/// <param name="str">原字符串</param>
		/// <param name="startIndex">子字符串的起始位置</param>
		/// <returns>子字符串</returns>
		public static string CutString(string str,int startIndex)
		{
			return CutString(str,startIndex,str.Length);
		}
		


		/// <summary>
		/// 获得当前绝对路径
		/// </summary>
		/// <param name="strPath">指定的路径</param>
		/// <returns>绝对路径</returns>
		public static string GetMapPath(string strPath)
		{
			return HttpContext.Current.Server.MapPath(strPath);
		}



		/// <summary>
		/// 返回文件是否存在
		/// </summary>
		/// <param name="filename">文件名</param>
		/// <returns>是否存在</returns>
		public static bool FileExists(string filename)
		{
			return File.Exists(filename);
		}



		/// <summary>
		/// 以指定的ContentType输出指定文件文件
		/// </summary>
		/// <param name="filepath">文件路径</param>
		/// <param name="filename">输出的文件名</param>
		/// <param name="filetype">将文件输出时设置的ContentType</param>
		public static void ResponseFile(string filepath, string  filename, string filetype)
		{
			Stream iStream = null;

			// 缓冲区为10k
			byte[] buffer = new Byte[10000];

			// 文件长度
			int length;

			// 需要读的数据长度
			long dataToRead;

			try
			{
				// 打开文件
				iStream = new FileStream(filepath, FileMode.Open, FileAccess.Read,FileShare.Read);


				// 需要读的数据长度
				dataToRead = iStream.Length;

				HttpContext.Current.Response.ContentType = filetype;
				HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + Utils.UrlEncode(filename.Trim()));

				while (dataToRead > 0)
				{
					// 检查客户端是否还处于连接状态
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
						// 如果不再连接则跳出死循环
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
					// 关闭文件
					iStream.Close();
				}
			}
			HttpContext.Current.Response.End();
		}

		/// <summary>
		/// 判断文件名是否为浏览器可以直接显示的图片文件名
		/// </summary>
		/// <param name="filename">文件名</param>
		/// <returns>是否可以直接显示</returns>
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
		/// int型转换为string型
		/// </summary>
		/// <returns>转换后的string类型结果</returns>
		public static string IntToStr(int intValue)
		{
			//
			return Convert.ToString(intValue);
		}
		/// <summary>
		/// MD5函数
		/// </summary>
		/// <param name="str">原始字符串</param>
		/// <returns>MD5结果</returns>
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
		/// SHA256函数
		/// </summary>
		/// /// <param name="str">原始字符串</param>
		/// <returns>SHA256结果</returns>
		public static string SHA256(string str)
		{
			byte[] SHA256Data = Encoding.UTF8.GetBytes(str);
			SHA256Managed Sha256 = new SHA256Managed();
			byte[] Result = Sha256.ComputeHash(SHA256Data);
			return Convert.ToBase64String(Result);  //返回长度为44字节的字符串
		}


		/// <summary>
		/// 字符串如果操过指定长度则将超出的部分用指定字符串代替
		/// </summary>
		/// <param name="p_SrcString">要检查的字符串</param>
		/// <param name="p_Length">指定长度</param>
		/// <param name="p_TailString">用于替换的字符串</param>
		/// <returns>截取后的字符串</returns>
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
		/// 自定义的替换字符串函数
		/// </summary>
		public static string ReplaceString(string SourceString, string SearchString,string ReplaceString, bool IsCaseInsensetive)
		{
			return Regex.Replace(SourceString, Regex.Escape(SearchString), ReplaceString, (IsCaseInsensetive==true)?RegexOptions.IgnoreCase:RegexOptions.None);
		}

		/// <summary>
		/// 生成指定数量的html空格符号
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
		/// 检测是否符合email格式
		/// </summary>
		/// <param name="strEmail">要判断的email字符串</param>
		/// <returns>判断结果</returns>
		public static bool IsValidEmail(string strEmail)
		{
			return Regex.IsMatch(strEmail, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
		}

        /// <summary>
        /// 获取EMAIL地址的域名
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
        /// 检测是否符合Url格式
        /// </summary>
        /// <param name="strEmail">要判断的Url字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsValidUrl(string strUrl)
        {
            return Regex.IsMatch(strUrl, @"^(http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?)$");
        }

		/// <summary>
		/// 判断是否为base64字符串
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static bool IsBase64String(string str)
		{
			//A-Z, a-z, 0-9, +, /, =
			return Regex.IsMatch(str, @"[A-Za-z0-9\+\/\=]"); 
		}
		/// <summary>
		/// 检测是否有Sql危险字符
		/// </summary>
		/// <param name="str">要判断字符串</param>
		/// <returns>判断结果</returns>
		public static bool IsSafeSqlString(string str)
		{

			return !Regex.IsMatch(str, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']"); 
		}
		/// <summary>
		/// 清理字符串
		/// </summary>
		public static string CleanInput(string strIn)
		{
			return Regex.Replace(strIn.Trim(), @"[^\w\.@-]", ""); 
		}

		/// <summary>
		/// 返回URL中结尾的文件名
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
		/// 根据阿拉伯数字返回月份的名称(可更改为某种语言)
		/// </summary>	
		public static string[] Monthes
		{
			get
			{
				return new string[]{"January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"};
			}
		}

		/// <summary>
		/// 替换回车换行符为html换行符
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
		/// 返回标准日期格式string
		/// </summary>
		public static string GetDate()
		{
			return DateTime.Now.ToString("yyyy-MM-dd");
		}

        /// <summary>
        /// 返回标准日期格式int32
        /// </summary>
        public static int GetIntDate()
        {
            return Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd"));
        }

		/// <summary>
		/// 返回指定日期格式
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
		/// 返回标准时间格式string
		/// </summary>
		public static string GetTime()
		{
			return DateTime.Now.ToString("HH:mm:ss");
		}

		/// <summary>
		/// 返回标准时间格式string
		/// </summary>
		public static string GetDateTime()
		{
			return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		}
		/// <summary>
		/// 返回标准时间格式string
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
        /// 获得当前页面客户端的IP
        /// </summary>
        /// <returns>当前页面客户端的IP</returns>
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
		/// 改正sql语句中的转义字符
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
		/// 替换sql语句中的有问题符号
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
		/// 转换为静态html
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
		/// 分割字符串
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
		/// 替换html字符
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
		/// 进行指定的替换(脏字过滤)
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
		/// 获得伪静态页码显示链接
		/// </summary>
		/// <param name="curPage">当前页数</param>
		/// <param name="countPage">总页数</param>
		/// <param name="url">超级链接地址</param>
		/// <param name="extendPage">周边页码显示个数上限</param>
		/// <returns>页码html</returns>
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
		/// 获得帖子的伪静态页码显示链接
		/// </summary>
		/// <param name="expname"></param>
		/// <param name="countPage">总页数</param>
		/// <param name="url">超级链接地址</param>
		/// <param name="extendPage">周边页码显示个数上限</param>
		/// <returns>页码html</returns>
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
		/// 获得页码显示链接
		/// </summary>
		/// <param name="curPage">当前页数</param>
		/// <param name="countPage">总页数</param>
		/// <param name="url">超级链接地址</param>
		/// <param name="extendPage">周边页码显示个数上限</param>
		/// <returns>页码html</returns>
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
		/// 返回 HTML 字符串的编码结果
		/// </summary>
		/// <param name="str">字符串</param>
		/// <returns>编码结果</returns>
		public static string HtmlEncode(string str)
		{
			return HttpUtility.HtmlEncode(str);
		}

		/// <summary>
		/// 返回 HTML 字符串的解码结果
		/// </summary>
		/// <param name="str">字符串</param>
		/// <returns>解码结果</returns>
		public static string HtmlDecode(string str)
		{
			return HttpUtility.HtmlDecode(str);
		}

		/// <summary>
		/// 返回 URL 字符串的编码结果
		/// </summary>
		/// <param name="str">字符串</param>
		/// <returns>编码结果</returns>
		public static string UrlEncode(string str)
		{
			return HttpUtility.UrlEncode(str);
		}

		/// <summary>
		/// 返回 URL 字符串的编码结果
		/// </summary>
		/// <param name="str">字符串</param>
		/// <returns>解码结果</returns>
		public static string UrlDecode(string str)
		{
			return HttpUtility.UrlDecode(str);
		}
	
	
		/// <summary>
		/// 返回指定目录下的非 UTF8 字符集文件
		/// </summary>
		/// <param name="Path">路径</param>
		/// <returns>文件名的字符串数组</returns>
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
		/// 判断文件流是否为UTF8字符集
		/// </summary>
		/// <param name="sbInputStream">文件流</param>
		/// <returns>判断结果</returns>
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
		/// 返回相差的秒数
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
		/// 返回相差的分钟数
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
		/// 返回相差的小时数
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
		/// 返回指定IP是否在指定的IP数组所限定的范围内, IP数组内的IP地址可以使用*表示该IP段任意, 例如192.168.1.*
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
		/// 获得Assembly版本号
		/// </summary>
		/// <returns></returns>
		public static string GetAssemblyVersion()
		{
			Assembly myAssembly = Assembly.GetExecutingAssembly();
			FileVersionInfo myFileVersion = FileVersionInfo.GetVersionInfo(myAssembly.Location);
			return string.Format("{0}.{1}.{2}",myFileVersion.FileMajorPart, myFileVersion.FileMinorPart, myFileVersion.FileBuildPart);
		}

		/// <summary>
		/// 获得Assembly产品名称
		/// </summary>
		/// <returns></returns>
		public static string GetAssemblyProductName()
		{
			Assembly myAssembly = Assembly.GetExecutingAssembly();
			FileVersionInfo myFileVersion = FileVersionInfo.GetVersionInfo(myAssembly.Location);
			return myFileVersion.ProductName;
		}

		/// <summary>
		/// 创建目录
		/// </summary>
		/// <param name="name">名称</param>
		/// <returns>创建是否成功</returns>
		[DllImport("dbgHelp", SetLastError=true)]
		private static extern bool MakeSureDirectoryPathExists(string name);


		/// <summary>
		/// 写cookie值
		/// </summary>
		/// <param name="strName">名称</param>
		/// <param name="strValue">值</param>
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
		/// 写cookie值
		/// </summary>
		/// <param name="strName">名称</param>
		/// <param name="strValue">值</param>
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
		/// 读cookie值
		/// </summary>
		/// <param name="strName">名称</param>
		/// <returns>cookie值</returns>
		public static string GetCookie(string strName)
		{
			if (HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies[strName] != null)
			{
				return HttpContext.Current.Request.Cookies[strName].Value.ToString();
			}

			return "";
		}

        /// <summary>
        /// 构造XML文件的名字，如果不足六位则补零
        /// </summary>
        /// <param name="fileName">数字型的文件名</param>
        /// <param name="fileNameLen">文件名长度</param>
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
        /// 构造XML文件的名字，如果不足六位则补零
        /// </summary>
        /// <param name="fileName">数字型的文件名</param>
        /// <param name="fileNameLen">文件名长度</param>
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

        # region 获取指定尺寸的图片Url

        /// <summary>
        /// 获取指定尺寸的图片URL
        /// </summary>
        /// <param name="photoUrl">真实图片的URL</param>
        /// <param name="photoSize">图片尺寸 1=t 2=s 3=m 4=b</param>
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
            html.Append("<a href='?page=1'>首页</a>");
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
                html.Append(" <a href='?page=" + (CurrentPage - 1) + "'>上页</a>");
            else
                html.Append(" <a href='javascript:void(0)'>上页</a> ");
            for (int i = StartPage; i <= EndPage; i++)
            {
                if (i == CurrentPage)
                    html.Append("<span>[" + i + "]</span>");
                else
                    html.Append("&nbsp;<a href='?page=" + i + "'>[" + i + "]</a>");
            }
            if (CurrentPage < PageCount)
                html.Append("<a href='?page=" + (CurrentPage + 1) + "'>下页</a>");
            else
                html.Append("&nbsp;<a href='javascript:void(0)'>下页</a>");
            html.Append("&nbsp;<a href='?page=" + PageCount + "'>末页</a>");
            html.Append("&nbsp;<a>第" + CurrentPage + "/" + PageCount + "页</a>");
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
                    mail.Subject = mailSubject;//主题
                    mail.Body = body; //邮件内容
                    mail.IsBodyHtml = true;//是否是HTML格式
                    mail.SubjectEncoding = Encoding.UTF8;
                    mail.BodyEncoding = Encoding.UTF8;//字体编码
                    mail.Priority = MailPriority.High;   //级别
                    SmtpClient smtpClient = new SmtpClient();  //发送邮件
                    smtpClient.Host = strHost;     //SMTP主机地址
                    smtpClient.Credentials = new NetworkCredential(strAccount, strPwd);//授权
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

        /// 将Url中key的值替换为value，如果不存在key则追加
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
