using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassToSql.Enums
{
    public enum WhereJoinType
    {
        [Display(Name = "与")]
        And = 1,
        [Display(Name = "或")]
        Or = 2,
        [Display(Name = "与非")]
        AndNot = 3,
        [Display(Name = "或非")]
        OrNot = 4,
    }
}
