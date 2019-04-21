using ClassToSql.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ClassToSql
{
    
    public class SqlPatten<T>
    {
        /// <summary>
        /// 选择字段
        /// </summary>
        private List<SelectPattenItem> _selectpattens { get; set; } = new List<SelectPattenItem>();

        /// <summary>
        /// 所有选择的列详细
        /// </summary>
        public List<string> SelectKeyPattens
        {
            get
            {
                return _selectpattens.Select(s => s.SelKey).Where(s=>s!=null).ToList();
            }
        }


        public List<string> SelectAliaPattens
        {
            get
            {
                return _selectpattens.Select(s => s.AsStr).ToList();
            }
        }


        /// <summary>
        /// 聚合函数条件
        /// </summary>
        public List<string> _groupByPattens { get; set; } = new List<string>();

        /// <summary>
        /// 是否去除重复项
        /// </summary>
        private bool _disinist { get; set; } = false;

        /// <summary>
        /// 选择前几项
        /// </summary>
        private int _top { get; set; } = 0;

        /// <summary>
        /// 筛选条件字段集合
        /// </summary>
        public List<WherePattenItem> _wherePattens { get; set; } = new List<WherePattenItem>();

        /// <summary>
        /// 当前正在编写的筛选条件字段集合
        /// </summary>
        private WherePattenItem _currentWherePatten;  //可以使用linq表达式添加最好

        private WhereJoinType _currentWhereJoin = WhereJoinType.And;

        //可以使用wherestr只有无名字的函数，后面加subwherestr

        #region WhereMatch筛选器
        /// <summary>
        /// 添加where条件筛选
        /// </summary>
        /// <param name="whereName">筛选名字</param>
        /// <param name="value">筛选值</param>
        /// <param name="valueType">比较操作</param>
        /// <param name="joinType">外联字符</param>
        /// <returns></returns>
        public SqlPatten<T> AddWhere(string whereName, string value, WhereValueType valueType, WhereJoinType? joinType=null)
        {
            _currentWherePatten = new WherePattenItem
            {
                OutJoinType = joinType==null?_currentWhereJoin: joinType.Value,
                WhereName = whereName,
                WhereType = valueType,
                WhereValue = "'" + value + "'",
            };
            _wherePattens.Add(_currentWherePatten);
            return this;
        }

        public SqlPatten<T> WhereIn(string whereName, List<string> value)
        {
            return AddWhere(whereName, string.Join("','", value), WhereValueType.WithInList);
        }

        public SqlPatten<T> WhereLeftLike(string whereName, string value)
        {
            return AddWhere(whereName, value + "%", WhereValueType.MatchLike);
        }

        public SqlPatten<T> WhereRightLike(string whereName, string value)
        {
            return AddWhere(whereName, "%" + value, WhereValueType.MatchLike);
        }

        public SqlPatten<T> WhereMidLike(string whereName, string value)
        {
            return AddWhere(whereName, "%" + value + "%", WhereValueType.MatchLike);
        }

        public SqlPatten<T> WherePreciseLike(string whereName, string value)
        {
            return AddWhere(whereName, value, WhereValueType.MatchLike);
        }

        public SqlPatten<T> WhereEqual(string whereName, string value)
        {
            return AddWhere(whereName,value, WhereValueType.Equal);
        }

        public SqlPatten<T> WhereEqual(string whereName, int value)
        {
            return WhereEqual(whereName, value.ToString());
        }

        public SqlPatten<T> WhereEqual(string whereName, Guid value)
        {
            return WhereEqual(whereName, value.ToString());
        }


        public SqlPatten<T> WhereEqual(string whereName, decimal value)
        {
            return WhereEqual(whereName, value.ToString("0.##"));
        }

        /// <summary>
        /// 添加并大于
        /// </summary>
        /// <param name="whereName">where字段</param>
        /// <param name="value">比较的值</param>
        /// <param name="hasEqual">是否可等于</param>
        /// <returns></returns>
        public SqlPatten<T> WhereBig(string whereName, string value,bool hasEqual=true)
        {

            return AddWhere(whereName, value,
                 hasEqual ? WhereValueType.BigEqualThen: WhereValueType.BigThen);
        }

        public SqlPatten<T> WhereBig(string whereName, int value, bool hasEqual = true)
        {
            return WhereBig(whereName,value.ToString(),hasEqual);
        }

        public SqlPatten<T> WhereBig(string whereName, decimal value, bool hasEqual = true)
        {
            return WhereBig(whereName, value.ToString("0.##"), hasEqual);
        }

        public SqlPatten<T> WhereBig(string whereName, Guid value, bool hasEqual = true)
        {
            return WhereBig(whereName, value.ToString(), hasEqual);
        }

        public SqlPatten<T> WhereSmall(string whereName, string value,bool hasEqual = true)
        {
            return AddWhere(whereName, value,
                 hasEqual ? WhereValueType.SmallEqualThen : WhereValueType.SmallThen);
        }

        public SqlPatten<T> WhereSmall(string whereName, int value, bool hasEqual = true)
        {
            return WhereSmall(whereName, value.ToString(),hasEqual);
        }

        public SqlPatten<T> WhereSmall(string whereName, decimal value, bool hasEqual = true)
        {
            return WhereSmall(whereName, value.ToString("0.##"), hasEqual);
        }


        public SqlPatten<T> WhereIn(string whereName, List<int> thes)
        {
            List<string> values = thes.ConvertAll<string>(delegate (int i) { return i.ToString(); });
            return WhereIn(whereName, values);
        }

        public SqlPatten<T> WhereIn(string whereName, List<decimal> thes)
        {
            List<string> values = thes.ConvertAll<string>(delegate (decimal i) { return i.ToString(); });
            return WhereIn(whereName, values);
        }

        public SqlPatten<T> WhereIn(string whereName, List<Guid> thes)
        {
            List<string> values = thes.ConvertAll<string>(delegate (Guid i) { return i.ToString(); });
            return WhereIn(whereName, values);
        }

        public SqlPatten<T> WhereNotNull(string whereName)
        {
            return AddWhere(whereName, null, WhereValueType.NotNull);
        }

        public SqlPatten<T> WhereOnlyNull(string whereName)
        {
            return AddWhere(whereName, null, WhereValueType.OnlyNull);
        }


        #endregion



        public SqlPatten<T> AddTheSubWheres(Expression<Func<WherePattenItem, WherePattenItem>> AddWhereExp)
        {
            var method = AddWhereExp.Compile();
            _currentWherePatten = new WherePattenItem();
            _currentWherePatten.OutJoinType = _currentWhereJoin;
            _currentWherePatten.WherePattenItems = method(new WherePattenItem()).WherePattenItems;
            _wherePattens.Add(_currentWherePatten);
            return this;
        }

        /// <summary>
        /// 添加普通选择字段
        /// </summary>
        /// <param name="selName"></param>
        /// <param name="asName"></param>
        /// <returns></returns>
       public SqlPatten<T> AddSelect(string selName,string asName=null)
        {
            SelectPattenItem item = new SelectPattenItem
            {
                SelKey=selName,
                TheSelectType= SelectType.Directly,
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
        public SqlPatten<T> AddCountSelect(string countName,string asName)
        {
            SelectPattenItem item = new SelectPattenItem
            {
                AsStr = asName == null ? countName : asName,
                SelStr = countName,
                TheSelectType= SelectType.Count,
                SelKey=countName,
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
        public SqlPatten<T> AddSumSelect(string sumName,string asName)
        {
            SelectPattenItem item = new SelectPattenItem
            {
                AsStr = asName == null ? sumName : asName,
                SelStr = sumName,
                TheSelectType= SelectType.Sum,
                SelKey=sumName,
            };

            CheckToAddSel(item);
            return this;
        }

        /// <summary>
        /// 添加自定义排序选择字段
        /// </summary>
        /// <param name="rowName"></param>
        /// <param name="orderByExp"></param>
        /// <param name="asName"></param>
        /// <returns></returns>
        public SqlPatten<T> AddGetRowNoSelect(string rowName,Expression<Func<OrderByPattens, OrderByPattens>> orderByExp,
            string asName)
        {
            var func = orderByExp.Compile();
            var orderByPattens = func(new OrderByPattens());

            if (orderByPattens._orderByPattens.Count == 0)
            {
                throw new Exception("表集合" + typeof(T).Name + "排序列" + rowName + "必须定义排序");
            }

            var orderByStr = orderByPattens.ToSqlString();

            SelectPattenItem item = new SelectPattenItem
            {
                AsStr = asName == null ? rowName : asName,
                SelStr = orderByStr,
                TheSelectType= SelectType.Row,
                SelKey= null,
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


        public SqlPatten<T> AddGroupBy(string groupName)
        {
            _groupByPattens.Add(groupName);
            return this;
        }

        public SqlPatten<T> AddGroupBy(List<string> groupNames)
        {
            _groupByPattens.AddRange(groupNames);
            return this;
        }


        public string ToSqlString()
        {
            //首先需要对select和groupby 进行验证

            //然后拼接字符串
            var selectStr = " select ";
            var distinct = _disinist ? " distinct " : "";
            var topstr = _top == 0 ? "" : " top " + _top+" ";
            selectStr += (distinct + topstr);
            if (_selectpattens.Count == 0)
            {
                selectStr += " * ";
            }
            else
            {
                selectStr += string.Join(",", _selectpattens.Select(s => s.ToSqlString()));
            }

            var fromStr = " from [" + typeof(T).Name + "] ";

            var whereStr =" where 1=1 ";
            foreach(var item in _wherePattens)
            {
                whereStr+=item.ToSqlString();
            }

            var groupStr = "";
            if (_groupByPattens.Count > 0)
            {
                groupStr = " group by ";
                groupStr += string.Join(",", _groupByPattens);
            }
            

            return selectStr + fromStr + whereStr + groupStr;
        }

    }
}
