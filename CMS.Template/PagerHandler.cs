using System;
using System.Collections.Generic;
using System.Data;
using CMS.Domain;
using CMS.Service;
using CMS.CommonLib.Utils;
using CMS.DataAccess.SQLHelper;

namespace CMS.Template
{
    /// <summary>
    /// 分页信息处理
    /// </summary>
    public class PagerHandler
    {
        // 分页sql 排序、where、between 1=2
        /*  由于sql server 2000不支持 row_number() 函数，因此修改为使用存储过程调用 xwarrior@
        private const string Sql_GetDocumentList = @"select * from (
                                          SELECT row_number() OVER (ORDER BY {0}) as rownum,
                                          [ID]  ,[ChannelID]  ,[SpecialID]  ,[title]  ,[subTitle],[TitleColor]
      ,[author] ,[publicTime]  ,[Summary]  ,[IsTop]  ,[IsRecommend] ,[IsBold]
      ,[Tags] ,[Linkurl],[smallImageUrl],[source] ,[clickCount] ,[status] ,[createUserID]
      ,[createUserIP] ,[createTime]  ,[auditUserID] ,[auditTime] ,[modifyUserID] ,[ModifyTime]  ,[ModifyUserIP]  ,[IsAuditing] ,[IsDelete]
                                          FROM  NewsDoc WHERE {1}
                                          ) temp
                                          where rownum between {2} and {3}";
       */
        /// <summary>
        /// 取得指定页码的数据
        /// </summary>
        /// <param name="channelID"></param>
        /// <param name="pageNumList"></param>
        /// <returns></returns>
        public IList<TemplateDoc> GetPagerDataList(long channelID, ref string pageNumList, Int32 pageIndex)
        {
            int curPageIndex = 1;
            curPageIndex = pageIndex;
            //pageIndex = System.Web.HttpContext.Current.Request["page"];
            //if (!string.IsNullOrEmpty(pageIndex))
            //{
            //    curPageIndex = TypeParse.ToInt(pageIndex);
            //}

            IList<TemplateDoc> datalist;
            ChannelService channelService = new ChannelService();
            ChannelInfo ciEntity = channelService.GetChannelInfo(channelID);
            if (ciEntity == null)
            {
                throw new ArgumentException("错误的栏目ID", "channelID");
            }
            if (ciEntity.PagerID == null)
            {
                throw new ArgumentException("指定的栏目没有设置分页配置信息", "ChannelID,PagerID");
            }
            int pagerID = ciEntity.PagerID.Value;
            if (pagerID == 0)
            {
                throw new ArgumentException("指定的栏目没有设置分页配置信息", "ChannelID,PagerID");
            }
            PagerSetService psService = new PagerSetService();
            PagerInfo piEntity = psService.GePagerSetInfo(pagerID);
            if (piEntity == null)
            {
                throw new ArgumentException("指定的栏目设置了错误的分页配置信息，请重新设置", "ChannelID,PagerID");
            }
            string orderBy = "publicTime desc";
            if (!string.IsNullOrEmpty(piEntity.OrderBy))
            {
                orderBy = piEntity.OrderBy;
            }
            string where = " IsDelete=0 AND IsAuditing=1 AND ChannelID=" + channelID;
            if (!string.IsNullOrEmpty(piEntity.Where))
            {
                where += " AND " + piEntity.Where;
            }


            /*
            string sqlGet = string.Format(Sql_GetDocumentList, orderBy, where, piEntity.PageSize.Value * (curPageIndex - 1)+1, piEntity.PageSize.Value * curPageIndex);
            string sqlGetCount = string.Format(Sql_GetDocumentCount, where);
            */

            int totalCount;
            int totalPages;
            using (SQLHelper dao = new SQLHelper())
            {
                DataSet dspage = dao.SQLPagging(@"[ID]  ,[ChannelID]  ,[SpecialID]  ,[title]  ,[subTitle],[TitleColor]
              ,[author] ,[publicTime]  ,[Summary]  ,[IsTop]  ,[IsRecommend] ,[IsBold]
              ,[Tags] ,[Linkurl],[smallImageUrl],[source] ,[clickCount] ,[status] ,[createUserID]
              ,[createUserIP] ,[createTime]  ,[auditUserID] ,[auditTime] ,[modifyUserID] ,[ModifyTime]  ,[ModifyUserIP]  ,[IsAuditing] ,[IsDelete]",
               "NewsDoc",
               where,
               "IsTop desc," + orderBy, "publicTime", "desc",
               piEntity.PageSize ?? 10,
               curPageIndex,
               out totalCount, out totalPages);

                /*
                object objCount = dao.ExecuteScalar(CommandType.Text,sqlGetCount);
                if (objCount != null)
                {
                    totalCount = TypeParse.ToInt(objCount);
                }*/
                datalist = BaseTemplate.GetSqlResult(dspage);
            }


            string linkurl = "http://www.beijing-dentsu.com.cn/news/s{0}/{1}";
            foreach (TemplateDoc item in datalist)
            {
                if (string.IsNullOrEmpty(item.get("Linkurl")))
                {
                    item.SetValue("Linkurl", string.Format(linkurl, item.get("ChannelID"), item.get("ID")));
                }
            }
            for (int i = 0; i < datalist.Count; i++)
            {
                datalist[i].SetValue("startflag", "0");
                datalist[i].SetValue("endflag", "0");

                if (datalist.Count == 1)
                {
                    datalist[i].SetValue("startflag", "1");
                    datalist[i].SetValue("endflag", "1");
                }
                else if (i == datalist.Count - 1)
                {
                    datalist[i].SetValue("endflag", "1");
                    if (datalist[i - 1].dateformat("publictime", "yyyy-MM") != datalist[i].dateformat("publictime", "yyyy-MM"))
                    {
                        datalist[i].SetValue("startflag", "1");
                    }
                    else
                    {
                        datalist[i].SetValue("startflag", "0");
                    }
                }
                else
                {
                    //first
                    if (i == 0)
                    {
                        datalist[i].SetValue("startflag", "1");
                    }
                    else
                    {
                        //next not eq current
                        if (datalist[i + 1].dateformat("publictime", "yyyy-MM") != datalist[i].dateformat("publictime", "yyyy-MM"))
                        {
                            datalist[i].SetValue("endflag", "1");
                        }

                        //prev not eq current
                        if (datalist[i - 1].dateformat("publictime", "yyyy-MM") != datalist[i].dateformat("publictime", "yyyy-MM"))
                        {
                            datalist[i].SetValue("startflag", "1");
                            //prev ended
                            datalist[i - 1].SetValue("endflag", "1");
                        }
                    }

                }


            }
            # region 计算页数

            // 计算页数
            int pageNum;
            if (totalCount % (piEntity.PageSize ?? 0) > 0)
            {
                pageNum = totalCount / piEntity.PageSize ?? 0 + 1;
            }
            else
            {
                pageNum = totalCount / piEntity.PageSize ?? 0;
            }
            # endregion
            string url = System.Web.HttpContext.Current.Request.Url.ToString();
            pageNumList = Utils.GetPageNumbers(curPageIndex, pageNum, url, 10);
            return datalist;
        }

        /// <summary>
        /// 获取栏目下列表的总页数 
        /// </summary>
        /// <param name="channelID"></param>
        /// <returns></returns>
        public Int32 GetPagerTatals(long channelID)
        {
            ChannelService channelService = new ChannelService();
            ChannelInfo ciEntity = channelService.GetChannelInfo(channelID);
            if (ciEntity == null)
            {
                throw new ArgumentException("错误的栏目ID", "channelID");
            }
            if (ciEntity.PagerID == null)
            {
                throw new ArgumentException("指定的栏目没有设置分页配置信息", "ChannelID,PagerID");
            }
            int pagerID = ciEntity.PagerID.Value;
            if (pagerID == 0)
            {
                throw new ArgumentException("指定的栏目没有设置分页配置信息", "ChannelID,PagerID");
            }
            PagerSetService psService = new PagerSetService();
            PagerInfo piEntity = psService.GePagerSetInfo(pagerID);
            if (piEntity == null)
            {
                throw new ArgumentException("指定的栏目设置了错误的分页配置信息，请重新设置", "ChannelID,PagerID");
            }

            string sql = "SELECT COUNT(*) FROM NewsDoc WITH(NOLOCK) WHERE IsDelete=0 AND IsAuditing=1 AND ChannelID=" + channelID;

            int totalCount=0;
            //int totalPages;
            using (SQLHelper dao = new SQLHelper())
            {
                object objCount = dao.ExecuteScalar(CommandType.Text, sql);
                if (objCount != null)
                {
                    totalCount = TypeParse.ToInt(objCount);
                }
            }
            return totalCount;
        }
    }
}
