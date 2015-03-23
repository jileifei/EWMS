using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;

namespace CMS.Domain
{
    [Serializable]
    public class TemplateDoc
    {
        private Dictionary<string, object> m_values = new Dictionary<string, object>(10);

        public void SetValue(string key, object value)
        {
            m_values[key.ToLower()] = value;
        }

        public string clearhtml(string key)
        {
            key = key.ToLower();
            if (m_values.ContainsKey(key))
            {
                object ovalue = m_values[key];
                if (ovalue != null && !Convert.IsDBNull(ovalue))
                {
                    return RemoveHtml(Convert.ToString(ovalue));
                }
                else
                {
                    return "null";
                }
            }
            else
            {
                return "invalid column:" + key;
            }
        }

        public string get(string key)
        {
            
            key = key.ToLower();
            if (m_values.ContainsKey(key))
            {
                object ovalue =  m_values[key];
                if (ovalue != null && !Convert.IsDBNull(ovalue))
                {
                    return Convert.ToString(ovalue);
                }
                else {
                    return "null";
                }
            }
            else
            {
                return "invalid column:" + key;
            }
        }

        public string gettrim(string key)
        {
            key = key.ToLower();
            if (m_values.ContainsKey(key))
            {
                object ovalue = m_values[key];
                if (ovalue != null && !Convert.IsDBNull(ovalue))
                {
                    return Convert.ToString(ovalue).Trim();
                }
                else
                {
                    return "null";
                }
            }
            else
            {
                return "invalid column:" + key;
            }
        }

        /// <summary>
        /// 日期格式化
        /// </summary>
        /// <param name="key"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public string dateformat(string key,string format)
        {
            key = key.ToLower();
            if (m_values.ContainsKey(key))
            {
                object ovalue = m_values[key];
                if (ovalue != null && !Convert.IsDBNull(ovalue))
                {
                    return Convert.ToDateTime(ovalue).ToString(format);
                }
                else
                {
                    return "null";
                }
            }
            else
            {
                return "invalid column:" + key;
            }
        }

        public string numformat(string key, string format)
        {
            key = key.ToLower();
            if (m_values.ContainsKey(key))
            {
                object ovalue = m_values[key];
                if (ovalue != null && !Convert.IsDBNull(ovalue))
                {
                    return Convert.ToDouble(ovalue).ToString(format);
                }
                else
                {
                    return "null";
                }
            }
            else
            {
                return "invalid column:" + key;
            }
        }

        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="key"></param>
        /// <param name="len">截取的长度*个汉字</param>
        /// <returns></returns>
        public string left(string key,int len)
        {
            return left(key, len, "", false);
        }

        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="key"></param>
        /// <param name="len">长度*个汉字</param>
        /// <param name="attr">截取的替换字符串</param>
        /// <returns></returns>
        public string left(string key, int len,string attr)
        {
            return left(key, len, attr, false);
        }

        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="key"></param>
        /// <param name="len">长度*个汉字</param>
        /// <param name="attr">截取的替换字符串</param>
        /// <returns></returns>
        public string left(string key, int len, bool removeHtml)
        {
            return left(key,len,"",removeHtml);
        }

        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="key"></param>
        /// <param name="len">长度*个汉字</param>
        /// <param name="attr">截取的替换字符串</param>
        /// <returns></returns>
        public string left(string key, int len,string attr, bool removeHtml)
        {
            key = key.ToLower();
            if (m_values.ContainsKey(key))
            {
                object ovalue = m_values[key];
                if (ovalue != null && !Convert.IsDBNull(ovalue))
                {
                    if (removeHtml)
                    {
                        return GetSubString(RemoveHtml(Convert.ToString(ovalue)), len * 2, attr);
                    }
                    else
                    {
                        return GetSubString(Convert.ToString(ovalue), len * 2, attr);
                    }
                }
                else
                {
                    return "null";
                }
            }
            else
            {
                return "invalid column:" + key;
            }
        }


        public string get(string key,string defaultValue)
        {

            key = key.ToLower();
            if (m_values.ContainsKey(key))
            {
                object ovalue = m_values[key];
                if (ovalue != null && !Convert.IsDBNull(ovalue))
                {
                    return Convert.ToString(ovalue);
                }
                else
                {
                    return defaultValue;
                }
            }
            else
            {
                return "invalid column:" + key;
            }
        }

        private string RemoveHtml(string content)
        {
            string newstr = content;
            string regexstr = @"<[^>]*>";
            return Regex.Replace(newstr, regexstr, string.Empty, RegexOptions.IgnoreCase);
        }

        # region 字符串如果操过指定长度则将超出的部分用指定字符串代替

        /// <summary>
        /// 字符串如果操过指定长度则将超出的部分用指定字符串代替
        /// </summary>
        /// <param name="p_SrcString">要检查的字符串</param>
        /// <param name="p_Length">指定长度</param>
        /// <param name="p_TailString">用于替换的字符串</param>
        /// <returns>截取后的字符串</returns>
        private string GetSubString(string p_SrcString, int p_Length, string p_TailString)
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

        # endregion
    }


    public class EntityConv
    {

        /// <summary>
        /// 将dataset转换为Entity List
        /// </summary>
        /// <param name="dsToLoad"></param>
        /// <returns></returns>
        public static IList<TemplateDoc> LoadDataSet(DataSet dsToLoad)
        {
            IList<TemplateDoc> list = new List<TemplateDoc>(dsToLoad.Tables[0].Rows.Count );
            foreach(DataRow row in dsToLoad.Tables[0].Rows ){
                TemplateDoc doc = new TemplateDoc();
                foreach(DataColumn col in dsToLoad.Tables[0].Columns ){
                     doc.SetValue(  col.ColumnName,row[ col.ColumnName] );
                }
                list.Add( doc );
            }
            return list;
        }
    }
}
