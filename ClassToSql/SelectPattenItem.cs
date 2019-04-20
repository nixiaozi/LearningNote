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
        public string SelStr { get; set; }

        public string AsStr { get; set; }

        public SelectType TheSelectType { get; set; } = SelectType.Directly;

        public override string ToString()
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
