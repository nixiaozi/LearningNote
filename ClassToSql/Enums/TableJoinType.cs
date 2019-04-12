using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassToSql.Enums
{
    public enum TableJoinType
    {
        [Display(Name = "左连接")]
        LeftJoin,
        [Display(Name = "右连接")]
        RightJoin,
        [Display(Name = "全连接")]
        InnerJoin,
    }
}
