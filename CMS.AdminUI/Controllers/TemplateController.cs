using System;
using System.Web.Mvc;

using Webdiyer.WebControls.Mvc;
using CMS.CommonLib.Extension;
using CMS.Domain;
using CMS.Service;

namespace CMS.AdminUI.Controllers
{
    /// <summary>
    /// 模板管理
    /// </summary>
    public class TemplateController : BaseController
    {
        private const int PAGESIZE = 15;

        public ActionResult Index(long? id, string name, int? type, int pageIndex = 1)
        {
            if (id != null)
            {
                ViewData["ID"] = id.Value;
            }
            else
            {
                ViewData["ID"] = "";
            }
            if (type != null)
            {
                ViewData["Type"] = type.Value;
            }
            else
            {
                ViewData["Type"] = "0";
            }
            ViewData["Name"] = name;
            TemplateService tpService = new TemplateService();
            PagerModel<Domain.TemplateInfo> pageModel = tpService.GetPagerTemplateList(id, name, type, PAGESIZE, pageIndex);
            var pagedTemplateList = new PagedList<Domain.TemplateInfo>(pageModel.ItemList, pageIndex, PAGESIZE, pageModel.TotalRecords);
            return View(pagedTemplateList);
        }

        /// <summary>
        /// 模板选择
        /// </summary>
        /// <param name="type"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public ActionResult SelTemplate(int? type,int pageIndex = 1)
        {
            if (type == null)
            {
                ViewData["TemplateType"] = "0";
            }
            else
            {
                ViewData["TemplateType"] = type.Value;
            }

            TemplateService tpService = new TemplateService();
            PagerModel<Domain.TemplateInfo> pageModel = tpService.GetPagerTemplateList(null, "", type, 10, pageIndex);
            var pagedTemplateList = new PagedList<Domain.TemplateInfo>(pageModel.ItemList, pageIndex, PAGESIZE, pageModel.TotalRecords);
            return View(pagedTemplateList);
        }

        public ActionResult Add(int?type)
        {
            if (type != null)
            {
                ViewData["Type"] = type;
            }
            else
            {
                ViewData["Type"] = 0;
            }
            return View();
        }

        /// <summary>
        /// 编辑模板信息
        /// </summary>
        /// <param name="templateID"></param>
        /// <returns></returns>
        public ActionResult Edit(long? templateID)
        {
            ViewData["Type"] = 0;
            Domain.TemplateInfo templateEntity;
            if (templateID != null)
            {
                ViewData["TemplateID"] = templateID.Value;
                TemplateService tpService = new TemplateService();
                templateEntity = tpService.GeTemplateInfo(templateID.Value);
                if (templateEntity == null)
                {
                    templateEntity = new Domain.TemplateInfo();
                }
                else
                {
                    if (templateEntity.Type == 6)
                    {
                        ViewData["Type"] = 6;
                    }
                }
            }
            else
            {
                ViewData["TemplateID"] = 0;
                templateEntity = new Domain.TemplateInfo();
            }
            return View(templateEntity);
        }

        public ActionResult Test(long? templateID,int? channelID,long? newsID)
        {
            string result = Template.TemplateHandler.DealTemplate(templateID, channelID, newsID);
            return Content(result);
        }

        # region 增 删 改

        [HttpPost, ValidateInput(false)]
        public ActionResult Create(Domain.TemplateInfo templateEntity)
        {
            string msg;
            try
            {
                if (ModelState.IsValid)
                {
                    TemplateService tpService = new TemplateService();
                    templateEntity.Status = 1;
                    templateEntity.CreateUserID = UserID;
                    bool flag = tpService.AddTemplate(templateEntity);
                    if (flag)
                    {
                        return Json(new {result = "ok"});
                    }
                    else
                    {
                        return Json(new {result = "error", msg = "添加失败，请联系管理员"});
                    }
                }
                else
                {
                    return Json(new { result = "error", msg = "添加失败，" + this.ExpendErrors() });
                }
            }
            catch (Exception ex)
            {
                return Json(new { result = "error", msg = "系统出现异常，"+ex.Message });
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="templateEntity"></param>
        /// <returns></returns>
        [HttpPost, ValidateInput(false)]
        public ActionResult Update(Domain.TemplateInfo templateEntity)
        {
            try
            {
                TemplateService tpService = new TemplateService();
                templateEntity.ModifyTime = DateTime.Now;
                templateEntity.ModifyUserID = UserID;
                bool flag = tpService.UpdateTemplate(templateEntity);
                if (flag)
                {
                    return Json(new { result = "ok" });
                }
                return Json(new { result = "error", msg = "更新失败，请联系管理员" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "error", msg = "系统出现异常，" + ex.Message });
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="templateEntity"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(Domain.TemplateInfo templateEntity)
        {
            try
            {
                TemplateService tpService = new TemplateService();
                bool flag = tpService.DeleteTemplate(templateEntity);
                if (flag)
                {
                    return Json(new { result = "ok" });
                }
                return Json(new { result = "error", msg = "删除失败，请联系管理员" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "error", msg = "系统出现异常，" + ex.Message });
            }
        }

        # endregion
    }
}
