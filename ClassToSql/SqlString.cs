using ClassToSql.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ClassToSql
{
    /// <summary>
    /// sql排序集合
    /// </summary>
    public class OrderByPattens
    {
        public List<OrderByPattenItem> _orderByPattens { get; set; } = new List<OrderByPattenItem>();

        public OrderByPattens Add(string orderByName, OrderByType orderBy)
        {
            _orderByPattens.Add(new OrderByPattenItem(orderByName, orderBy));
            return this;
        }

        public OrderByPattens AddRanges(List<string> orderByNames, OrderByType orderBy)
        {
            foreach (var item in orderByNames)
            {
                _orderByPattens.Add(new OrderByPattenItem(item, orderBy));
            }
            return this;
        }

        public string ToSqlString()
        {
            if (_orderByPattens.Count == 0)
            {
                return "  ";
            }
            else
            {
                List<string> orderStrs = new List<string>();
                foreach (var item in _orderByPattens)
                {
                    var orderBystr = item.OrderBy == OrderByType.Asc ? " asc " : 
                        item.OrderBy == OrderByType.Desc ? " desc " : "";
                    orderStrs.Add(item.OrderByName + " " + orderBystr);
                }

                return " order by " + string.Join(",", orderStrs);
            }
        }
    }

    /// <summary>
    /// sql单个排序
    /// </summary>
    public class OrderByPattenItem
    {
        public OrderByPattenItem(string orderByName, OrderByType orderBy)
        {
            this.OrderByName = orderByName;
            OrderBy = orderBy;
        }

        public string OrderByName { get; set; }

        public OrderByType OrderBy { get; set; }
    }



    public static class SqlString
    {
        /// <summary>
        /// 单表查询生成SQL
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="it"></param>
        /// <returns></returns>
        public static string ToSqlString<T>(SqlPatten<T> it)
        {
            return it.ToSqlString();
        }

        /// <summary>
        /// 单表查询增加排序号字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="it"></param>
        /// <param name="orderByExp"></param>
        /// <returns></returns>
        public static string ToSqlString<T>(SqlPatten<T> it, Expression<Func<OrderByPattens, OrderByPattens>> orderByExp)
        {
            var method = orderByExp.Compile();
            OrderByPattens orderByPattens =method.Invoke(new OrderByPattens());
            CheckOrderby(orderByPattens, it.SelectAliaPattens, "单表查询增加自定义排序查询排序字段无效");
            return it.ToSqlString() + orderByPattens.ToSqlString();
        }

        /// <summary>
        /// 单表查询增加自定义分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="it"></param>
        /// <param name="orderByExp"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public static string ToSqlString<T>(SqlPatten<T> it, Expression<Func<OrderByPattens, OrderByPattens>> orderByExp,
            int PageIndex,int PageSize)
        {
            var method = orderByExp.Compile();
            OrderByPattens orderByPattens = method.Invoke(new OrderByPattens());
            CheckOrderby(orderByPattens, it.SelectAliaPattens, "单表查询增加自定义分页查询排序字段无效");

            var orderSql= orderByPattens.ToSqlString();
            var whereSql = string.Format(" _RowNum>{0} and _RowNum<={1} ", (PageIndex - 1) * PageSize, PageIndex * PageSize);

            var selectSql = string.Join(",", it.SelectAliaPattens);
            var sql = "select * from ( select "+ selectSql + ", ROW_NUMBER() over({0}) as _RowNum from ({1}) as table_join_base ) as table_join_new where {2}";
            return string.Format(sql, orderSql, it.ToSqlString(), whereSql);

        }

        /// <summary>
        /// 联表查询生成sql
        /// </summary>
        /// <param name="joinedTable"></param>
        /// <returns></returns>
        public static string ToSqlString(JoinedTable joinedTable)
        {
            var sql = "select * from ({0}) as join_table_base";
            return string.Format(sql, joinedTable.sqlString);
        }

        /// <summary>
        /// 联表查询并使用自定义排序生成sql
        /// </summary>
        /// <param name="it"></param>
        /// <param name="orderByExp"></param>
        /// <returns></returns>
        public static string ToSqlString(JoinedTable it, Expression<Func<OrderByPattens, OrderByPattens>> orderByExp)
        {
            var method = orderByExp.Compile();
            OrderByPattens orderByPattens = method.Invoke(new OrderByPattens());
            CheckOrderby(orderByPattens, it.SelectList, "联表查询使用自定义排序的查询排序字段无效");

            return it.sqlString + orderByPattens.ToSqlString();
        }

        /// <summary>
        /// 联表查询使用自定义排序的分页查询
        /// </summary>
        /// <param name="it"></param>
        /// <param name="orderByExp"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public static string ToSqlString(JoinedTable it, Expression<Func<OrderByPattens, OrderByPattens>> orderByExp,
            int PageIndex, int PageSize)
        {
            var method = orderByExp.Compile();
            OrderByPattens orderByPattens = method.Invoke(new OrderByPattens());
            CheckOrderby(orderByPattens, it.SelectList, "联表查询使用自定义排序的分页查询排序字段无效");

            var orderSql = orderByPattens.ToSqlString();
            var whereSql = string.Format(" _RowNum>{0} and _RowNum<={1} ", (PageIndex - 1) * PageSize, PageIndex * PageSize);

            var selectSql = string.Join(",", it.SelectList);
            var sql = "select * from ( select "+ selectSql + ", ROW_NUMBER() over({0}) as _RowNum from ({1}) as table_join_base ) as table_join_new where {2}";
            return string.Format(sql, orderSql, it.sqlString, whereSql);
        }

        /// <summary>
        /// 验证最终排序中是否存在无效字段
        /// </summary>
        private static void CheckOrderby(OrderByPattens orderByPattens,List<string> SelList,string ErrorMessage)
        {
            var OrderList = orderByPattens._orderByPattens.Select(s => s.OrderByName);
            foreach(var item in OrderList)
            {
                if (!SelList.Contains(item))
                {
                    throw new Exception(ErrorMessage,new Exception("出现了无效的排序字段："+ item));
                }
            }

        }

    }

}
