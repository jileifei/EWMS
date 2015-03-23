using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;

namespace CMS.CommonLib.Utils
{
    public class TicketTool
    {
        /// <summary>
        /// 创建一个票据，放在cookie中
        /// 票据中的数据经过加密，解决了cookie的安全问题。
        /// </summary>
        /// <param name="userID">员工ID</param>
        /// <param name="userData">员工实体类json字符串</param>
        public static void SetCookie(string userID, string userData)
        {
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, userID, DateTime.Now, DateTime.Now.AddHours(8), false, userData, FormsAuthentication.FormsCookiePath);
            string encTicket = FormsAuthentication.Encrypt(ticket);
            HttpCookie newCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
            HttpContext.Current.Response.Cookies.Add(newCookie);
        }
        /// <summary>
        /// 通过此法判断登录
        /// </summary>
        /// <returns>已登录返回true</returns>
        public static bool IsLogin()
        {
            return HttpContext.Current.User.Identity.IsAuthenticated;
        }
        /// <summary>
        /// 退出登录
        /// </summary>
        public static void Logout()
        {
            FormsAuthentication.SignOut();
        }
        /// <summary>
        /// 取得登录员工ID
        /// </summary>
        /// <returns></returns>
        public static string GetUserID()
        {
            return HttpContext.Current.User.Identity.Name;
        }
        /// <summary>
        /// 取得票据中数据
        /// </summary>
        /// <returns></returns>
        public static string GetUserData()
        {
            return (HttpContext.Current.User.Identity as FormsIdentity).Ticket.UserData;
        }
    }
}
