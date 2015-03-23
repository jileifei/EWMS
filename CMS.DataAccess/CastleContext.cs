using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;
using Castle.Core.Resource;

namespace CMS.DataAccess
{
   /// <summary>
   /// Castle������ʵ��
   /// </summary>
    public class CastleContext
    {
        private static volatile  WindsorContainer _instance;
        private static volatile object _olock = new object();

        /// <summary>
        /// ��ȡCastle������ʵ��
        /// </summary>
        public static WindsorContainer Instance {
            get {
                if (_instance == null) {
                    lock (_olock){
                        if (_instance == null) { 
                               _instance  = new WindsorContainer(new XmlInterpreter(new ConfigResource()));
                        }
                    }
                }

                return _instance;
            }
        }

        public static void ReloadWinsorContainer() {
            lock (_olock)
            {
                if (_instance != null) {
                    _instance.Dispose();
                }
                _instance = null;
            }
        }
    }
}
