using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;

namespace CMS.CommonLib.Utils
{
    public class TypeParse
    {
        #region 类型转化

        #region ToString
        /// <summary>
        /// 摘要:使用指定的区域性特定格式设置信息将此实例的值转换为等效的 System.String
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>   
        public static string ToString(object target)
        {
            if (target == null || target == DBNull.Value)
                return string.Empty;
            return ((IConvertible)target).ToString(CurrentCulture);
        }
        #endregion

        #region ToBool
        /// <summary>
        ///  摘要:
        ///     使用指定的区域性特定格式设置信息将此实例的值转换为等效的 Boolean 值。
        /// </summary>
        /// <param name="target">要转化的对象</param>
        /// <returns>与此实例的值等效的 Boolean 值。</returns>    
        public static bool ToBool(object target)
        {
            return ToBool(target, false);
        }



        /// <summary>
        ///  摘要:
        ///     使用指定的区域性特定格式设置信息将此实例的值转换为等效的 Boolean 值。
        /// </summary>
        /// <param name="target">要转化的对象</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>与此实例的值等效的 Boolean 值。</returns>    
        public static bool ToBool(object target, bool defaultValue)
        {
            IConvertible tmp = target as IConvertible;
            if (tmp == null || tmp == DBNull.Value) return false;
            try
            {
                return ((IConvertible)target).ToBoolean(CurrentCulture);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }
        #endregion

        #region ToInt
        /// <summary>
        /// 摘要:使用指定的区域性特定格式设置信息将此实例的值转换为等效的 32 位有符号整数。
        /// </summary>
        /// <param name="value"></param>
        /// <returns>与此实例的值等效的 32 位有符号整数。类型转化失败为0</returns>
        public static int ToInt(object target)
        {
            return ToInt(target, 0);
        }

        /// <summary>
        /// 摘要:使用指定的区域性特定格式设置信息将此实例的值转换为等效的 32 位有符号整数。
        /// </summary>
        /// <param name="target">要转换的对象</param>
        /// <param name="defaultValue">如果转化失败，返回默认值</param>
        /// <returns>与此实例的值等效的 32 位有符号整数。</returns>
        public static int ToInt(object target, int defaultValue)
        {
            IConvertible tmp = target as IConvertible;
            if (tmp == null || tmp == DBNull.Value) return default(Int32);
            try
            {
                return tmp.ToInt32(CurrentCulture);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        public static short ToInt16(string target, short defaultValue)
        {
            short retValue = 0;
            bool flag = Int16.TryParse(target, out retValue);
            if (flag)
            {
                return retValue;
            }
            else
            {
                return defaultValue;
            }
        }

        public static short ToInt16(object target, short defaultValue)
        {
            IConvertible tmp = target as IConvertible;
            if (tmp == null || tmp == DBNull.Value) return default(short);
            try
            {
                return tmp.ToInt16(CurrentCulture);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        #endregion

        #region CurrentCulture
        /// <summary>
        /// 摘要:
        ///     获取表示当前线程使用的区域性的 System.Globalization.CultureInfo。
        /// </summary>
        /// <remarks>shaoshuai add</remarks>       
        private static CultureInfo CurrentCulture
        {
            get
            {
                return System.Globalization.CultureInfo.CurrentCulture;
            }
        }
        #endregion

        #region ToDouble
        /// <summary>
        /// 使用指定的区域性特定格式设置信息将此实例的值转换为等效的双精度浮点数字。
        /// </summary>
        /// <param name="target">要转化的目标对象</param>
        /// <returns> 与此实例的值等效的双精度浮点数字。类型转化失败返回0</returns>
        public static double ToDouble(object target)
        {
            return ToDouble(target, 0);
        }

        /// <summary>
        /// 使用指定的区域性特定格式设置信息将此实例的值转换为等效的双精度浮点数字。
        /// </summary>
        /// <param name="target">要转化的目标对象</param>
        /// <param name="defaultValue">设置默认的值</param>
        /// <returns> 与此实例的值等效的双精度浮点数字。类型转化失败返回设置的默认值</returns>
        public static double ToDouble(object target, double defaultValue)
        {
            IConvertible tmp = target as IConvertible;
            if (tmp == null || tmp == DBNull.Value) return 0;
            try
            {
                return tmp.ToDouble(CultureInfo.CurrentCulture);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        #endregion

        #region ToFloat
        /// <summary>
        /// 使用指定的区域性特定格式设置信息将此实例的值转换为等效的单精度浮点数字。
        /// </summary>
        /// <param name="target">要转化的实例</param>
        /// <returns>与此实例的值等效的单精度浮点数字。</returns>

        public static float ToFloat(object target)
        {
            return ToFloat(target, 0);
        }

        /// <summary>
        /// 使用指定的区域性特定格式设置信息将此实例的值转换为等效的单精度浮点数字。
        /// </summary>
        /// <param name="target">要转化的实例</param>
        /// <param name="defaultValue">转化失败返回的默认值</param>
        /// <returns>与此实例的值等效的单精度浮点数字。</returns>
        public static float ToFloat(object target, float defaultValue)
        {
            IConvertible tmp = target as IConvertible;
            if (tmp == null || tmp == DBNull.Value) return 0;
            try
            {
                return tmp.ToSingle(CultureInfo.CurrentCulture);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }
        #endregion

        #region ToDateTime
        /// <summary>
        /// 摘要:
        ///    使用指定的区域性特定格式设置信息将此实例的值转换为等效的 System.DateTime。
        /// </summary>
        /// <param name="target">要进行转化的实例</param>
        /// <returns>与实例的值等效的日期时间类型值</returns>
        public static DateTime ToDateTime(object target)
        {
            return ToDateTime(target, DateTime.Now);
        }


        /// <summary>
        /// 摘要:
        ///    使用指定的区域性特定格式设置信息将此实例的值转换为等效的 System.DateTime。
        /// </summary>
        /// <param name="target">要进行转化的实例</param>
        /// <returns>与实例的值等效的日期时间类型值</returns>
        public static DateTime ToDateTime(object target, DateTime defaultTime)
        {
            try
            {
                return Convert.ToDateTime(target.ToString());
            }
            catch
            {
                return defaultTime;
            }
        }
        #endregion

        #region ToLong
        /// <summary>
        /// 摘要:使用指定的区域性特定格式设置信息将此实例的值转换为等效的 64 位有符号整数。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long ToLong(object target)
        {
            return ToLong(target, 0);
        }


        /// <summary>
        /// 摘要:使用指定的区域性特定格式设置信息将此实例的值转换为等效的 64 位有符号整数。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long ToLong(object target, long defaultValue)
        {
            try
            {
                IConvertible tmp = target as IConvertible;
                return (tmp == null || System.DBNull.Value == target)
                    ? defaultValue : ((IConvertible)target).ToInt64(CultureInfo.CurrentCulture);
            }
            catch
            {
                return defaultValue;
            }
        }
        #endregion

        #region ToGuid
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Guid ToGuid(object value)
        {
            if (value == null || value == DBNull.Value)
                return Guid.Empty;
            Guid guid = new Guid(value.ToString());
            return guid;

        }
        #endregion

        #region ToDecimal
        /// <summary>
        /// 摘要:
        ///     使用指定的区域性特定格式设置信息将此实例的值转换为等效的 System.Decimal 数字。
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>

        public static decimal ToDecimal(object target)
        {
            return ToDecimal(target, 0);
        }


        /// <summary>
        /// 摘要:
        ///     使用指定的区域性特定格式设置信息将此实例的值转换为等效的 System.Decimal 数字。
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>

        public static decimal ToDecimal(object target, decimal defaultValue)
        {
            try
            {
                IConvertible tmp = target as IConvertible;
                return (tmp == null || System.DBNull.Value == target) ? defaultValue :
                    ((IConvertible)target).ToDecimal(CultureInfo.CurrentCulture);
            }
            catch
            {
                return defaultValue;
            }
        }
        #endregion

        #endregion

        #region 类型判断

        #region int
        /// <summary>
        /// 检测字符串是否为整数
        /// </summary>
        /// <param name="strInteger"></param>
        /// <returns></returns>
        public static bool IsInteger(string strInteger)
        {
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"^[-+]?\d+$", System.Text.RegularExpressions.RegexOptions.Compiled);
            return reg.IsMatch(strInteger);
        }
        #endregion

        /// <summary>
        /// 判断对象是否为Int32类型的数字
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public static bool IsNumeric(object expression)
        {
            string expressionString = ToString(expression);
            if (!string.IsNullOrEmpty(expressionString))
            {
                string str = expressionString;
                if (str.Length > 0 && str.Length <= 11 && Regex.IsMatch(str, @"^[-]?[0-9]*[.]?[0-9]*$"))
                {
                    if ((str.Length < 10) || (str.Length == 10 && str[0] == '1') || (str.Length == 11 && str[0] == '-' && str[1] == '1'))
                    {
                        return true;
                    }
                }
            }
            return false;

        }

        /// <summary>
        /// 是否为Double类型
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static bool IsDouble(object expression)
        {
            string expressionString = ToString(expression);
            if (!string.IsNullOrEmpty(expressionString))
            {
                return Regex.IsMatch(expressionString, @"^([0-9])[0-9]*(\.\w*)?$");
            }
            return false;
        }

        /// <summary>
        /// 判断给定的字符串数组(strNumber)中的数据是不是都为数值型
        /// </summary>
        /// <param name="strNumber">要确认的字符串数组</param>
        /// <returns>是则返加true 不是则返回 false</returns>
        public static bool IsNumericArray(string[] strNumber)
        {
            if (strNumber == null)
            {
                return false;
            }
            if (strNumber.Length < 1)
            {
                return false;
            }
            foreach (string id in strNumber)
            {
                if (!IsNumeric(id))
                {
                    return false;
                }
            }
            return true;

        }
        #endregion

        public static string ToDayMaxTime(DateTime time)
        {
            return time.ToString("yyyy-MM-dd") + " 23:59:59.999";
        }

        public static string ToDayMinTime(DateTime time)
        {
            return time.ToString("yyyy-MM-dd") + " 00:00:00.000";
        }

        public static IList<int> ToListInt(string[] strArray)
        {
            IList<int> intList = new List<int>();
            if (strArray.Length > 0)
            {
                int itemValue = 0;
                foreach (string strItem in strArray)
                {
                    itemValue = 0;
                    Int32.TryParse(strItem, out itemValue);
                    if (itemValue > 0)
                    {
                        intList.Add(itemValue);
                    }
                }
            }
            return intList;
        }


        public static DbType GetDbType(object obj)
        {
            DbType dbt;
            if (!Enum.TryParse(obj.GetType().Name, out dbt))
            {
                dbt = DbType.Object;
            }
            return dbt;
        }



    }
}
