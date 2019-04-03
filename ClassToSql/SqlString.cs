using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ClassToSql
{
    class SelectPattenItem
    {
        public string SelStr { get; set; }

        public string AsStr { get; set; }

        public override string ToString()
        {
            return SelStr + " as " + AsStr;
        }

    }

    public class SqlString<T>
    {
        private List<SelectPattenItem> SelectPattens { get; set; } = new List<SelectPattenItem>();
        public string SqlSelect(string selName,string asName=null)
        {
            SelectPattenItem item = new SelectPattenItem
            {
                AsStr = asName == null ? selName : asName,
                SelStr = selName,
            };

            CheckToAddSel(item);
            


            return ToSqlString();
        }

        private void CheckToAddSel(SelectPattenItem item)
        {
            var HasAsList = SelectPattens.Select(s => s.AsStr).ToList();
            if (HasAsList.Contains(item.AsStr))
            {
                throw new Exception("表集合" + typeof(T).Name + "已存在选择字段" + item.AsStr);
            }
            else
            {
                SelectPattens.Add(item);
            }
        }


        public string SqlCountSelect(string countName,string asName)
        {
            SelectPattenItem item = new SelectPattenItem
            {
                AsStr = asName == null ? countName : asName,
                SelStr = " count(" + countName + ") as " + asName,
            };

            SelectPattens.Add(" count(" + countName + ") as " + asName);
        }


        public string ToSqlString()
        {
            string selectHold = "";
            if (SelectPattens.Count == 0)
            {
                selectHold = " * ";
            }
            else
            {
                selectHold=" "+string.Join(",", SelectPattens.Select(s=>s.ToString())) +" ";
            }

            var seletSql = "select " + selectHold + " from " + typeof(T).Name;


            return seletSql;
        }
    }
}
