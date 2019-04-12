using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassToSql.Enums
{
    public enum OrderByType
    {
        [Display(Name = "升序")]
        Asc = 1,
        [Display(Name = "降序")]
        Desc = 2,
    }
}
