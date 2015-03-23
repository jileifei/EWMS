using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMS.CommonLib.Extension
{
    public static class IListExtension
    {
        public static string CToString(this IList<int> listA)
        {
            string result = "";
            foreach (int item in listA)
            {
                result += item + ",";
            }
            if (result.Length > 0)
            {
                result = result.Substring(0, result.Length - 1);
            }
            return result;
        }

        public static string CToString(this IList<long> listA)
        {
            string result = "";
            foreach (long item in listA)
            {
                result += item + ",";
            }
            if (result.Length > 0)
            {
                result = result.Substring(0, result.Length - 1);
            }
            return result;
        }

        #region 类型转换 小石 2011/8/29
        /// <summary>
        /// 获取与指定的键相关联的值，如果没有则返回输入的默认值
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dic"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static TValue GetValue<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key, TValue defaultValue=default(TValue))
        {
            if (dic == null || dic.Count == 0)
                return defaultValue;
            return dic.ContainsKey(key) ? dic[key] : defaultValue;
        }

        /// <summary>
        /// 获取与指定的键相关联的值，如果没有则返回输入的默认值
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dic"></param>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static TValue GetValue<TKey, TValue>(this IDictionary<TKey, TValue> dic, object obj, TValue defaultValue = default(TValue))
        {
            if (dic == null || dic.Count == 0 || obj == null)
                return defaultValue;
            TKey key = obj.TryParse<TKey>(default(TKey));
            if (key == null)
                return defaultValue;
            return dic.ContainsKey(key) ? dic[key] : defaultValue;
        }
      
        /// <summary>
        /// 转换字符串为指定类型
        /// </summary>
        /// <typeparam name="T">类型(int,double等)</typeparam>
        /// <param name="str">待转换字符串</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static T TryParse<T>(this string str, T defaultValue = default(T))
        {
            if (str == null) return defaultValue;
            str = str.Trim();
            if (string.IsNullOrWhiteSpace(str)) return defaultValue;
            try
            {
                defaultValue = (T)ChangeType(str, typeof(T));
            }
            catch { }
            return defaultValue;
        }

        /// <summary>
        /// 转换object类型为指定类型
        /// </summary>
        /// <typeparam name="T">类型(int,double等)</typeparam>
        /// <param name="obj">待转对象</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static T TryParse<T>(this object obj, T defaultValue = default(T))
        {
            if (obj == null) return defaultValue;
            string str = obj.ToString();
            if (string.IsNullOrWhiteSpace(str)) return defaultValue;
            try
            {
                defaultValue = (T)ChangeType(obj, typeof(T));
            }
            catch
            { }
            return defaultValue;
        }

        /// <summary>
        /// 处理可空类型
        /// </summary>
        /// <param name="value"></param>
        /// <param name="conversionType"></param>
        /// <returns></returns>
        public static object ChangeType(object value, Type conversionType)
        {
            if (conversionType.IsGenericType &&
                conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {

                if (value == null)
                    return null;
                System.ComponentModel.NullableConverter nullableConverter
                    = new System.ComponentModel.NullableConverter(conversionType);
                conversionType = nullableConverter.UnderlyingType;
            }
            return Convert.ChangeType(value, conversionType);
        }
        #endregion
    }
}
