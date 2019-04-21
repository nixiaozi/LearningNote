using ClassToSql.Enums;
using System;
using System.Collections.Generic;

namespace ClassToSql
{
    public class WherePattenItem
    {
        public WhereJoinType OutJoinType { get; set; }

        public string WhereName { get; set; }

        public WhereValueType WhereType { get; set; }

        public string WhereValue { get; set; }

        public List<WherePattenItem> WherePattenItems { get; set; } = new List<WherePattenItem>();

        public WhereJoinType _currentJoinType { get; set; } = WhereJoinType.And;

        #region SubWhereMatch添加器
        /// <summary>
        /// 添加where条件筛选
        /// </summary>
        /// <param name="whereName">筛选名字</param>
        /// <param name="value">筛选值</param>
        /// <param name="valueType">比较操作</param>
        /// <param name="joinType">外联字符</param>
        /// <returns></returns>
        public WherePattenItem AddWhere(string whereName, string value, WhereValueType valueType, WhereJoinType? joinType = null)
        {
            var _currentWherePatten = new WherePattenItem
            {
                OutJoinType = joinType == null ? _currentJoinType : joinType.Value,
                WhereName = whereName,
                WhereType = valueType,
                WhereValue = "'" + value + "'",
            };
            WherePattenItems.Add(_currentWherePatten);
            return this;
        }

        public WherePattenItem WhereIn(string whereName, List<string> value)
        {
            return AddWhere(whereName, string.Join("','", value), WhereValueType.WithInList);
        }

        public WherePattenItem WhereLeftLike(string whereName, string value, WhereJoinType joinType)
        {
            return AddWhere(whereName, value + "%", WhereValueType.MatchLike);
        }

        public WherePattenItem WhereRightLike(string whereName, string value, WhereJoinType joinType)
        {
            return AddWhere(whereName, "%" + value, WhereValueType.MatchLike);
        }

        public WherePattenItem WhereMidLike(string whereName, string value, WhereJoinType joinType)
        {
            return AddWhere(whereName, "%" + value + "%", WhereValueType.MatchLike);
        }

        public WherePattenItem WherePreciseLike(string whereName, string value, WhereJoinType joinType)
        {
            return AddWhere(whereName, value, WhereValueType.MatchLike);
        }

        public WherePattenItem WhereEqual(string whereName, string value)
        {
            return AddWhere(whereName, value, WhereValueType.Equal);
        }

        public WherePattenItem WhereEqual(string whereName, int value)
        {
            return WhereEqual(whereName, value.ToString());
        }

        public WherePattenItem WhereEqual(string whereName, decimal value)
        {
            return WhereEqual(whereName, value.ToString("0.##"));
        }

        public WherePattenItem WhereEqual(string whereName, Guid value)
        {
            return WhereEqual(whereName, value.ToString());
        }

        /// <summary>
        /// 添加并大于
        /// </summary>
        /// <param name="whereName">where字段</param>
        /// <param name="value">比较的值</param>
        /// <param name="hasEqual">是否可等于</param>
        /// <returns></returns>
        public WherePattenItem WhereBig(string whereName, string value, bool hasEqual = true)
        {

            return AddWhere(whereName, value,
                 hasEqual ? WhereValueType.BigEqualThen : WhereValueType.BigThen);
        }

        public WherePattenItem WhereBig(string whereName, int value, bool hasEqual = true)
        {
            return WhereBig(whereName, value.ToString(),hasEqual);
        }

        public WherePattenItem WhereBig(string whereName, decimal value, bool hasEqual = true)
        {
            return WhereBig(whereName, value.ToString("0.##"), hasEqual);
        }

        public WherePattenItem WhereSmall(string whereName, string value, bool hasEqual = true)
        {
            return AddWhere(whereName, value,
                 hasEqual ? WhereValueType.SmallEqualThen : WhereValueType.SmallThen);
        }

        public WherePattenItem WhereSmall(string whereName, int value, bool hasEqual = true)
        {
            return WhereSmall(whereName, value.ToString(), hasEqual);
        }

        public WherePattenItem WhereSmall(string whereName, decimal value, bool hasEqual = true)
        {
            return WhereSmall(whereName, value.ToString(), hasEqual);
        }


        public WherePattenItem WhereIn(string whereName, List<int> thes)
        {
            List<string> values = thes.ConvertAll<string>(delegate (int i) { return i.ToString(); });
            return WhereIn(whereName, values);
        }

        public WherePattenItem WhereIn(string whereName, List<decimal> thes)
        {
            List<string> values = thes.ConvertAll<string>(delegate (decimal i) { return i.ToString(); });
            return WhereIn(whereName, values);
        }

        public WherePattenItem WhereIn(string whereName, List<Guid> thes)
        {
            List<string> values = thes.ConvertAll<string>(delegate (Guid i) { return i.ToString(); });
            return WhereIn(whereName, values);
        }

        public WherePattenItem WhereNotNull(string whereName)
        {
            return AddWhere(whereName, null, WhereValueType.NotNull);
        }

        public WherePattenItem WhereOnlyNull(string whereName)
        {
            return AddWhere(whereName, null, WhereValueType.OnlyNull);
        }


        #endregion

        public WherePattenItem ToAndJoin()
        {
            this._currentJoinType = WhereJoinType.And;
            return this;
        }

        public WherePattenItem ToNotAndJoin()
        {
            this._currentJoinType = WhereJoinType.AndNot;
            return this;
        }

        public WherePattenItem ToOrJoin()
        {
            this._currentJoinType = WhereJoinType.Or;
            return this;
        }

        public WherePattenItem ToNotOrJoin()
        {
            this._currentJoinType = WhereJoinType.OrNot;
            return this;
        }

        #region 生成where筛选字符串
        private bool _isContain { get { return string.IsNullOrEmpty(WhereName); } }

        public string toWhereStr()
        {
            var result = "";
            var whereNameStr = " [" + WhereName + "] ";
            switch (WhereType)
            {
                case WhereValueType.BigThen:
                    return whereNameStr + " > " + WhereValue;
                case WhereValueType.BigEqualThen:
                    return whereNameStr + " >= " + WhereValue;
                case WhereValueType.Equal:
                    return whereNameStr + " = " + WhereValue;
                case WhereValueType.MatchLike:
                    return whereNameStr + " like " + WhereValue;
                case WhereValueType.NotNull:
                    return whereNameStr + " is not null ";
                case WhereValueType.OnlyNull:
                    return whereNameStr + " is null ";
                case WhereValueType.SmallEqualThen:
                    return whereNameStr + " <= " + WhereValue;
                case WhereValueType.SmallThen:
                    return whereNameStr + " < " + WhereValue;
                case WhereValueType.WithInList:
                    return whereNameStr + " in(" + WhereValue + ") ";
            }


            return result;
        }
        private string toJoinStr()
        {
            switch (OutJoinType)
            {
                case WhereJoinType.And:
                    return _isContain? " and ({0}) " : " and {0} ";
                case WhereJoinType.AndNot:
                    return _isContain? " and not ({0}) " : " and not {0} ";
                case WhereJoinType.Or:
                    return _isContain? " or ({0}) " : " or {0} ";
                case WhereJoinType.OrNot:
                    return _isContain? " or not ({0}) " : " or not {0} ";
                default:
                    throw new Exception("未知的wherejoin类型");
            }
        }
        #endregion


        public string ToSqlString()
        {
            if (_isContain)
            {
                if (WherePattenItems.Count > 0)
                {
                    List<string> eachwhereList = new List<string>();
                    foreach (var item in WherePattenItems)
                    {
                        eachwhereList.Add(item.ToSqlString());
                    }
                    var result = string.Join(" ", eachwhereList);
                    return string.Format(toJoinStr(), result.Substring(result.IndexOf("and") + 3));

                }
            }
            else
            {
                return string.Format(toJoinStr(), toWhereStr());
            }

            return " ";
        }
    }
}
