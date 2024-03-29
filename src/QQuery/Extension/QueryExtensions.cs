﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace QQuery.Repo
{
    public static class QueryExtensions
    {
        public static IQueryable<T> OrAll<T>(this IQueryable<T> query, IEnumerable<Expression<Func<T, bool>>> predicates)
        {
            if (!predicates.Any())
                return query;

            return query.Where(predicates.Aggregate((acc, next) => acc.Or(next)));
        }

        public static IQueryable<T> AndAll<T>(this IQueryable<T> query, IEnumerable<Expression<Func<T, bool>>> predicates)
        {
            if (!predicates.Any())
                return query;

            return query.Where(predicates.Aggregate((acc, next) => acc.And(next)));
        }

        private static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> left,
           Expression<Func<T, bool>> right)
        {
            return CombineLambdas(left, right, ExpressionType.AndAlso);
        }

        private static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        {
            return CombineLambdas(left, right, ExpressionType.OrElse);
        }

        private static Expression<Func<T, bool>> CombineLambdas<T>(this Expression<Func<T, bool>> left,
                  Expression<Func<T, bool>> right, ExpressionType expressionType)
        {
            //Remove expressions created with Begin<T>()
            if (IsExpressionBodyConstant(left))
                return (right);

            ParameterExpression p = left.Parameters[0];

            SubstituteParameterVisitor visitor = new SubstituteParameterVisitor();
            visitor.Sub[right.Parameters[0]] = p;

            Expression body = Expression.MakeBinary(expressionType, left.Body, visitor.Visit(right.Body));
            return Expression.Lambda<Func<T, bool>>(body, p);
        }

        private static bool IsExpressionBodyConstant<T>(Expression<Func<T, bool>> left)
        {
            return left.Body.NodeType == ExpressionType.Constant;
        }
    }

    internal class SubstituteParameterVisitor : ExpressionVisitor
    {
        public Dictionary<Expression, Expression> Sub = new Dictionary<Expression, Expression>();

        protected override Expression VisitParameter(ParameterExpression node)
        {
            Expression newValue;
            if (Sub.TryGetValue(node, out newValue))
            {
                return newValue;
            }
            return node;
        }
    }
}