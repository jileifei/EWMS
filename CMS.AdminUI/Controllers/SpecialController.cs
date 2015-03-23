using System;
using System.Web.Mvc;

using Webdiyer.WebControls.Mvc;

using CMS.CommonLib.Extension;
using CMS.Domain;
using CMS.Service;

namespace CMS.AdminUI.Controllers
{
    public class SpecialController : BaseController
    {
        private const int PAGESIZE = 20;

        // 专题
        public ActionResult Index(int pageIndex = 1)
        {
            SpecialService specialService = new SpecialService();
            PagerModel<SpecialInfo> pageModel = specialService.GetPagerSpecialList(PAGESIZE, pageIndex);
            PagedList<SpecialInfo> pagedPosition = new PagedList<SpecialInfo>(pageModel.ItemList, pageIndex, PAGESIZE, pageModel.TotalRecords);
            return View(pagedPosition);
        }

        # region add

        public ActionResult Add()
        {
            return View();
        }

        /// <summary>
        /// 新增专题
        /// </summary>
        /// <param name="specialEntity"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddHandler(SpecialInfo specialEntity)
        {
            string msg;
            if (specialEntity != null)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        if (string.IsNullOrEmpty(specialEntity.SmallPicUrl))
                        {
                            specialEntity.SmallPicUrl = "";
                        }
                        if (string.IsNullOrEmpty(specialEntity.PicUrl))
                        {
                            specialEntity.PicUrl = "";
                        }
                        SpecialService specialService = new SpecialService();
                        bool flag = specialService.AddSpecial(specialEntity);
                        if (flag)
                        {
                            msg = "{\"result\":\"ok\"}";
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
                msg = "{\"result\":\"error\",\"msg\":\"添加失败，请填写专题的相关信息\"}";
            }
            return Json(msg);
        }

        # endregion

        # region edit

        public ActionResult Edit(int? specialID)
        {
            SpecialInfo specialEntity;
            if (specialID != null)
            {
                SpecialService specialService = new SpecialService();
                specialEntity = specialService.GetSpecialInfoByID(specialID.Value);
                if (specialEntity == null)
                {
                    specialEntity = new SpecialInfo();
                }
            }
            else
            {
                specialEntity = new SpecialInfo();
            }
            return View(specialEntity);
        }

        [HttpPost]
        public ActionResult EditHandler(SpecialInfo specialEntity)
        {
            string msg;
            if (specialEntity != null)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        if (string.IsNullOrEmpty(specialEntity.SmallPicUrl))
                        {
                            specialEntity.SmallPicUrl = "";
                        }
                        if (string.IsNullOrEmpty(specialEntity.PicUrl))
                        {
                            specialEntity.PicUrl = "";
                        }
                        SpecialService specialService = new SpecialService();
                        bool flag = specialService.EditSpecial(specialEntity);
                        if (flag)
                        {
                            msg = "{\"result\":\"ok\"}";
                        }
                        else
                        {
                            msg = "{\"result\":\"error\",\"msg\":\"修改专题信息失败，请联系管理员\"}";
                        }
                    }
                    catch (Exception ex)
                    {
                        msg = "{\"result\":\"error\",\"msg\":\"系统出现异常，" + ex.Message + "\"}";
                    }
                }
                else
                {
                    msg = "{\"result\":\"error\",\"msg\":\"编辑修改专题信息失败失败，" + this.ExpendErrors() + "\"}";
                }
            }
            else
            {
                msg = "{\"result\":\"error\",\"msg\":\"修改专题信息失败，请填写专题的相关信息\"}";
            }
            return Json(msg);
        }

        # endregion

        # region delete

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="spEntity"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(SpecialInfo spEntity)
        {
            try
            {
                SpecialService spService = new SpecialService();
                bool flag = spService.DelSpecial(spEntity);
                return Content(flag ? "{\"result\":\"ok\"}" : "{\"result\":\"error\",\"msg\":\"系统出现异常，请联系管理员\"}");
            }
            catch (Exception ex)
            {
                return Content("{\"result\":\"error\",\"msg\":\"" + ex.Message + "\"}");
            }
        }

        # endregion

        /// <summary>
        /// 选择专题
        /// </summary>
        /// <returns></returns>
        public ActionResult SelSpecial()
        {
            return View();
        }


        # region 专题栏目

        /// <summary>
        /// 获取专题树节点数据
        /// </summary>
        /// <returns></returns>
        public ActionResult ChannelAjaxLoading()
        {
            SpecialService specialService = new SpecialService();
            DynatreeModel channelDynatreeModelList = specialService.GetSpecialTreeList();
            return Json(channelDynatreeModelList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 专题栏目
        /// </summary>
        /// <returns></returns>
        public ActionResult SpecialChannel(long? specialID)
        {
            if (specialID != null)
            {
                ViewBag.SpecialID = specialID.Value;
            }
            else
            {
                ViewBag.SpecialID = 0;
            }
            return View();
        }

        /// <summary>
        /// 专题栏目数据读取
        /// </summary>
        /// <param name="specialID"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult SpecialChannelServer(long? specialID, int pageIndex = 1, int pageSize = 10)
        {
            PagerModel<SpecialChannel> pageModel = new PagerModel<SpecialChannel>();
            if (Request.IsAjaxRequest() && specialID != null)
            {
                SpecialChannelService sChannelService = new SpecialChannelService();
                pageModel = sChannelService.GetPagerSpecialChannelList(specialID.Value, pageSize, pageIndex);
            }
            return Json(pageModel);
        }

        # region 专题栏目增删改查

        /// <summary>
        /// 新建专题栏目
        /// </summary>
        /// <param name="sChannelInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SpecialChannelCreate(SpecialChannel sChannelInfo)
        {
            string msg;
            try
            {
                if (ModelState.IsValid)
                {
                    SpecialChannelService sChannelService = new SpecialChannelService();
                    sChannelInfo.CreateUserID = UserID;
                    sChannelInfo.ModifyUserID = UserID;
                    sChannelInfo.CreateTiem = DateTime.Now;
                    sChannelInfo.ModifyTime = DateTime.Now;
                    bool flag = sChannelService.AddSpecialChannel(sChannelInfo);
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
        /// <param name="sChannelInfo"></param>
        /// <returns></returns>
        [HttpPost, ValidateInput(false)]
        public ActionResult UpdateSpecialChannel(SpecialChannel sChannelInfo)
        {
            try
            {
                SpecialChannelService sChannelService = new SpecialChannelService();
                sChannelInfo.ModifyTime = DateTime.Now;
                sChannelInfo.ModifyUserID = UserID;
                bool flag = sChannelService.UpdateSpecialChannel(sChannelInfo);
                return Content(flag ? "{\"result\":\"ok\"}" : "{\"result\":\"error\",\"msg\":\"系统出现异常，请联系管理员\"}");
            }
            catch (Exception ex)
            {
                return Content("{\"result\":\"error\",\"msg\":\"" + ex.Message + "\"}");
            }
        }

        /// <summary>
        /// 删除专题栏目
        /// </summary>
        /// <param name="sChannelInfo"></param>
        /// <returns></returns>
        public ActionResult DelSpecialChannel(SpecialChannel sChannelInfo)
        {
            try
            {
                SpecialChannelService sChannelService = new SpecialChannelService();
                bool flag = sChannelService.DeleteSpecialChannel(sChannelInfo);
                return Content(flag ? "{\"result\":\"ok\"}" : "{\"result\":\"error\",\"msg\":\"系统出现异常，请联系管理员\"}");
            }
            catch (Exception ex)
            {
                return Content("{\"result\":\"error\",\"msg\":\"" + ex.Message + "\"}");
            }
        }

        /// <summary>
        /// select by id
        /// </summary>
        /// <param name="specialChannelID"></param>
        /// <returns></returns>
        public ActionResult GetSpecialChannel(long? specialChannelID)
        {
            SpecialChannel sChannelInfo;
            if (specialChannelID != null)
            {
                SpecialChannelService sChannelService = new SpecialChannelService();
                sChannelInfo = sChannelService.GetSpecialChannel(specialChannelID.Value);
            }
            else
            {
                sChannelInfo = new SpecialChannel();
            }
            return Json(sChannelInfo, JsonRequestBehavior.AllowGet);
        }

        # endregion

        # endregion
    }
}
