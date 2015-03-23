using System;
using System.Collections.Generic;
using System.Web.Mvc;

using CMS.Domain;
using CMS.Service;
using CMS.CommonLib.Extension;

namespace CMS.AdminUI.Controllers
{
    public class RoleController : BaseController
    {
        public ActionResult Index()
        {
            RoleService roleService = new RoleService();
            IList<RoleInfo> list = roleService.GetRoleList();
            return View(list);
        }

        # region 增 删 改

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="roleEntity"></param>
        /// <returns></returns>
        public ActionResult Add(RoleInfo roleEntity)
        {
            if (ModelState.IsValid)
            {
                RoleService roleService = new RoleService();
                roleEntity.CreateTime = DateTime.Now;
                roleEntity.CreateUserID = UserID;
               roleService.AddRole(roleEntity);
                return Content("{\"result\":\"ok\"}");
            }
            return Content("{\"result\":\"error\",\"msg\":\"" + this.ExpendErrors() + "\"}");
        }

        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="roleEntity"></param>
        /// <returns></returns>
        public ActionResult Update(RoleInfo roleEntity)
        {
            RoleService roleService = new RoleService();
            bool flag = roleService.Update(roleEntity);
            return Content(flag ? "{\"result\":\"ok\"}" : "{\"result\":\"error\",\"msg\":\"系统出现异常，请联系管理员\"}");
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="roleEntity"></param>
        /// <returns></returns>
        public ActionResult Delete(RoleInfo roleEntity)
        {
            RoleService roleService = new RoleService();
            bool checkflag = roleService.CheckRoleIsHaveUser(roleEntity.ID);
            if (checkflag)
            {
                return Content("{\"result\":\"error\",\"msg\":\"该角色下有分配的用户，所以不能删除\"}");
            }
            bool flag = roleService.Delete(roleEntity);
            return Content(flag ? "{\"result\":\"ok\"}" : "{\"result\":\"error\",\"msg\":\"系统出现异常，请联系管理员\"}");
        }

        # endregion
    }
}
