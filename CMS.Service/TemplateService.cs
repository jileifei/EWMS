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
    public class TemplateService
    {
        # region add update delete select

        /// <summary>
        /// add template
        /// </summary>
        /// <param name="templateInfo"></param>
        /// <returns></returns>
        public bool AddTemplate(TemplateInfo templateInfo)
        {
            ITemplateInfoDao templateDao = CastleContext.Instance.GetService<ITemplateInfoDao>();
            templateDao.Insert(templateInfo);
            return true;
        }

        /// <summary>
        /// update template
        /// </summary>
        /// <param name="templateInfo"></param>
        /// <returns></returns>
        public bool UpdateTemplate(TemplateInfo templateInfo)
        {
            ITemplateInfoDao templateDao = CastleContext.Instance.GetService<ITemplateInfoDao>();

            # region 保存模板备份

            TemplateInfo oldTemplate = templateDao.Find(templateInfo.ID);

            ITemplateBackupDao templateBackDao = CastleContext.Instance.GetService<ITemplateBackupDao>();
            TemplateBackup tpBackupEntity = new TemplateBackup();
            tpBackupEntity.TemplateID = oldTemplate.ID;
            tpBackupEntity.TemplateCode = oldTemplate.TemplateCode;
            tpBackupEntity.CreateTime = DateTime.Now;
            templateBackDao.Insert(tpBackupEntity);

            # endregion

            templateDao.Update(templateInfo);
            return true;
        }

        /// <summary>
        /// delete template
        /// </summary>
        /// <param name="templateInfo"></param>
        /// <returns></returns>
        public bool DeleteTemplate(TemplateInfo templateInfo)
        {
            ITemplateInfoDao templateDao = CastleContext.Instance.GetService<ITemplateInfoDao>();
            templateDao.Delete(templateInfo);
            return true;
        }

        /// <summary>
        /// select by ID
        /// </summary>
        /// <param name="templateID"></param>
        /// <returns></returns>
        public TemplateInfo GeTemplateInfo(long templateID)
        {
            ITemplateInfoDao templateDao = CastleContext.Instance.GetService<ITemplateInfoDao>();
            return templateDao.Find(templateID);
        }

        # endregion

        # region GetTemplateList

        /// <summary>
        /// get template list
        /// </summary>
        /// <param name="type"></param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public PagerModel<TemplateInfo> GetPagerTemplateList(long? id, string name, int? type, int pageSize, int pageIndex)
        {
            PagerModel<TemplateInfo> globalVarPagerEntity = new PagerModel<TemplateInfo>();
            int totalPager;
            using (SQLHelper dao = new SQLHelper())
            {
                string where = "1=1 ";
                if (id != null && id.Value > 0)
                {
                    where += " AND ID=" + id;
                }
                if (!string.IsNullOrEmpty(name))
                {
                    where += " AND Name LIKE '%" + Utils.CleanInput(name) + "%'";
                }
                if (type != null && type.Value > 0)
                {
                    where += " AND Type=" + type;
                }
                int totalCount;
                DataSet dsTemplateList = dao.PageingQuery("TemplateInfo", "ID,Name,[Type],Status,CreateTime", "ID", where, "", 2, pageSize, pageIndex, out totalCount, out totalPager);
                globalVarPagerEntity.PageSize = pageSize;
                globalVarPagerEntity.CurrentPage = pageIndex;
                globalVarPagerEntity.TotalRecords = totalCount;
                globalVarPagerEntity.ItemList = MakeTemplateList(dsTemplateList);
            }
            return globalVarPagerEntity;
        }

        /// <summary>
        /// Convert DataSet To IList<NewsModel/>
        /// </summary>
        /// <param name="dsTemplateList"></param>
        /// <returns></returns>
        private IList<TemplateInfo> MakeTemplateList(DataSet dsTemplateList)
        {
            IList<TemplateInfo> templateList = new List<TemplateInfo>(10);

            foreach (DataRow row in dsTemplateList.Tables[0].Rows)
            {
                TemplateInfo templateEntity = new TemplateInfo
                {
                    ID = TypeParse.ToLong(row["ID"]),
                    Name = row["Name"].ToString(),
                    Type = TypeParse.ToInt(row["Type"])
                };
                switch (templateEntity.Type)
                {
                    case 1:
                        templateEntity.TypeName = "栏目模板";
                        break;
                    case 2:
                        templateEntity.TypeName = "新闻模板";
                        break;
                    case 3:
                        templateEntity.TypeName = "列表页模板";
                        break;
                    case 4:
                        templateEntity.TypeName = "专题模板";
                        break;
                    case 5:
                        templateEntity.TypeName = "其他模板";
                        break;
                    case 6:
                        templateEntity.TypeName = "区块模版";
                        break;
                }
                templateEntity.Status = TypeParse.ToInt(row["Status"]);
                switch (templateEntity.Status)
                {
                    case 0:
                        templateEntity.StatusName = "停用";
                        break;
                    case 1:
                        templateEntity.StatusName = "启用";
                        break;
                    case 2:
                        templateEntity.StatusName = "删除";
                        break;
                }
                templateEntity.CreateTime = TypeParse.ToDateTime(row["CreateTime"]);
                templateList.Add(templateEntity);
            }
            return templateList;
        }

        # endregion

        # region get template dictionary

        /// <summary>
        /// get template dictionary
        /// </summary>
        /// <returns></returns>
        public IDictionary<long, string> GetDicTemplateList()
        {
            IDictionary<long, string> dicTemplateList = new Dictionary<long, string>(10);
            ITemplateInfoDao templateDAO = CastleContext.Instance.GetService<ITemplateInfoDao>();
            IList<TemplateInfo> templateList = templateDAO.FindAll();
            foreach (TemplateInfo item in templateList)
            {
                if (!dicTemplateList.ContainsKey(item.ID))
                {
                    dicTemplateList.Add(item.ID, item.Name);
                }
            }
            return dicTemplateList;
        }

        # endregion

        /// <summary>
        /// check Name is exists
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool CheckName(string name)
        {
            ITemplateInfoDao templateDao = CastleContext.Instance.GetService<ITemplateInfoDao>();
            IList<TemplateInfo> templateList = templateDao.FindByName(name);
            if (templateList != null && templateList.Count > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// check name is exists
        /// </summary>
        /// <param name="name"></param>
        /// <param name="templateID"></param>
        /// <returns></returns>
        public bool CheckName(string name, long? templateID)
        {
            string sql = "SELECT ID FROM dbo.TemplateInfo WITH(NOLOCK) WHERE Name=@Name";
            int count = 1;
            if (templateID != null)
            {
                sql += " AND ID!=@TemplateID";
                count = 2;
            }
            SqlParameter[] parameters = new SqlParameter[count];
            parameters[0] = new SqlParameter("@Name", name);
            if (templateID != null)
            {
                parameters[1] = new SqlParameter("@TemplateID", templateID.Value);
            }
            bool flag = false;
            using (SQLHelper dao = new SQLHelper())
            {
                SqlDataReader reader = dao.ExecuteReader(CommandType.Text, sql, parameters);
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
