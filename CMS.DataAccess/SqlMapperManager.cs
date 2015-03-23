using IBatisNet.Common.Utilities;
using IBatisNet.DataMapper;
using IBatisNet.DataMapper.Configuration;

namespace CMS.DataAccess
{

    public class SqlMapperManager
    {
        private static volatile ISqlMapper _mapper;

        protected static void MaperConfigure(object obj)
        {
            _mapper = null;
        }


        protected static void InitMapper()
        {
            ConfigureHandler handler = MaperConfigure;
            DomSqlMapBuilder builder = new DomSqlMapBuilder();
            _mapper = builder.ConfigureAndWatch("SqlMap.config", handler);
        }

        public static ISqlMapper Instance
        {
            get
            {
                if (_mapper == null)
                {
                    lock (typeof(SqlMapper))
                    {
                        if (_mapper == null) // double-check
                        {
                            InitMapper();
                        }
                    }
                }
                return _mapper;
            }
        }
    }
}
