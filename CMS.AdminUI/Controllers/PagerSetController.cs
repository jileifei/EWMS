using System;
using System.Web.Mvc;

using Webdiyer.WebControls.Mvc;
using CMS.CommonLib.Extension;
using CMS.Domain;
using CMS.Service;

namespace CMS.AdminUI.Controllers
{
    /// <summary>
    /// 分页设置信息
    /// </summary>
    public class PagerSetController : BaseController
    {
        private const int PAGESIZE = 20;

        public ActionResult Index(int pageIndex = 1)
        {
            PagerSetService psService = new PagerSetService();
            PagerModel<PagerInfo> pageModel = psService.GetPagerSetList(PAGESIZE, pageIndex);
            PagedList<PagerInfo> pagedPagerSet = new PagedList<PagerInfo>(pageModel.ItemList, pageIndex, PAGESIZE, pageModel.TotalRecords);
            return View(pagedPagerSet);
        }

        public ActionResult SelPagerSet(int pageIndex = 1)
        {
            PagerSetService psService = new PagerSetService();
            PagerModel<PagerInfo> pageModel = psService.GetPagerSetList(10, pageIndex);
            PagedList<PagerInfo> pagedPagerSet = new PagedList<PagerInfo>(pageModel.ItemList, pageIndex, 10, pageModel.TotalRecords);
            return View(pagedPagerSet);
        }

        public ActionResult Add()
        {
            return View();
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="pageID"></param>
        /// <returns></returns>
        public ActionResult Edit(int? pageID)
        {
            PagerInfo piEntity;
            if (pageID != null)
            {
                ViewData["PageID"] = pageID.Value;
                PagerSetService psService = new PagerSetService();
                piEntity = psService.GePagerSetInfo(pageID.Value);
                if (piEntity == null)
                {
                    piEntity = new PagerInfo();
                }
            }
            else
            {
                ViewData["PageID"] = 0;
                piEntity = new PagerInfo();
            }
            return View(piEntity);
        }

        # region 增 删 改

        [HttpPost, ValidateInput(false)]
        public ActionResult Create(PagerInfo piEntity)
        {
            string msg;
            try
            {
                if (ModelState.IsValid)
                {
                    PagerSetService psService = new PagerSetService();
                    bool flag = psService.AddPagerSet(piEntity);
                    if (flag)
                    {
                        msg = "{\"result\":\"ok\"}";
                    }
                    else
                    {
                        msg = "{\"result\":\"error\",\"msg\":\"添加失败，请联系管理员\"}";
                    }
                }
                else
                {
                    msg = "{\"result\":\"error\",\"msg\":\"添加失败，" + this.ExpendErrors() + "\"}";
                }
            }
            catch (Exception ex)
            {
                msg = "{\"result\":\"error\",\"msg\":\"系统出现异常，" + ex.Message + "\"}";
            }
            return Json(msg);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="piEntity"></param>
        /// <returns></returns>
        [HttpPost, ValidateInput(false)]
        public ActionResult Update(PagerInfo piEntity)
        {
            try
            {
                PagerSetService psService = new PagerSetService();
                bool flag = psService.UpdatePagerSet(piEntity);
                return Content(flag ? "{\"result\":\"ok\"}" : "{\"result\":\"error\",\"msg\":\"系统出现异常，请联系管理员\"}");
            }
            catch (Exception ex)
            {
                return Content("{\"result\":\"error\",\"msg\":\"" + ex.Message + "\"}");
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="piEntity"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(PagerInfo piEntity)
        {
            try
            {
                PagerSetService psService = new PagerSetService();
                bool flag = psService.DeletePagerSet(piEntity);
                return Content(flag ? "{\"result\":\"ok\"}" : "{\"result\":\"error\",\"msg\":\"系统出现异常，请联系管理员\"}");
            }
            catch (Exception ex)
            {
                return Content("{\"result\":\"error\",\"msg\":\"" + ex.Message + "\"}");
            }
        }

        # endregion
    }
}
