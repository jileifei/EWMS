using System.Collections.Generic;
using CMS.Domain;
using CMS.DataAccess;
using CMS.DataAccess.Interface;

namespace CMS.Service
{
    public class RoleService
    {
        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <returns></returns>
        public IList<RoleInfo> GetRoleList()
        {
            IRoleInfoDao roleInfoDao = CastleContext.Instance.GetService<IRoleInfoDao>();
            return roleInfoDao.FindAll();
        }

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <returns></returns>
        public long AddRole(RoleInfo roleEntity)
        {
            long roleID = roleEntity.ID;
            return roleID;
        }

        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="roleEntity"></param>
        /// <returns></returns>
        public bool Update(RoleInfo roleEntity)
        {
            IRoleInfoDao roleInfoDao = CastleContext.Instance.GetService<IRoleInfoDao>();
            roleInfoDao.Update(roleEntity);
            return true;
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="roleEntity"></param>
        /// <returns></returns>
        public bool Delete(RoleInfo roleEntity)
        {
            IRoleInfoDao roleInfoDao = CastleContext.Instance.GetService<IRoleInfoDao>();
            roleInfoDao.Delete(roleEntity);
            return true;
        }

        /// <summary>
        /// 判断当前角色下是否有用户
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public bool CheckRoleIsHaveUser(long roleID)
        {
            IRolePrivilegeDao userRoleDao = CastleContext.Instance.GetService<IRolePrivilegeDao>();
            return userRoleDao.FindByRoleID(roleID).Count > 0;
        }
    }
}
