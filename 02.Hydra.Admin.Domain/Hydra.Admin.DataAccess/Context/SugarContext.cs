using SqlSugar;
using System;
using System.Linq;
using Newtonsoft.Json;
using Hydra.Admin.Utility;
using System.Collections.Generic;
using Hydra.Admin.Utility.Helper;

namespace Hydra.Admin.DataAccess
{
    public class SugarContext<T> where T : class, new()
    {

        /// <summary>
        /// 返回 bool 
        /// </summary>
        /// <param name="function"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        protected static bool DbFunction(Func<SqlSugarClient, bool> function)
        {
            try
            {
                using (var db = GetInstance())
                {
                    return function(db);
                }
            }
            catch (Exception ex)
            {
                Log.Error("DBContext.DbFunction:", ex);
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        /// <summary>
        /// 返回 T
        /// </summary>
        /// <param name="function"></param>
        /// <returns></returns>
        protected static T DbFunction(Func<SqlSugarClient, T> function)
        {
            try
            {
                using (var db = GetInstance())
                {
                    return function(db);
                }
            }
            catch (Exception ex)
            {
                Log.Error("DBContext.DbFunction:", ex);
                return default(T);
            }
        }
        /// <summary>
        /// 返回 AjaxResult
        /// </summary>
        /// <param name="function"></param>
        /// <returns></returns>
        protected static AjaxResult DbFunction(Func<SqlSugarClient, AjaxResult> function)
        {
            try
            {
                using (var db = GetInstance())
                {
                    return function(db);
                }
            }
            catch (Exception ex)
            {
                Log.Error("DBContext.DbFunction:", ex);
                return new AjaxResult
                {
                    message = ex.Message
                };
            }
        }
        /// <summary>
        /// 返回  int
        /// </summary>
        /// <param name="function"></param>
        /// <returns></returns>
        protected static int DbFunction(Func<SqlSugarClient, int> function)
        {
            try
            {
                using (var db = GetInstance())
                {
                    return function(db);
                }
            }
            catch (Exception ex)
            {
                Log.Error("DBContext.DbFunction:", ex);
                return 0;
            }
        }
        /// <summary>
        /// 返回 List<T>
        /// </summary>
        /// <param name="function"></param>
        /// <returns></returns>
        protected static List<T> DbFunction(Func<SqlSugarClient, List<T>> function)
        {
            try
            {
                using (var db = GetInstance())
                {
                    return function(db);
                }
            }
            catch (Exception ex)
            {
                Log.Error("DBContext.DbFunction:", ex);
                return default(List<T>);
            }
        }
        /// <summary>
        /// 返回 dynamic
        /// </summary>
        /// <param name="func"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        protected static dynamic DbFunction(Func<SqlSugarClient, dynamic> func, dynamic def = null)
        {
            try
            {
                using (var context = GetInstance())
                {
                    var result = func(context);
                    return result;
                }
            }
            catch (Exception ex)
            {
                Log.Error("DBContext.DbFunction:" , ex);
                return def ?? default(dynamic);
            }
        }
        /// <summary>
        /// 无返回值
        /// </summary>
        /// <param name="action"></param>
        /// <param name="type"></param>
        public static void DbAction(Action<SqlSugarClient> action)
        {
            try
            {
                using (var db = GetInstance())
                {
                    action(db);
                }
            }
            catch (Exception ex)
            {
                Log.Error("DBContext.DbAction:" ,ex);
            }
        }
        public static SqlSugarClient GetInstance(DbType type = DbType.MySql)
        {
            if (string.IsNullOrEmpty(DbConfig.MasterDB)) throw new Exception("未配置数据库链接,请检查....");
            SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = DbConfig.MasterDB,
                DbType = type,
                IsAutoCloseConnection = true
            });
            //#if Debug
            db.Ado.IsEnableLogEvent = true;
            db.Ado.LogEventStarting = (sql, pars) =>
            {
                PrintSql(sql, JsonConvert.SerializeObject(pars.ToDictionary(s => s.ParameterName, s => s.Value)));
            };
            //#endif
            return db;
        }
        /// <summary>
        /// 打印Sql
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pars"></param>
        private static void PrintSql(string sql, string pars)
        {
            Log.Debug("执行 SQL 语句 如下:" + sql);
            if (pars != null)
            {
                Log.Debug(" 提交参数 :" + pars);
            }
        }
    }
}
