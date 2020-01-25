using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using vega.Core.Models;

namespace vega.Extensions
{
    public static class IQueryableExtensions
    {
         public static IQueryable<T> ApplyOrdering<T>(this IQueryable<T> query, IQueryObject queryObj, Dictionary<string, Expression<Func<T, object>>> columnsMap){
            if( String.IsNullOrWhiteSpace(queryObj.SortBy) || !columnsMap.ContainsKey(queryObj.SortBy))
                return query;
             if(queryObj.IsSortAscending)
               return query = query.OrderBy(columnsMap[queryObj.SortBy]);
            else
               return query = query.OrderByDescending(columnsMap[queryObj.SortBy]);
        }
        
        public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, IQueryObject queryObject){
            if(queryObject.PageSize <= 0)
                queryObject.PageSize = 10;

            if(queryObject.Page <= 0)
                queryObject.Page = 1;
            
            return query.Skip((queryObject.Page - 1)* queryObject.PageSize).Take(queryObject.PageSize);
        }
    }

    
}