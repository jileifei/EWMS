using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using CMS.CommonLib.Encrypt;
using CMS.CommonLib.Utils;
using CMS.Domain;
using CMS.DataAccess;
using CMS.DataAccess.SQLHelper;
using CMS.DataAccess.Interface;

namespace CMS.Service
{
    public class UserService
    {
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="oldPwd"></param>
        /// <param name="newPwd"></param>
        /// <returns></returns>
        public bool UpdatePwd(long userID, string oldPwd, string newPwd)
        {
            int count;
            using (SQLHelper dao = new SQLHelper())
            {
                string sql = "UPDATE UserInfo SET Password=@Password WHERE ID=@UserID AND Password=@OldPassword";
                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@UserID", userID);
                parameters[1] = new SqlParameter("@Password", DES.Encrypt(newPwd));
                parameters[2] = new SqlParameter("@OldPassword", DES.Encrypt(oldPwd));
                count = dao.ExecuteNonQuery(CommandType.Text, sql, parameters);
            }
            return count > 0;
        }

        # region select insert update delete

        /// <summary>
        /// get user info by UserID
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public UserInfo GetUserInfoByID(long userID)
        {
            IUserInfoDao userDao = CastleContext.Instance.GetService<IUserInfoDao>();
            UserInfo userEntity = userDao.Find(userID);
            return userEntity;
        }

        /// <summary>
        /// save user
        /// </summary>
        /// <param name="userEntity"></param>
        /// <returns></returns>
        public bool InsertUser(UserInfo userEntity)
        {
            IUserInfoDao userDao = CastleContext.Instance.GetService<IUserInfoDao>();
            userEntity.Password = DES.Encrypt(userEntity.Password);
            userDao.Insert(userEntity);
            return true;
        }

        /// <summary>
        /// update user
        /// </summary>
        /// <param name="userEntity"></param>
        /// <returns></returns>
        public bool UpdateUser(UserInfo userEntity)
        {
            IUserInfoDao userDao = CastleContext.Instance.GetService<IUserInfoDao>();
            userDao.Update(userEntity);
            return true;

        }

        /// <summary>
        /// delete user
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public bool DeleteUser(long userID)
        {
            IUserInfoDao userDao = CastleContext.Instance.GetService<IUserInfoDao>();
            UserInfo userEntity = new UserInfo();
            userEntity.ID = userID;
            userDao.Delete(userEntity);
            return true;
        }

        # endregion

        /// <summary>
        /// check username is exists
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool CheckUserName(string userName)
        {
            IUserInfoDao userDao = CastleContext.Instance.GetService<IUserInfoDao>();
            IList<UserInfo> userList = userDao.FindByUserName(userName);
            if (userList != null && userList.Count > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// check CheckUserName is exists
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public bool CheckUserName(string userName, long? userID)
        {
            string sql = "SELECT ID FROM dbo.UserInfo WITH(NOLOCK) WHERE UserName=@UserName";
            int count = 1;
            if (userID != null)
            {
                sql += " AND ID!=@UserID";
                count = 2;
            }
            SqlParameter[] parameters = new SqlParameter[count];
            parameters[0] = new SqlParameter("@UserName", userName);
            if (userID != null)
            {
                parameters[1] = new SqlParameter("@UserID", userID.Value);
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

        # region 获取用户列表 分页

        /// <summary>
        /// 获取用户列表 分页
        /// </summary>
        /// <param name="pageSize">每页条数</param>
        /// <param name="pageIndex">当前页码</param>
        /// <returns></returns>
        public PagerModel<UserInfo> GetPagerList(int pageSize, int pageIndex)
        {
            PagerModel<UserInfo> userPagerEntity = new PagerModel<UserInfo>();
            using (SQLHelper dao = new SQLHelper())
            {
                int totalCount;
                int totalPager;
                DataSet dsUserList = dao.PageingQuery("UserInfo", "[ID],[UserName],[RealName],[Department],[Email],[Status],[CreateTime]", "ID", "", "", 2, pageSize, pageIndex, out totalCount, out totalPager);
                userPagerEntity.PageSize = pageSize;
                userPagerEntity.CurrentPage = pageIndex;
                userPagerEntity.TotalRecords = totalCount;
                userPagerEntity.ItemList = MakeUserList(dsUserList);
            }
            return userPagerEntity;
        }

        /// <summary>
        /// Convert DataSet To IList<NewsModel/>
        /// </summary>
        /// <param name="dsUserList"></param>
        /// <returns></returns>
        private IList<UserInfo> MakeUserList(DataSet dsUserList)
        {
            IList<UserInfo> userList = new List<UserInfo>(10);
            foreach (DataRow row in dsUserList.Tables[0].Rows)
            {
                UserInfo userEntity = new UserInfo
                    {
                        ID = TypeParse.ToInt(row["ID"]),
                        UserName = row["UserName"].ToString(),
                        RealName = row["RealName"].ToString(),
                        Department = row["Department"].ToString(),
                        Email = row["Email"].ToString(),
                        Status = TypeParse.ToInt(row["Status"]),
                        CreateTime = TypeParse.ToDateTime(row["CreateTime"])
                    };
                userList.Add(userEntity);
            }
            return userList;
        }

        # endregion

        # region 根据角色得到用户列表

        /// <summary>
        /// 根据角色得到用户列表
        /// </summary>
        /// <param name="roleID">角色ID</param>
        /// <returns></returns>
        public IList<UserInfo> GetUserListByRoleID(long roleID)
        {
            IList<UserInfo> listUser = new List<UserInfo>();
            using (SQLHelper dao = new SQLHelper())
            {
                string sql = "SELECT a.ID,a.UserName,a.Department FROM dbo.UserInfo a WITH(NOLOCK) WHERE RoleID=@RoleID ORDER BY a.CreateTime DESC";
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@RoleID", roleID);
                using (SqlDataReader reader = dao.ExecuteReader(CommandType.Text, sql, parameters))
                    while (reader.Read())
                    {
                        UserInfo userEntity = new UserInfo
                        {
                            ID = TypeParse.ToLong(reader["ID"]),
                            UserName = reader["UserName"].ToString()
                        };
                        listUser.Add(userEntity);
                    }
            }
            return listUser;
        }

        # endregion

        # region 设置用户角色

        /// <summary>
        /// 设置用户角色
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public bool SetUserRole(long userID, long roleID)
        {
            int count;
            using (SQLHelper dao = new SQLHelper())
            {
                string sql = "UPDATE UserInfo SET RoleID=@RoleID WHERE ID=@UserID";
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@UserID", userID);
                parameters[1] = new SqlParameter("@RoleID", roleID);
                count = dao.ExecuteNonQuery(CommandType.Text, sql, parameters);

            }
            return count > 0;
        }

        # endregion

        # region 移除用户角色

        /// <summary>
        /// 移除用户角色
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public bool DeleteUserRole(long userID)
        {
            int count;
            using (SQLHelper dao = new SQLHelper())
            {
                string sql = "UPDATE UserInfo SET RoleID=0 WHERE ID=@UserID";
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@UserID", userID);
                count = dao.ExecuteNonQuery(CommandType.Text, sql, parameters);
            }
            return count > 0;
        }

        # endregion
    }
}
