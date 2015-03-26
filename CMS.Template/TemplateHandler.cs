using System;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

using Castle.Components.Common.TemplateEngine;

using CMS.Domain;
using CMS.Service;

namespace CMS.Template
{
    public class TemplateHandler
    {
        /// <summary>
        /// 分页数据标签
        /// </summary>
        private const string PAGEDATALISTTAG = "PagerDataList";
        /// <summary>
        /// 分页页码信息
        /// </summary>
        private const string PAGENUMINFOTAG = "PagerNumInfo";

        # region 从模板中摘取变量信息

        /// <summary>
        /// 从模板中摘取变量信息
        /// </summary>
        /// <param name="pageTempate">模板内容</param>
        /// <returns></returns>
        public static IList<string> GetTemplateArea(string pageTempate)
        {
            string pattern = @"\${(?<TemplateName>((?!{}).)*?)}";

            Regex r = new Regex(pattern, RegexOptions.Compiled);

            MatchCollection mc = r.Matches(pageTempate);

            IList<string> list = new List<string>(mc.Count);

            string name;
            foreach (Match m in mc)
            {
                if (m.Success)
                {
                    name = m.Groups["TemplateName"].Value.Trim();
                    if (!list.Contains(name))
                        list.Add(m.Groups["TemplateName"].Value.Trim());
                }
            }

            return list;
        }

        # endregion

        # region 根据模板生成静态文件

        /// <summary>
        /// 生成静态菜品
        /// </summary>
        /// <param name="ht">保存值的Hashtable</param>
        /// <param name="vmpath">模版文件</param>
        /// <param name="fileName">生成静态页的名称</param>
        public static void CreateFile(Hashtable ht, string vmpath, string fileName)
        {
            ITemplateEngine velocity = DataAccess.CastleContext.Instance.GetService<ITemplateEngine>();
            StringWriter sw = new StringWriter();
            velocity.Process(ht, vmpath, sw);
            FileStream fs = File.Open(fileName, FileMode.Create, FileAccess.Write, FileShare.Read);
            StreamWriter writer = new StreamWriter(fs, new UTF8Encoding(false));
            writer.Write(sw.ToString());
            writer.Close();
            fs.Close();
        }

        /// <summary>
        /// 生成静态菜品
        /// </summary>
        /// <param name="ht">保存值的Hashtable</param>
        /// <param name="vmpath">模版文件</param>
        /// <param name="encoding">编码格式</param>
        /// <param name="fileName">生成静态页的名称</param>
        public static void CreateFile(Hashtable ht, string vmpath, string fileName, Encoding encoding)
        {
            ITemplateEngine velocity = DataAccess.CastleContext.Instance.GetService<ITemplateEngine>();
            StringWriter sw = new StringWriter();
            velocity.Process(ht, vmpath, sw);

            FileStream fs = File.Open(fileName, FileMode.Create, FileAccess.Write, FileShare.Read);
            StreamWriter writer = new StreamWriter(fs, encoding);
            writer.Write(sw.ToString());
            writer.Close();
            fs.Close();
        }

        private static bool IsVelocityInited = false;
        private static object velocitylock = new object();
        /// <summary>
        /// 生成静态页面
        /// </summary>
        /// <param name="ht">模板参数值</param>
        /// <param name="templateContent">模板内容</param>
        /// <param name="encoding">编码</param>
        /// <param name="fileName">文件名</param>
        public static void CreateFileByTemplateContent(Hashtable ht, string templateContent, Encoding encoding, string fileName)
        {
            StringWriter swtemplate = null;
            StringWriter swhtml = null;
            try
            {
                //ITemplateEngine velocity = DataAccess.CastleContext.Instance.GetService<ITemplateEngine>();
                //StringWriter sw = new StringWriter();
                //velocity.Process(ht, string.Empty, sw, templateContent);
                string newtemplate = templateContent;
                newtemplate = newtemplate.Replace("#include", "\\#include");
                NVelocity.VelocityContext context = new NVelocity.VelocityContext();
                foreach (string key in ht.Keys)
                {
                    context.Put(key, ht[key]);
                }
                //初始化Nvelocity
                if (!IsVelocityInited)
                {
                    lock (velocitylock)
                    {
                        if (!IsVelocityInited)
                        {
                            NVelocity.App.Velocity.Init();
                            IsVelocityInited = true;
                        }
                    }
                }
                //将首次处理结果作为模析再解析一遍
                swhtml = new StringWriter();
                NVelocity.App.Velocity.Evaluate(context, swhtml, "cmsreprocess", newtemplate);

                //保存文件的html代码
                using (FileStream fs = File.Open(fileName, FileMode.Create, FileAccess.Write, FileShare.Read))
                {
                    StreamWriter writer = new StreamWriter(fs, encoding);
                    writer.Write(swhtml.ToString());
                    writer.Close();
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (swtemplate != null)
                {
                    swtemplate.Close();
                }

                if (swhtml != null)
                {
                    swhtml.Close();
                }
            }
        }

        # endregion

        # region 解析模板内容

        /// <summary>
        /// 解析模板内容
        /// </summary>
        /// <param name="ht">变量值key：变量 value：值</param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string DealTemplateContent(Hashtable ht, string content)
        {
            string result;
            StringWriter sw = new StringWriter();
            try
            {
                ITemplateEngine velocity = DataAccess.CastleContext.Instance.GetService<ITemplateEngine>();
                velocity.Process(ht, string.Empty, sw, content);
                result = sw.ToString();
            }
            finally
            {
                sw.Dispose();
            }
            return result;
        }

        # endregion

        # region 根据模板ID解析模板内容

        /// <summary>
        /// 根据模板ID解析模板内容
        /// </summary>
        /// <param name="templateID"></param>
        /// <param name="channelID"></param>
        /// <returns></returns>
        public static string DealTemplate(long? templateID, long? channelID)
        {
            return DealTemplate(templateID, channelID, null);
        }


        /// <summary>
        /// 显示指定列表页模板
        /// </summary>
        /// <param name="channelID"></param>
        /// <returns></returns>
        public static string DealListTemplate(long channelID)
        {
            ChannelService cservice = new ChannelService();
            ChannelInfo channelInfo = cservice.GetChannelInfo(channelID);
            if (channelInfo != null)
            {
                return DealTemplate(channelInfo.ListTemplateID, channelID);
            }
            return "list template no defined";
        }

        /// <summary>
        /// 显示指定列表页模板
        /// </summary>
        /// <param name="channelID"></param>
        /// <param name="newsID"></param>
        /// <returns></returns>
        public static string DealNewsTemplate(long channelID, long newsID)
        {
            ChannelService cservice = new ChannelService();
            ChannelInfo channelInfo = cservice.GetChannelInfo(channelID);
            if (channelInfo != null)
            {
                return DealTemplate(channelInfo.ContentTemplateID, channelID, newsID);
            }
            return "content template no defined";
        }

        /// <summary>
        /// 根据模板ID解析模板内容
        /// </summary>
        /// <param name="templateID">模板ID</param>
        /// <param name="channelID">栏目ID</param>
        /// <param name="newsID">新闻ID</param>
        /// <returns></returns>
        public static string DealTemplate(long? templateID, long? channelID, long? newsID)
        {
            // 模板处理结果
            string templateResult = "";

            //get template id by channel
            if (templateID == null || templateID.Value == 0)
            {
                if (channelID != null)
                {
                    ChannelService cservice = new ChannelService();
                    ChannelInfo channelInfo = cservice.GetChannelInfo(channelID.Value);
                    templateID = channelInfo.TemplateID;
                }
            }

            if (templateID != null)
            {
                // 1. 得到模板内容
                TemplateService tpService = new TemplateService();
                TemplateInfo tpEntity = tpService.GeTemplateInfo(templateID.Value);
                if (tpEntity != null)
                {
                    string templateContent = tpEntity.TemplateCode;
                    if (channelID != null)
                    {
                        templateContent = templateContent.Replace("${ChannelID}", channelID.Value.ToString(CultureInfo.InvariantCulture));

                        ChannelService cservice = new ChannelService();
                        long rootChannelID = cservice.GetRootChannelID(channelID.Value);
                        templateContent = templateContent.Replace("${RootChannelID}", rootChannelID.ToString(CultureInfo.InvariantCulture));
                    }

                    if (newsID != null && newsID > 0)
                    {
                        templateContent = templateContent.Replace("${NewsID}", newsID.Value.ToString(CultureInfo.InvariantCulture));
                    }
                    // 2.从模板中摘出变量
                    IList<string> listVar = GetTemplateArea(templateContent);
                    // 3.如果新闻ID不为空，则先处理新闻标签
                    // 4.定义Hashtable，保存变量名及对应的变量处理结果

                    # region 新闻模板处理

                    // 新闻模板处理
                    Hashtable hashTagValue = new Hashtable(1);
                    if (newsID != null && newsID > 0)
                    {
                        NewsService nService = new NewsService();
                        NewsDoc newsEntity = nService.GetNewsInfoByID(newsID.Value);
                        hashTagValue.Add("NewsID", newsEntity.ID);
                        hashTagValue.Add("Title", newsEntity.Title);
                        hashTagValue.Add("SubTitle", newsEntity.SubTitle);
                        hashTagValue.Add("TitleColor", newsEntity.TitleColor);
                        hashTagValue.Add("IsBold", newsEntity.IsBold);
                        hashTagValue.Add("Linkurl", newsEntity.Linkurl);
                        hashTagValue.Add("SmallImageUrl", newsEntity.SmallImageUrl);
                        hashTagValue.Add("Author", newsEntity.Author);
                        hashTagValue.Add("Source", newsEntity.Source);
                        hashTagValue.Add("Tags", newsEntity.Tags);
                        hashTagValue.Add("Summary", newsEntity.Summary);
                        hashTagValue.Add("Content", newsEntity.Content);
                        if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["SystemTimeFormat"]))
                        {
                            hashTagValue.Add("PublicTime", (newsEntity.PublicTime ?? DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss"));
                        }
                        else
                        {
                            hashTagValue.Add("PublicTime", (newsEntity.PublicTime ?? DateTime.Now).ToString(ConfigurationManager.AppSettings["SystemTimeFormat"]));
                        }
                    }

                    # endregion

                    foreach (string tagVar in listVar)
                    {
                        if (!hashTagValue.ContainsKey(tagVar))
                        {
                            if (tagVar == PAGEDATALISTTAG || tagVar == PAGENUMINFOTAG)// 分页列表数据 分页页码
                            {
                                PagerHandler pagerHandler = new PagerHandler();
                                string pageNumList = "";
                                IList<TemplateDoc> listData = pagerHandler.GetPagerDataList(channelID ?? 0, ref pageNumList);
                                hashTagValue.Add(PAGEDATALISTTAG, listData);
                                hashTagValue.Add(PAGENUMINFOTAG, pageNumList);

                            }
                            else
                            {
                                hashTagValue.Add(tagVar, TagFactory.TagDeal(tagVar, channelID));
                            }
                        }
                    }

                    templateResult = DealTemplateContent(hashTagValue, templateContent);
                }
            }
            return templateResult;
        }

        # endregion

        # region 专题模板

        /// <summary>
        /// 根据模板ID解析模板内容
        /// </summary>
        /// <param name="templateID">模板ID</param>
        /// <param name="specialID">专题ID</param>
        /// <returns></returns>
        public static string DealSpecialTemplate(long templateID, long specialID)
        {
            // 模板处理结果
            string templateResult = "";


            // 1. 得到模板内容
            TemplateService tpService = new TemplateService();
            TemplateInfo tpEntity = tpService.GeTemplateInfo(templateID);
            if (tpEntity != null)
            {
                string templateContent = tpEntity.TemplateCode;

                // 2.从模板中摘出变量
                IList<string> listVar = GetTemplateArea(templateContent);
                // 3.处理专题信息标签
                // 4.定义Hashtable，保存变量名及对应的变量处理结果
                # region 专题模板处理

                // 专题模板处理
                Hashtable hashTagValue = new Hashtable(1);
                if (specialID > 0)
                {
                    SpecialService specialService = new SpecialService();
                    SpecialInfo specialEntity = specialService.GetSpecialInfoByID(specialID);
                    hashTagValue.Add("S-ID", specialEntity.ID);
                    hashTagValue.Add("S-Name", specialEntity.Name);
                    hashTagValue.Add("S-SmallPicUrl", specialEntity.SmallPicUrl);
                    hashTagValue.Add("S-PicUrl", specialEntity.PicUrl);
                    hashTagValue.Add("S-Keyword", specialEntity.Keyword);
                    hashTagValue.Add("S-Description", specialEntity.Description);
                    if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["SystemTimeFormat"]))
                    {
                        hashTagValue.Add("CreateTime", specialEntity.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                    else
                    {
                        hashTagValue.Add("CreateTime", specialEntity.CreateTime.ToString(ConfigurationManager.AppSettings["SystemTimeFormat"]));
                    }
                }

                # endregion

                foreach (string tagVar in listVar)
                {
                    if (!hashTagValue.ContainsKey(tagVar))
                    {
                        if (tagVar.StartsWith("SC-"))
                        {
                            hashTagValue.Add(tagVar, SpecialTag.GetSpecialChannelNews(specialID, tagVar));
                        }
                        else
                        {
                            hashTagValue.Add(tagVar, TagFactory.TagDeal(tagVar, null));
                        }
                    }
                }

                templateResult = DealTemplateContent(hashTagValue, templateContent);
            }

            return templateResult;
        }

        # endregion
    }
}
