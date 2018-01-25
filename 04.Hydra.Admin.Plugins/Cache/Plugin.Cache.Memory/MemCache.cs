using Hydra.Admin.Utility.Helper;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace Plugin.Cache.Memory
{
    public class MemCache : ICache
    {
        private MemoryCache _cache;
        public MemCache()
        {
            InitializeCache();
        }
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
            _cache.Set(key, value);
            return Exists(key);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiresSliding">TimeSpan.MinValue 不滑动</param>
        /// <param name="expiressAbsoulte"></param>
        /// <returns></returns>
        public bool Add(string key, object value, TimeSpan expiresSliding, TimeSpan expiressAbsoulte)
        {
            if (key.IsNullOrBlank())
                throw new ArgumentNullException(nameof(key));
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            if (expiresSliding != TimeSpan.MinValue)
                _cache.Set(key, value, new MemoryCacheEntryOptions().SetSlidingExpiration(expiresSliding).SetAbsoluteExpiration(expiressAbsoulte));
            else
                _cache.Set(key, value, new MemoryCacheEntryOptions().SetAbsoluteExpiration(expiressAbsoulte));
            return Exists(key);
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
            return _cache.TryGetValue(key, out object cached);
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
            return (T)_cache.Get(key);
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
            _cache.Remove(key);
            return !Exists(key);
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
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool RemoveAll()
        {
            throw new NotImplementedException();
        }
        private void InitializeCache()
        {
            this._cache = new MemoryCache(new MemoryCacheOptions());
        }

        public void Dispose()
        {
            _cache?.Dispose();
        }
    }
}
