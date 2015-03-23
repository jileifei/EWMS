using System.Collections.Generic;
using CMS.Domain;
using CMS.CommonLib.Utils;

namespace CMS.Template
{
    public class SpecialTag
    {
        public static IList<TemplateDoc> GetSpecialChannelNews(long specialID,string channeltag)
        {
            string[] tagSets = channeltag.Split('-');
            IList<TemplateDoc> listResult = new List<TemplateDoc>();
            if (tagSets.Length == 3)
            {
                long specialChannelID = TypeParse.ToLong(tagSets[1].Replace("C",""));
                if (specialChannelID > 0)
                {
                    int rowCount = TypeParse.ToInt(tagSets[2]);
                    if (rowCount == 0)
                    {
                        rowCount = 5;
                    }
                    const string sqlGet = "SELECT TOP {0} [ID] ,[ChannelID] ,[SpecialID],[title] ,[subTitle],[TitleColor],[author],[publicTime] ,[Summary]  ,[IsTop]  ,[IsRecommend] ,[IsBold],[Tags] ,[Linkurl],[smallImageUrl],[source] ,[clickCount] ,[status] FROM dbo.NewsDoc WITH(NOLOCK) WHERE IsAuditing=1 AND IsDelete=0 AND SpecialID={1} AND SpecialChannelID={2} ORDER BY publicTime DESC";
                    listResult = BaseTemplate.GetSqlResult(string.Format(sqlGet, rowCount, specialID, specialChannelID));// 分页数据
                    const string linkurl = "http://www.beijing-dentsu.com.cn/news/s{0}/{1}";
                    foreach (TemplateDoc item in listResult)
                    {
                        if (string.IsNullOrEmpty(item.get("Linkurl")))
                        {
                            item.SetValue("Linkurl", string.Format(linkurl, item.get("ChannelID"), item.get("ID")));
                        }
                    }
                }
            }
            return listResult;
        }
    }
}
