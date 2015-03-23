using System;
using System.Web.Mvc;
using System.Configuration;

using Webdiyer.WebControls.Mvc;
using CMS.CommonLib.Utils;
using CMS.CommonLib.Extension;
using CMS.Domain;
using CMS.Service;

namespace CMS.AdminUI.Controllers
{
    /// <summary>
    /// 全局变量
    /// </summary>
    public class GlobalController : BaseController
    {
        private const int PAGESIZE = 20;

        public ActionResult Index(int pageIndex = 1)
        {
            GlobalService globalService = new GlobalService();
            PagerModel<GlobalVariable> pageModel = globalService.GetPagerGlobalVarList(PAGESIZE, pageIndex);
            PagedList<GlobalVariable> pagedGlobalVar = new PagedList<GlobalVariable>(pageModel.ItemList, pageIndex, PAGESIZE, pageModel.TotalRecords);
            return View(pagedGlobalVar);
        }

        public ActionResult SelGlobal(int pageIndex = 1)
        {
            GlobalService globalService = new GlobalService();
            PagerModel<GlobalVariable> pageModel = globalService.GetPagerGlobalVarList(PAGESIZE, pageIndex);
            PagedList<GlobalVariable> pagedGlobalVar = new PagedList<GlobalVariable>(pageModel.ItemList, pageIndex, PAGESIZE, pageModel.TotalRecords);
            return View(pagedGlobalVar);
        }

        /// <summary>
        /// 选择系统变量
        /// </summary>
        /// <returns></returns>
        public ActionResult Var()
        {
            return View();
        }

        /// <summary>
        /// 添加全局变量
        /// </summary>
        /// <returns></returns>
        public ActionResult Add()
        {
            return View();
        }

        /// <summary>
        /// 修改全局变量
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit(long? globalID)
        {
            GlobalVariable globalVarEntity;
            if (globalID != null)
            {
                ViewData["GlobalID"] = globalID.Value;
                GlobalService globalService = new GlobalService();
                globalVarEntity = globalService.GeGlobalInfo(globalID.Value);
                if (globalVarEntity == null)
                {
                    globalVarEntity = new GlobalVariable();
                }
            }
            else
            {
                ViewData["GlobalID"] = 0;
                globalVarEntity = new GlobalVariable();
            }
            return View(globalVarEntity);
        }

        # region 增 删 改

        [HttpPost, ValidateInput(false)]
        public ActionResult Create(GlobalVariable globalEntity, string fileEncode)
        {
            string msg;
            try
            {
                if (ModelState.IsValid)
                {
                    GlobalService globalVarService = new GlobalService();
                    bool flag = globalVarService.AddGlobalVar(globalEntity);
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
        /// <param name="globalVarInfo"></param>
        /// <param name="fileEncode"></param>
        /// <returns></returns>
        [HttpPost, ValidateInput(false)]
        public ActionResult Update(GlobalVariable globalVarInfo, string fileEncode)
        {
            try
            {
                GlobalService globalService = new GlobalService();
                bool flag = globalService.UpdateGlobalVar(globalVarInfo);
                if (flag)
                {
                    if (globalVarInfo.IsInclude)
                    {
                        // 生成静态shtml页面
                        FileHandler.Write(globalVarInfo.Content, ConfigurationManager.AppSettings["IncludePath"]
                           + "/" + globalVarInfo.EnName + ".shtml", Utils.GetEncodingByEncode(fileEncode));
                    }
                    return Content("{\"result\":\"ok\"}");
                }
                return Content("{\"result\":\"error\",\"msg\":\"系统出现异常，请联系管理员\"}");
            }
            catch (Exception ex)
            {
                return Content("{\"result\":\"error\",\"msg\":\"" + ex.Message + "\"}");
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="globalInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(GlobalVariable globalInfo)
        {
            try
            {
                GlobalService globalService = new GlobalService();
                bool flag = globalService.DeleteGlobalVar(globalInfo);
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

        # endregion

        # region select

        /// <summary>
        /// select by id
        /// </summary>
        /// <param name="globalID"></param>
        /// <returns></returns>
        public ActionResult GetGlobalVarInfo(long? globalID)
        {
            GlobalVariable globalVarInfo;
            if (globalID != null)
            {
                GlobalService globalVarService = new GlobalService();
                globalVarInfo = globalVarService.GeGlobalInfo(globalID.Value);
            }
            else
            {
                globalVarInfo = new GlobalVariable();
            }
            return Json(globalVarInfo, JsonRequestBehavior.AllowGet);
        }

        # endregion

        # region Check EnName is exists

        /// <summary>
        /// 检测全局变量英文名称是否存在
        /// </summary>
        /// <param name="enName"></param>
        /// <param name="globalID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CheckEnName(string enName,long? globalID)
        {
            GlobalService globalService = new GlobalService();
            bool exists = globalService.CheckEnName(enName, globalID);
            return Json(!exists, JsonRequestBehavior.AllowGet);
        }

        # endregion
    }
}
