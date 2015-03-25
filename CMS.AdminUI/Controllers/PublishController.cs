﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using CMS.CommonLib.Utils;
using CMS.Domain;
using CMS.Service;
using CMS.Template;

namespace CMS.AdminUI.Controllers
{
    public class PublishController : BaseController
    {
        public string AbsoPath = ConfigurationManager.AppSettings["SiteAbsolutePath"];
        /// <summary>
        /// 发布栏目
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult PublishChannelPage(int? id)
        {
            ChannelInfo channelInfo;
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
                string linkurl = AbsoPath+channelInfo.ChannelUrlPart.Remove(channelInfo.ChannelUrlPart.LastIndexOf("/"));
                FileHandler.CheckDirectory(linkurl);
                TemplateHandler.CreateFileByTemplateContent(new Hashtable(), templateContent, Encoding.UTF8,
                    AbsoPath + channelInfo.ChannelUrlPart);
                channelInfo.Status = 1;
                channelService.Update(channelInfo);
            }
            else
            {
                return Json(new { result = "error", message = "没有找到要发布的栏目" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = "success", message = "已经发布成功" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PublishDetailPage(Int64 id, Int32 channelId)
        {
            string templateContent;
            ChannelService channelService = new ChannelService();
            ChannelInfo channelInfo = channelService.GetChannelInfo(channelId);
            try
            {
                templateContent = TemplateHandler.DealTemplate(channelInfo.ContentTemplateID, channelInfo.ID, id);
            }
            catch (Exception ex)
            {
                return Json(new { result = "error", message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            string p = channelInfo.ChannelUrlPart.Remove(channelInfo.ChannelUrlPart.LastIndexOf("/"));
            string linkurl = AbsoPath + p + "\\d\\" + DateTime.Now.ToString("yyyy-MM-dd");

            FileHandler.CheckDirectory(linkurl);
            TemplateHandler.CreateFileByTemplateContent(new Hashtable(), templateContent, Encoding.UTF8,
                linkurl + "\\0000000" + id + ".shtml");
            return Json(new {result = "success", message = "发布文章成功"}, JsonRequestBehavior.AllowGet);
        }
    }
}