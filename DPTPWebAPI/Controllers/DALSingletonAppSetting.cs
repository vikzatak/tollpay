using DPTPWebAPI;
using System.Collections.Generic;
using System.Linq;

namespace WebApplicationTP.DAL
{
    public class DALSingletonAppSetting
    {

        //private static int counter = 0;
        List<AppKey> ak = new List<AppKey>();
        private static DALSingletonAppSetting instance = null;
        public static DALSingletonAppSetting GetInstance
        {
            get
            {
                if (instance == null)
                    instance = new DALSingletonAppSetting();
                return instance;
            }
        }

        private DALSingletonAppSetting()
        {

            DP_TPEntities dp = new DP_TPEntities();
            ak = dp.AppKeys.ToList();
        }

        public List<AppKey> GetAppKey()
        {
            return ak;
        }
    }
}
