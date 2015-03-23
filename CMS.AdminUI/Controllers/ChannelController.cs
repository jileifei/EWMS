using System;
using System.Collections.Generic;
using System.Web.Mvc;

using CMS.Domain;
using CMS.Service;
using CMS.CommonLib.Extension;

namespace CMS.AdminUI.Controllers
{
    public class ChannelController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        # region 增 删 改 排序

        /// <summary>
        /// 添加栏目
        /// </summary>
        /// <param name="channelInfo"></param>
        /// <returns></returns>
        public ActionResult Add(ChannelInfo channelInfo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ChannelService channelService = new ChannelService();
                    channelInfo.CreateUserID = UserID;
                    channelInfo.ModifyUserID = UserID;
                    long channelID = channelService.Add(channelInfo);
                    return Content("{\"ID\":\"" + channelID + "\"}");
                }
                return Content("{\"ID\":\"0\",\"msg\":\"" + this.ExpendErrors() + "\"}");
            }
            catch (Exception ex)
            {
                return Content("{\"ID\":\"0\",\"msg\":\"" + ex.Message + "\"}");
            }
        }

        /// <summary>
        /// 修改栏目
        /// </summary>
        /// <param name="channelInfo"></param>
        /// <returns></returns>
        public ActionResult Update(ChannelInfo channelInfo)
        {
            try
            {
                ChannelService channelService = new ChannelService();
                channelInfo.ModifyUserID = UserID;
                channelInfo.ModifyTime = DateTime.Now;
                bool flag = channelService.Update(channelInfo);
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
        /// 删除栏目
        /// </summary>
        /// <param name="channelInfo"></param>
        /// <returns></returns>
        public ActionResult Delete(ChannelInfo channelInfo)
        {
            try
            {
                ChannelService channelService = new ChannelService();
                bool flag = channelService.Delete(channelInfo);
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
                    ChannelService channelService = new ChannelService();
                    bool flag = channelService.Sort(sourceID.Value, id.Value);
                    if (flag)
                    {
                        return Content("{\"result\":\"ok\"}");
                    }
                    return Content("{\"result\":\"error\",\"msg\":\"服务器端出现异常，请稍后再试\"}");
                }
                return Content("{\"result\":\"error\",\"msg\":\"请传递正确的栏目ID\"}");
            }
            catch (Exception ex)
            {
                return Content("{\"result\":\"error\",\"msg\":\"" + ex.Message + "\"}");
            }
        }

        # endregion

        /// <summary>
        /// 得到指定的栏目信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetChannelInfo(int? id)
        {
            ChannelInfo channelInfo;
            if (id != null)
            {
                ChannelService channelService = new ChannelService();
                channelInfo = channelService.GetChannelInfo(id.Value);
            }
            else
            {
                channelInfo = new ChannelInfo();
            }
            return Json(channelInfo, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取栏目树节点数据
        /// </summary>
        /// <returns></returns>
        public ActionResult AjaxLoading()
        {
            ChannelService channelService = new ChannelService();
            DynatreeModel channelDynatreeModelList = channelService.GetChannelTreeList();
            return Json(channelDynatreeModelList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取栏目树节点数据
        /// </summary>
        /// <returns></returns>
        public ActionResult AuthAjaxLoading(int? roleID)
        {
            ChannelService channelService = new ChannelService();
            List<DynatreeModel> channelAuthDynatreeModelList = channelService.GetAuthChannelTreeList(roleID??0);
            return Json(channelAuthDynatreeModelList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取栏目树节点数据
        /// </summary>
        /// <returns></returns>
        public ActionResult BrowseAuthAjaxLoading()
        {
            ChannelService channelService = new ChannelService();
            List<ComboTreeNode> channelAuthDynatreeModelList = channelService.GetBrowseChannelComboTreeList(RoleID);
            return Json(channelAuthDynatreeModelList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取Combotree节点数据
        /// </summary>
        /// <returns></returns>
        public ActionResult CombotreeAjaxLoading()
        {
            ChannelService channelService = new ChannelService();
            List<ComboTreeNode> channelCombotreeModelList = channelService.GetChannelComboTreeList();
            return Json(channelCombotreeModelList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取有添加权限的Combotree节点数据
        /// </summary>
        /// <returns></returns>
        public ActionResult AddAuthCombotreeAjaxLoading()
        {
            ChannelService channelService = new ChannelService();
            List<ComboTreeNode> channelCombotreeModelList = channelService.GetChannelComboTreeList(RoleID);
            return Json(channelCombotreeModelList, JsonRequestBehavior.AllowGet);
        }

        # region 保存角色 + 栏目

        /// <summary>
        /// 保存角色 + 栏目
        /// </summary>
        /// <param name="listMap"></param>
        /// <returns></returns>
        public JsonResult SaveChannelRoleMap(List<ChannelPrivilege> listMap)
        {
            string msg;
            try
            {
                ChannelService channelService = new ChannelService();
                channelService.SaveRoleChannelAuth(listMap,UserID);
                msg = "{\"result\":\"ok\"}";
            }
            catch (Exception ex)
            {
                msg = "{\"ChannelID\":\"0\",\"msg\":\"" + ex.Message + "\"}";
            }
            return Json(msg);
        }

        # endregion
    }
}
