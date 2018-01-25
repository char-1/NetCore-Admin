using Hydra.Admin.Models.Query;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Hydra.Admin.IServices
{
    public interface IBaseService<T> where T : class, new()
    {
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="model">对象</param>
        /// <returns></returns>
        bool Delete(T model);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="predi">条件表达式</param>
        /// <returns></returns>
        bool Delete(Expression<Func<T, bool>> predi);
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">对象</param>
        /// <returns></returns>
        bool Insert(T model);
        /// <summary>
        /// 新增[批量]
        /// </summary>
        /// <param name="model">对象</param>
        /// <returns></returns>
        bool BatchInsert(List<T> model);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">对象</param>
        /// <returns></returns>
        bool Update(T model);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">对象</param>
        /// <param name="where">条件</param>
        /// <returns></returns>
        bool Update(T model, Expression<Func<T, bool>> where);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="columns">字段</param>
        /// <param name="where">条件</param>
        /// <returns></returns>
        bool Update(Expression<Func<T, T>> columns, Expression<Func<T, bool>> where);
        /// <summary>
        /// 获取单条信息
        /// </summary>
        /// <param name="where">条件</param>
        /// <returns></returns>
        T FirstOrDefault(Expression<Func<T, bool>> where);
        /// <summary>
        /// 获取单条信息
        /// </summary>
        /// <param name="pkValue">主键值</param>
        /// <returns></returns>
        T FirstOrDefault(object pkValue);
        /// <summary>
        /// 获取条数
        /// </summary>
        /// <param name="where">条件</param>
        /// <returns></returns>
        int Count(Expression<Func<T, bool>> where);

    }
}
