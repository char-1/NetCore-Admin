using Hydra.Admin.IServices;
using Hydra.Admin.Models;
using Hydra.Admin.Models.Model;
using Plugin.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Hydra.Admin.Services
{
    public class ConfigService : BaseService<Config>, IConfigService
    {
        public ConfigService()
        {
        }
        public Config GetConfig(bool useCache = true)
        {
            Config _config;
            if (useCache && Cache.Get<Config>(CacheKeys.Cache_System_Config) != null)
                _config = Cache.Get<Config>(CacheKeys.Cache_System_Config);
            else
            {
                _config = DbFunction((db) =>
            {
                IEnumerable<Config> configs = db.Queryable<Config>().ToList();
                _config = new Config();
                Config temp;
                PropertyInfo[] properties = _config.GetType().GetProperties();
                foreach (var property in properties)
                {
                    if (property.Name != "Id" && property.Name != "Value" && property.Name != "Key")
                    {
                        temp = configs.FirstOrDefault(item => item.Key == property.Name);
                        if (temp != null)
                            property.SetValue(_config, Convert.ChangeType(temp.Value, property.PropertyType));
                    }
                }
                if (useCache)
                {
                    Cache.Add(CacheKeys.Cache_System_Config, _config, TimeSpan.MinValue, TimeSpan.FromHours(5));
                }
                return _config;
            });
            }
            return _config;
        }

        public bool SetConfig(string key, object value)
        {
            bool flag = true;
            if (value == null) throw new ArgumentNullException("值不能为null");
            return DbFunction((db) =>
            {
                PropertyInfo[] properties = typeof(Config).GetProperties();
                if (properties.FirstOrDefault(item => item.Name == key) == null) throw new ApplicationException("未找到" + key + "对应的配置项");
                var _config = db.Queryable<Config>().Single(item => item.Key == key);
                if (_config == null)
                {
                    _config = new Config();
                    db.Insertable(_config).ExecuteCommand();
                }
                else
                {
                    _config.Key = key;
                    _config.Value = value.ToString();
                    db.Updateable(_config).ExecuteCommand();
                }
                Cache.Remove(CacheKeys.Cache_System_Config);
                return true;
            }, flag);
        }

        public bool SetConfig(Config config)
        {
            bool flag = true;
            PropertyInfo[] properties = config.GetType().GetProperties();
            Config temp;
            string value;
            object obj;
            return DbFunction((db) =>
            {
                var configs = db.Queryable<Config>().ToList();
                foreach (var property in properties)
                {
                    obj = property.GetValue(config);
                    if (obj != null)
                        value = obj.ToString();
                    else
                        value = "";
                    if (property.Name != "Id" && property.Name != "Value" && property.Name != "Key")
                    {
                        temp = configs.FirstOrDefault(item => item.Key == property.Name);
                        if (temp == null)
                            db.Insertable(new Config() { Key = property.Name, Value = value, Id = Guid.NewGuid().ToString() }).ExecuteCommand();
                        else
                            db.Updateable(new Config() { Key = property.Name, Value = value, Id = temp.Id }).ExecuteCommand();
                    }
                }
                Cache.Remove(CacheKeys.Cache_System_Config);
                return true;
            }, flag);
        }
    }
}
