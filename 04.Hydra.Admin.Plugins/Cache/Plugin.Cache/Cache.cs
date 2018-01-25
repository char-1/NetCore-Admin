using Autofac;
using Autofac.Builder;
using Autofac.Configuration;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace Plugin.Cache
{
    public class Cache
    {
        private static ICache _cache = null;
        private static readonly object _cacheLocker = new object();
        static Cache()
        {
            Load();
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static bool Add(string key, object value)
        {
            if (!string.IsNullOrWhiteSpace(key) && (value != null))
            {
                return _cache.Add(key, value);
            }
            return false;
        }
        /// <summary>
        /// 添加 async
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static async Task<bool> AddAsync(string key, object value)
        {
            if (!string.IsNullOrWhiteSpace(key) && (value != null))
            {
                bool x = await _cache.AddAsync(key, value);
                return x;
            }
            return false;
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expiresSliding">滑动过期时长（如果在过期时间内有操作，则以当前时间点延长过期时间） MinValue 不滑动</param>
        /// <param name="expiressAbsoulte">有效时长</param>
        /// <returns></returns>
        public static bool Add(string key, object value, TimeSpan expiresSliding, TimeSpan expiressAbsoulte)
        {
            if (!string.IsNullOrWhiteSpace(key) && (value != null))
            {
                return _cache.Add(key, value, expiresSliding, expiressAbsoulte);
            }
            return false;
        }
        /// <summary>
        /// 添加 async
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expiressAbsoulte">过期时间</param>
        public static async Task<bool> AddAsync(string key, object value, TimeSpan expiresSliding, TimeSpan expiressAbsoulte)
        {
            if (!string.IsNullOrWhiteSpace(key) && (value != null))
            {
                return await _cache.AddAsync(key, value, expiresSliding,expiressAbsoulte);
            }
            return false;
        }
        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="key"></param>
        public static bool Remove(string key)
        {
            if (!string.IsNullOrWhiteSpace(key))
            {
                return _cache.Remove(key);
            }
            return false;
        }
        /// <summary>
        /// 移除 async
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<bool> RemoveAsync(string key)
        {
            if (!string.IsNullOrWhiteSpace(key))
            {
                return await _cache.RemoveAsync(key);
            }
            return false;
        }
        /// <summary>
        /// 移除所有
        /// </summary>
        public static bool RemoveAll()
        {
            return _cache.RemoveAll();
        }
        /// <summary>
        /// 获取
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Get<T>(string key)
        {
            return string.IsNullOrWhiteSpace(key) ? default(T) : _cache.Get<T>(key);
        }
        /// <summary>
        /// 获取 async
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<T> GetAsync<T>(string key)
        {
            return string.IsNullOrWhiteSpace(key) ? default(T) : await _cache.GetAsync<T>(key);
        }
        private static void Load()
        {
            var builder = new ContainerBuilder();
            var config = new ConfigurationBuilder();
            config.AddJsonFile("Configs/autofac.json");
            var module = new ConfigurationModule(config.Build());
            builder.RegisterModule(module);
            IContainer context = null;
            try
            {
                context = builder.Build(ContainerBuildOptions.None);
                _cache = context.Resolve<ICache>();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (context != null)
                    context.Dispose();
            }
        }
    }
}
