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
    public class RecommendService
    {
        # region add update delete select

        /// <summary>
        /// add recommend
        /// </summary>
        /// <param name="recommendInfo"></param>
        /// <returns></returns>
        public bool AddRecommend(RecommedInfoList recommendInfo)
        {
            IRecommedInfoListDao recommendDao = CastleContext.Instance.GetService<IRecommedInfoListDao>();
            recommendDao.Insert(recommendInfo);
            return true;
        }

        /// <summary>
        /// update recommend
        /// </summary>
        /// <param name="recommendInfo"></param>
        /// <returns></returns>
        public bool UpdatePosition(RecommedInfoList recommendInfo)
        {
            IRecommedInfoListDao recommendDao = CastleContext.Instance.GetService<IRecommedInfoListDao>();
            recommendDao.Update(recommendInfo);
            return true;
        }

        /// <summary>
        /// delete recommend
        /// </summary>
        /// <param name="recommendInfo"></param>
        /// <returns></returns>
        public bool DeletePosition(RecommedInfoList recommendInfo)
        {
            IRecommedInfoListDao recommendDao = CastleContext.Instance.GetService<IRecommedInfoListDao>();
            recommendDao.Delete(recommendInfo);
            return true;
        }

        /// <summary>
        /// select by ID
        /// </summary>
        /// <param name="recommendID"></param>
        /// <returns></returns>
        public RecommedInfoList GetRecommendInfo(long recommendID)
        {
            IRecommedInfoListDao recommendDao = CastleContext.Instance.GetService<IRecommedInfoListDao>();
            return recommendDao.Find(recommendID);
        }

        # endregion

        # region GetRecommendList

        /// <summary>
        /// get recommend list
        /// </summary>
        /// <param name="positionID">位置ID</param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="pageIndex">当前页码</param>
        /// <returns></returns>
        public PagerModel<RecommedInfoList> GetPagerRecommendList(long positionID, int pageSize, int pageIndex)
        {
            PagerModel<RecommedInfoList> recommendPagerEntity = new PagerModel<RecommedInfoList>();
            using (SQLHelper dao = new SQLHelper())
            {
                string where = "LocationID=" + positionID + " AND status=1 ";

                int totalCount;
                int totalPager;
                DataSet dsRecommendList = dao.PageingQuery("RecommedInfoList", "[ID],DocID,Title,SmallPicUrl,LinkUrl,SortID,CreateTime", "ID", where, "", 2, pageSize, pageIndex, out totalCount, out totalPager);
                recommendPagerEntity.PageSize = pageSize;
                recommendPagerEntity.CurrentPage = pageIndex;
                recommendPagerEntity.TotalRecords = totalCount;
                recommendPagerEntity.ItemList = MakeRecommendList(dsRecommendList);
            }
            return recommendPagerEntity;
        }

        /// <summary>
        /// Convert DataSet To IList<NewsModel/>
        /// </summary>
        /// <param name="dsPositionList"></param>
        /// <returns></returns>
        private IList<RecommedInfoList> MakeRecommendList(DataSet dsPositionList)
        {
            IList<RecommedInfoList> recommendList = new List<RecommedInfoList>(10);
            
            foreach (DataRow row in dsPositionList.Tables[0].Rows)
            {
                RecommedInfoList recommendEntity = new RecommedInfoList();
                recommendEntity.ID = TypeParse.ToLong(row["ID"]);
                recommendEntity.DocID = TypeParse.ToLong(row["DocID"]);
                recommendEntity.Title = row["Title"].ToString();
                recommendEntity.SmallPicUrl = row["SmallPicUrl"].ToString();
                recommendEntity.LinkUrl = row["LinkUrl"].ToString();
                recommendEntity.SortID = TypeParse.ToInt(row["SortID"]);
                recommendEntity.CreateTime = TypeParse.ToDateTime(row["CreateTime"]);
                recommendList.Add(recommendEntity);
            }
            return recommendList;
        }

        # endregion
    }
}
