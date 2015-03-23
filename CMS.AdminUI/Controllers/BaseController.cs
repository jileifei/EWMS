using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Text;

using CMS.Domain;
using CMS.CommonLib.Json;
using CMS.CommonLib.Utils;
using CMS.Service;

namespace CMS.AdminUI.Controllers
{
    public class BaseController : Controller
    {
        private long _userID;
        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserID
        {
            get { return _userID; }
            set { _userID = value; }
        }

        private string _userName = "";
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        private long _roleID;
        /// <summary>
        /// 用户角色
        /// </summary>
        public long RoleID
        {
            get { return _roleID; }
            set { _roleID = value; }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (TicketTool.IsLogin())
            {
                SetLoginInfo();
                LoadTopMenu();
            }
            else
            {
                filterContext.HttpContext.Response.Redirect("/", true);
            }
            base.OnActionExecuting(filterContext);
        }

        # region set logined user info

        /// <summary>
        /// 设置登录信息
        /// </summary>
        private void SetLoginInfo()
        {
            UserInfo curUser = JsonUtility.JsonToObject<UserInfo>(TicketTool.GetUserData());
            if (curUser == null)
            {
                throw new Exception("请登录");
            }
            // 阻止XSS跨站攻击

            SSOServer ssoService = new SSOServer();
            bool isXSS = ssoService.ProtectXSS(curUser);
            if (!isXSS)
            {
                HttpContext.Response.Redirect("/", true);
            }

            ModuleService moduleService = new ModuleService();
            bool flag = moduleService.CheckUreIsHaveRight(curUser.RoleID, HttpContext.Request.Path);
            if (!flag)
            {
                HttpContext.Response.Redirect("/Index", true);
            }
            UserID = curUser.ID;
            UserName = curUser.UserName;
            RoleID = curUser.RoleID;
        }

        # endregion

        # region  load top menu

        private void LoadTopMenu()
        {
            StringBuilder sbTopMenu = new StringBuilder();
            ModuleService moduleService = new ModuleService();
            IList<ModuleInfo> listRootModule = moduleService.GetRootModuleByRoleID(0, RoleID);
            foreach (ModuleInfo module in listRootModule)
            {
                sbTopMenu.AppendFormat("<li id=\"li{0}\" class=\"\"><a href=\"{1}\">{2}</a></li>\n", module.ModuleUrl.Replace("/", ""), module.ModuleUrl, module.ModuleName);
            }
            ViewBag.TopMenu = new HtmlString(sbTopMenu.ToString());
        }

        # endregion
    }
}
