using System.Collections.Generic;
using CMS.Domain;

namespace CMS.Service
{
    /// <summary>
    /// 字段处理
    /// </summary>
    public class FieldService
    {
        /// <summary>
        /// 根据数据类型得到字段列表
        /// 1=新闻 2=推荐数据
        /// </summary>
        /// <param name="dataType"></param>
        /// <returns></returns>
        public IList<FieldEntity> GetFieldList(int dataType)
        {
            if (dataType == 2)
            {
                return GetRecommendFieldList();
            }
            return GetNewsFieldList();
        }

        /// <summary>
        /// 获取新闻字段列表
        /// </summary>
        /// <returns></returns>
        private IList<FieldEntity> GetNewsFieldList()
        {
            IList<FieldEntity> newsFieldList = new List<FieldEntity>(9);
            newsFieldList.Add(new FieldEntity { FieldName = "新闻ID", Field = "ID", FieldType = 2, IsOrder = true, IsWhere = true });
            newsFieldList.Add(new FieldEntity { FieldName = "栏目ID", Field = "ChannelID", FieldType = 2, IsOrder = true, IsWhere = true });
            newsFieldList.Add(new FieldEntity { FieldName = "新闻标题", Field = "title", FieldType = 1, IsOrder = false, IsWhere = true });
            newsFieldList.Add(new FieldEntity { FieldName = "副标题", Field = "subtitle", FieldType = 1 });
            newsFieldList.Add(new FieldEntity { FieldName = "作者", Field = "author", FieldType = 1, IsOrder = false, IsWhere = true });
            newsFieldList.Add(new FieldEntity { FieldName = "来源", Field = "source", FieldType = 1, IsOrder = false, IsWhere = true });
            newsFieldList.Add(new FieldEntity { FieldName = "是否置顶", Field = "IsTop", FieldType = 1, IsOrder = true, IsWhere = true });
            newsFieldList.Add(new FieldEntity { FieldName = "是否推荐", Field = "IsRecommend", FieldType = 1, IsOrder = false, IsWhere = true });
            newsFieldList.Add(new FieldEntity { FieldName = "简介", Field = "Summary", FieldType = 1 });
            newsFieldList.Add(new FieldEntity { FieldName = "缩略图", Field = "smallImageUrl", FieldType = 1 ,IsWhere = true });
            newsFieldList.Add(new FieldEntity { FieldName = "发表时间", Field = "publicTime", FieldType = 3, IsOrder = true, IsWhere = true });
            return newsFieldList;
        }

        /// <summary>
        /// 获取推荐内容字段列表
        /// </summary>
        /// <returns></returns>
        private IList<FieldEntity> GetRecommendFieldList()
        {
            IList<FieldEntity> recommendFieldList = new List<FieldEntity>(10);
            recommendFieldList.Add(new FieldEntity { FieldName = "内容ID", Field = "ID", FieldType = 2, IsOrder = true, IsWhere = true });
            recommendFieldList.Add(new FieldEntity { FieldName = "位置ID", Field = "LocationID", FieldType = 2, IsOrder = true, IsWhere = true });
            recommendFieldList.Add(new FieldEntity { FieldName = "标题", Field = "Title", FieldType = 1, IsOrder = false, IsWhere = true });
            recommendFieldList.Add(new FieldEntity { FieldName = "缩略小图", Field = "SmallPicUrl", FieldType = 1 });
            recommendFieldList.Add(new FieldEntity { FieldName = "缩略大图", Field = "BigPicUrl", FieldType = 1 });
            recommendFieldList.Add(new FieldEntity { FieldName = "简介", Field = "Summary", FieldType = 1 });
            recommendFieldList.Add(new FieldEntity { FieldName = "链接地址", Field = "LinkUrl", FieldType = 1 });
            recommendFieldList.Add(new FieldEntity { FieldName = "排序ID", Field = "SortID", FieldType = 2, IsOrder = true, IsWhere = false });
            recommendFieldList.Add(new FieldEntity { FieldName = "添加时间", Field = "CreateTime", FieldType = 3, IsOrder = true, IsWhere = true });
            return recommendFieldList;
        }
    }
}
