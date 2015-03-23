using IBatisNet.DataMapper;


namespace CMS.DataAccess
{
    /// <summary>
    /// 所有keyword数据库操作类的基类
    /// </summary>
    public abstract class BaseDAO
    {
        /// <summary>
        /// 获取数据印射对象实例
        /// </summary>
        /// <returns></returns>
        protected virtual ISqlMapper Instance
        {
            get
            {
                return SqlMapperManager.Instance;
            }
        }
    }
}
