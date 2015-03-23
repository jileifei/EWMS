using System;
using System.Web.Mvc;

using Webdiyer.WebControls.Mvc;

using CMS.CommonLib.Extension;
using CMS.Domain;
using CMS.Service;

namespace CMS.AdminUI.Controllers
{
    /// <summary>
    /// 推荐内容
    /// </summary>
    public class RecommendController : BaseController
    {
        # region Index Action

        private const int PAGESIZE = 20;

        /// <summary>
        /// 推荐内容
        /// </summary>
        /// <param name="positionID"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public ActionResult Index(long? positionID,int pageIndex = 1)
        {
            if (positionID == null)
            {
                RedirectToAction("Index", "Position");
                ViewData["PositionID"] = "0";
                ViewData["PositionName"] = "";
                ViewData["PositionType"] = "";
            }
            else
            {
                
                RecommedPositionService positionService = new RecommedPositionService();
                RecommedPosition positionInfo = positionService.GetPositionInfo(positionID.Value);
                if (positionInfo != null)
                {
                    ViewData["PositionID"] = positionID.Value;
                    ViewData["PositionName"] = positionInfo.Name;
                    ViewData["PositionType"] = positionInfo.LocationType;
                }
                else
                {
                    RedirectToAction("Index", "Position");
                    ViewData["PositionID"] = "0";
                    ViewData["PositionName"] = "";
                    ViewData["PositionType"] = "";
                }
            }
            RecommendService recommendService = new RecommendService();
            PagerModel<RecommedInfoList> pageModel = recommendService.GetPagerRecommendList(positionID??0, PAGESIZE, pageIndex);
            if (pageModel.ItemList.Count == 0)
            {
                pageModel.ItemList.Add(new RecommedInfoList());
            }
            PagedList<RecommedInfoList> pagedPosition = new PagedList<RecommedInfoList>(pageModel.ItemList, pageIndex, PAGESIZE, pageModel.TotalRecords);
            return View(pagedPosition);
        }

        # endregion

        # region 增 删 改

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="recommendInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Add(RecommedInfoList recommendInfo)
        {
            string msg;
            try
            {
                if (ModelState.IsValid)
                {
                    RecommendService recommendService = new RecommendService();
                    recommendInfo.CreateUserID = UserID;
                    recommendInfo.Status = 1;
                    bool flag = recommendService.AddRecommend(recommendInfo);
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
        /// <param name="recommendInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Update(RecommedInfoList recommendInfo)
        {
            try
            {
                RecommendService recommendService = new RecommendService();
                recommendInfo.Status = 1;
                bool flag = recommendService.UpdatePosition(recommendInfo);
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
        /// <param name="recommendInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(RecommedInfoList recommendInfo)
        {
            try
            {
                RecommendService recommendService = new RecommendService();
                bool flag = recommendService.DeletePosition(recommendInfo);
                return Content(flag ? "{\"result\":\"ok\"}" : "{\"result\":\"error\",\"msg\":\"系统出现异常，请联系管理员\"}");
            }
            catch (Exception ex)
            {
                return Content("{\"result\":\"error\",\"msg\":\"" + ex.Message + "\"}");
            }
        }

        # endregion

        # region select

        /// <summary>
        /// select by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetRecommendInfo(long? id)
        {
            RecommedInfoList recommendInfo;
            if (id != null)
            {
                RecommendService recommendService = new RecommendService();
                recommendInfo = recommendService.GetRecommendInfo(id.Value);
            }
            else
            {
                recommendInfo = new RecommedInfoList();
            }
            return Json(recommendInfo, JsonRequestBehavior.AllowGet);
        }

        # endregion
    }
}
