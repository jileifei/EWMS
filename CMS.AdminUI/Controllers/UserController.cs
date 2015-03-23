using System;
using System.Collections.Generic;
using System.Web.Mvc;

using Webdiyer.WebControls.Mvc;
using CMS.CommonLib.Utils;
using CMS.Domain;
using CMS.Service;
using CMS.AdminUI.Models;

namespace CMS.AdminUI.Controllers
{
    public class UserController : BaseController
    {
        private const int PAGESIZE = 20;

        public ActionResult Index(int pageIndex = 1)
        {
            UserService userService = new UserService();
            PagerModel<UserInfo> pageModel = userService.GetPagerList(PAGESIZE, pageIndex);
            PagedList<UserInfo> pagedUser = new PagedList<UserInfo>(pageModel.ItemList, pageIndex, PAGESIZE, pageModel.TotalRecords);
            UserPageModel userPageModel = new UserPageModel();
            userPageModel.PageList = pagedUser;
            return View(userPageModel);
        }

        /// <summary>
        /// 用户选择框
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public ActionResult SelUser(int pageIndex = 1)
        {
            UserService userService = new UserService();
            PagerModel<UserInfo> pageModel = userService.GetPagerList(10, pageIndex);
            PagedList<UserInfo> pagedUser = new PagedList<UserInfo>(pageModel.ItemList, pageIndex, 10, pageModel.TotalRecords);
            UserPageModel userPageModel = new UserPageModel();
            userPageModel.PageList = pagedUser;
            return View(userPageModel);
        }

        # region 修改密码

        public ActionResult EditPwd()
        {
            return View();
        }

        [HttpPost]
        public ActionResult EditPwdAjax(string oldpwd, string newpwd)
        {
            try
            {
                UserService userService = new UserService();
                bool flag = userService.UpdatePwd(UserID, oldpwd, newpwd);
                return Content(flag ? "{\"result\":\"success\"}" : "{\"result\":\"failure\",\"msg\":\"请输入正确的旧密码\"}");
            }
            catch (Exception ex)
            {
                return Content("{\"result\":\"error\",\"msg\":\"系统出现异常：" + ex.Message + "\"}");
            }
        }

        # endregion

        # region 退出

        public ActionResult Logout()
        {
            TicketTool.Logout();
            return Redirect("/");
        }

        # endregion

        # region Check UserName is exists

        public ActionResult CheckUserName(string userName, long? userID)
        {
            UserService userService = new UserService();
            bool exists = userService.CheckUserName(userName, userID);
            return Json(!exists, JsonRequestBehavior.AllowGet);
        }

        # endregion

        # region get insert update delete

        public ActionResult Details(long? id)
        {
            UserInfo userInfo;
            if (id != null)
            {
                UserService userService = new UserService();
                userInfo = userService.GetUserInfoByID(id.Value);
            }
            else
            {
                userInfo = new UserInfo();
            }
            return Json(userInfo, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// add by jileifei 2011-11-15
        /// 主要用于获取当前用户的email等。
        /// </summary>
        /// <returns></returns>
        public ActionResult GetCurrentUser()
        {
            UserService userService = new UserService();
            UserInfo userInfo = userService.GetUserInfoByID(UserID);
            return Json(userInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create(UserInfo userEntity)
        {
            if (Request.IsAjaxRequest())
            {
                try
                {
                    UserService userService = new UserService();
                    userEntity.CreateUserID = UserID;
                    bool flag = userService.InsertUser(userEntity);
                    return Content(flag ? "{\"result\":\"ok\"}" : "{\"result\":\"error\",\"msg\":\"系统出现异常，请联系管理员\"}");
                }
                catch (Exception ex)
                {
                    return Content("{\"result\":\"error\",\"msg\":\"" + ex.Message + "\"}");
                }
            }
            return Content("{\"result\":\"error\",\"msg\":\"请正常访问页面\"}");
        }

        [HttpPost]
        public ActionResult Update(UserInfo userEntity)
        {
            if (Request.IsAjaxRequest())
            {
                try
                {
                    UserService userService = new UserService();
                    bool flag = userService.UpdateUser(userEntity);
                    return Content(flag ? "{\"result\":\"ok\"}" : "{\"result\":\"error\",\"msg\":\"系统出现异常，请联系管理员\"}");
                }
                catch (Exception ex)
                {
                    return Content("{\"result\":\"error\",\"msg\":\"" + ex.Message + "\"}");
                }
            }
            return Content("{\"result\":\"error\",\"msg\":\"请正常访问页面\"}");
        }

        [HttpPost]
        public ActionResult Delete(long? id)
        {
            if (id == null)
                return Content("{\"result\":\"error\",\"msg\":\"请选择要删除的用户信息\"}");
            if (Request.IsAjaxRequest())
            {
                try
                {
                    UserService userService = new UserService();
                    bool flag = userService.DeleteUser(id.Value);
                    return Content(flag ? "{\"result\":\"ok\"}" : "{\"result\":\"error\",\"msg\":\"系统出现异常，请联系管理员\"}");
                }
                catch (Exception ex)
                {
                    return Content("{\"result\":\"error\",\"msg\":\"" + ex.Message + "\"}");
                }
            }
            return Content("{\"result\":\"error\",\"msg\":\"请正常访问页面\"}");
        }

        # endregion

        # region 根据角色得到用户列表

        /// <summary>
        /// 根据角色得到用户列表
        /// </summary>
        /// <param name="roleID">角色ID</param>
        /// <returns></returns>
        public ActionResult GetUserList(long? roleID)
        {
            UserService userService = new UserService();
            IList<UserInfo> listUser = new List<UserInfo>();
            if (roleID != null)
            {
                listUser = userService.GetUserListByRoleID(roleID.Value);
            }
            return Json(listUser);
        }

        # endregion

        # region 设置角色

        [HttpPost]
        public ActionResult SetUserRole(long? userID, long? roleID)
        {
            if (userID != null && roleID != null)
            {
                try
                {
                    UserService userService = new UserService();
                    bool flag = userService.SetUserRole(userID.Value, roleID.Value);
                    return Content(flag ? "{\"result\":\"success\"}" : "{\"result\":\"failure\",\"msg\":\"请设置正确的用户和岗位\"}");
                }
                catch (Exception ex)
                {
                    return Content("{\"result\":\"error\",\"msg\":\"系统出现异常：" + ex.Message + "\"}");
                }
            }
            return Content("{\"result\":\"failure\",\"msg\":\"请设置正确的用户和岗位\"}");
        }

        # endregion

        # region 移除角色

        [HttpPost]
        public ActionResult DeleteUserRole(long? userID)
        {
            if (userID != null)
            {
                try
                {
                    UserService userService = new UserService();
                    bool flag = userService.DeleteUserRole(userID.Value);
                    if (flag)
                    {
                        return Content("{\"result\":\"success\"}");
                    }
                    return Content("{\"result\":\"failure\",\"msg\":\"请选择要删除的用户\"}");
                }
                catch (Exception ex)
                {
                    return Content("{\"result\":\"error\",\"msg\":\"系统出现异常：" + ex.Message + "\"}");
                }
            }
            return Content("{\"result\":\"failure\",\"msg\":\"请选择要删除的用户\"}");
        }

        # endregion
    }
}
