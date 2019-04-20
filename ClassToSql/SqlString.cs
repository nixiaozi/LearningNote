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
                    orderStrs.Add(item.OrderByName + " " + item.OrderBy.GetDisplayName());
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


        
    }

}
