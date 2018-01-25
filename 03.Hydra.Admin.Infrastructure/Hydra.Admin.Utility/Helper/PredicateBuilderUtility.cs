﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Hydra.Admin.Utility.Helper
{
    public static class PredicateBuilderUtility
    {
        public static Expression<Func<T, bool>> True<T>()
        {
            return f => true;
        }

        public static Expression<Func<T, T1, bool>> True<T, T1>()
        {
            return (t1, t2) => true;
        }
        public static Expression<Func<T, T1, T2, bool>> True<T, T1, T2>()
        {
            return (t1, t2, t3) => true;
        }

        public static Expression<Func<T, bool>> False<T>()
        {
            return f => false;
        }

        public static Expression<Func<T, T1, T2, bool>> False<T, T1, T2>()
        {
            return (t1, t2,t3) => false;
        }

        public static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second,
            Func<Expression, Expression, Expression> merge)
        {
            // build parameter map (from parameters of second to parameters of first)
            var map = first.Parameters.Select((f, i) => new {f, s = second.Parameters[i]})
                .ToDictionary(p => p.s, p => p.f);

            // replace parameters in the second lambda expression with parameters from the first
            var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);

            // apply composition of lambda expression bodies to parameters from the first expression 
            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }
        #region AND
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first,
            Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.And);
        }

        public static Expression<Func<T, T1, bool>> And<T, T1>(this Expression<Func<T, T1, bool>> first,Expression<Func<T, T1, bool>> second)
        {
            return first.Compose(second, Expression.And);
        }
        public static Expression<Func<T, T1,T2, bool>> And<T, T1, T2>(this Expression<Func<T, T1, T2, bool>> first, Expression<Func<T, T1, T2, bool>> second)
        {
            return first.Compose(second, Expression.And);
        }
        #endregion

        #region OR
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.Or);
        }
        public static Expression<Func<T, T1, bool>> Or<T, T1>(this Expression<Func<T, T1, bool>> first, Expression<Func<T, T1, bool>> second)
        {
            return first.Compose(second, Expression.Or);
        }
        public static Expression<Func<T, T1, T2, bool>> Or<T, T1, T2>(this Expression<Func<T, T1, T2, bool>> first, Expression<Func<T, T1, T2, bool>> second)
        {
            return first.Compose(second, Expression.Or);
        }
        #endregion 

        public class ParameterRebinder : ExpressionVisitor
        {
            private readonly Dictionary<ParameterExpression, ParameterExpression> map;

            public ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
            {
                this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
            }

            public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map,
                Expression exp)
            {
                return new ParameterRebinder(map).Visit(exp);
            }

            protected override Expression VisitParameter(ParameterExpression p)
            {
                ParameterExpression replacement;
                if (map.TryGetValue(p, out replacement))
                {
                    p = replacement;
                }
                return base.VisitParameter(p);
            }
        }

    }
}