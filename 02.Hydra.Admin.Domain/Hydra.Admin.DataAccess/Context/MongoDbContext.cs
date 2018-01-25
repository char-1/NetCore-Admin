using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Hydra.Admin.DataAccess
{
    public class MongoDbContext 
    {
        private static IMongoDatabase GetDatabase()
        {
            var mc = new MongoClient("DbConfig.MongoDBUrl");
            return mc.GetDatabase("DbConfig.DataBaseName");
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName">集合名称</param>
        /// <param name="model"></param>
        public static void Insert<T>(string collectionName, T model) where T : class, new()
        {
            if (model == null)
                throw new ArgumentNullException("model", "待插入数据不能为空");
            var db = GetDatabase();
            var collection = db.GetCollection<T>(collectionName);
            collection.InsertOne(model);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="model"></param>
        /// <returns></returns>

        public static async Task InsertAsync<T>(string collectionName, T model) where T : class, new()
        {
            if (model == null)
                throw new ArgumentNullException("model", "待插入数据不能为空");
            var db = GetDatabase();
            var collection = db.GetCollection<T>(collectionName);
            await collection.InsertOneAsync(model);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="collectionName">集合名称</param>
        /// <param name="filter"></param>
        /// <param name="update"></param>
        public static UpdateResult Update<T>(string collectionName, Expression<Func<T, bool>> filter,
            UpdateDefinition<T> update) where T : class, new()
        {
            var db = GetDatabase();
            var collection = db.GetCollection<T>(collectionName);
            return collection.UpdateOne(filter, update);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="collectionName">集合名称</param>
        /// <param name="filter"></param>
        /// <param name="update"></param>
        public static async Task<UpdateResult> UpdateAsync<T>(string collectionName, Expression<Func<T, bool>> filter,
            UpdateDefinition<T> update) where T : class, new()
        {
            var db = GetDatabase();
            var collection = db.GetCollection<T>(collectionName);
            return await collection.UpdateOneAsync(filter, update);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="collectionName">集合名称</param>
        /// <param name="filter">条件</param>
        public static DeleteResult Remove<T>(string collectionName, Expression<Func<T, bool>> filter) where T : class, new()
        {
            var db = GetDatabase();
            var collection = db.GetCollection<T>(collectionName);
            return collection.DeleteOne(filter);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="collectionName">集合名称</param>
        /// <param name="filter">条件</param>
        public static async Task<DeleteResult> RemoveAsync<T>(string collectionName, Expression<Func<T, bool>> filter) where T : class, new()
        {
            var db = GetDatabase();
            var collection = db.GetCollection<T>(collectionName);
            return await collection.DeleteOneAsync(filter);
        }

        /// <summary>
        /// 删除所有
        /// </summary>
        /// <param name="collectionName">集合名称</param>
        /// <param name="filter"></param>
        public static DeleteResult RemoveAll<T>(string collectionName, Expression<Func<T, bool>> filter) where T : class, new()
        {
            var db = GetDatabase();
            var collection = db.GetCollection<T>(collectionName);
            return collection.DeleteMany(filter);
        }

        /// <summary>
        /// 删除所有
        /// </summary>
        /// <param name="collectionName">集合名称</param>
        /// <param name="filter"></param>
        public static async Task<DeleteResult> RemoveAllAsync<T>(string collectionName, Expression<Func<T, bool>> filter) where T : class, new()
        {
            var db = GetDatabase();
            var collection = db.GetCollection<T>(collectionName);
            return await collection.DeleteManyAsync(filter);
        }

        /// <summary>
        /// 获取单个（根据条件）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName">集合名称</param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static T FirstOrDefault<T>(string collectionName, Expression<Func<T, bool>> filter) where T : class, new()
        {
            var db = GetDatabase();
            var collection = db.GetCollection<T>(collectionName);
            return collection.Find(filter).FirstOrDefault();
        }

        /// <summary>
        /// 获取多个
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName">集合名称</param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static List<T> FindBy<T>(string collectionName, Expression<Func<T, bool>> filter) where T : class, new()
        {
            var db = GetDatabase();
            var collection = db.GetCollection<T>(collectionName);
            return collection.Find(filter).ToList();
        }

        /// <summary>
        /// 获取多个
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName">集合名称</param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static async Task<List<T>> FindByAsync<T>(string collectionName, Expression<Func<T, bool>> filter) where T : class, new()
        {
            var db = GetDatabase();
            var collection = db.GetCollection<T>(collectionName);
            var grid = await collection.Find(filter).ToListAsync();
            return grid;
        }
    }
}
