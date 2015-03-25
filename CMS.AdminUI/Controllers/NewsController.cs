using System;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Services.Discovery;
using CMS.CommonLib.Extension;
using CMS.CommonLib.Utils;
using CMS.Domain;
using CMS.Service;
using Webdiyer.WebControls.Mvc;

namespace CMS.AdminUI.Controllers
{
    public class NewsController : BaseController
    {
        private const int PAGESIZE = 10;
        private string _siteurl = ConfigurationManager.AppSettings["WebUrl"];
        //public ActionResult Index()
        //{
        //    ModuleService moduleService = new ModuleService();
        //    ViewBag.IsHaveAdd = moduleService.CheckUreIsHaveRight(RoleID, "/news/post");
        //    return View();
        //}

        public ActionResult Index(long? channelID, long? id, string title, string beginTime, string endTime,
            int pageIndex = 1)
        {
            //if (beginTime == null) throw new ArgumentNullException("beginTime");

            NewsService newsService = new NewsService();
            PagerModel<NewsDoc> pageModel = newsService.GetPagerNewsListByChannelID(0, channelID, id, title, beginTime, endTime, PAGESIZE, pageIndex);
            var pagedNewsList = new PagedList<Domain.NewsDoc>(pageModel.ItemList, pageIndex, PAGESIZE, pageModel.TotalRecords);
            return View(pagedNewsList);
        }

        /// <summary>
        /// 搜索分页
        /// </summary>
        /// <param name="channelID"></param>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult ServerAjax(long? channelID, long? id, string title, string beginTime, string endTime, int pageIndex = 1, int pageSize = 10)
        {
            if (beginTime == null) throw new ArgumentNullException("beginTime");
            PagerModel<NewsDoc> pageModel = new PagerModel<NewsDoc>();
            if (Request.IsAjaxRequest())
            {
                NewsService newsService = new NewsService();
                pageModel = newsService.GetPagerNewsListByChannelID(0, channelID, id, title, beginTime, endTime, pageSize, pageIndex);
            }
            return Json(pageModel);
        }

        # region post

        public ActionResult Post()
        {
            ViewData["PublisDate"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            return View();
        }

        /// <summary>
        /// 发表新闻提交
        /// </summary>
        /// <param name="newsEntity"></param>
        /// <returns></returns>
        [HttpPost, ValidateInput(false)]
        public ActionResult PostNews(NewsDoc newsEntity)
        {
            string msg;
            if (newsEntity != null)
            {
                if (string.IsNullOrEmpty(newsEntity.Linkurl) && string.IsNullOrEmpty(newsEntity.Content))
                {
                    ModelState.AddModelError("Content", "请输入新闻内容");
                }
                if (ModelState.IsValid)
                {
                    if (string.IsNullOrEmpty(newsEntity.Author))
                    {
                        newsEntity.Author = "梦都酒家";
                    }
                    if (newsEntity.SortID == null)
                    {
                        newsEntity.SortID = 0;
                    }
                    if (newsEntity.SpecialChannelID == null)
                    {
                        newsEntity.SpecialChannelID = 0;
                    }
                    if (newsEntity.PublicTime == null)
                    {
                        newsEntity.PublicTime = DateTime.Now;
                    }
                    newsEntity.CreateUserID = UserID;
                    newsEntity.CreateTime = DateTime.Now;
                    newsEntity.CreateUserIP = Utils.GetRealIP();
                    ChannelService channelService = new ChannelService();
                    ChannelInfo channelInfo = channelService.GetChannelInfo(newsEntity.ChannelID);
                    string linkUrl =_siteurl+channelInfo.ChannelUrlPart.Remove(channelInfo.ChannelUrlPart.LastIndexOf("/"));
                    try
                    {
                        NewsService newsService = new NewsService();
                        Int64 flag = newsService.AddPost(newsEntity);
                        if (flag > 0)
                        {
                            linkUrl = linkUrl + "/d/" + DateTime.Now.ToString("yyyy-MM-dd") + "/0000000"+ flag + ".shtml";
                            newsService.UpdateLinkUrl(linkUrl, flag);
                            msg = "{\"result\":\"ok\",\"NewsID\":\""+flag+"\"}";
                        }
                        else
                        {
                            msg = "{\"result\":\"error\",\"msg\":\"添加失败，请联系管理员\"}";
                        }
                    }
                    catch (Exception ex)
                    {
                        msg = "{\"result\":\"error\",\"msg\":\"系统出现异常，" + ex.Message + "\"}";
                    }
                }
                else
                {
                    msg = "{\"result\":\"error\",\"msg\":\"添加失败，" + this.ExpendErrors() + "\"}";
                }
            }
            else
            {
                msg = "{\"result\":\"error\",\"msg\":\"添加失败，请按照正常顺序发布新闻\"}";
            }
            return Json(msg);
        }

        # endregion

        # region edit

        public ActionResult Edit(int? newsID)
        {
            NewsDoc newsEntity;
            if (newsID != null)
            {
                NewsService newsService = new NewsService();
                newsEntity = newsService.GetNewsInfoByID(newsID.Value);
                if (newsEntity == null)
                {
                    newsEntity = new NewsDoc();
                }
            }
            else
            {
                newsEntity = new NewsDoc();
            }
            return View(newsEntity);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult EditNews(NewsDoc newsEntity)
        {
            string msg;
            if (newsEntity != null)
            {
                if (string.IsNullOrEmpty(newsEntity.Linkurl) && string.IsNullOrEmpty(newsEntity.Content))
                {
                    ModelState.AddModelError("Content", "请输入新闻内容");
                }
                if (ModelState.IsValid)
                {
                    if (string.IsNullOrEmpty(newsEntity.Author))
                    {
                        newsEntity.Author = "梦都酒家";
                    }
                    if (newsEntity.SortID == null)
                    {
                        newsEntity.SortID = 0;
                    }
                    if (newsEntity.SpecialChannelID == null)
                    {
                        newsEntity.SpecialChannelID = 0;
                    }
                    if (newsEntity.PublicTime == null)
                    {
                        newsEntity.PublicTime = DateTime.Now;
                    }
                    newsEntity.ModifyUserID = UserID;
                    newsEntity.ModifyTime = DateTime.Now;
                    newsEntity.ModifyUserIP = Utils.GetRealIP();
                    ChannelService channelService = new ChannelService();
                    ChannelInfo channelInfo = channelService.GetChannelInfo(newsEntity.ChannelID);
                    string linkUrl = _siteurl+channelInfo.ChannelUrlPart.Remove(channelInfo.ChannelUrlPart.LastIndexOf("/"));
                    try
                    {
                        NewsService newsService = new NewsService();
                        bool flag = newsService.EditPost(newsEntity);
                        if (flag)
                        {
                            linkUrl = linkUrl + "/d/" + DateTime.Now.ToString("yyyy-MM-dd") + "/0000000" + newsEntity.ID + ".shtml";
                            newsService.UpdateLinkUrl(linkUrl, newsEntity.ID);
                            msg = "{\"result\":\"ok\",\"NewsID\":\"" + newsEntity.ID + "\"}";
                        }
                        else
                        {
                            msg = "{\"result\":\"error\",\"msg\":\"编辑失败，请联系管理员\"}";
                        }
                    }
                    catch (Exception ex)
                    {
                        msg = "{\"result\":\"error\",\"msg\":\"系统出现异常，" + ex.Message + "\"}";
                    }
                }
                else
                {
                    msg = "{\"result\":\"error\",\"msg\":\"编辑失败，" + this.ExpendErrors() + "\"}";
                }
            }
            else
            {
                msg = "{\"result\":\"error\",\"msg\":\"编辑失败，请按照正常顺序编辑新闻\"}";
            }
            return Json(msg);
        }

        # endregion

        # region del

        [HttpPost]
        public ActionResult DeleteNews(long? newsID)
        {
            if (newsID == null)
                return Content("{\"result\":\"error\",\"msg\":\"请选择要删除的文章\"}");
            if (Request.IsAjaxRequest())
            {
                try
                {
                    NewsService newsService = new NewsService();
                    bool flag = newsService.UpdateDelFlag(newsID.Value);
                    if (flag)
                    {
                        return Content("{\"result\":\"ok\"}");
                    }
                    return Content("{\"result\":\"error\",\"msg\":\"系统出现异常，请联系管理员\"}");
                }
                catch (Exception ex)
                {
                    return Content("{\"result\":\"error\",\"msg\":\"" + ex.Message + "\"}");
                }
            }
            return Content("{\"result\":\"error\",\"msg\":\"请正常访问删除文章页面\"}");
        }

        # endregion

        # region restore

        [HttpPost]
        public ActionResult RestoreNews(long? newsID)
        {
            if (newsID == null)
                return Content("{\"result\":\"error\",\"msg\":\"请选择要恢复的文章\"}");
            if (Request.IsAjaxRequest())
            {
                try
                {
                    NewsService newsService = new NewsService();
                    bool flag = newsService.RestoreDelFlag(newsID.Value);
                    return flag
                        ? Content("{\"result\":\"ok\"}")
                        : Content("{\"result\":\"error\",\"msg\":\"系统出现异常，请联系管理员\"}");
                }
                catch (Exception ex)
                {
                    return Content("{\"result\":\"error\",\"msg\":\"" + ex.Message + "\"}");
                }
            }
            return Content("{\"result\":\"error\",\"msg\":\"请正常访问删除文章页面\"}");
        }

        # endregion

        # region auditing

        [HttpPost]
        public ActionResult AuditingNews(long? newsID, int? auditing)
        {
            if (newsID == null)
                return Content("{\"result\":\"error\",\"msg\":\"请选择要审核的文章\"}");
            if (Request.IsAjaxRequest())
            {
                try
                {
                    NewsService newsService = new NewsService();
                    bool flag = newsService.AuditNews(newsID.Value, auditing ?? 0, UserID);
                    return Content(flag ? "{\"result\":\"ok\"}" : "{\"result\":\"error\",\"msg\":\"系统出现异常，请联系管理员\"}");
                }
                catch (Exception ex)
                {
                    return Content("{\"result\":\"error\",\"msg\":\"" + ex.Message + "\"}");
                }
            }
            return Content("{\"result\":\"error\",\"msg\":\"请正常访问审核文章页面\"}");
        }

        # endregion

        # region check is have right

        /// <summary>
        /// 检测用户是否有操作权限
        /// </summary>
        /// <param name="newsID">新闻ID</param>
        /// <param name="checkType">检测类型 1=编辑 2=删除 3=审核</param>
        /// <returns></returns>
        public ActionResult CheckIsHaveRight(long? newsID, int checkType)
        {
            bool flag = false;
            if (newsID != null)
            {
                ChannelService channelService = new ChannelService();
                flag = channelService.CheckNewsIsHaveOperRight(newsID.Value, RoleID, checkType);
            }
            return Json(flag);
        }

        # endregion

        # region recycle

        /// <summary>
        /// 回收站
        /// </summary>
        /// <returns></returns>
        public ActionResult Recycle()
        {
            return View();
        }

        /// <summary>
        /// 搜索分页
        /// </summary>
        /// <param name="channelID"></param>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult RecycleServerAjax(long? channelID, long? id, string title, string beginTime, string endTime, int pageIndex = 1, int pageSize = 10)
        {
            PagerModel<NewsDoc> pageModel = new PagerModel<NewsDoc>();
            if (Request.IsAjaxRequest())
            {
                NewsService newsService = new NewsService();
                pageModel = newsService.GetPagerNewsListByChannelID(1, channelID, id, title, beginTime, endTime, pageSize, pageIndex);
            }
            return Json(pageModel);
        }

        # endregion
    }
}
