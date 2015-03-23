namespace CMS.Template
{
    public class TagFactory
    {
        /// <summary>
        /// tag处理
        /// </summary>
        /// <param name="tagName"></param>
        /// <param name="channelID"></param>
        /// <returns></returns>
        public static string TagDeal(string tagName, long? channelID)
        {
            if (string.IsNullOrEmpty(tagName))
            {
                return "";
            }
            string tagValue;
            // 变量类型
            char tagType = tagName.ToCharArray()[0];
            switch (tagType)
            {
                case 'G': // 全局变量
                    tagValue = GlobalVarTag.DealGlobalVal(tagName);
                    break;
                case 'D': // 数据块
                    tagValue = DataBlockTag.DealDataBlockVar(tagName);
                    break;
                default: // 系统变量
                    tagValue = SystemTag.DealSystemVar(tagName, channelID);
                    break;
            }
            return tagValue;
        }
    }
}
