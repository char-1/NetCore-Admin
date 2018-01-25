using Hydra.Admin.Utility;
using Hydra.Admin.Utility.Helper;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Plugin.Cache.Redis
{
    public class RedisAction
    {
        private static RedisHost Config
        {
            get
            {
                try
                {
                    string filePath = AppDomain.CurrentDomain.BaseDirectory + @"Configs/redis.json";
                    return ConfigUtility<List<RedisHost>>.JsonConfig(filePath).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                    return new RedisHost
                    {
                        Host = "127.0.0.1",
                        Prot = 6379
                    };
                }
            }
        }
        public dynamic Connect(Func<RedisClient, dynamic> func)
        {
            try
            {
                using (var client = new RedisClient(Config.Host, Config.Prot, password: Config.PassWord))
                {
                    return func(client);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw ex;
            }
        }
        public void Connect(Action<RedisClient> action)
        {
            try
            {
                using (var client = new RedisClient(Config.Host, Config.Prot, password: Config.PassWord))
                {
                    action(client);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw ex;
            }
        }
    }
}
