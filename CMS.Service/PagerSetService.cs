using System;
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
    /// 分页信息设置
    /// </summary>
    public class PagerSetService
    {
        # region add update delete select

        /// <summary>
        /// add pager set info
        /// </summary>
        /// <param name="pagerInfo"></param>
        /// <returns></returns>
        public bool AddPagerSet(PagerInfo pagerInfo)
        {
            IPagerInfoDao pagerSetDao = CastleContext.Instance.GetService<IPagerInfoDao>();
            pagerSetDao.Insert(pagerInfo);
            return true;
        }

        /// <summary>
        /// update pager set
        /// </summary>
        /// <param name="pagerInfo"></param>
        /// <returns></returns>
        public bool UpdatePagerSet(PagerInfo pagerInfo)
        {
            IPagerInfoDao pagerSetDao = CastleContext.Instance.GetService<IPagerInfoDao>();
            pagerSetDao.Update(pagerInfo);
            return true;
        }

        /// <summary>
        /// delete pager set
        /// </summary>
        /// <param name="pagerInfo"></param>
        /// <returns></returns>
        public bool DeletePagerSet(PagerInfo pagerInfo)
        {
            IPagerInfoDao pagerSetDao = CastleContext.Instance.GetService<IPagerInfoDao>();
            pagerSetDao.Delete(pagerInfo);
            return true;
        }

        /// <summary>
        /// select by ID
        /// </summary>
        /// <param name="pageID"></param>
        /// <returns></returns>
        public PagerInfo GePagerSetInfo(int pageID)
        {
            IPagerInfoDao pagerSetDao = CastleContext.Instance.GetService<IPagerInfoDao>();
            PagerInfo pagerInfo = pagerSetDao.Find(pageID);
            return pagerInfo;
        }

        # endregion

        # region GetPagerSetList

        /// <summary>
        /// get pagerset list
        /// </summary>
        /// <param name="pageSize">每页条数</param>
        /// <param name="pageIndex">当前页码</param>
        /// <returns></returns>
        public PagerModel<PagerInfo> GetPagerSetList(int pageSize, int pageIndex)
        {
            PagerModel<PagerInfo> pagerSetEntity = new PagerModel<PagerInfo>();
            using (SQLHelper dao = new SQLHelper())
            {
                string where = "  ";

                int totalCount;
                int totalPager;
                DataSet dsPagerSetList = dao.PageingQuery("PagerInfo", "[PagerName],[PageID],[DataType],[Where],[OrderBy],[PageSize],[AddDate]", "PageID", where, "", 2, pageSize, pageIndex, out totalCount, out totalPager);
                pagerSetEntity.PageSize = pageSize;
                pagerSetEntity.CurrentPage = pageIndex;
                pagerSetEntity.TotalRecords = totalCount;
                pagerSetEntity.ItemList = MakePagerSetList(dsPagerSetList);
            }
            return pagerSetEntity;
        }

        /// <summary>
        /// Convert DataSet To IList<NewsModel/>
        /// </summary>
        /// <param name="dsPagerSetList"></param>
        /// <returns></returns>
        private IList<PagerInfo> MakePagerSetList(DataSet dsPagerSetList)
        {
            IList<PagerInfo> pagerSetList = new List<PagerInfo>(10);

            foreach (DataRow row in dsPagerSetList.Tables[0].Rows)
            {
                PagerInfo pagerSetEntity = new PagerInfo();
                pagerSetEntity.PageID = TypeParse.ToInt(row["PageID"]);
                pagerSetEntity.DataType = TypeParse.ToInt(row["DataType"]);
                pagerSetEntity.PagerName = Convert.ToString(row["PagerName"]);
                pagerSetEntity.Where = row["Where"].ToString();
                pagerSetEntity.OrderBy = row["OrderBy"].ToString();
                pagerSetEntity.PageSize = TypeParse.ToInt(row["PageSize"]);
                pagerSetEntity.AddDate = TypeParse.ToDateTime(row["AddDate"]);
                pagerSetList.Add(pagerSetEntity);
            }
            return pagerSetList;
        }

        # endregion
    }
}
