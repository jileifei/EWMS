using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using System.Collections;
using Webdiyer.WebControls.Mvc;
using CMS.Domain;
using CMS.Service;
using CMS.CommonLib.Utils;

namespace CMS.AdminUI.Controllers
{
    public class FeedBackController : BaseController
    {
        //
        // GET: /FeedBack/

        public ActionResult Index(int? pageIndex, int status = 0, int messageType = 0)
        {
            ViewData["messageType"] = 2;
            ViewData["status"] = status;
            IList<FeedBack> fbList;
            if (messageType == 0 && status == 0)
            {
                fbList = FeedBackService.FindAll();
            }
            else if (messageType > 0 && status > 0)
            {
                fbList = FeedBackService.FindByMTStatus(Convert.ToInt32(messageType), Convert.ToInt32(status));
            }
            else if (messageType > 0 && status == 0)
            {
                fbList = FeedBackService.FindByMessageType(messageType);
            }
            else
            {
                fbList = FeedBackService.FindAllByStatus(status);
            }

            PagedList<FeedBack> pagedList = new PagedList<FeedBack>(fbList, pageIndex ?? 1, 1);
            return View(pagedList);
        }

        //
        // GET: /FeedBack/Delete/5

        public ActionResult Delete(int id)
        {
            FeedBack fb = new FeedBack();
            fb.Id = id;
            int count = FeedBackService.Delete(fb);
            if (count > 0)
                return Content("success");
            return Content("false");
        }

        /// <summary>
        /// 审核通过
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult AuditComment(int id)
        {
            Hashtable ht = new Hashtable();
            ht.Add("Status", 1);
            ht.Add("ID", id);
            int count = FeedBackService.AuditComment(ht);
            if (count > 0)
                return Content("success");
            return Content("false");
        }

        /// <summary>
        /// 添加回复信息，修改回复状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="content"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public ActionResult ReplayContent(int id, string content, string email)
        {
            FeedBack commentInfo = new FeedBack
            {
                Id = id,
                ReplyMessae = content,
                ReplyTime = DateTime.Now,
                Status = 2,
                ReplyUserid = UserID
            };
            int count = FeedBackService.ReplayContent(commentInfo);
            if (count > 0)
            {
                SendMail(id, email, "电通给您的回复信息", content);
                return Content("success");
            }
            return Content("false");
        }

        public ActionResult RemarksContent(int id, string content)
        {
            try
            {
                FeedBack fb = new FeedBack();
                fb.Remarks = content;
                fb.Id = id;
                FeedBackService.UpdateRemarks(fb);
                return Json(new { result = "ok" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "error", msg = ex.Message });
            }
        }
        /// <summary>
        /// 删除回复信息，修改回复状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeleteReplay(int id)
        {
            FeedBack commentInfo = new FeedBack();
            commentInfo.Id = id;
            commentInfo.ReplyMessae = "";
            commentInfo.Status = 1;
            commentInfo.ReplyUserid = UserID;
            commentInfo.ReplyTime = DateTime.Now;
            int count = FeedBackService.ReplayContent(commentInfo);
            if (count > 0)
                return Content("success");
            return Content("false");
        }

        #region send mail
        public void SendMail(int id, string mailTo, string title, string content)
        {
            string[] adminMail = ConfigurationManager.AppSettings["AdminMail"].Split(',');
            string smtp = ConfigurationManager.AppSettings["SmtpHost"];
            Utils.SendMails(smtp, adminMail[0], adminMail[1], adminMail[0], mailTo, content, title);
        }
        #endregion
    }
}
