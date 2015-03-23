using System.Collections.Generic;
using System.Data;
using CMS.Domain;
using CMS.CommonLib.Utils;
using CMS.DataAccess;
using CMS.DataAccess.SQLHelper;
using CMS.DataAccess.Interface;

namespace CMS.Service
{
    /// <summary>
    /// 专题栏目
    /// </summary>
    public class SpecialChannelService
    {
        # region add update delete select

        /// <summary>
        /// add SpecialChannel
        /// </summary>
        /// <param name="sChannelInfo"></param>
        /// <returns></returns>
        public bool AddSpecialChannel(SpecialChannel sChannelInfo)
        {
            ISpecialChannelDao sChannelDao = CastleContext.Instance.GetService<ISpecialChannelDao>();
            sChannelDao.Insert(sChannelInfo);
            return true;
        }

        /// <summary>
        /// update SpecialChannel
        /// </summary>
        /// <param name="sChannelInfo"></param>
        /// <returns></returns>
        public bool UpdateSpecialChannel(SpecialChannel sChannelInfo)
        {
            ISpecialChannelDao sChannelDao = CastleContext.Instance.GetService<ISpecialChannelDao>();
            sChannelDao.Update(sChannelInfo);
            return true;
        }

        /// <summary>
        /// delete SpecialChannel
        /// </summary>
        /// <param name="sChannelInfo"></param>
        /// <returns></returns>
        public bool DeleteSpecialChannel(SpecialChannel sChannelInfo)
        {
            ISpecialChannelDao sChannelDao = CastleContext.Instance.GetService<ISpecialChannelDao>();
            sChannelDao.Delete(sChannelInfo);
            return true;
        }

        /// <summary>
        /// select by ID
        /// </summary>
        /// <param name="speicalChannelID"></param>
        /// <returns></returns>
        public SpecialChannel GetSpecialChannel(long speicalChannelID)
        {
            TemplateService tpService = new TemplateService();
            IDictionary<long, string> dicTemplate = tpService.GetDicTemplateList();
            ISpecialChannelDao sChannelDao = CastleContext.Instance.GetService<ISpecialChannelDao>();
            SpecialChannel sChannelEntity = sChannelDao.Find(speicalChannelID);
            if (sChannelEntity != null)
            {
                if (dicTemplate.ContainsKey(sChannelEntity.TemplateID))
                {
                    sChannelEntity.TemplateName = dicTemplate[sChannelEntity.TemplateID];
                }
            }
            return sChannelEntity;
        }

        # endregion

        # region GetSpecialChannelList

        /// <summary>
        /// get SpecialChannel list
        /// </summary>
        /// <param name="speicalID"></param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="pageIndex">当前页码</param>
        /// <returns></returns>
        public PagerModel<SpecialChannel> GetPagerSpecialChannelList(long speicalID,int pageSize, int pageIndex)
        {
            PagerModel<SpecialChannel> specialChannelEntity = new PagerModel<SpecialChannel>();
            using (SQLHelper dao = new SQLHelper())
            {
                string where = "SpecialID=" + speicalID;

                int totalCount;
                int totalPager;
                DataSet dsSpecialChannelList = dao.PageingQuery("SpecialChannel", "ID,SpecialID,subSpecialName,Url,TemplateID,createTiem", "ID", where, "", 2, pageSize, pageIndex, out totalCount, out totalPager);
                specialChannelEntity.PageSize = pageSize;
                specialChannelEntity.CurrentPage = pageIndex;
                specialChannelEntity.TotalRecords = totalCount;
                specialChannelEntity.ItemList = MakeSpecialChannelList(dsSpecialChannelList);
            }
            return specialChannelEntity;
        }

        /// <summary>
        /// Convert DataSet To IList<NewsModel/>
        /// </summary>
        /// <param name="dsSpecialChannelList"></param>
        /// <returns></returns>
        private IList<SpecialChannel> MakeSpecialChannelList(DataSet dsSpecialChannelList)
        {
            IList<SpecialChannel> sChannelList = new List<SpecialChannel>(10);

            foreach (DataRow row in dsSpecialChannelList.Tables[0].Rows)
            {
                SpecialChannel sChannelEntity = new SpecialChannel();
                sChannelEntity.ID = TypeParse.ToLong(row["ID"]);
                sChannelEntity.SubSpecialName = row["subSpecialName"].ToString();
                sChannelEntity.Url = row["Url"].ToString();
                sChannelEntity.TemplateID = TypeParse.ToLong(row["TemplateID"]);
                sChannelEntity.CreateTiem = TypeParse.ToDateTime(row["createTiem"]);
                sChannelList.Add(sChannelEntity);
            }
            return sChannelList;
        }

        # endregion
    }
}
