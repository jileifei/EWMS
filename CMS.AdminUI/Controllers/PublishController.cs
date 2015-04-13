using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
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
        private string _siteurl = ConfigurationManager.AppSettings["WebUrl"];
        /// <summary>
        /// 发布栏目
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult PublishChannelPage(long? id)
        {
            StringBuilder builder = new StringBuilder();
            ChannelInfo channelInfo;
            if (id != null)
            {
                string templateContent;
                //string listContent;
                ChannelService channelService = new ChannelService();
                channelInfo = channelService.GetChannelInfo(id.Value);
                try
                {
                    templateContent = TemplateHandler.DealTemplate(channelInfo.TemplateID, channelInfo.ID, null);

                    string dir = channelInfo.ChannelUrlPart.Remove(channelInfo.ChannelUrlPart.LastIndexOf("/"));
                    string linkurl = AbsoPath + dir;
                    FileHandler.CheckDirectory(linkurl);
                    TemplateHandler.CreateFileByTemplateContent(new Hashtable(), templateContent, Encoding.UTF8,
                        AbsoPath + channelInfo.ChannelUrlPart);
                    builder.AppendLine(channelInfo.ChannelUrlPart + "  发布成功！");
                    channelInfo.Status = 1;
                    channelService.Update(channelInfo);
                }
                catch (Exception ex)
                {
                    return Json(new { result = "error", message = ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { result = "error", message = "没有找到要发布的栏目" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = "success", message = builder }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 发布详情页
        /// </summary>
        /// <param name="id"></param>
        /// <param name="channelId"></param>
        /// <returns></returns>
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
            return Json(new { result = "success", message = "发布信息成功" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 发布列表页
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns></returns>
        public ActionResult PublishListPage(Int32 channelId)
        {
            try
            {
                ChannelService channelService = new ChannelService();
                ChannelInfo channelInfo = channelService.GetChannelInfo(channelId);
                if (channelInfo.ListTemplateID == null)
                {
                    return Json(new {result = "false", message = "没有设置列表模版"}, JsonRequestBehavior.AllowGet);
                }
                string listContent = "";
                string dir = channelInfo.ChannelUrlPart.Remove(channelInfo.ChannelUrlPart.LastIndexOf("/"));
                string linkurl = AbsoPath + dir;
                StringBuilder builder = new StringBuilder();
                FileHandler.CheckDirectory(linkurl);
                //列表页
                PagerHandler pagerHandler = new PagerHandler();
                Int32 tatals = pagerHandler.GetPagerTatals(channelId);
                Dictionary<int, string> pageLink = new Dictionary<int, string>();
                for (Int32 i = 1; i <= tatals; i++)
                {
                    string pagedir = _siteurl + dir + "/" + channelInfo.EnName + "-list-" + i + ".shtml";
                    pageLink.Add(i, pagedir);
                }

                for (Int32 i = 1; i <= tatals; i++)
                {
                    string pagedir = dir + "/" + channelInfo.EnName + "-list-" + i + ".shtml";
                    listContent = TemplateHandler.DealListTemplate(channelInfo.ListTemplateID, channelInfo.ID, i,
                        pageLink);
                    TemplateHandler.CreateFileByTemplateContent(new Hashtable(), listContent, Encoding.UTF8,
                        AbsoPath + dir + "/" + channelInfo.EnName + "-list-" + i + ".shtml");
                }
                return Json(new { result = "success", message = "列表页更新成功" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = "error", message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
           
        }
    }
}