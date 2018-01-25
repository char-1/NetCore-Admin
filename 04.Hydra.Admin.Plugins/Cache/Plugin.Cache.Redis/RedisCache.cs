using Hydra.Admin.Utility.Helper;
using System;
using System.Threading.Tasks;

namespace Plugin.Cache.Redis
{
    public class RedisCache : RedisAction, ICache
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Add(string key, object value)
        {
            if (key.IsNullOrBlank())
                throw new ArgumentNullException(nameof(key));
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            return Connect((client) =>
            {
                return client.Set(key, value, TimeSpan.MaxValue);
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiresSliding"></param>
        /// <param name="expiressAbsoulte"></param>
        /// <returns></returns>
        public bool Add(string key, object value, TimeSpan expiresSliding, TimeSpan expiressAbsoulte)
        {
            if (key.IsNullOrBlank())
                throw new ArgumentNullException(nameof(key));
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            return Connect((client) =>
            {
                return client.Set(key, value, expiressAbsoulte);
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(string key, object value)
        {
            return await Task.Run(() => Add(key, value));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiresSliding"></param>
        /// <param name="expiressAbsoulte"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(string key, object value, TimeSpan expiresSliding, TimeSpan expiressAbsoulte)
        {
            return await Task.Run(() => Add(key, value, expiresSliding, expiressAbsoulte));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Exists(string key)
        {
            if (key.IsNullOrBlank())
                throw new ArgumentNullException(nameof(key));
            return Connect((client) =>
            {
                return client.Get(key) == null;
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            if (key.IsNullOrBlank())
                throw new ArgumentNullException(nameof(key));
            return Connect((client) =>
            {
                return client.Exists(key) > 0 ? client.Get<T>(key) : default(T);
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(string key)
        {
            return await Task.Run(() => Get<T>(key));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(string key)
        {
            if (key.IsNullOrBlank())
                throw new ArgumentNullException(nameof(key));
            return Connect((client) =>
            {
                return client.Del(key) > 0;
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool RemoveAll()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<bool> RemoveAsync(string key)
        {
            return await Task.Run(() => Remove(key));
        }
    }
}
