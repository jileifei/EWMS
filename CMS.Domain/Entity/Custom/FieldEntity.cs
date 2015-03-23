using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMS.Domain
{
    /// <summary>
    /// 字段信息
    /// </summary>
    public class FieldEntity
    {
        /// <summary>
        /// 字段名称
        /// </summary>
        public string FieldName
        {
            get;
            set;
        }

        /// <summary>
        /// 字段
        /// </summary>
        public string Field
        {
            get;
            set;
        }

        /// <summary>
        /// 字段类型
        /// 1=字符串 2=数字 3=日期 4=布尔
        /// </summary>
        public int FieldType
        {
            get;
            set;
        }

        private bool _isOrder = false;
        /// <summary>
        /// 是否参与排序
        /// </summary>
        public bool IsOrder
        {
            get { return _isOrder; }
            set { _isOrder = value; }
        }

        private bool _isWhere = false;
        /// <summary>
        /// 是否作为查询条件
        /// </summary>
        public bool IsWhere
        {
            get { return _isWhere; }
            set { _isWhere = value; }
        }
    }
}
