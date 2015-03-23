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
    public class ModuleService
    {
        # region tree

        /// <summary>
        /// 获取模块树
        /// </summary>
        /// <returns></returns>
        public DynatreeModel GetModuleTreeList()
        {
            DynatreeModel dynatreeRootNode = new DynatreeModel();
            dynatreeRootNode.key = "0";
            dynatreeRootNode.title = "模块管理";
            dynatreeRootNode.expand = true;

            IModuleInfoDao moduleDao = CastleContext.Instance.GetService<IModuleInfoDao>();
            IList<ModuleInfo> rootModuleList = moduleDao.FindByPID(0);
            List<DynatreeModel> rootList = MakeDynatreeModelList(rootModuleList);
            foreach (DynatreeModel childeItem in rootList)
            {
                dynatreeRootNode.children.Add(childeItem);
                childeItem.children = GetModuleTreeNode(childeItem);
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
        private List<DynatreeModel> GetModuleTreeNode(DynatreeModel node)
        {
            IModuleInfoDao moduleDao = CastleContext.Instance.GetService<IModuleInfoDao>();
            IList<ModuleInfo> childModuleList = moduleDao.FindByPID(TypeParse.ToLong(node.key));
            if (childModuleList.Count > 0)
            {
                node.isFolder = true;// 有子节点
            }
            List<DynatreeModel> childeList = MakeDynatreeModelList(childModuleList);
            foreach (DynatreeModel childeItem in childeList)
            {
                node.children.Add(childeItem);
                GetModuleTreeNode(childeItem);
            }

            return childeList;
        }

        /// <summary>
        /// convert channelinfo entity list to dynatreenode entity list
        /// </summary>
        /// <param name="moduleList"></param>
        /// <returns></returns>
        private List<DynatreeModel> MakeDynatreeModelList(IList<ModuleInfo> moduleList)
        {
            List<DynatreeModel> listDynatreeNode = new List<DynatreeModel>(1);
            foreach (ModuleInfo moduleEntity in moduleList)
            {
                DynatreeModel dynatreeModel = new DynatreeModel
                {
                    expand = true,
                    title = moduleEntity.ModuleName,
                    key = moduleEntity.ID.ToString(CultureInfo.InvariantCulture),
                    ParentID = 0
                };
                listDynatreeNode.Add(dynatreeModel);
            }
            return listDynatreeNode;
        }

        # endregion

        # region add edit delete select sort

        /// <summary>
        /// 添加模块
        /// </summary>
        /// <param name="moduleInfo"></param>
        /// <returns></returns>
        public long Add(ModuleInfo moduleInfo)
        {
            IModuleInfoDao moduleDao = CastleContext.Instance.GetService<IModuleInfoDao>();
            moduleDao.Insert(moduleInfo);
            long moduleID = moduleInfo.ID;

            return moduleID;
        }

        /// <summary>
        /// 修改栏目
        /// </summary>
        /// <param name="moduleInfo"></param>
        /// <returns></returns>
        public bool Update(ModuleInfo moduleInfo)
        {
            IModuleInfoDao moduleDao = CastleContext.Instance.GetService<IModuleInfoDao>();
            moduleDao.Update(moduleInfo);
            return true;
        }

        /// <summary>
        /// 删除模块
        /// </summary>
        /// <param name="moduleInfo"></param>
        /// <returns></returns>
        public bool Delete(ModuleInfo moduleInfo)
        {
            IModuleInfoDao moduleDao = CastleContext.Instance.GetService<IModuleInfoDao>();
            moduleDao.Delete(moduleInfo);
            return true;
        }

        /// <summary>
        /// 得到栏目信息
        /// </summary>
        /// <param name="moduleID"></param>
        /// <returns></returns>
        public ModuleInfo GetModuleInfo(long moduleID)
        {
            IModuleInfoDao moduleDao = CastleContext.Instance.GetService<IModuleInfoDao>();
            ModuleInfo moduleEntity = moduleDao.Find(moduleID);
            return moduleEntity;
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="sourceModuleID"></param>
        /// <param name="moduleID"></param>
        /// <returns></returns>
        public bool Sort(long sourceModuleID, long moduleID)
        {
            int result;
            using (SQLHelper helper = new SQLHelper())
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@ModuleID", moduleID);
                parameters[1] = new SqlParameter("@SourceModuleID", sourceModuleID);
                result = helper.ExecuteNonQuery(CommandType.StoredProcedure, "UP_ModuleSort", parameters);
            }
            return result > 0;
        }

        # endregion

        # region save role module auth

        /// <summary>
        /// 保存角色拥有权限的菜单映射关系
        /// </summary>
        /// <param name="roleID">角色ID</param>
        /// <param name="moduleIDList">模块ID列表</param>
        /// <param name="curUserID"></param>
        /// <returns></returns>
        public void SaveRoleModuleAuth(int roleID, List<string> moduleIDList, long curUserID)
        {
            // 先删除
            using (SQLHelper dao = new SQLHelper())
            {
                // 启动事务
                dao.BeginTrans();
                try
                {
                    string sqlDel = "DELETE dbo.RolePrivilege WHERE RoleID=@RoleID";
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@RoleID", roleID);
                    // 先批量删除
                    dao.ExecuteNonQuery(CommandType.Text, sqlDel, parameters);

                    // 开始批量插入
                    DataTable dtMenuAuth = ConstructionMenuAuthDT();
                    foreach (string moduleID in moduleIDList)
                    {
                        if (!string.IsNullOrEmpty(moduleID) && TypeParse.ToInt(moduleID) > 0)
                        {
                            DataRow row = dtMenuAuth.NewRow();
                            row["RoleID"] = roleID;
                            row["ModuleID"] = moduleID;
                            row["CreateTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            row["CreateUserID"] = curUserID;
                            dtMenuAuth.Rows.Add(row);
                        }
                    }
                    dao.GoBulkCopy(dtMenuAuth, "dbo.RolePrivilege");
                    dao.CommitTrans();
                }
                catch (Exception ex)
                {
                    dao.RollbackTrans();
                    throw ex;
                }
            }
        }



        /// <summary>
        ///  构造批量插入的Table
        /// </summary>
        /// <returns></returns>
        private DataTable ConstructionMenuAuthDT()
        {
            DataTable menuAuthTable = new DataTable("RolePrivilege");
            DataColumn colMenuAuthID = menuAuthTable.Columns.Add("ID", typeof(int));
            colMenuAuthID.AutoIncrement = true;
            colMenuAuthID.AutoIncrementSeed = 1;
            menuAuthTable.Columns.Add("RoleID", typeof(int));
            menuAuthTable.Columns.Add("ModuleID", typeof(int));
            menuAuthTable.Columns.Add("CreateTime", typeof(DateTime));
            menuAuthTable.Columns.Add("CreateUserID", typeof(int));
            return menuAuthTable;
        }

        # endregion

        # region get user have auth module list

        /// <summary>
        /// 获取角色有权限的模块ID
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public List<long> GetModuleListByRoleID(long roleID)
        {
            List<long> moduleList = new List<long>();
            using (SQLHelper dao = new SQLHelper())
            {
                string sql = "SELECT ModuleID FROM dbo.RolePrivilege WITH(NOLOCK) WHERE RoleID=@RoleID";
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@RoleID", roleID);
                SqlDataReader reader = dao.ExecuteReader(CommandType.Text, sql, parameters);
                while (reader.Read())
                {
                    moduleList.Add(TypeParse.ToInt(reader["ModuleID"]));
                }
            }
            return moduleList;
        }

        # endregion

        # region 得到有权限菜单

        /// <summary>
        /// 得到指定父级的菜单模块列表
        /// </summary>
        /// <param name="parentID"></param>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public IList<ModuleInfo> GetRootModuleByRoleID(long parentID, long roleID)
        {
            IList<ModuleInfo> listModule = new List<ModuleInfo>();
            using (SQLHelper dao = new SQLHelper())
            {
                string sql = "SELECT a.ID,a.PID,a.ModuleName,a.ModuleUrl FROM dbo.ModuleInfo a,dbo.RolePrivilege b WHERE a.PID=@PID AND a.ID=b.ModuleID AND b.RoleID=@RoleID ORDER BY SortID";
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@PID", parentID);
                parameters[1] = new SqlParameter("@RoleID", roleID);
                using (SqlDataReader dr = dao.ExecuteReader(CommandType.Text, sql, parameters))
                {
                    while (dr.Read())
                    {
                        ModuleInfo moduleEntity = new ModuleInfo();
                        moduleEntity.ID = TypeParse.ToLong(dr[dr.GetOrdinal("ID")]);
                        moduleEntity.PID = TypeParse.ToLong(dr[dr.GetOrdinal("PID")]);
                        moduleEntity.ModuleName = dr[dr.GetOrdinal("ModuleName")].ToString();
                        moduleEntity.ModuleUrl = dr[dr.GetOrdinal("ModuleUrl")].ToString();
                        listModule.Add(moduleEntity);
                    }
                    dr.Close();
                }
            }
            return listModule;
        }

        # endregion

        # region 判断当前Url是否有权限访问

        /// <summary>
        /// 判断当前Url是否有权限访问
        /// </summary>
        /// <param name="roleID">角色ID</param>
        /// <param name="curUrl">当前访问Url</param>
        /// <returns></returns>
        public bool CheckUreIsHaveRight(long roleID, string curUrl)
        {
            bool flag = false;
            bool isHave = false;
            using (SQLHelper dao = new SQLHelper())
            {
                string sql = "SELECT TOP 1 ID FROM ModuleInfo WHERE ModuleUrl = @ModuleUrl";
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@ModuleUrl", curUrl);
                using (SqlDataReader dr = dao.ExecuteReader(CommandType.Text, sql, parameters))
                {
                    if (dr.Read())
                    {
                        isHave = true;
                    }
                    else
                    {
                        flag = true;
                    }
                    dr.Close();
                }
                if (isHave)
                {
                    string sqlHaveRight = "SELECT a.ModuleUrl FROM dbo.ModuleInfo a,dbo.RolePrivilege b WHERE a.ID=b.ModuleID AND b.RoleID=@RoleID ORDER BY SortID";
                    parameters[0] = new SqlParameter("@RoleID", roleID);
                    using (SqlDataReader dr = dao.ExecuteReader(CommandType.Text, sqlHaveRight, parameters))
                    {
                        int urlIndex = dr.GetOrdinal("ModuleUrl");
                        while (dr.Read())
                        {
                            if (curUrl.ToLower() == dr[urlIndex].ToString().ToLower())
                            {
                                flag = true;
                                break;
                            }
                        }
                        dr.Close();
                    }
                }
            }
            return flag;
        }

        # endregion
    }
}
