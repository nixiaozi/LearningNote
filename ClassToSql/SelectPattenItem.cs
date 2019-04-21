using ClassToSql.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassToSql
{
    public class SelectPattenItem
    {
        /// <summary>
        /// 选择的Table字段
        /// </summary>
        public string SelKey { get; set; }

        /// <summary>
        /// 选择的字符串
        /// </summary>
        public string SelStr { get; set; }

        /// <summary>
        /// 选择的自定义字符串
        /// </summary>
        public string AsStr { get; set; }

        /// <summary>
        /// 选择的类型
        /// </summary>
        public SelectType TheSelectType { get; set; } = SelectType.Directly;

        public string ToSqlString()
        {
            switch (TheSelectType)
            {
                case SelectType.Directly:
                    return " [" + SelStr + "] as " + AsStr + " ";
                case SelectType.Count:
                    return " count([" + SelStr + "]) as " + AsStr + " ";
                case SelectType.Sum:
                    return " sum([" + SelStr + "]) as " + AsStr + " ";
                case SelectType.Row:
                    return " ROW_NUMBER() over(" + SelStr + ") as " + AsStr + " ";
            }
            return "";
        }

    }
}
