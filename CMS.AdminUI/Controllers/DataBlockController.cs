using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Web.Mvc;
using CMS.CommonLib.Utils;
using CMS.Template;
using Webdiyer.WebControls.Mvc;
using CMS.CommonLib.Extension;
using CMS.Domain;
using CMS.Service;

namespace CMS.AdminUI.Controllers
{
    /// <summary>
    /// 数据块管理
    /// </summary>
    public class DataBlockController : BaseController
    {
        private string IncludePath = ConfigurationManager.AppSettings["IncludePath"];
        private const int PAGESIZE = 20;

        public ActionResult Index(int pageIndex = 1)
        {
            DataBlockService dbService = new DataBlockService();
            PagerModel<DataBlock> pageModel = dbService.GetPagerDataBlockList(PAGESIZE, pageIndex);
            PagedList<DataBlock> pagedDataBlock = new PagedList<DataBlock>(pageModel.ItemList, pageIndex, PAGESIZE, pageModel.TotalRecords);
            return View(pagedDataBlock);
        }

        public ActionResult SelDataBlock(int pageIndex = 1)
        {
            DataBlockService dbService = new DataBlockService();
            PagerModel<DataBlock> pageModel = dbService.GetPagerDataBlockList(PAGESIZE, pageIndex);
            PagedList<DataBlock> pagedDataBlock = new PagedList<DataBlock>(pageModel.ItemList, pageIndex, PAGESIZE, pageModel.TotalRecords);
            return View(pagedDataBlock);
        }

        public ActionResult Add()
        {
            return View();
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="dataBlockID"></param>
        /// <returns></returns>
        public ActionResult Edit(long? dataBlockID)
        {
            DataBlock datablockEntity;
            if (dataBlockID != null)
            {
                ViewData["DataBlockID"] = dataBlockID.Value;
                DataBlockService dataBlockService = new DataBlockService();
                datablockEntity = dataBlockService.GeDataBlockInfo(dataBlockID.Value);
                if (datablockEntity == null)
                {
                    datablockEntity = new DataBlock();
                }
            }
            else
            {
                ViewData["DataBlockID"] = 0;
                datablockEntity = new DataBlock();
            }
            return View(datablockEntity);
        }

        /// <summary>
        /// 根据数据类型得到字段列表
        /// </summary>
        /// <param name="dataType"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetFieldList(int dataType)
        {
            IList<FieldEntity> fieldList = new List<FieldEntity>();
            string msg = "";
            if (Request.IsAjaxRequest())
            {
                try
                {
                    FieldService fieldService = new FieldService();
                    fieldList = fieldService.GetFieldList(dataType);
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
            return Json(new { list = fieldList, msg = msg });
        }

        # region 增 删 改

        [HttpPost, ValidateInput(false)]
        public ActionResult Create(DataBlock dataBlockEntity)
        {
            string msg;
            try
            {
                if (ModelState.IsValid)
                {
                    DataBlockService dataBlockService = new DataBlockService();
                    Int64 flag = dataBlockService.AddDataBlock(dataBlockEntity);

                    if (flag > 0)
                    {
                        if (dataBlockEntity.Type == 2)
                        {
                            if (dataBlockEntity.TemplateID > 0)
                            {
                                RecommedPositionService positionService = new RecommedPositionService();
                                RecommedPosition postion = positionService.GetPositionInfo(dataBlockEntity.TemplateID);
                                if (postion.IsInclude)
                                {
                                    String dealContent = TemplateHandler.DealTempate(flag, dataBlockEntity.TemplateID);
                                    FileHandler.Write(dealContent, IncludePath + "/Positions/" + postion.Name + ".shtml",
                                        Utils.GetEncodingByEncode("UTF-8"));
                                }
                            }
                        }
                        return Json(new { result = "ok" });
                    }
                    return Json(new { result = "error", msg = "添加失败，请联系管理员" });
                }
                return Json(new { result = "error", msg = "添加失败，" + this.ExpendErrors() });
            }
            catch (Exception ex)
            {
                return Json(new { result = "error", msg = ex.Message});
            }
            return Json(msg);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="dataBlockEntity"></param>
        /// <returns></returns>
        [HttpPost, ValidateInput(false)]
        public ActionResult Update(DataBlock dataBlockEntity)
        {
            try
            {
                DataBlockService dataBlockService = new DataBlockService();
                bool flag = dataBlockService.UpdateDataBlock(dataBlockEntity);
                if (flag)
                {
                    if (dataBlockEntity.Type == 2)
                    {
                        if (dataBlockEntity.TemplateID > 0)
                        {
                            RecommedPositionService positionService = new RecommedPositionService();
                            RecommedPosition postion = positionService.GetPositionInfo(dataBlockEntity.TemplateID);
                            if (postion.IsInclude)
                            {
                                String dealContent = TemplateHandler.DealTempate(dataBlockEntity.ID,
                                    dataBlockEntity.TemplateID);
                                FileHandler.Write(dealContent, IncludePath + "/Positions/" + postion.Name + ".shtml",
                                    Utils.GetEncodingByEncode("UTF-8"));
                            }
                            return Json(new { result = "ok" });
                        }
                    }

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
        /// <param name="dataBlockEntity"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(DataBlock dataBlockEntity)
        {
            try
            {
                DataBlockService dataBlockService = new DataBlockService();
                bool flag = dataBlockService.DeleteDataBlock(dataBlockEntity);
                if (flag)
                {
                    return Json(new { result = "ok" });
                }
                else
                {
                    return Json(new { result = "error", msg = "系统出现异常，请联系管理员" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { result = "error", msg = ex.Message });
            }
        }

        # endregion
    }
}
