using System;
using System.Globalization;
using System.Web;
using System.Web.Mvc;

using CMS.CommonLib.Encrypt;
using CMS.CommonLib.Utils;
using CMS.CommonLib.Json;
using CMS.Domain;
using CMS.Service;

namespace CMS.AdminUI.Controllers
{
    public class LoginController : Controller
    {
        public string ReturnUrl = "";
        private const string VERFIYCODEKEY = "CORP_CMS_VERFIFYCODE";

        # region 登录

        [HttpPost]
        public ActionResult Index(string userName, string password, string verifyCode)
        {
            string returnUrl = "/Index";
            string msg;
            string result;
            string jsonContent = "\"result\":\"{0}\",\"msg\":\"{1}\",\"url\":\"{2}\"";
            try
            {
                # region 得到验证码
                string code = "";
                HttpCookie cookie = Request.Cookies[VERFIYCODEKEY];
                if (cookie != null)
                {
                    code = cookie.Value;
                }
                # endregion

                if (DES.Encrypt(verifyCode.ToLower()) == code)
                {
                    SSOServer ssoService = new SSOServer();
                    UserInfo userInfo = ssoService.GetUserInfoByUserNameAndPassword(userName, password);
                    if (userInfo == null)
                    {
                        msg = "用户名或密码错误";
                        result = "failure";
                    }
                    else
                    {
                        string jsonObject = JsonUtility.ObjectToJson(userInfo);
                        // 保存登录ticket
                        TicketTool.SetCookie(userInfo.ID.ToString(CultureInfo.InvariantCulture), jsonObject);
                        msg = "登录成功";
                        result = "success";
                    }
                }
                else
                {
                    msg = "请输入正确的验证码";
                    result = "failure";
                }
            }
            catch (Exception ex)
            {
                msg = "系统出现异常，请稍后再试:" + ex.Message.Replace("\"","'").Replace("\r\n"," ");
                result = "error";
            }
            string json = string.Format(jsonContent, result, msg, returnUrl);
            json = "{" + json + "}";
            return Content(json);
        }

        # endregion
    }
}
