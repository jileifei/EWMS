using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using CMS.Domain;
using CMS.Service;
using CMS.Template;

namespace CMS.AdminUI.Controllers
{
    public class PublishController : BaseController
    {
        /// <summary>
        /// 发布栏目
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult PublishChannelPage(int? id)
        {
            ChannelInfo channelInfo;
            string absoPath = ConfigurationManager.AppSettings["SiteAbsolutePath"];

            if (id != null)
            {
                string templateContent;
                ChannelService channelService = new ChannelService();
                channelInfo = channelService.GetChannelInfo(id.Value);
                try
                {
                    templateContent = TemplateHandler.DealTemplate(channelInfo.TemplateID, channelInfo.ID, null);
                }
                catch (Exception ex)
                {
                    return Json(new {result = "error", message = ex.Message}, JsonRequestBehavior.AllowGet);
                }
                TemplateHandler.CreateFileByTemplateContent(new Hashtable(), templateContent, Encoding.UTF8,
                    absoPath + channelInfo.ChannelUrlPart);
                channelInfo.Status = 1;
                channelService.Update(channelInfo);
            }
            else
            {
                return Json(new { result = "error", message = "没有找到要发布的栏目" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = "success", message = "已经发布成功" }, JsonRequestBehavior.AllowGet);
        }
    }
}