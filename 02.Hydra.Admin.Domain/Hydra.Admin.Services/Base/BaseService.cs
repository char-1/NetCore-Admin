using Hydra.Admin.DataAccess;
using Hydra.Admin.IServices;
using Hydra.Admin.Models.Query;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Hydra.Admin.Services
{
    public abstract class BaseService<T> : SugarContext<T>, IBaseService<T> where T : class, new()
    {
        public int total = 0;


        public bool Delete(T model)
        {
            return DbFunction((db) =>
            {
                return db.Deleteable(model).ExecuteCommand() > 0;
            });
        }

        public bool Delete(Expression<Func<T, bool>> predi)
        {
            return DbFunction((db) =>
            {
                return db.Deleteable<T>().Where(predi).ExecuteCommand() > 0;
            });
        }

        public bool Insert(T model)
        {
            return DbFunction((db) =>
            {
                return db.Insertable(model).ExecuteCommand() > 0;
            });
        }
        public bool BatchInsert(List<T> model)
        {
            return DbFunction((db) =>
            {

                return db.Insertable(model).ExecuteCommand() > 0;
            });
        }
        public bool Update(T model)
        {
            return DbFunction((db) =>
            {
                return db.Updateable(model).ExecuteCommand() > 0;
            });
        }

        public bool Update(T model, Expression<Func<T, bool>> where)
        {
            return DbFunction((db) =>
            {
                return db.Updateable(model).Where(where).ExecuteCommand() > 0;
            });
        }

        public bool Update(Expression<Func<T, T>> columns, Expression<Func<T, bool>> where)
        {
            return DbFunction((db) =>
            {
                return db.Updateable<T>()
                .UpdateColumns(columns)
                .Where(where).ExecuteCommand() > 0;
            });
        }

        public T FirstOrDefault(Expression<Func<T, bool>> where)
        {
            return DbFunction((db) =>
            {
                return db.Queryable<T>().First(where);
            });
        }
        public T FirstOrDefault(object pkValue)
        {
            return DbFunction((db) =>
            {
                return db.Queryable<T>().InSingle(pkValue);
            });
        }

        public int Count(Expression<Func<T, bool>> where)
        {
            return DbFunction((db) =>
            {
                return db.Queryable<T>().Count(where);
            });
        }


    }
}
