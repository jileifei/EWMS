using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text.RegularExpressions;

using CMS.Domain;
using CMS.DataAccess.SQLHelper;

namespace CMS.Template
{
    public class BaseTemplate
    {
        /// <summary>
        /// Key list
        /// </summary>
        private Dictionary<string, SQLHelper> connectionDict = new Dictionary<string, SQLHelper>();
        
        /// <summary>
        /// template hashtable
        /// </summary>
        private Hashtable hashTable = new Hashtable();

        /// <summary>
        /// 获得使用putsql放入的内容
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IList<TemplateDoc> GetDoc(string key)
        {
            return hashTable[key] as IList<TemplateDoc>;
        }
        
        /*
        /// <summary>
        /// return dao object
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public SQLHelper getDAO(string key)
        {
              if ( !connectionDict.ContainsKey(key) ){
                  connectionDict[key] = new SQLHelper(ConfigurationManager.ConnectionStrings[key].ToString());
              }
              
              return connectionDict[key];
        }
         */
       
        /*
        /// <summary>
        /// return template hashtable
        /// </summary>
        public Hashtable HashResult()
        {
            CloseDAO();
            return hashTable;   
        }*/

        /// <summary>
        /// 解析html中出现的第一个url,必须是包含单引号的url
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public string ExtractFirstUrl(string html)
        {
            string url = string.Empty;
            Regex regex = new Regex(".*href=\"(?<url>.*?)\">");
            MatchCollection mc = regex.Matches(html);
            if (mc.Count > 0)
            {
                Match m = mc[0];
                url = m.Groups["url"].Value;
            }
            return url;
        }

        
        /// <summary>
        /// put object value to hashtable
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void PutData(string key, object value)
        {
            if (hashTable.ContainsKey(key))
            {
                hashTable[key] = value;
            }
            else
            {
                hashTable.Add(key,value);
            }
        }
        
        /// <summary>
        /// put string value to hashtable
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void PutData(string key, string value)
        {
            if (hashTable.ContainsKey(key))
            {
                hashTable[key] = value;
            }
            else
            {
                hashTable.Add(key, value);
            }
        }
        
        /// <summary>
        /// put dataset value to hashtable
        /// </summary>
        /// <param name="key"></param>
        /// <param name="ds"></param>
        public void PutData(string key, DataSet ds)
        {
            if (hashTable.ContainsKey(key))
            {
                hashTable[key] = EntityConv.LoadDataSet(ds);
            }
            else
            {
                hashTable.Add(key, EntityConv.LoadDataSet(ds));
            }
        }

        /*
        /// <summary>
        /// put sql exec result to hashtable
        /// </summary>
        /// <param name="key"></param>
        /// <param name="sql"></param>
        public void PutSql(string key,string conKey, string sql)
        {
            if (hashTable.ContainsKey(key))
            {
                hashTable[key] = EntityConv.LoadDataSet(RunSQL(conKey, sql));
            }
            else
            {
                hashTable.Add(key, EntityConv.LoadDataSet(RunSQL(conKey, sql)));
            }
        }
         */

        /// <summary>
        /// 根据SQL得到List TemplateDoc
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static IList<TemplateDoc> GetSqlResult(string sql)
        {
            IList<TemplateDoc> listValue;
            using (SQLHelper sqlHelper = new SQLHelper())
            {
                DataSet dsResult = sqlHelper.ExecuteDataset(sql);
                listValue = EntityConv.LoadDataSet(dsResult);
            }
            return listValue;
        }

        /// <summary>
        /// 根据SQL得到List TemplateDoc
        /// </summary>
        /// <returns></returns>
        public static IList<TemplateDoc> GetSqlResult(DataSet ds)
        {
            IList<TemplateDoc> listValue = EntityConv.LoadDataSet(ds);
            return listValue;
        }

        public string SerUrlEncode(string value)
        {
            return CommonLib.Utils.Utils.UrlEncode(value);
        }
    
        /*
        /// <summary>
        /// run sql get dataset
        /// </summary>
        /// <param name="connectionKey"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataSet RunSQL(string connectionKey, string sql)
        {
              return getDAO(connectionKey).ExecuteDataset(sql);
        }
        */

        # region IList<T> 转换成 DataSet

        /// <summary>
        /// Ilist<T/> 转换成 DataSet
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public DataSet ConvertToDataSet<T>(IList<T> list)
        {
            if (list == null || list.Count <= 0)
            {
                return null;
            }

            DataSet ds = new DataSet();
            DataTable dt = new DataTable(typeof(T).Name);
            DataColumn column;
            DataRow row;

            PropertyInfo[] myPropertyInfo = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (T t in list)
            {
                if (t == null)
                {
                    continue;
                }

                row = dt.NewRow();

                for (int i = 0, j = myPropertyInfo.Length; i < j; i++)
                {
                    PropertyInfo pi = myPropertyInfo[i];
                    if (pi.PropertyType.Name != "Nullable`1")
                    {
                        string name = pi.Name;

                        if (dt.Columns[name] == null)
                        {
                            column = new DataColumn(name, pi.PropertyType);
                            dt.Columns.Add(column);
                        }

                        row[name] = pi.GetValue(t, null);
                    }
                }

                dt.Rows.Add(row);
            }

            ds.Tables.Add(dt);

            return ds;
        }

        # endregion

        public void PutSearch(string tKey, string searchKey, int pageSize)
        {
        }

        /// <summary>
        /// close dao object
        /// </summary>
        public void CloseDAO()
        {
            foreach (SQLHelper dao in connectionDict.Values)
            {
                if (dao != null)
                {
                    try
                    {
                        dao.Dispose();
                    }
                    catch { }
                }
            }
        }
    }
}
