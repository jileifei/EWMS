using IBatisNet.DataMapper;


namespace CMS.DataAccess
{
    /// <summary>
    /// ����keyword���ݿ������Ļ���
    /// </summary>
    public abstract class BaseDAO
    {
        /// <summary>
        /// ��ȡ����ӡ�����ʵ��
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
