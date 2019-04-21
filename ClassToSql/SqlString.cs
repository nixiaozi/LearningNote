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
        public static string ToSqlString<T>(SqlPatten<T> it)
        {
            return it.ToSqlString();
        }

        public static string ToSqlString<T>(SqlPatten<T> it, Expression<Func<OrderByPattens, OrderByPattens>> orderByExp)
        {
            var method = orderByExp.Compile();
            OrderByPattens orderByPattens =method.Invoke(new OrderByPattens());
            return it.ToSqlString() + orderByPattens.ToSqlString();
        }

        public static string ToSqlString<T>(SqlPatten<T> it, Expression<Func<OrderByPattens, OrderByPattens>> orderByExp,
            int PageIndex,int PageSize)
        {
            var method = orderByExp.Compile();
            OrderByPattens orderByPattens = method.Invoke(new OrderByPattens());
            var orderSql= orderByPattens.ToSqlString();
            var whereSql = string.Format(" _RowNum>{0} and _RowNum<={1} ", (PageIndex - 1) * PageSize, PageIndex * PageSize);

            var sql = "select * ROW_NUMBER() over({0}) as _RowNum from ({1}) where (2)";
            return string.Format(sql, orderSql, it.ToSqlString(), whereSql);

        }

        public static string ToSqlString(JoinedTable joinedTable)
        {
            var sql = "select * from ({0}) as join_table_base";
            return string.Format(sql, joinedTable.sqlString);
        }

        public static string ToSqlString(JoinedTable it, Expression<Func<OrderByPattens, OrderByPattens>> orderByExp)
        {
            var method = orderByExp.Compile();
            OrderByPattens orderByPattens = method.Invoke(new OrderByPattens());
            return it.sqlString + orderByPattens.ToSqlString();
        }

        public static string ToSqlString(JoinedTable it, Expression<Func<OrderByPattens, OrderByPattens>> orderByExp,
            int PageIndex, int PageSize)
        {
            var method = orderByExp.Compile();
            OrderByPattens orderByPattens = method.Invoke(new OrderByPattens());
            var orderSql = orderByPattens.ToSqlString();
            var whereSql = string.Format(" _RowNum>{0} and _RowNum<={1} ", (PageIndex - 1) * PageSize, PageIndex * PageSize);

            var sql = "select * ROW_NUMBER() over({0}) as _RowNum from ({1}) where (2)";
            return string.Format(sql, orderSql, it.sqlString, whereSql);
        }

    }

}
