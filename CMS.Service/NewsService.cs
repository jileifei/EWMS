using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using CMS.Domain;
using CMS.CommonLib.Utils;
using CMS.CommonLib.Extension;
using CMS.DataAccess.SQLHelper;

namespace CMS.Service
{
    public class NewsService
    {
        # region post news

        /// <summary>
        /// 发表新闻
        /// </summary>
        /// <param name="newsEntity"></param>
        /// <returns></returns>
        public Int64 AddPost(NewsDoc newsEntity)
        {
            bool flag = false;
            using (SQLHelper dao = new SQLHelper())
            {
                if (string.IsNullOrEmpty(newsEntity.SubTitle))
                {
                    newsEntity.SubTitle = newsEntity.Title;
                }

                if (string.IsNullOrEmpty(newsEntity.Summary))
                {
                    newsEntity.Summary = TextHelper.SubStr(TextHelper.ClearHtml(newsEntity.Content), 100);
                }

                SqlParameter[] parameters = new SqlParameter[22];
                parameters[0] = new SqlParameter("@Title", newsEntity.Title);
                parameters[1] = new SqlParameter("@SubTitle", newsEntity.SubTitle);
                parameters[2] = new SqlParameter("@Tags", newsEntity.Tags);
                parameters[3] = new SqlParameter("@Summary", newsEntity.Summary);
                parameters[4] = new SqlParameter("@Author", newsEntity.Author);
                parameters[5] = new SqlParameter("@Source", newsEntity.Source);
                parameters[6] = new SqlParameter("@SmallImageUrl", newsEntity.SmallImageUrl);
                parameters[7] = new SqlParameter("@TitleColor", newsEntity.TitleColor);
                parameters[8] = new SqlParameter("@IsTop", newsEntity.IsTop);
                parameters[9] = new SqlParameter("@IsRecommend", newsEntity.IsRecommend);
                parameters[10] = new SqlParameter("@IsBold", newsEntity.IsBold);
                parameters[11] = new SqlParameter("@AddUser", newsEntity.CreateUserID);
                parameters[12] = new SqlParameter("@AddIP", newsEntity.CreateUserIP);
                parameters[13] = new SqlParameter("@NewsContent", newsEntity.Content);
                parameters[14] = new SqlParameter("@ChannelID", newsEntity.ChannelID);
                parameters[15] = new SqlParameter("@SpecialID", newsEntity.SpecialID);
                parameters[16] = new SqlParameter("@SpecialChannelID", newsEntity.SpecialChannelID);
                parameters[17] = new SqlParameter("@LinkUrl", newsEntity.Linkurl);
                parameters[18] = new SqlParameter("@PublicTime", newsEntity.PublicTime);
                parameters[19] = new SqlParameter("@IsAuditing", newsEntity.IsAuditing);
                parameters[20] = new SqlParameter("@SortID", newsEntity.SortID);
                parameters[21] = new SqlParameter("@o_id",SqlDbType.BigInt) {Direction = ParameterDirection.Output};
                dao.ExecuteNonQuery(CommandType.StoredProcedure, "UP_PostNews", parameters);
                Int64 count = Convert.ToInt64(parameters[21].Value);
                if (count > 0)
                {
                    return count;
                }
            }
            return 0;
        }

        # endregion

        # region Edit news

        /// <summary>
        /// 编辑新闻
        /// </summary>
        /// <param name="newsEntity"></param>
        /// <returns></returns>
        public bool EditPost(NewsDoc newsEntity)
        {
            int result;
            using (SQLHelper dao = new SQLHelper())
            {
                SqlParameter[] parameters = new SqlParameter[23];
                parameters[0] = new SqlParameter("@NewsID", newsEntity.ID);
                parameters[1] = new SqlParameter("@Title", newsEntity.Title);
                parameters[2] = new SqlParameter("@SubTitle", newsEntity.SubTitle);
                parameters[3] = new SqlParameter("@Tags", newsEntity.Tags);
                parameters[4] = new SqlParameter("@Summary", newsEntity.Summary);
                parameters[5] = new SqlParameter("@Author", newsEntity.Author);
                parameters[6] = new SqlParameter("@Source", newsEntity.Source);
                parameters[7] = new SqlParameter("@SmallImageUrl", newsEntity.SmallImageUrl);
                parameters[8] = new SqlParameter("@TitleColor", newsEntity.TitleColor);
                parameters[9] = new SqlParameter("@IsTop", newsEntity.IsTop);
                parameters[10] = new SqlParameter("@IsRecommend", newsEntity.IsRecommend);
                parameters[11] = new SqlParameter("@IsBold", newsEntity.IsBold);
                parameters[12] = new SqlParameter("@EditUser", newsEntity.ModifyUserID);
                parameters[13] = new SqlParameter("@EditIP", newsEntity.ModifyUserIP);
                parameters[14] = new SqlParameter("@EditDate", newsEntity.ModifyTime);
                parameters[15] = new SqlParameter("@NewsContent", newsEntity.Content);
                parameters[16] = new SqlParameter("@ChannelID", newsEntity.ChannelID);
                parameters[17] = new SqlParameter("@SpecialID", newsEntity.SpecialID);
                parameters[18] = new SqlParameter("@SpecialChannelID", newsEntity.SpecialChannelID);
                parameters[19] = new SqlParameter("@LinkUrl", newsEntity.Linkurl);
                parameters[20] = new SqlParameter("@publicTime", newsEntity.PublicTime);
                parameters[21] = new SqlParameter("@IsAuditing", newsEntity.IsAuditing);
                parameters[22] = new SqlParameter("@SortID", newsEntity.SortID);

               result= dao.ExecuteNonQuery(CommandType.StoredProcedure, "UP_EditNews", parameters);

               
            }
            return result>0;
        }

        # endregion

        # region update delete flag

        /// <summary>
        ///逻辑删除
        /// </summary>
        /// <param name="NewsID"></param>
        /// <returns></returns>
        public bool UpdateDelFlag(long NewsID)
        {
            int result;
            using (SQLHelper dao = new SQLHelper())
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@NewsID", NewsID);
               result= dao.ExecuteNonQuery(CommandType.StoredProcedure, "UP_DeleteNews", parameters);
            }
            return   result>0;
        }

        public bool UpdateLinkUrl(string linkUrl, long newsId)
        {
            int result;
            string sql = "UPDATE NewsDoc SET LinkUrl=@linkUrl WHERE ID=@ID";
            SqlParameter[] parameters = new SqlParameter[2];
            parameters[0] = new SqlParameter("@linkUrl", linkUrl);
            parameters[1] = new SqlParameter("@ID", newsId);
            using (SQLHelper dao = new SQLHelper())
            {
                result = dao.ExecuteNonQuery(CommandType.Text, sql, parameters);
            }
            return result > 0;
        }

        # endregion

        # region restore delete flag

        /// <summary>
        ///恢复删除
        /// </summary>
        /// <param name="newsID"></param>
        /// <returns></returns>
        public bool RestoreDelFlag(long newsID)
        {
            int result;
            using (SQLHelper dao = new SQLHelper())
            {
                string sql = "UPDATE dbo.NewsDoc SET IsDelete=0,[status]=0 WHERE ID=@NewsID";
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@NewsID", newsID);
               result= dao.ExecuteNonQuery(CommandType.Text, sql, parameters);
            }
            return result>0;
        }

        # endregion

        # region news stat

        /// <summary>
        /// 新闻点击量统计
        /// </summary>
        /// <param name="newsID"></param>
        /// <returns></returns>
        public bool NewsStat(long newsID)
        {
            int result;
            using (SQLHelper dao = new SQLHelper())
            {
                string sql = "UPDATE dbo.NewsDoc SET clickCount=clickCount+1 WHERE ID=@NewsID";
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@NewsID", newsID);
               result= dao.ExecuteNonQuery(CommandType.Text, sql, parameters);
            }
            return result>0;
        }

        # endregion

        # region delete news

        /// <summary>
        ///物理删除
        /// </summary>
        /// <param name="newsID"></param>
        /// <returns></returns>
        public bool DelNews(long newsID)
        {
            int result;
            using (SQLHelper dao = new SQLHelper())
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@NewsID", newsID);
               result= dao.ExecuteNonQuery(CommandType.StoredProcedure, "UP_PhysicalDeleteNews", parameters);
            }
            return result>0;
        }

        # endregion

        # region auditnews

        ///  <summary>
        /// 新闻审核
        ///  </summary>
        ///  <param name="newsID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public bool AuditNews(long newsID,long userID)
        {
            int result;
            using (SQLHelper dao = new SQLHelper())
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@NewsID", newsID);
                parameters[0] = new SqlParameter("@UserID", userID);
                parameters[0] = new SqlParameter("@AuditTime", DateTime.Now);
               result= dao.ExecuteNonQuery(CommandType.StoredProcedure, "UP_AuditNews", parameters);
              
            }
            return result>0;
        }

        ///  <summary>
        /// 新闻审核
        ///  </summary>
        ///  <param name="newsID"></param>
        /// <param name="auditing"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public bool AuditNews(long newsID,int? auditing,long userID)
        {
            int result;
            using (SQLHelper dao = new SQLHelper())
            {
                string sql = "UPDATE NewsDoc SET IsAuditing=@IsAuditing,[status]=1,auditUserID=@UserID,auditTime=@AuditTime WHERE ID=@NewsID";
                SqlParameter[] parameters = new SqlParameter[4];
                parameters[0] = new SqlParameter("@NewsID", newsID);
                parameters[1] = new SqlParameter("@UserID", userID);
                parameters[2] = new SqlParameter("@AuditTime", DateTime.Now);
                parameters[3] = new SqlParameter("@IsAuditing", auditing == null ? 0 : auditing.Value);
               result= dao.ExecuteNonQuery(CommandType.Text, sql, parameters);
            }
            return result>0;
        }

        # endregion

        # region GetNewsInfo

        /// <summary>
        /// get news info by newsid
        /// </summary>
        /// <param name="newsID"></param>
        /// <returns></returns>
        public NewsDoc GetNewsInfoByID(long newsID)
        {
            NewsDoc newsEntity = null;
            using (SQLHelper dao = new SQLHelper())
            {
                ChannelService channelService = new ChannelService();
                IDictionary<long,string> dicChannel = channelService.GetDicChannelList();

                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@NewsID", newsID);
                SqlDataReader reader= dao.ExecuteReader(CommandType.StoredProcedure, "UP_GetNewsFullInfoByID", parameters);
                if (reader.Read())
                {
                    newsEntity = new NewsDoc();
                    newsEntity.ID = TypeParse.ToLong(reader["ID"]);
                    string color = reader["TitleColor"].ToString();
                    if (!string.IsNullOrEmpty(color))
                    {
                        newsEntity.Title = string.Format("<font color=\"{0}\">{1}</font>", color, reader["Title"]);
                    }
                    else
                    {
                        newsEntity.Title = reader["Title"].ToString();
                    }
                    newsEntity.SubTitle = reader["SubTitle"].ToString();
                    newsEntity.Tags = reader["Tags"].ToString();
                    newsEntity.Summary = reader["Summary"].ToString();
                    newsEntity.Source = reader["Source"].ToString();
                    newsEntity.Author = reader["Author"].ToString();
                    newsEntity.SmallImageUrl = reader["SmallImageUrl"].ToString();
                    newsEntity.TitleColor = reader["TitleColor"].ToString();
                    newsEntity.IsAuditing = TypeParse.ToInt16(reader["IsAuditing"],0);
                    newsEntity.IsTop = TypeParse.ToBool(reader["IsTop"]);
                    newsEntity.IsRecommend = TypeParse.ToBool(reader["IsRecommend"]);
                    newsEntity.IsBold = TypeParse.ToBool(reader["IsBold"]);
                    newsEntity.Linkurl = reader["LinkUrl"].ToString();
                    newsEntity.PublicTime = TypeParse.ToDateTime(reader["PublicTime"]);
                    long channelID = TypeParse.ToLong(reader["ChannelID"]);
                    newsEntity.ChannelID = channelID;
                    if (dicChannel.ContainsKey(channelID))
                    {
                        newsEntity.ChannelName = dicChannel[channelID];
                    }
                    newsEntity.SpecialID = TypeParse.ToLong(reader["SpecialID"]);
                    newsEntity.Content = reader["Content"].ToString();
                }
            }
            return newsEntity;
        }

        # endregion

        # region GetNewsList

        /// <summary>
        /// 按照栏目读取新闻列表
        /// </summary>
        /// <param name="isDelete">0=未删除 1=已删除</param>
        /// <param name="channelID">栏目ID</param>
        /// <param name="endTime"></param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <param name="beginTime"></param>
        /// <returns></returns>
        public PagerModel<NewsDoc> GetPagerNewsListByChannelID(int isDelete,long? channelID, long? id, string title, string beginTime, string endTime, int pageSize, int pageIndex)
        {
            PagerModel<NewsDoc> newsPagerEntity = new PagerModel<NewsDoc>();
            using (SQLHelper dao = new SQLHelper())
            {
                string where = "IsDelete=" + isDelete;
                if (channelID != null)
                {
                    // 查找所有子分类
                    ChannelService channelService = new ChannelService();
                    IList<long> listAllChildID = channelService.GetAllChildID(channelID.Value);
                    if (listAllChildID.Count > 0)
                    {
                        if (listAllChildID.Count == 1)
                        {
                            where += " AND ChannelID=" + listAllChildID[0];
                        }
                        else
                        {
                            where += " AND ChannelID IN (" + listAllChildID.CToString() +")";
                        }
                    }
                    else
                    {
                        where += " AND ChannelID=" + channelID.Value;
                    }
                }
                if (id != null)
                {
                    where += " AND ID=" + id.Value;
                }
                if (!string.IsNullOrEmpty(title))
                {
                    where += " AND Title LIKE '%" + Utils.CleanInput(title) + "%'";
                }
                if (!string.IsNullOrEmpty(beginTime) && !string.IsNullOrEmpty(endTime))
                {
                    where += " AND PublicTime BETWEEN '" + TypeParse.ToDayMinTime(TypeParse.ToDateTime(beginTime)) + "' AND '" + TypeParse.ToDayMaxTime(TypeParse.ToDateTime(endTime)) + "'";
                }
                else if (!string.IsNullOrEmpty(beginTime) && string.IsNullOrEmpty(endTime))
                {
                    where += " AND PublicTime > '" + TypeParse.ToDayMinTime(TypeParse.ToDateTime(beginTime)) + "'";
                }
                else if (string.IsNullOrEmpty(beginTime) && !string.IsNullOrEmpty(endTime))
                {
                    where += " AND PublicTime < '" + TypeParse.ToDayMaxTime(TypeParse.ToDateTime(endTime)) + "'";
                }
                int totalCount;
                int totalPager;
                DataSet dsNewsList = dao.PageingQuery("NewsDoc", "[ID],Title,Source,IsAuditing,ChannelID,Author,PublicTime,Linkurl,TitleColor", "ID", where, "", 2, pageSize, pageIndex, out totalCount, out totalPager);
                newsPagerEntity.PageSize = pageSize;
                newsPagerEntity.CurrentPage = pageIndex;
                newsPagerEntity.TotalRecords = totalCount;
                newsPagerEntity.ItemList = MakeNewsList(dsNewsList);
            }
            return newsPagerEntity;
        }

        /// <summary>
        /// Convert DataSet To IList<NewsModel/>
        /// </summary>
        /// <param name="dsNewsList"></param>
        /// <returns></returns>
        private IList<NewsDoc> MakeNewsList(DataSet dsNewsList)
        {
            IList<NewsDoc> newsList = new List<NewsDoc>(10);
            long channelID;
            ChannelService channelService = new ChannelService();
            IDictionary<long,string> dicChannel = channelService.GetDicChannelList();
            foreach (DataRow row in dsNewsList.Tables[0].Rows)
            {
                string color = row["TitleColor"].ToString();
                NewsDoc newsEntity = new NewsDoc
                {
                    ID = TypeParse.ToLong(row["ID"])
                };
                newsEntity.Title = !string.IsNullOrEmpty(color) ? string.Format("<font color=\"{0}\">{1}</font>",color,row["Title"]) : row["Title"].ToString();
                newsEntity.Source = row["Source"].ToString();
                newsEntity.Author = row["Author"].ToString();
                channelID = TypeParse.ToLong(row["ChannelID"]);
                if (dicChannel.ContainsKey(channelID))
                {
                    newsEntity.ChannelName = dicChannel[channelID];
                }
                if (TypeParse.ToInt16(row["IsAuditing"].ToString(),0) == 1)
                {
                    newsEntity.AuditingStats = "已审核";
                }
                else
                {
                    newsEntity.AuditingStats = "未审核";
                }
                newsEntity.Linkurl = row["Linkurl"].ToString();
                if (string.IsNullOrEmpty(newsEntity.Linkurl))
                {
                    newsEntity.Linkurl = string.Format("http://www.beijing-dentsu.com.cn/shownews/{0}/{1}", channelID, newsEntity.ID);
                }
                newsEntity.TitleColor = color;
                newsEntity.PublicTime = TypeParse.ToDateTime(row["PublicTime"]);
                newsList.Add(newsEntity);
            }
            return newsList;
        }

        # endregion
    }
}
