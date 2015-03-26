using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;

using CMS.Service;
using CMS.Domain;
using CMS.CommonLib.Json;
using CMS.CommonLib.Utils;

namespace CMS.Template
{
    /// <summary>
    /// 系统标签处理
    /// 例如：发表时间、模板ID、分类ID、菜品ID、网站名称、分类名称
    /// </summary>
    public class SystemTag
    {
        /// <summary>
        /// 系统变量标签处理
        /// </summary>
        /// <param name="systemVar">系统变量</param>
        /// <param name="channelID"></param>
        /// <returns></returns>
        public static string DealSystemVar(string systemVar,long? channelID)
        {
            string systemValue = "";
            switch (systemVar)
            {
                case "SystemDate":// 系统日期
                    systemValue = DateTime.Now.ToString(ConfigurationManager.AppSettings["SystemDateFormat"]);
                    break;
                case "SystemTime":// 系统时间
                    systemValue = DateTime.Now.ToString(ConfigurationManager.AppSettings["SystemTimeFormat"]);
                    break;
                case "ChannelID":// 栏目ID
                    systemValue = channelID != null ? channelID.Value.ToString(CultureInfo.InvariantCulture) : "0";
                    break;
                case "ParentChannelName":
                    if (channelID != null)
                    {
                        ChannelService channelService = new ChannelService();
                        long channelid = channelService.GetRootChannelID(channelID.Value);
                        ChannelInfo channelInfo = channelService.GetChannelInfo(channelid);
                        if (channelInfo != null)
                        {
                            systemValue = channelInfo.Name;
                        }
                    }
                    break;
                case "ParentChannelUrl":
                    if (channelID != null)
                    {
                        ChannelService channelService = new ChannelService();
                        long channelid = channelService.GetRootChannelID(channelID.Value);
                        ChannelInfo channelInfo = channelService.GetChannelInfo(channelid);
                        if (channelInfo != null)
                        {
                            systemValue = channelInfo.ChannelUrlPart;
                        }
                    }
                    break;
                case "NewsUrl":
                    if (channelID != null)
                    {
                        ChannelService channelService = new ChannelService();
                        long channelid = channelService.GetRootChannelID(channelID.Value);
                        ChannelInfo channelInfo = channelService.GetChannelInfo(channelid);
                        if (channelInfo != null)
                        {
                            systemValue = "/"+channelInfo.EnName+"/Detail?id=";
                        }
                    }
                    break;
                case "ChannelName":// 栏目名称
                    if (channelID != null)
                    {
                        ChannelService channelService = new ChannelService();
                        ChannelInfo channelInfo = channelService.GetChannelInfo(channelID.Value);
                        if (channelInfo != null)
                        {
                            systemValue = channelInfo.Name;
                        }
                    }
                    break;
                case "ChannelUrl":// 栏目地址
                    if (channelID != null)
                    {
                        ChannelService channelService = new ChannelService();
                        ChannelInfo channelInfo = channelService.GetChannelInfo(channelID.Value);
                        if (channelInfo != null)
                        {
                            systemValue = channelInfo.ChannelUrlPart;
                        }
                    }
                    break;
                case "CurUserID":// 当前员工ID
                    if (TicketTool.IsLogin())
                    {
                        UserInfo curUser = JsonUtility.JsonToObject<UserInfo>(TicketTool.GetUserData());
                        if (curUser != null)
                        {
                            systemValue = curUser.ID.ToString(CultureInfo.InvariantCulture);
                        }
                    }
                    break;
                case "CurUserName":// 当前员工姓名
                    if (TicketTool.IsLogin())
                    {
                        UserInfo curUser = JsonUtility.JsonToObject<UserInfo>(TicketTool.GetUserData());
                        if (curUser != null)
                        {
                            systemValue = curUser.RealName;
                        }
                    }
                    break;
                default:
                    if (systemVar.StartsWith("Nav",true,CultureInfo.CurrentCulture))
                    {
                        // 导航
                        string[] navSets = systemVar.Split('-');// 第一项得到栏目ID，第二项得到使用的模板
                        if (navSets.Length == 2)
                        {
                            string navChannelSet = navSets[0];// 栏目ID
                            string navTemplateSet = navSets[1];//模板ID
                            TemplateService tpService = new TemplateService();
                            TemplateInfo tpEntity = tpService.GeTemplateInfo(TypeParse.ToLong(navTemplateSet.Replace(TagTypeEnum.T.ToString(),"")));
                            ChannelService channelService = new ChannelService();
                            
                            if (tpEntity != null)
                            {
                                string templateContent = tpEntity.TemplateCode;
                                // 得到同级栏目列表
                                int navchannelID = Int32.Parse(navChannelSet.Replace("Nav", ""));
                                long rootid = channelService.GetRootChannelID(navchannelID);

                                IList<ChannelInfo> listChannel = channelService.GetBrotherList(Convert.ToInt32(rootid));
                                if (listChannel.Count == 0)
                                {
                                    listChannel = channelService.GetParentChannelList();
                                }
                                //url中的{id}处理
                                foreach (ChannelInfo c in listChannel) {
                                    c.ChannelUrlPart = c.ChannelUrlPart.Replace("{id}", c.ID.ToString(CultureInfo.InvariantCulture));
                                    if (navchannelID == 0)
                                    {
                                        c.Childs = channelService.GetChildList(c.ID);
                                    }
                                }

                                // 2.从模板中摘出变量
                                IList<string> listVar = TemplateHandler.GetTemplateArea(templateContent);
                                // 3.定义Hashtable，保存变量名及对应的变量处理结果
                                Hashtable hashTagValue = new Hashtable(1);
                                foreach (string tagVar in listVar)
                                {
                                    if (!hashTagValue.ContainsKey(tagVar))
                                    {
                                        if (tagVar == "ChannelID" && channelID != null)
                                        {
                                            hashTagValue.Add(tagVar, channelID.Value);

                                        }
                                        else if (tagVar == "RootChannelID")
                                        {
                                            
                                            hashTagValue.Add("RootChannelID", rootid);
                                        }　　
                                        else
                                        {
                                            hashTagValue.Add(tagVar, listChannel);
                                        }
                                    }
                                }
                                systemValue = TemplateHandler.DealTemplateContent(hashTagValue, templateContent);
                            }
                        }
                    }else if (systemVar.StartsWith("SecondNav",true,CultureInfo.CurrentCulture))
                    {
                        // 二级树形栏目导航
                        string[] navSets = systemVar.Split('-');// 第一项得到栏目ID，第二项得到使用的模板
                        if (navSets.Length == 2)
                        {
                            string navChannelSet = navSets[0];// 栏目ID
                            string navTemplateSet = navSets[1];//模板ID
                            TemplateService tpService = new TemplateService();
                            ChannelService channelService = new ChannelService();
                            var tpEntity = tpService.GeTemplateInfo(TypeParse.ToLong(navTemplateSet.Replace(TagTypeEnum.T.ToString(),"")));
                            if (tpEntity != null)
                            {
                                string templateContent = tpEntity.TemplateCode;

                                int navchannelID = Int32.Parse(navChannelSet.Replace("SecondNav", ""));
                                long rootid = channelService.GetRootChannelID(navchannelID);
                                // 得到同级栏目列表
                                IList<ChannelInfo> listChannel = channelService.GetChildList(Convert.ToInt32( rootid ));
                                foreach (ChannelInfo c in listChannel)
                                {    //url中的{id}处理
                                    c.ChannelUrlPart = c.ChannelUrlPart.Replace("{id}", c.ID.ToString(CultureInfo.InvariantCulture));
                                    //得到同级栏目的下级栏目列表
                                    IList<ChannelInfo> clist = channelService.GetChildList(Convert.ToInt32(c.ID));
                           
                                    if (clist.Count > 0) {
                                        foreach (ChannelInfo s3 in clist)
                                        {
                                            s3.ChannelUrlPart = s3.ChannelUrlPart.Replace("{id}", s3.ID.ToString(CultureInfo.InvariantCulture));
                                        }
                                        c.Childs = clist;
                                    }
                                }

                                // 2.从模板中摘出变量
                                IList<string> listVar = TemplateHandler.GetTemplateArea(templateContent);
                                // 3.定义Hashtable，保存变量名及对应的变量处理结果
                                Hashtable hashTagValue = new Hashtable(1);
                                foreach (string tagVar in listVar)
                                {
                                    if (!hashTagValue.ContainsKey(tagVar))
                                    {
                                        if (tagVar == "ChannelID" && channelID != null)
                                        {
                                            hashTagValue.Add(tagVar, channelID.Value);
                                        }
                                        else if (tagVar == "RootChannelID")
                                        {
                                              
                                              hashTagValue.Add("RootChannelID", rootid);
                                        }
                                        else
                                        {
                                            hashTagValue.Add(tagVar, listChannel);
                                        }
                                    }
                                }
                                systemValue = TemplateHandler.DealTemplateContent(hashTagValue, templateContent);
                            }
                        }
                    }
                    else if (systemVar.StartsWith("CommonDisplayChannel", true, CultureInfo.CurrentCulture))
                    {
                        string[] navSets = systemVar.Split('-');// 第二项得到栏目ID
                        if (navSets.Length == 2)
                        {
                            string navTemplateSet = navSets[1]; //模版ID
                            string navChannelSet = navSets[2]; //栏目ID
                            ChannelService channelService = new ChannelService();
                             TemplateService tpService = new TemplateService();
                            var tpEntity = tpService.GeTemplateInfo(TypeParse.ToLong(navTemplateSet.Replace(TagTypeEnum.T.ToString(),"")));
                            if (tpEntity != null)
                            {
                                string templateContent = tpEntity.TemplateCode;
                                int navchannelID = Int32.Parse(navChannelSet.Replace("CommonDisplayChannel", ""));
                                // 得到同级栏目列表
                                IList<ChannelInfo> listChannel = channelService.GetChildList(Convert.ToInt32(navchannelID));
                                // 2.从模板中摘出变量
                                IList<string> listVar = TemplateHandler.GetTemplateArea(templateContent);
                                // 3.定义Hashtable，保存变量名及对应的变量处理结果
                                Hashtable hashTagValue = new Hashtable(1);
                                foreach (string tagVar in listVar)
                                {
                                    if (!hashTagValue.ContainsKey(tagVar))
                                    {
                                        if (tagVar == "CommonDisplayChannel")
                                        {
                                            hashTagValue.Add(tagVar, listChannel);
                                        }
                                    }
                                }
                                systemValue = TemplateHandler.DealTemplateContent(hashTagValue, templateContent);
                            }
                        }
                    }
                    break;
            }
            return systemValue;
        }
    }
}
