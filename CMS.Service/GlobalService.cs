using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using CMS.Domain;
using CMS.CommonLib.Utils;
using CMS.DataAccess;
using CMS.DataAccess.SQLHelper;
using CMS.DataAccess.Interface;

namespace CMS.Service
{
    /// <summary>
    /// 全局变量
    /// </summary>
    public class GlobalService
    {
        # region add update delete select

        /// <summary>
        /// add global
        /// </summary>
        /// <param name="globalInfo"></param>
        /// <returns></returns>
        public bool AddGlobalVar(GlobalVariable globalInfo)
        {
            IGlobalVariableDao globalDao = CastleContext.Instance.GetService<IGlobalVariableDao>();
            globalDao.Insert(globalInfo);
            return true;
        }

        /// <summary>
        /// update global
        /// </summary>
        /// <param name="globalInfo"></param>
        /// <returns></returns>
        public bool UpdateGlobalVar(GlobalVariable globalInfo)
        {
            IGlobalVariableDao globalDao = CastleContext.Instance.GetService<IGlobalVariableDao>();
            globalDao.Update(globalInfo);
            return true;
        }

        /// <summary>
        /// delete global
        /// </summary>
        /// <param name="globalInfo"></param>
        /// <returns></returns>
        public bool DeleteGlobalVar(GlobalVariable globalInfo)
        {
            IGlobalVariableDao globalDao = CastleContext.Instance.GetService<IGlobalVariableDao>();
            globalDao.Delete(globalInfo);
            return true;
        }

        /// <summary>
        /// select by ID
        /// </summary>
        /// <param name="globalID"></param>
        /// <returns></returns>
        public GlobalVariable GeGlobalInfo(long globalID)
        {
            IGlobalVariableDao globalDao = CastleContext.Instance.GetService<IGlobalVariableDao>();
            return globalDao.Find(globalID);
        }

        # endregion

        # region GetGlobalVarList

        /// <summary>
        /// get globalvar list
        /// </summary>
        /// <param name="pageSize">每页条数</param>
        /// <param name="pageIndex">当前页码</param>
        /// <returns></returns>
        public PagerModel<GlobalVariable> GetPagerGlobalVarList(int pageSize, int pageIndex)
        {
            PagerModel<GlobalVariable> globalVarPagerEntity = new PagerModel<GlobalVariable>();
            using (SQLHelper dao = new SQLHelper())
            {
                string where = "IsDeleted=0 ";

                int totalCount;
                int totalPager;
                DataSet dsGlobalVarList = dao.PageingQuery("GlobalVariable", "GlobalID,GlobalName,EnName,IsInclude,AddDate", "GlobalID", where, "", 2, pageSize, pageIndex, out totalCount, out totalPager);
                globalVarPagerEntity.PageSize = pageSize;
                globalVarPagerEntity.CurrentPage = pageIndex;
                globalVarPagerEntity.TotalRecords = totalCount;
                globalVarPagerEntity.ItemList = MakeGlobalVarList(dsGlobalVarList);
            }
            return globalVarPagerEntity;
        }

        /// <summary>
        /// Convert DataSet To IList<NewsModel/>
        /// </summary>
        /// <param name="dsGlobalList"></param>
        /// <returns></returns>
        private IList<GlobalVariable> MakeGlobalVarList(DataSet dsGlobalList)
        {
            IList<GlobalVariable> globalVarList = new List<GlobalVariable>(10);            
            foreach (DataRow row in dsGlobalList.Tables[0].Rows)
            {
                GlobalVariable globalVarEntity = new GlobalVariable();
                globalVarEntity.GlobalID = TypeParse.ToLong(row["GlobalID"]);
                globalVarEntity.GlobalName = row["GlobalName"].ToString();
                globalVarEntity.EnName = row["EnName"].ToString();
                globalVarEntity.IsInclude = TypeParse.ToBool(row["IsInclude"]);
                globalVarEntity.AddDate = TypeParse.ToDateTime(row["AddDate"]);
                globalVarList.Add(globalVarEntity);
            }
            return globalVarList;
        }

        # endregion

        /// <summary>
        /// check enname is exists
        /// </summary>
        /// <param name="enName"></param>
        /// <returns></returns>
        public bool CheckEnName(string enName)
        {
            IGlobalVariableDao globalVarDao = CastleContext.Instance.GetService<IGlobalVariableDao>();
            IList<GlobalVariable> globalList = globalVarDao.FindByEnName(enName);
            if (globalList != null && globalList.Count > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// check enname is exists
        /// </summary>
        /// <param name="enName"></param>
        /// <param name="globalID"></param>
        /// <returns></returns>
        public bool CheckEnName(string enName,long? globalID)
        {
            string sql = "SELECT GlobalID FROM dbo.GlobalVariable WITH(NOLOCK) WHERE EnName=@EnName";
            int count = 1;
            if (globalID != null)
            {
                sql += " AND GlobalID!=@GlobalID";
                count = 2;
            }
            SqlParameter[] parameters = new SqlParameter[count];
            parameters[0] = new SqlParameter("@EnName",enName);
            if (globalID != null)
            {
                parameters[1] = new SqlParameter("@GlobalID", globalID.Value);
            }
            bool flag = false;
            using (SQLHelper dao = new SQLHelper())
            {
                SqlDataReader reader = dao.ExecuteReader(CommandType.Text,sql,parameters);
                if (reader.Read())
                {
                    flag = true;
                }
                reader.Close();
            }
            return flag;
        }
    }
}
