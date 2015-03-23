using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using CMS.Domain;
using CMS.Service;
using CMS.CommonLib.Utils;

namespace CMS.Template
{
    /// <summary>
    /// 全局变量标签处理
    /// </summary>
    public class GlobalVarTag
    {
        /// <summary>
        /// 全局变量标签处理
        /// </summary>
        /// <param name="globalVar">变量名称</param>        
        /// <returns></returns>
        public static string DealGlobalVal(string globalVar)
        {
            return DealGlobalVal(globalVar,null);
        }

        /// <summary>
        /// 全局变量标签处理
        /// </summary>
        /// <param name="globalVar">变量名称</param>
        /// <param name="channelID">栏目ID</param>
        /// <returns></returns>
        public static string DealGlobalVal(string globalVar,int? channelID)
        {
            string globalVarContent = "";
            if (string.IsNullOrEmpty(globalVar))
            {
                return "";
            }
            GlobalService globalService = new GlobalService();
            GlobalVariable globalEntity = globalService.GeGlobalInfo(TypeParse.ToLong(globalVar.Replace(TagTypeEnum.G.ToString(),"")));
            if (globalEntity != null)
            {
                string templateContent = globalEntity.Content;
                if (channelID != null)
                {
                    templateContent = templateContent.Replace("${ChannelID}", channelID.Value.ToString(CultureInfo.InvariantCulture));
                }
                // 2.从模板中摘出变量
                IList<string> listVar = TemplateHandler.GetTemplateArea(templateContent);
                // 3.定义Hashtable，保存变量名及对应的变量处理结果
                Hashtable hashTagValue = new Hashtable(1);
                foreach (string tagVar in listVar)
                {
                    if (!hashTagValue.ContainsKey(tagVar))
                    {
                        hashTagValue.Add(tagVar, SystemTag.DealSystemVar(tagVar, channelID));
                    }
                }
                globalVarContent = TemplateHandler.DealTemplateContent(hashTagValue, templateContent);
            }
            return globalVarContent;
        }
    }
}
