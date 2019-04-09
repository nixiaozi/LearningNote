using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
            return SelStr + " as " + AsStr;
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

    public enum WhereJoinType
    {
        And=1,
        Or=2,
        AndNot=3,
        OrNot=4,
    }

    public enum WhereValueType
    {
        Equal=1,
        BigThen=11,
        BigEqualThen=12,
        SmallThen=21,
        SmallEqualThen=22,
        InWithInt=31,
        InWithDecimal=32,
        InWithString=33,
        LeftMatchLike=41,
        RighMatchLike=42,
        PattenMatchLike=43,
    }

    public enum OrderByType
    {
        [Display(Name = "升序")]
        Asc =1,
        [Display(Name = "降序")]
        Desc =2,
    }

    public class SqlPatten<T>
    {
        /// <summary>
        /// 选择字段
        /// </summary>
        private List<SelectPattenItem> _selectpattens { get; set; }

        private bool _disinist { get; set; }

        public List<WherePattenItem> _wherePatten { get; set; }

        private WherePattenItem _currentWherePatten;  //可以使用linq表达式添加最好

       //可以使用wherestr只有无名字的函数，后面加subwherestr
       // public 
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
