using System;
using System.Collections.Generic;
using System.Web.Mvc;

using CMS.Domain;
using CMS.Service;
using CMS.CommonLib.Extension;

namespace CMS.AdminUI.Controllers
{
    /// <summary>
    /// 模块管理
    /// </summary>
    public class ModuleController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        # region 增 删 改 排序

        /// <summary>
        /// 添加模块信息
        /// </summary>
        /// <param name="moduleInfo"></param>
        /// <returns></returns>
        public ActionResult Add(ModuleInfo moduleInfo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ModuleService moduleService = new ModuleService();
                    moduleInfo.ModuleType = 0;// 0=菜单 1=按钮
                    moduleInfo.CreateUserID = UserID;
                    if (string.IsNullOrEmpty(moduleInfo.Description))
                    {
                        moduleInfo.Description = "";
                    }
                    long channelID = moduleService.Add(moduleInfo);
                    return Content("{\"ID\":\"" + channelID + "\"}");
                }
                else
                {
                    return Content("{\"ID\":\"0\",\"msg\":\"" + this.ExpendErrors() + "\"}");
                }
            }
            catch (Exception ex)
            {
                return Content("{\"ID\":\"0\",\"msg\":\"" + ex.Message + "\"}");
            }
        }

        /// <summary>
        /// 修改模块信息
        /// </summary>
        /// <param name="moduleInfo"></param>
        /// <returns></returns>
        public ActionResult Update(ModuleInfo moduleInfo)
        {
            try
            {
                ModuleService moduleService = new ModuleService();
                if (string.IsNullOrEmpty(moduleInfo.Description))
                {
                    moduleInfo.Description = "";
                }
                bool flag = moduleService.Update(moduleInfo);
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

        /// <summary>
        /// 删除模块信息
        /// </summary>
        /// <param name="moduleInfo"></param>
        /// <returns></returns>
        public ActionResult Delete(ModuleInfo moduleInfo)
        {
            try
            {
                ModuleService moduleService = new ModuleService();
                bool flag = moduleService.Delete(moduleInfo);
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

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="sourceID"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Sort(int? sourceID, int? id)
        {
            try
            {
                if (sourceID != null && id != null)
                {
                    ModuleService moduleService = new ModuleService();
                    bool flag = moduleService.Sort(sourceID.Value, id.Value);
                    if (flag)
                    {
                        return Content("{\"result\":\"ok\"}");
                    }
                    return Content("{\"result\":\"error\",\"msg\":\"服务器端出现异常，请稍后再试\"}");
                }
                return Content("{\"result\":\"error\",\"msg\":\"请传递正确的模块ID\"}");
            }
            catch (Exception ex)
            {
                return Content("{\"result\":\"error\",\"msg\":\"" + ex.Message + "\"}");
            }
        }

        # endregion

        /// <summary>
        /// 得到指定的模块信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetModuleInfo(long? id)
        {
            ModuleInfo moduleInfo;
            if (id != null)
            {
                ModuleService moduleService = new ModuleService();
                moduleInfo = moduleService.GetModuleInfo(id.Value);
            }
            else
            {
                moduleInfo = new ModuleInfo();
            }
            return Json(moduleInfo, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取栏目树节点数据
        /// </summary>
        /// <returns></returns>
        public ActionResult AjaxLoading()
        {
            ModuleService moduleService = new ModuleService();
            DynatreeModel channelDynatreeModelList = moduleService.GetModuleTreeList();
            return Json(channelDynatreeModelList, JsonRequestBehavior.AllowGet);
        }

        # region 得到有权限的模块列表

        /// <summary>
        /// 得到指定角色有权限的模块列表
        /// </summary>
        /// <param name="roleID">角色ID</param>
        /// <returns></returns>
        public ActionResult GetModuleListByRole(long? roleID)
        {
            List<long> listMenu = new List<long>();
            if (roleID != null)
            {
                ModuleService moduleService = new ModuleService();
                listMenu = moduleService.GetModuleListByRoleID(roleID.Value);
            }
            return Json(listMenu);
        }

        # endregion

        # region 保存角色 + 菜单

        /// <summary>
        /// 保存角色 + 菜单
        /// </summary>
        /// <param name="roleID">角色ID</param>
        /// <param name="moduleList"></param>
        /// <returns></returns>
        public ActionResult SaveModuleListByRole(int? roleID, string moduleList)
        {
            string msg;
            try
            {
                if (!string.IsNullOrEmpty(moduleList))
                {
                    string[] menuIDArray = moduleList.Split(',');
                    ModuleService menuService = new ModuleService();
                    menuService.SaveRoleModuleAuth(roleID ?? 0, new List<string>(menuIDArray), UserID);
                    msg = "{\"result\":\"ok\"}";
                }
                else
                {
                    msg = "{\"ID\":\"0\",\"msg\":\"请选择菜单列表\"}";
                }
            }
            catch (Exception ex)
            {
                msg = "{\"ID\":\"0\",\"msg\":\"" + ex.Message + "\"}";
            }
            return Content(msg);
        }

        # endregion
    }
}
