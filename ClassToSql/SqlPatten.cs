using ClassToSql.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ClassToSql
{
    public class SelectPattenItem
    {
        public string SelStr { get; set; }

        public string AsStr { get; set; }

        public override string ToString()
        {
            return "["+SelStr + "] as " + AsStr;
        }

    }

    public class WherePattenItem
    {
        public WhereJoinType OutJoinType { get; set; }

        public string WhereName { get; set; }

        public WhereValueType WhereType { get; set; }

        public List<string> WhereValue { get; set; }

        public List<WherePattenItem> WherePattenItems { get; set; }

        
    }

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
            foreach(var item in orderByNames)
            {
                _orderByPattens.Add(new OrderByPattenItem(item, orderBy));
            }
            return this;
        }

        public override string ToString()
        {
            if (_orderByPattens.Count == 0)
            {
                return "  ";
            }
            else
            {
                List<string> orderStrs = new List<string>();
                foreach(var item in _orderByPattens)
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

    public class SqlPatten<T>
    {
        /// <summary>
        /// 选择字段
        /// </summary>
        private List<SelectPattenItem> _selectpattens { get; set; } = new List<SelectPattenItem>();

        /// <summary>
        /// 
        /// </summary>
        public List<string> SelectPattens
        {
            get
            {
                return _selectpattens.Select(s => s.SelStr).ToList();
            }
        }

        public List<OrderByPattenItem> _orderByPattens { get; set; } = new List<OrderByPattenItem>();


        /// <summary>
        /// 聚合函数条件
        /// </summary>
        public List<string> _groupByPattens { get; set; } = new List<string>();

        /// <summary>
        /// 是否去除重复项
        /// </summary>
        private bool _disinist { get; set; } = false;

        /// <summary>
        /// 筛选条件字段集合
        /// </summary>
        public List<WherePattenItem> _wherePattens { get; set; } = new List<WherePattenItem>();

        /// <summary>
        /// 当前正在编写的筛选条件字段集合
        /// </summary>
        private WherePattenItem _currentWherePatten;  //可以使用linq表达式添加最好

       //可以使用wherestr只有无名字的函数，后面加subwherestr

        public SqlPatten<T> AddTheWhere()
        {


            return this;
        }

        public SqlPatten<T> AddTheSubWheres()
        {


            return this;
        }

        /// <summary>
        /// 添加普通选择字段
        /// </summary>
        /// <param name="selName"></param>
        /// <param name="asName"></param>
        /// <returns></returns>
       public SqlPatten<T> SqlSelect(string selName,string asName=null)
        {
            SelectPattenItem item = new SelectPattenItem
            {
                AsStr = asName == null ? selName : asName,
                SelStr = selName,
            };

            CheckToAddSel(item);
            return this;
        }

        /// <summary>
        /// 添加计数选择字段
        /// </summary>
        /// <param name="countName"></param>
        /// <param name="asName"></param>
        /// <returns></returns>
        public SqlPatten<T> SqlCountSelect(string countName,string asName)
        {
            SelectPattenItem item = new SelectPattenItem
            {
                AsStr = asName == null ? countName : asName,
                SelStr = " count(" + countName + ") as " + asName,
            };

            CheckToAddSel(item);
            return this;
        }

        /// <summary>
        /// 添加求和选择字段
        /// </summary>
        /// <param name="sumName"></param>
        /// <param name="asName"></param>
        /// <returns></returns>
        public SqlPatten<T> SqlSumSelect(string sumName,string asName)
        {
            SelectPattenItem item = new SelectPattenItem
            {
                AsStr = asName == null ? sumName : asName,
                SelStr = " sum(" + sumName + ") as " + asName,
            };

            CheckToAddSel(item);
            return this;
        }

        public SqlPatten<T> SqlGetRowNoSelect(string rowName,Expression<Func<OrderByPattens, OrderByPattens>> orderByExp,
            string asName)
        {
            var func = orderByExp.Compile();
            var orderByPattens = func(new OrderByPattens());

            if (orderByPattens._orderByPattens.Count == 0)
            {
                throw new Exception("表集合" + typeof(T).Name + "排序列" + rowName + "必须定义排序");
            }

            var orderByStr = orderByPattens.ToString();

            SelectPattenItem item = new SelectPattenItem
            {
                AsStr = asName == null ? rowName : asName,
                SelStr = " ROW_NUMBER() over("+ orderByStr + ") as " + asName,
            };

            CheckToAddSel(item);
            return this;
        }


        private void CheckToAddSel(SelectPattenItem item)
        {
            var HasAsList = _selectpattens.Select(s => s.AsStr).ToList();
            if (HasAsList.Contains(item.AsStr))
            {
                throw new Exception("表集合" + typeof(T).Name + "已存在选择字段" + item.AsStr);
            }
            else
            {
                _selectpattens.Add(item);
            }
        }
    }
}
