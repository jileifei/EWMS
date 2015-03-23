using System.Collections.Generic;
using System.Data;
using System.Globalization;
using CMS.Domain;
using CMS.CommonLib.Utils;
using CMS.DataAccess;
using CMS.DataAccess.SQLHelper;
using CMS.DataAccess.Interface;

namespace CMS.Service
{
    /// <summary>
    /// 专题
    /// </summary>
    public class SpecialService
    {
        # region 增 删 改 查

        /// <summary>
        /// 添加专题信息
        /// </summary>
        /// <param name="specialEntity"></param>
        /// <returns></returns>
        public bool AddSpecial(SpecialInfo specialEntity)
        {
            ISpecialInfoDao specialDao = CastleContext.Instance.GetService<ISpecialInfoDao>();
            specialDao.Insert(specialEntity);
            return true;
        }

        /// <summary>
        /// 更新专题信息
        /// </summary>
        /// <param name="specialEntity"></param>
        /// <returns></returns>
        public bool EditSpecial(SpecialInfo specialEntity)
        {
            ISpecialInfoDao specialDao = CastleContext.Instance.GetService<ISpecialInfoDao>();
            specialDao.Update(specialEntity);
            return true;
        }

        /// <summary>
        /// 删除专题信息
        /// </summary>
        /// <param name="specialEntity"></param>
        /// <returns></returns>
        public bool DelSpecial(SpecialInfo specialEntity)
        {
            ISpecialInfoDao specialDao = CastleContext.Instance.GetService<ISpecialInfoDao>();
            specialDao.Delete(specialEntity);
            return true;
        }

        /// <summary>
        /// 得到指定的专题信息
        /// </summary>
        /// <param name="specialID"></param>
        /// <returns></returns>
        public SpecialInfo GetSpecialInfoByID(long specialID)
        {
            TemplateService tpService = new TemplateService();
            IDictionary<long, string> dicTemplate = tpService.GetDicTemplateList();
            ISpecialInfoDao specialDao = CastleContext.Instance.GetService<ISpecialInfoDao>();
            SpecialInfo spEntity = specialDao.Find(specialID);
            if (spEntity != null)
            {
                if (dicTemplate.ContainsKey(spEntity.TemplateID))
                {
                    spEntity.TemplateName = dicTemplate[spEntity.TemplateID];
                }
            }
            return spEntity;
        }

        # endregion

        # region GetSpecialList

        /// <summary>
        /// 获取专题列表 分页
        /// </summary>
        /// <param name="pageSize">每页条数</param>
        /// <param name="pageIndex">当前页码</param>
        /// <returns></returns>
        public PagerModel<SpecialInfo> GetPagerSpecialList(int pageSize, int pageIndex)
        {
            PagerModel<SpecialInfo> specialPagerEntity = new PagerModel<SpecialInfo>();
            using (SQLHelper dao = new SQLHelper())
            {
                string where = "";
                int totalCount;
                int totalPager;
                DataSet dsSpecialList = dao.PageingQuery("SpecialInfo", "[ID],[Name],[EnName],PicUrl,SpecialUrlPart,[SmallPicUrl],[Keyword],[CreateTime]", "ID", where, "", 2, pageSize, pageIndex, out totalCount, out totalPager);
                specialPagerEntity.PageSize = pageSize;
                specialPagerEntity.CurrentPage = pageIndex;
                specialPagerEntity.TotalRecords = totalCount;
                specialPagerEntity.ItemList = MakeSpecialList(dsSpecialList);
            }
            return specialPagerEntity;
        }

        /// <summary>
        /// Convert DataSet To IList<NewsModel/>
        /// </summary>
        /// <param name="dsSpecialList"></param>
        /// <returns></returns>
        private IList<SpecialInfo> MakeSpecialList(DataSet dsSpecialList)
        {
            IList<SpecialInfo> specialList = new List<SpecialInfo>(10);
            foreach (DataRow row in dsSpecialList.Tables[0].Rows)
            {
                SpecialInfo specialEntity = new SpecialInfo
                {
                    ID = TypeParse.ToInt(row["ID"]),
                    Name = row["Name"].ToString(),
                    EnName = row["EnName"].ToString(),
                    SmallPicUrl = row["SmallPicUrl"].ToString(),
                    PicUrl = row["PicUrl"].ToString(),
                    Keyword = row["Keyword"].ToString(),
                    SpecialUrlPart = row["SpecialUrlPart"].ToString(),
                    CreateTime = TypeParse.ToDateTime(row["CreateTime"])
                };
                specialList.Add(specialEntity);
            }
            return specialList;
        }

        # endregion

        # region tree

        /// <summary>
        /// 获取专题树
        /// </summary>
        /// <returns></returns>
        public DynatreeModel GetSpecialTreeList()
        {
            DynatreeModel dynatreeRootNode = new DynatreeModel {key = "0", title = "专题列表", expand = true};

            ISpecialInfoDao specialDao = CastleContext.Instance.GetService<ISpecialInfoDao>();
            IList<SpecialInfo> rootSpecialList = specialDao.FindAll();
            List<DynatreeModel> rootList = MakeDynatreeModelList(rootSpecialList);
            foreach (DynatreeModel childeItem in rootList)
            {
                dynatreeRootNode.children.Add(childeItem);
                GetSpecialChannelTreeNode(childeItem);
            }
            if (rootList.Count > 0)
            {
                dynatreeRootNode.isFolder = true;
            }
            return dynatreeRootNode;
        }

        /// <summary>
        /// 获取子节点
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private void GetSpecialChannelTreeNode(DynatreeModel node)
        {
            ISpecialChannelDao channelDao = CastleContext.Instance.GetService<ISpecialChannelDao>();
            IList<SpecialChannel> childChannelList = channelDao.FindBySpecialID(TypeParse.ToLong(node.key));
            if (childChannelList.Count > 0)
            {
                node.isFolder = true;// 有子节点
            }
            foreach (SpecialChannel itemEntity in childChannelList)
            {
                DynatreeModel dynatreeModel = new DynatreeModel
                {
                    expand = true,
                    title = itemEntity.SubSpecialName,
                    key = itemEntity.ID.ToString(CultureInfo.InvariantCulture),
                    ParentID = 0
                };
                node.children.Add(dynatreeModel);
            }
        }

        /// <summary>
        /// convert specialinfo entity list to dynatreenode entity list
        /// </summary>
        /// <param name="specialList">specialinfo entity list</param>
        /// <returns></returns>
        private List<DynatreeModel> MakeDynatreeModelList(IList<SpecialInfo> specialList)
        {
            List<DynatreeModel> listDynatreeNode = new List<DynatreeModel>(1);
            foreach (SpecialInfo itemEntity in specialList)
            {
                DynatreeModel dynatreeModel = new DynatreeModel
                {
                    expand = true,
                    title = itemEntity.Name,
                    key = itemEntity.ID.ToString(CultureInfo.InvariantCulture),
                    ParentID = 0
                };
                listDynatreeNode.Add(dynatreeModel);
            }
            return listDynatreeNode;
        }

        # endregion
    }
}
