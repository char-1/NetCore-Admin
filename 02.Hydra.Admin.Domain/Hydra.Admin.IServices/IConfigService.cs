using Hydra.Admin.Models.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Admin.IServices
{
    public interface IConfigService : IBaseService<Config>
    {
        /// <summary>
        /// 设置cofig
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool SetConfig(string key, object value);
        /// <summary>
        /// 设置config
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        bool SetConfig(Config config);
        /// <summary>
        /// 获取config 
        /// </summary>
        /// <param name="useCache">是否使用缓存</param>
        /// <returns></returns>
        Config GetConfig(bool useCache = true);
    }
}
