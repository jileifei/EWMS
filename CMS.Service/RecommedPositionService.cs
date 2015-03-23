using System.Collections.Generic;
using System.Data;
using CMS.Domain;
using CMS.CommonLib.Utils;
using CMS.DataAccess;
using CMS.DataAccess.SQLHelper;
using CMS.DataAccess.Interface;

namespace CMS.Service
{
    public class RecommedPositionService
    {
        # region add update delete select

        /// <summary>
        /// add position
        /// </summary>
        /// <param name="positionInfo"></param>
        /// <returns></returns>
        public bool AddPosition(RecommedPosition positionInfo)
        {
            IRecommedPositionDao positionDao = CastleContext.Instance.GetService<IRecommedPositionDao>();
            positionDao.Insert(positionInfo);
            return true;
        }

        /// <summary>
        /// update position
        /// </summary>
        /// <param name="positionInfo"></param>
        /// <returns></returns>
        public bool UpdatePosition(RecommedPosition positionInfo)
        {
            IRecommedPositionDao positionDao = CastleContext.Instance.GetService<IRecommedPositionDao>();
            positionDao.Update(positionInfo);
            return true;
        }

        /// <summary>
        /// delete position
        /// </summary>
        /// <param name="positionInfo"></param>
        /// <returns></returns>
        public bool DeletePosition(RecommedPosition positionInfo)
        {
            IRecommedPositionDao positionDao = CastleContext.Instance.GetService<IRecommedPositionDao>();
            positionDao.Delete(positionInfo);
            return true;
        }

        /// <summary>
        /// select by ID
        /// </summary>
        /// <param name="positionID"></param>
        /// <returns></returns>
        public RecommedPosition GetPositionInfo(long positionID)
        {
            IRecommedPositionDao positionDao = CastleContext.Instance.GetService<IRecommedPositionDao>();
            return positionDao.Find(positionID);
        }

        # endregion

        # region GetPositionList

        /// <summary>
        /// GetPositionList
        /// </summary>
        /// <returns></returns>
        public IList<RecommedPosition> GetPostionList()
        {
            IRecommedPositionDao positionDao = CastleContext.Instance.GetService<IRecommedPositionDao>();
            return positionDao.FindAll();
        }

        /// <summary>
        /// get position list
        /// </summary>
        /// <param name="channelID">栏目ID</param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="pageIndex">当前页码</param>
        /// <returns></returns>
        public PagerModel<RecommedPosition> GetPagerPositionList(long? channelID, int pageSize, int pageIndex)
        {
            PagerModel<RecommedPosition> positionPagerEntity = new PagerModel<RecommedPosition>();
            using (SQLHelper dao = new SQLHelper())
            {
                string where = "";
                if (channelID != null)
                {
                    where += "ChannelID=" + channelID.Value;
                }
               
                int totalCount;
                int totalPager;
                DataSet dsPositionList = dao.PageingQuery("RecommedPosition", "[ID],Name,ChannelID,LocationType,CreateTime", "ID", where, "", 2, pageSize, pageIndex, out totalCount, out totalPager);
                positionPagerEntity.PageSize = pageSize;
                positionPagerEntity.CurrentPage = pageIndex;
                positionPagerEntity.TotalRecords = totalCount;
                positionPagerEntity.ItemList = MakePositionList(dsPositionList);
            }
            return positionPagerEntity;
        }

        /// <summary>
        /// Convert DataSet To IList<NewsModel/>
        /// </summary>
        /// <param name="dsPositionList"></param>
        /// <returns></returns>
        private IList<RecommedPosition> MakePositionList(DataSet dsPositionList)
        {
            IList<RecommedPosition> positionList = new List<RecommedPosition>(10);
            long channelID;
            int locationType;
            ChannelService channelService = new ChannelService();
            IDictionary<long, string> dicChannel = channelService.GetDicChannelList();
            foreach (DataRow row in dsPositionList.Tables[0].Rows)
            {
                RecommedPosition positionEntity = new RecommedPosition();
                positionEntity.ID = TypeParse.ToLong(row["ID"]);
                positionEntity.Name = row["Name"].ToString();
                locationType = TypeParse.ToInt(row["LocationType"]);
                if (locationType == 1)
                {
                    positionEntity.LocationTypeName = "文字推荐";
                }
                else
                {
                    positionEntity.LocationTypeName = "图片推荐";
                }
                channelID = TypeParse.ToLong(row["ChannelID"]);
                if (dicChannel.ContainsKey(channelID))
                {
                    positionEntity.ChannelName = dicChannel[channelID];
                }
                positionEntity.CreateTime = TypeParse.ToDateTime(row["CreateTime"]);
                positionList.Add(positionEntity);
            }
            return positionList;
        }

        # endregion
    }
}
