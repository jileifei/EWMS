using System;
using System.Collections.Generic;
using System.Web.Mvc;

using Webdiyer.WebControls.Mvc;

using CMS.CommonLib.Extension;
using CMS.Domain;
using CMS.Service;

namespace CMS.AdminUI.Controllers
{
    public class PositionController : BaseController
    {
        private const int PAGESIZE = 20;

        public ActionResult Index(long? channelID, int pageIndex = 1)
        {
            RecommedPositionService positionService = new RecommedPositionService();
            PagerModel<RecommedPosition> pageModel = positionService.GetPagerPositionList(channelID, PAGESIZE, pageIndex);
            if (pageModel.ItemList.Count == 0)
            {
                pageModel.ItemList.Add(new RecommedPosition());
            }
            PagedList<RecommedPosition> pagedPosition = new PagedList<RecommedPosition>(pageModel.ItemList, pageIndex, PAGESIZE, pageModel.TotalRecords);
            return View(pagedPosition);
        }

        # region 增 删 改

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="positionInfo"></param>
        /// <returns></returns>
        [HttpPost, ValidateInput(false)]
        public ActionResult Add(RecommedPosition positionInfo)
        {
            string msg;
            try
            {
                if (ModelState.IsValid)
                {
                    RecommedPositionService positionService = new RecommedPositionService();
                    bool flag = positionService.AddPosition(positionInfo);
                    if (flag)
                    {
                        return Json(new { result = "ok" });
                    }
                    return Json(new { result = "error", msg = "添加失败，请联系管理员" });
                }
                return Json(new { result = "error", msg = "添加失败，" + this.ExpendErrors() });
            }
            catch (Exception ex)
            {
                return Json(new { result = "error", msg = ex.Message });
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="positionInfo"></param>
        /// <returns></returns>
        [HttpPost, ValidateInput(false)]
        public ActionResult Update(RecommedPosition positionInfo)
        {
            try
            {
                RecommedPositionService positionService = new RecommedPositionService();
                bool flag = positionService.UpdatePosition(positionInfo);
                if (flag)
                {
                    return Json(new { result = "ok" });
                }
                return Json(new { result = "error", msg = "系统出现异常，请联系管理员" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "error", msg = ex.Message });
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="positionInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(RecommedPosition positionInfo)
        {
            try
            {
                RecommedPositionService positionService = new RecommedPositionService();
                bool flag = positionService.DeletePosition(positionInfo);
                if (flag)
                {
                    return Json(new { result = "ok" });
                }
                return Json(new { result = "error", msg = "系统出现异常，请联系管理员" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "error", msg = ex.Message });
            }
        }

        # endregion

        # region select

        /// <summary>
        /// select by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetPositionInfo(long? id)
        {
            RecommedPosition positionInfo;
            if (id != null)
            {
                RecommedPositionService positionService = new RecommedPositionService();
                positionInfo = positionService.GetPositionInfo(id.Value);
            }
            else
            {
                positionInfo = new RecommedPosition();
            }
            return Json(positionInfo, JsonRequestBehavior.AllowGet);
        }

        # endregion

        /// <summary>
        /// 根据推荐位置列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetPositionList()
        {
            IList<RecommedPosition> positionList = new List<RecommedPosition>();
            string msg = "";
            if (Request.IsAjaxRequest())
            {
                try
                {
                    RecommedPositionService positionService = new RecommedPositionService();
                    positionList = positionService.GetPostionList();
                }
                catch (Exception ex)
                {
                    msg = "数据请求出现异常，请稍后再试：" + ex.Message;
                }
            }
            else
            {
                msg = "请正常访问数据源";
            }
            return Json(new { list = positionList, msg = msg });
        }
    }
}
