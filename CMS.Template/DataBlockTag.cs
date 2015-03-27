using System;
using System.Collections;
using System.Collections.Generic;
using CMS.Domain;
using CMS.Service;
using CMS.CommonLib.Utils;

namespace CMS.Template
{
    /// <summary>
    /// 数据标签处理
    /// 从数据库中读取数据，支持参数定义
    /// 标题长度，条数，分类，排序
    /// </summary>
    public class DataBlockTag
    {
        /// <summary>
        /// 新闻基本SQL
        /// </summary>
        public const string NEWSELECTSQL = @"SELECT TOP {0} NewsDoc.ID, NewsDoc.ChannelID, NewsDoc.title, NewsDoc.subTitle,NewsDoc.content, NewsDoc.author, NewsDoc.source, NewsDoc.Summary, NewsDoc.smallImageUrl, 
                        NewsDoc.Linkurl, NewsDoc.Tags, ChannelInfo.ChannelUrlPart, NewsDoc.publicTime
FROM         NewsDoc INNER JOIN  ChannelInfo ON NewsDoc.ChannelID = ChannelInfo.ID
WHERE     (NewsDoc.IsAuditing = 1) AND (NewsDoc.IsDelete = 0) ";

        /// <summary>
        /// 推荐内容基本SQL
        /// </summary>
        public const string RECOMMENDELECTSQL = @"SELECT TOP {0} ID,LocationID,Title,SmallPicUrl,BigPicUrl,Summary,LinkUrl,SortID,CreateTime FROM dbo.RecommedInfoList WHERE  status = 1 ";

        /// <summary>
        /// 数据块标签处理
        /// </summary>
        /// <param name="dataBlockVar">数据块变量名称</param>
        /// <returns></returns>
        public static string DealDataBlockVar(string dataBlockVar)
        {
            long dataBlockID = 0;
            long templateID = 0;
            int channelID = 0;
            int rowCount = 0;
            string dataBlockContent = "";
            string[] dataBlockSets = dataBlockVar.Split('-');
            foreach (string datablockset in dataBlockSets)
            {
                if (datablockset.StartsWith(TagTypeEnum.D.ToString()))// 数据块ID
                {
                    dataBlockID = TypeParse.ToLong(datablockset.Replace(TagTypeEnum.D.ToString(),""));
                }
                else if (datablockset.StartsWith(TagTypeEnum.T.ToString()))// 模板ID
                {
                    templateID = TypeParse.ToLong(datablockset.Replace(TagTypeEnum.T.ToString(), ""));
                }
                else if (datablockset.StartsWith(TagTypeEnum.C.ToString()))// 栏目ID
                {
                    channelID = TypeParse.ToInt(datablockset.Replace(TagTypeEnum.C.ToString(), ""));
                }
                else if (datablockset.StartsWith(TagTypeEnum.R.ToString()))// 条数
                {
                    rowCount = TypeParse.ToInt(datablockset.Replace(TagTypeEnum.R.ToString(), ""));
                }
            }
            // 1.根据数据块ID得到数据块的设置信息
            DataBlockService dbService = new DataBlockService();
            DataBlock dbEntity = dbService.GeDataBlockInfo(dataBlockID);
            if (dbEntity != null)
            {
                #region 解析板块数据
                if (dbEntity.TemplateID != null && dbEntity.TemplateID > 0&&dbEntity.Type==2)
                {
                    RecommedPositionService positionService = new RecommedPositionService();
                    RecommedPosition positionInfo=positionService.GetPositionInfo(dbEntity.TemplateID);
                    if (positionInfo != null)
                    {
                        if (positionInfo.IsInclude)
                        {
                            return "<!-- #include virtual=\"/include/Positions/" + positionInfo.Name + ".shtml\"-->";
                        }
                        else
                        {
                            Hashtable hashTagValue = new Hashtable(1);
                            IList<string> listVar = TemplateHandler.GetTemplateArea(positionInfo.PlateContent);
                            foreach (var varTag in listVar)
                            {
                                IList<TemplateDoc> datalist = GetDataBlockTemplateDoc(dataBlockID);
                                hashTagValue.Add(varTag, datalist);
                            }
                            return TemplateHandler.DealTemplateContent(hashTagValue, positionInfo.PlateContent);
                        }
                    }
                }
                #endregion

                // 2.添加栏目的搜索条件及读取条数
                string execSql;
                if (dbEntity.Type == 2)
                {
                    // 推荐内容
                    execSql = RECOMMENDELECTSQL;
                }
                else
                {
                    execSql = NEWSELECTSQL;
                }
                if (!string.IsNullOrEmpty(dbEntity.Where))
                {
                    execSql += " AND (" + dbEntity.Where + ")";

                    //如果查询条件中未指定频道ID，则带上频道ID参数，否则不再覆盖频道ID参数
                    if (dbEntity.Where.ToLower().IndexOf(" channelid", StringComparison.Ordinal) == -1)
                    {
                        if (channelID > 0)
                        {
                            execSql += " AND ChannelID=" + channelID;
                        } 
                    }
                }
                else
                {
                    if (channelID > 0)
                    {
                        execSql += " AND ChannelID=" + channelID;
                    }
                }
                
                if (!string.IsNullOrEmpty(dbEntity.OrderByField))
                {
                    execSql += " ORDER BY " + dbEntity.OrderByField;
                }
                if (rowCount > 0)
                {
                    execSql = string.Format(execSql, rowCount);
                }
                else
                {
                    execSql = string.Format(execSql, dbEntity.RowCount);
                }
               
                // 3.根据模板ID得到模板内容
                if (templateID == 0)
                {
                    templateID = dbEntity.TemplateID;
                }
                TemplateService tpService = new TemplateService();
                TemplateInfo tpEntity = tpService.GeTemplateInfo(templateID);
                if (tpEntity != null)
                {
                    // 4.根据第二步得到的SQL得到SQL执行结果，然后再解析模板最终得到解析内容
                    string templateContent = tpEntity.TemplateCode;
                    // 从模板中摘出变量
                    IList<string> listVar = TemplateHandler.GetTemplateArea(templateContent);
                    // 定义Hashtable，保存变量名及对应的变量处理结果
                    Hashtable hashTagValue = new Hashtable(1);
                    foreach (string tagVar in listVar)
                    {
                        if (!hashTagValue.ContainsKey(tagVar))
                        {
                            //add by xwarrior @ 2011/12/4
                            IList<TemplateDoc> datalist;
                            try
                            {
                                datalist = BaseTemplate.GetSqlResult(execSql);
                                
                            }
                            catch (Exception) {
                                throw new Exception("block sql error:" + execSql);
                            }
                            
                            //新闻列表，生成菜品页访问的url
                            if (dbEntity.Type == 1) { 
                                foreach(TemplateDoc doc in datalist){
                                    string urlpart = doc.get("ChannelUrlPart"); //About/s{id}
                                    if (!urlpart.EndsWith("/"))
                                        urlpart += "/";
                                    urlpart = urlpart.Replace("{id}", doc.get("ChannelID"));
                                    string url = urlpart  + doc.get("ID");

                                    doc.SetValue("url", url);
                                }
                            }

                            hashTagValue.Add(tagVar,datalist);
                        }
                    }
                    dataBlockContent = TemplateHandler.DealTemplateContent(hashTagValue, templateContent);
                }
                
            }
            return dataBlockContent;
        }

        /// <summary>
        /// 根据数据块ID获取数据List
        /// </summary>
        /// <param name="dataBlockID"></param>
        /// <returns></returns>
        public static IList<TemplateDoc> GetDataBlockTemplateDoc(Int64 dataBlockID)
        {
            DataBlockService dbService = new DataBlockService();
            DataBlock dbEntity = dbService.GeDataBlockInfo(dataBlockID);
            // 2.添加栏目的搜索条件及读取条数
            string execSql;
            if (dbEntity.Type == 2)
            {
                // 推荐内容
                execSql = RECOMMENDELECTSQL;
            }
            else
            {
                execSql = NEWSELECTSQL;
            }
            if (!string.IsNullOrEmpty(dbEntity.Where))
            {
                execSql += " AND (" + dbEntity.Where + ")";
            }

            if (!string.IsNullOrEmpty(dbEntity.OrderByField))
            {
                execSql += " ORDER BY " + dbEntity.OrderByField;
            }
            execSql = string.Format(execSql, dbEntity.RowCount);
            IList<TemplateDoc> datalist;
            try
            {
                datalist = BaseTemplate.GetSqlResult(execSql);

            }
            catch (Exception)
            {
                throw new Exception("block sql error:" + execSql);
            }
            return datalist;
        }
    }
}
