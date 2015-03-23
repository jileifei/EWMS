using System.Web;
using System.Web.Mvc;

using CMS.CommonLib.Utils;
using CMS.CommonLib.Encrypt;

namespace CMS.AdminUI.Controllers
{
    public class VerifyCodeController : Controller
    {
        /// <summary>
        /// 验证码Key
        /// </summary>
        private const string VERFIYCODEKEY = "CORP_CMS_VERFIFYCODE";

        /// <summary>
        /// 验证码
        /// </summary>
        public string VerifyCodeText
        {
            get
            {
                HttpCookie cookie = Request.Cookies[VERFIYCODEKEY];
                return cookie != null ? cookie.Value : "";
            }
        }

        /// <summary>
        /// 验证码
        /// </summary>
        /// <returns></returns>
        public FileContentResult VerifyCode()
        {
            ValidateCode vCode = new ValidateCode {FontSize = 28, Length = 68};
            string code = vCode.CreateVerifyCode(4);
            Response.Cookies.Add(new HttpCookie(VERFIYCODEKEY, DES.Encrypt(code.ToLower())));
            byte[] bytes = vCode.CreateImageToByte(code, false);
            return File(bytes, @"image/jpeg");
        }
    }
}
