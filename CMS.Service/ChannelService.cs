using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using CMS.Domain;
using CMS.CommonLib.Utils;
using CMS.DataAccess;
using CMS.DataAccess.SQLHelper;
using CMS.DataAccess.Interface;

namespace CMS.Service
{
    public class ChannelService
    {
        # region get all chiled nodeid

        /// <summary>
        /// 获取指定节点的所有子节点ID
        /// </summary>
        /// <returns></returns>
        public IList<long> GetAllChildID(long channelID)
        {
            IList<long> listAllChildID = new List<long>();
            IChannelInfoDao channelDao = CastleContext.Instance.GetService<IChannelInfoDao>();
            IList<ChannelInfo> rootChannelList = channelDao.FindByParentChannelID(channelID);
            foreach (ChannelInfo childeItem in rootChannelList)
            {
                listAllChildID.Add(childeItem.ID);
                GetChannelID(childeItem, ref listAllChildID);
            }
            return listAllChildID;
        }

        /// <summary>
        /// 获取子节点
        /// </summary>
        /// <param name="node"></param>
        /// <param name="listAllChildID"></param>
        /// <returns></returns>
        private void GetChannelID(ChannelInfo node, ref IList<long> listAllChildID)
        {
            IChannelInfoDao channelDao = CastleContext.Instance.GetService<IChannelInfoDao>();
            IList<ChannelInfo> childChannelList = channelDao.FindByParentChannelID(node.ID);
            foreach (ChannelInfo childeItem in childChannelList)
            {
                listAllChildID.Add(childeItem.ID);
                GetChannelID(childeItem, ref listAllChildID);
            }
        }

        public IList<ChannelInfo> GetParentChannelList()
        {
            IChannelInfoDao channelDao = CastleContext.Instance.GetService<IChannelInfoDao>();
            IList<ChannelInfo> rootChannelList = channelDao.FindByParentChannelID(0);
            return rootChannelList;
        }

        # endregion

        # region tree

        /// <summary>
        /// 获取栏目树
        /// </summary>
        /// <returns></returns>
        public DynatreeModel GetChannelTreeList()
        {
            DynatreeModel dynatreeRootNode = new DynatreeModel();
            dynatreeRootNode.key = "0";
            dynatreeRootNode.title = "栏目管理";
            dynatreeRootNode.expand = true;

            IChannelInfoDao channelDao = CastleContext.Instance.GetService<IChannelInfoDao>();
            IList<ChannelInfo> rootChannelList = channelDao.FindByParentChannelID(0);
            List<DynatreeModel> rootList = MakeDynatreeModelList(rootChannelList);
            foreach (DynatreeModel childeItem in rootList)
            {
                dynatreeRootNode.children.Add(childeItem);
                childeItem.children = GetChannelTreeNode(childeItem);
            }
            if (rootList.Count > 0)
            {
                dynatreeRootNode.isFolder = true;
            }
            return dynatreeRootNode;
        }

        /// <summary>
        /// 获取子节点
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private List<DynatreeModel> GetChannelTreeNode(DynatreeModel node)
        {
            IChannelInfoDao channelDao = CastleContext.Instance.GetService<IChannelInfoDao>();
            IList<ChannelInfo> childChannelList = channelDao.FindByParentChannelID(TypeParse.ToInt(node.key));
            if (childChannelList.Count > 0)
            {
                node.isFolder = true;// 有子节点
            }
            List<DynatreeModel> childeList = MakeDynatreeModelList(childChannelList);
            foreach (DynatreeModel childeItem in childeList)
            {
                node.children.Add(childeItem);
                GetChannelTreeNode(childeItem);
            }

            return childeList;
        }

        /// <summary>
        /// convert channelinfo entity list to dynatreenode entity list
        /// </summary>
        /// <param name="channelInfoList">channelinfo entity list</param>
        /// <returns></returns>
        private List<DynatreeModel> MakeDynatreeModelList(IList<ChannelInfo> channelInfoList)
        {
            List<DynatreeModel> listDynatreeNode = new List<DynatreeModel>(1);
            DynatreeModel dynatreeModel;
            foreach (ChannelInfo channelEntity in channelInfoList)
            {
                dynatreeModel = new DynatreeModel();
                dynatreeModel.expand = true;
                dynatreeModel.title = channelEntity.Name;
                dynatreeModel.key = channelEntity.ID.ToString(CultureInfo.InvariantCulture);
                dynatreeModel.ParentID = channelEntity.ParentChannelID ?? 0;
                listDynatreeNode.Add(dynatreeModel);
            }
            return listDynatreeNode;
        }

        # endregion

        # region combotree

        /// <summary>
        /// 获取Combotree
        /// </summary>
        /// <returns></returns>
        public List<ComboTreeNode> GetChannelComboTreeList()
        {
            IChannelInfoDao channelDao = CastleContext.Instance.GetService<IChannelInfoDao>();
            IList<ChannelInfo> rootChannelList = channelDao.FindByParentChannelID(0);
            List<ComboTreeNode> rootList = MakeCombotreeModelList(rootChannelList);
            foreach (ComboTreeNode childeItem in rootList)
            {
                childeItem.children = GetChannelComboTreeNode(childeItem);
            }
            return rootList;
        }

        /// <summary>
        /// 获取子节点
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private List<ComboTreeNode> GetChannelComboTreeNode(ComboTreeNode node)
        {
            IChannelInfoDao channelDao = CastleContext.Instance.GetService<IChannelInfoDao>();
            IList<ChannelInfo> childChannelList = channelDao.FindByParentChannelID(TypeParse.ToInt(node.id));
            List<ComboTreeNode> childeList = MakeCombotreeModelList(childChannelList);
            node.children = childeList;
            foreach (ComboTreeNode childeItem in childeList)
            {
                //node.children.Add(childeItem);
                childeItem.children=GetChannelComboTreeNode(childeItem);
            }

            return childeList;
        }

        /// <summary>
        /// convert channelinfo entity list to dynatreenode entity list
        /// </summary>
        /// <param name="channelInfoList">channelinfo entity list</param>
        /// <returns></returns>
        private List<ComboTreeNode> MakeCombotreeModelList(IList<ChannelInfo> channelInfoList)
        {
            List<ComboTreeNode> listDynatreeNode = new List<ComboTreeNode>(1);
            foreach (ChannelInfo channelEntity in channelInfoList)
            {
                ComboTreeNode combotreeModel = new ComboTreeNode
                {
                    id = channelEntity.ID.ToString(CultureInfo.InvariantCulture),
                    text = channelEntity.Name,
                    url=channelEntity.ChannelUrlPart
                };
                listDynatreeNode.Add(combotreeModel);
            }
            return listDynatreeNode;
        }

        # endregion

        # region add auth combotree

        /// <summary>
        /// 获取Combotree
        /// </summary>
        /// <returns></returns>
        public List<ComboTreeNode> GetChannelComboTreeList(long roleID)
        {
            IList<ChannelInfo> rootChannelList = new List<ChannelInfo>();
            using (SQLHelper dao = new SQLHelper())
            {
                string sql = "SELECT a.ID,a.Name FROM dbo.ChannelInfo a,dbo.ChannelPrivilege b WHERE b.AddAuth=1 AND b.RoleID=@RoleID AND a.ID=b.ChannelID AND a.ParentChannelID=0";
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@RoleID", roleID);
                SqlDataReader reader = dao.ExecuteReader(CommandType.Text, sql, parameters);
                while (reader.Read())
                {
                    ChannelInfo channelInfo = new ChannelInfo
                    {
                        ID = TypeParse.ToLong(reader[reader.GetOrdinal("ID")]),
                        Name = reader[reader.GetOrdinal("Name")].ToString()
                    };
                    rootChannelList.Add(channelInfo);
                }
                reader.Close();
            }
            List<ComboTreeNode> rootList = MakeCombotreeModelList(rootChannelList);
            foreach (ComboTreeNode childeItem in rootList)
            {
                childeItem.children = GetAddAuthChannelComboTreeNode(childeItem, roleID);
            }
            return rootList;
        }

        /// <summary>
        /// 获取子节点
        /// </summary>
        /// <param name="node"></param>
        /// <param name="roleID"></param>
        /// <returns></returns>
        private List<ComboTreeNode> GetAddAuthChannelComboTreeNode(ComboTreeNode node, long roleID)
        {
            IList<ChannelInfo> childChannelList = new List<ChannelInfo>();
            using (SQLHelper dao = new SQLHelper())
            {
                string sql = "SELECT a.ID,a.Name FROM dbo.ChannelInfo a,dbo.ChannelPrivilege b WHERE b.AddAuth=1 AND b.RoleID=@RoleID AND a.ID=b.ChannelID AND a.ParentChannelID=@PID";
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@RoleID", roleID);
                parameters[1] = new SqlParameter("@PID", node.id);
                SqlDataReader reader = dao.ExecuteReader(CommandType.Text, sql, parameters);
                while (reader.Read())
                {
                    ChannelInfo channelInfo = new ChannelInfo();
                    channelInfo.ID = TypeParse.ToLong(reader[reader.GetOrdinal("ID")]);
                    channelInfo.Name = reader[reader.GetOrdinal("Name")].ToString();
                    childChannelList.Add(channelInfo);
                }
                reader.Close();
            }
            List<ComboTreeNode> childeList = MakeCombotreeModelList(childChannelList);
            foreach (ComboTreeNode childeItem in childeList)
            {
                node.children.Add(childeItem);
                GetAddAuthChannelComboTreeNode(childeItem, roleID);
            }

            return childeList;
        }

        # endregion

        # region brower auth combotree

        /// <summary>
        /// 获取Combotree
        /// </summary>
        /// <returns></returns>
        public List<ComboTreeNode> GetBrowseChannelComboTreeList(long roleID)
        {
            IList<ChannelInfo> rootChannelList = new List<ChannelInfo>();
            using (SQLHelper dao = new SQLHelper())
            {
                const string sql = "SELECT a.ID,a.Name FROM dbo.ChannelInfo a,dbo.ChannelPrivilege b WHERE b.IsBrowse=1 AND b.RoleID=@RoleID AND a.ID=b.ChannelID AND a.ParentChannelID=0";
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@RoleID", roleID);
                SqlDataReader reader = dao.ExecuteReader(CommandType.Text, sql, parameters);
                while (reader.Read())
                {
                    ChannelInfo channelInfo = new ChannelInfo
                    {
                        ID = TypeParse.ToLong(reader[reader.GetOrdinal("ID")]),
                        Name = reader[reader.GetOrdinal("Name")].ToString()
                    };
                    rootChannelList.Add(channelInfo);
                }
                reader.Close();
            }
            List<ComboTreeNode> rootList = MakeCombotreeModelList(rootChannelList);
            foreach (ComboTreeNode childeItem in rootList)
            {
                childeItem.children = GetBrowseAuthChannelComboTreeNode(childeItem, roleID);
            }
            return rootList;
        }

        /// <summary>
        /// 获取子节点
        /// </summary>
        /// <param name="node"></param>
        /// <param name="roleID"></param>
        /// <returns></returns>
        private List<ComboTreeNode> GetBrowseAuthChannelComboTreeNode(ComboTreeNode node, long roleID)
        {
            IList<ChannelInfo> childChannelList = new List<ChannelInfo>();
            using (SQLHelper dao = new SQLHelper())
            {
                string sql = "SELECT a.ID,a.Name FROM dbo.ChannelInfo a,dbo.ChannelPrivilege b WHERE b.IsBrowse=1 AND b.RoleID=@RoleID AND a.ID=b.ChannelID AND a.ParentChannelID=@PID";
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@RoleID", roleID);
                parameters[1] = new SqlParameter("@PID", node.id);
                SqlDataReader reader = dao.ExecuteReader(CommandType.Text, sql, parameters);
                while (reader.Read())
                {
                    ChannelInfo channelInfo = new ChannelInfo
                    {
                        ID = TypeParse.ToLong(reader[reader.GetOrdinal("ID")]),
                        Name = reader[reader.GetOrdinal("Name")].ToString()
                    };
                    childChannelList.Add(channelInfo);
                }
                reader.Close();
            }
            List<ComboTreeNode> childeList = MakeCombotreeModelList(childChannelList);
            foreach (ComboTreeNode childeItem in childeList)
            {
                node.children.Add(childeItem);
                GetBrowseAuthChannelComboTreeNode(childeItem, roleID);
            }

            return childeList;
        }

        # endregion

        # region auth tree

        /// <summary>
        /// 获取子节点
        /// </summary>
        /// <param name="node"></param>
        /// <param name="dicAuthChannelList"></param>
        /// <returns></returns>
        private List<DynatreeModel> GetAuthChannelTreeNode(DynatreeModel node, IDictionary<long, ChannelPrivilege> dicAuthChannelList)
        {
            IChannelInfoDao channelDao = CastleContext.Instance.GetService<IChannelInfoDao>();
            IList<ChannelInfo> childChannelList = channelDao.FindByParentChannelID(TypeParse.ToInt(node.key));
            if (childChannelList.Count > 0)
            {
                node.isFolder = true;// 有子节点
            }
            List<DynatreeModel> childeList = MakeAuthDynatreeModelList(childChannelList, dicAuthChannelList);
            foreach (DynatreeModel childeItem in childeList)
            {
                node.children.Add(childeItem);
                GetAuthChannelTreeNode(childeItem, dicAuthChannelList);
            }
            return childeList;
        }

        /// <summary>
        /// 获取栏目树
        /// </summary>
        /// <returns></returns>
        public List<DynatreeModel> GetAuthChannelTreeList(long roleID)
        {
            IChannelPrivilegeDao channelPrivilegeDao = CastleContext.Instance.GetService<IChannelPrivilegeDao>();
            // 得到当前角色有权限的栏目列表 start
            IList<ChannelPrivilege> authChannelList = channelPrivilegeDao.FindByRoleID(roleID);
            IDictionary<long, ChannelPrivilege> dicAuthChannelList = new Dictionary<long, ChannelPrivilege>(1);
            foreach (ChannelPrivilege item in authChannelList)
            {
                if (!dicAuthChannelList.ContainsKey(item.ChannelID))
                {
                    dicAuthChannelList.Add(item.ChannelID, item);
                }
            }
            // end

            IChannelInfoDao channelDao = CastleContext.Instance.GetService<IChannelInfoDao>();
            IList<ChannelInfo> rootChannelList = channelDao.FindByParentChannelID(0);
            List<DynatreeModel> rootList = MakeAuthDynatreeModelList(rootChannelList, dicAuthChannelList);
            foreach (DynatreeModel childeItem in rootList)
            {
                childeItem.children = GetAuthChannelTreeNode(childeItem, dicAuthChannelList);
            }

            return rootList;
        }

        /// <summary>
        /// convert channelinfo entity list to dynatreenode entity list
        /// </summary>
        /// <param name="channelInfoList">channelinfo entity list</param>
        /// <param name="dicAuthChannelList"></param>
        /// <returns></returns>
        private List<DynatreeModel> MakeAuthDynatreeModelList(IEnumerable<ChannelInfo> channelInfoList, IDictionary<long, ChannelPrivilege> dicAuthChannelList)
        {
            List<DynatreeModel> listDynatreeNode = new List<DynatreeModel>(1);
            foreach (ChannelInfo channelEntity in channelInfoList)
            {
                DynatreeModel dynatreeModel = new DynatreeModel
                {
                    expand = true,
                    title = channelEntity.Name,
                    key = channelEntity.ID.ToString(CultureInfo.InvariantCulture),
                    ParentID = channelEntity.ParentChannelID ?? 0
                };
                if (dicAuthChannelList.ContainsKey(channelEntity.ID))
                {
                    dynatreeModel.IsBrowse = dicAuthChannelList[channelEntity.ID].IsBrowse;
                    dynatreeModel.AddAuth = dicAuthChannelList[channelEntity.ID].AddAuth;
                    dynatreeModel.EditAuth = dicAuthChannelList[channelEntity.ID].EditAuth;
                    dynatreeModel.DelAuth = dicAuthChannelList[channelEntity.ID].DelAuth;
                    dynatreeModel.AuditingAuth = dicAuthChannelList[channelEntity.ID].AuditingAuth;
                }
                listDynatreeNode.Add(dynatreeModel);
            }
            return listDynatreeNode;
        }

        # endregion

        # region add edit delete select sort

        /// <summary>
        /// 添加栏目
        /// </summary>
        /// <param name="channelInfo"></param>
        /// <returns></returns>
        public long Add(ChannelInfo channelInfo)
        {
            long channelID;
            IChannelInfoDao channelDao = CastleContext.Instance.GetService<IChannelInfoDao>();
            channelInfo.Sort = channelDao.GetOrderID(channelInfo.ParentChannelID ?? 0) + 1;
            channelDao.Insert(channelInfo);
            channelID = channelInfo.ID;
            return channelID;
        }

        /// <summary>
        /// 修改栏目
        /// </summary>
        /// <param name="channelInfo"></param>
        /// <returns></returns>
        public bool Update(ChannelInfo channelInfo)
        {
            IChannelInfoDao channelDao = CastleContext.Instance.GetService<IChannelInfoDao>();
            channelDao.Update(channelInfo);
            return true;
        }

        /// <summary>
        /// 删除栏目
        /// </summary>
        /// <param name="channelInfo"></param>
        /// <returns></returns>
        public bool Delete(ChannelInfo channelInfo)
        {
            IChannelInfoDao channelDao = CastleContext.Instance.GetService<IChannelInfoDao>();
            channelDao.Delete(channelInfo);
            return true;
        }

        /// <summary>
        /// 得到栏目信息
        /// </summary>
        /// <param name="channelID"></param>
        /// <returns></returns>
        public ChannelInfo GetChannelInfo(long channelID)
        {
            TemplateService tpService = new TemplateService();
            IDictionary<long, string> dicTemplate = tpService.GetDicTemplateList();
            IChannelInfoDao channelDao = CastleContext.Instance.GetService<IChannelInfoDao>();
            ChannelInfo channnelEntity = channelDao.Find(channelID);
            if (channnelEntity != null)
            {
                if (dicTemplate.ContainsKey(channnelEntity.TemplateID))
                {
                    channnelEntity.TemplateName = dicTemplate[channnelEntity.TemplateID];
                }
                if (channnelEntity.ListTemplateID != null && dicTemplate.ContainsKey(channnelEntity.ListTemplateID.Value))
                {
                    channnelEntity.ListTemplateName = dicTemplate[channnelEntity.ListTemplateID.Value];
                }
                if (channnelEntity.ContentTemplateID != null && dicTemplate.ContainsKey(channnelEntity.ContentTemplateID.Value))
                {
                    channnelEntity.ContentTemplateName = dicTemplate[channnelEntity.ContentTemplateID.Value];
                }
            }
            return channnelEntity;
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="sourceChannelID"></param>
        /// <param name="channelID"></param>
        /// <returns></returns>
        public bool Sort(int sourceChannelID, int channelID)
        {
            int result;
            using (SQLHelper helper = new SQLHelper())
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@ChannelID", channelID);
                parameters[1] = new SqlParameter("@SourceChannelID", sourceChannelID);
                result = helper.ExecuteNonQuery(CommandType.StoredProcedure, "UP_ChannelSort", parameters);
            }
            return result > 0;
        }

        # endregion

        # region save role channel auth

        /// <summary>
        /// 保存角色拥有权限的栏目映射关系
        /// </summary>
        /// <param name="channelMapList"></param>
        /// <param name="curUserID"></param>
        /// <returns></returns>
        public void SaveRoleChannelAuth(List<ChannelPrivilege> channelMapList, long curUserID)
        {
            if (channelMapList.Count > 0)
            {
                // 先删除
                using (SQLHelper dao = new SQLHelper())
                {
                    // 启动事务
                    dao.BeginTrans();
                    try
                    {
                        string sqlDel = "DELETE dbo.ChannelPrivilege WHERE RoleID=@RoleID";
                        SqlParameter[] parameters = new SqlParameter[1];
                        parameters[0] = new SqlParameter("@RoleID", channelMapList[0].RoleID);
                        // 先批量删除
                        dao.ExecuteNonQuery(CommandType.Text, sqlDel, parameters);

                        // 开始批量插入
                        DataTable dtChannelAuth = ConstructionChannelAuthDT();
                        foreach (ChannelPrivilege mapItem in channelMapList)
                        {
                            if (mapItem.IsBrowse || mapItem.AddAuth || mapItem.EditAuth || mapItem.DelAuth || mapItem.AuditingAuth)
                            {
                                DataRow row = dtChannelAuth.NewRow();
                                row["RoleID"] = mapItem.RoleID;
                                row["ChannelID"] = mapItem.ChannelID;
                                row["IsBrowse"] = mapItem.IsBrowse;
                                row["AddAuth"] = mapItem.AddAuth;
                                row["EditAuth"] = mapItem.EditAuth;
                                row["DelAuth"] = mapItem.DelAuth;
                                row["AuditingAuth"] = mapItem.AuditingAuth;
                                row["CreateTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                row["CreateUserID"] = curUserID;
                                dtChannelAuth.Rows.Add(row);
                            }
                        }
                        dao.GoBulkCopy(dtChannelAuth, "dbo.ChannelPrivilege");
                        dao.CommitTrans();
                    }
                    catch (Exception)
                    {
                        dao.RollbackTrans();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        ///  构造批量插入的Table
        /// </summary>
        /// <returns></returns>
        private DataTable ConstructionChannelAuthDT()
        {
            DataTable menuAuthTable = new DataTable("ChannelPrivilege");
            DataColumn colMenuAuthID = menuAuthTable.Columns.Add("ID", typeof(int));
            colMenuAuthID.AutoIncrement = true;
            colMenuAuthID.AutoIncrementSeed = 1;
            menuAuthTable.Columns.Add("RoleID", typeof(int));
            menuAuthTable.Columns.Add("ChannelID", typeof(int));
            menuAuthTable.Columns.Add("IsBrowse", typeof(bool));
            menuAuthTable.Columns.Add("AddAuth", typeof(bool));
            menuAuthTable.Columns.Add("EditAuth", typeof(bool));
            menuAuthTable.Columns.Add("DelAuth", typeof(bool));
            menuAuthTable.Columns.Add("AuditingAuth", typeof(bool));
            menuAuthTable.Columns.Add("CreateTime", typeof(DateTime));
            menuAuthTable.Columns.Add("CreateUserID", typeof(int));
            return menuAuthTable;
        }

        # endregion

        # region get channel dictionary

        /// <summary>
        /// get channel dictionary
        /// </summary>
        /// <returns></returns>
        public IDictionary<long, string> GetDicChannelList()
        {
            IDictionary<long, string> dicChannelList = new Dictionary<long, string>(10);
            IChannelInfoDao channelDAO = CastleContext.Instance.GetService<IChannelInfoDao>();
            IList<ChannelInfo> channelList = channelDAO.FindAll();
            foreach (ChannelInfo item in channelList)
            {
                if (!dicChannelList.ContainsKey(item.ID))
                {
                    dicChannelList.Add(item.ID, item.Name);
                }
            }
            return dicChannelList;
        }

        # endregion

        # region 得到同级栏目列表

        public IList<ChannelInfo> GetBrotherList(int channelID)
        {
            IList<ChannelInfo> list = new List<ChannelInfo>(1);
            const string sql = "SELECT ID,Name,EnName,ChannelUrlPart FROM dbo.ChannelInfo WITH(NOLOCK) WHERE Status=1 AND ParentChannelID = (SELECT TOP 1 ParentChannelID FROM dbo.ChannelInfo WITH(NOLOCK) WHERE ID=@ChannelID) ORDER BY Sort";
            using (SQLHelper dao = new SQLHelper())
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@ChannelID", channelID);
                SqlDataReader reader = dao.ExecuteReader(CommandType.Text, sql, parameters);
                while (reader.Read())
                {
                    ChannelInfo itemChannel = new ChannelInfo
                    {
                        ID = TypeParse.ToLong(reader["ID"]),
                        Name = reader["Name"].ToString(),
                        EnName = reader["EnName"].ToString(),
                        ChannelUrlPart = reader["ChannelUrlPart"].ToString()
                    };
                    list.Add(itemChannel);
                }
                reader.Close();
            }
            return list;
        }

        # endregion

        # region 得到下级栏目列表

        public IList<ChannelInfo> GetChildList(long channelID)
        {
            IList<ChannelInfo> list = new List<ChannelInfo>(1);
            const string sql = "SELECT ID,Name,EnName,ChannelUrlPart FROM dbo.ChannelInfo WITH(NOLOCK) WHERE Status=1 AND ParentChannelID =  @ChannelID ORDER BY Sort";
            using (SQLHelper dao = new SQLHelper())
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@ChannelID", channelID);
                SqlDataReader reader = dao.ExecuteReader(CommandType.Text, sql, parameters);
                while (reader.Read())
                {
                    ChannelInfo itemChannel = new ChannelInfo();
                    itemChannel.ID = TypeParse.ToLong(reader["ID"]);
                    itemChannel.Name = reader["Name"].ToString();
                    itemChannel.EnName = reader["EnName"].ToString();
                    itemChannel.ChannelUrlPart = reader["ChannelUrlPart"].ToString();
                    list.Add(itemChannel);
                }
                reader.Close();
            }
            return list;
        }

        # endregion

        /// <summary>
        /// 获取所有分类
        /// </summary>
        /// <returns></returns>
        public IList<ChannelInfo> GetAllChannel()
        {
            IChannelInfoDao channelDao = CastleContext.Instance.GetService<IChannelInfoDao>();
            return channelDao.FindAll();
        }

        /// <summary>
        /// 获取指定分类的根分类ID，如果本身是根分类则返回自己
        /// </summary>
        /// <param name="channelID"></param>
        /// <returns></returns>
        public long GetRootChannelID(long channelID)
        {
            string sql = "select ID,ParentChannelID from ChannelInfo where ID=@ChannelID";
            long rootid = channelID;
            using (SQLHelper dao = new SQLHelper())
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@ChannelID", channelID);
                DataSet ds = dao.ExecuteDataset(CommandType.Text, sql, parameters);
                int count = 0;
                while (ds.Tables[0].Rows.Count > 0)
                {
                    rootid = Convert.ToInt64(ds.Tables[0].Rows[0]["ID"]);
                    long parentid = Convert.ToInt64(ds.Tables[0].Rows[0]["ParentChannelID"]);

                    //root found
                    if (parentid == 0)
                        break;

                    parameters[0] = new SqlParameter("@ChannelID", parentid);
                    ds = dao.ExecuteDataset(CommandType.Text, sql, parameters);
                    count++;

                    //max level less than 10
                    if (count > 10)
                        break;
                }
            }
            return rootid;

        }

        # region check is have oper right

        /// <summary>
        /// 检测用户是否有操作权限
        /// </summary>
        /// <param name="newsID">新闻ID</param>
        /// <param name="roleID">角色ID</param>
        /// <param name="operType">操作类型 1=修改 2=删除 3=审核</param>
        /// <returns></returns>
        public bool CheckNewsIsHaveOperRight(long newsID, long roleID, int operType)
        {
            bool flag = false;
            string sql = @"SELECT a.[ID],[RoleID],a.[ChannelID],a.[IsBrowse],a.[AddAuth],a.[EditAuth],a.[DelAuth],a.[AuditingAuth],a.[CreateTime],a.[CreateUserID]
                              FROM [dbo].[ChannelPrivilege] a,dbo.NewsDoc b 
                              WHERE b.ID=@NewsID AND a.ChannelID=b.ChannelID AND a.RoleID=@RoleID";
            using (SQLHelper dao = new SQLHelper())
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@NewsID", newsID);
                parameters[1] = new SqlParameter("@RoleID", roleID);
                using (SqlDataReader reader = dao.ExecuteReader(CommandType.Text, sql, parameters))
                {
                    if (reader.Read())
                    {
                        switch (operType)
                        {
                            case 1:
                                if (TypeParse.ToBool(reader[reader.GetOrdinal("EditAuth")]))
                                {
                                    flag = true;
                                }
                                break;
                            case 2:
                                if (TypeParse.ToBool(reader[reader.GetOrdinal("DelAuth")]))
                                {
                                    flag = true;
                                }
                                break;
                            case 3:
                                if (TypeParse.ToBool(reader[reader.GetOrdinal("AuditingAuth")]))
                                {
                                    flag = true;
                                }
                                break;
                        }
                    }
                    else
                    {
                        flag = true;
                    }
                    reader.Close();
                }
            }
            return flag;
        }


        # endregion
    }
}
