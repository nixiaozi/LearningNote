using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassToSql.Enums
{
    public enum WhereValueType
    {
        [Display(Name = "等于")]
        Equal = 1,
        [Display(Name = "大于")]
        BigThen = 11,
        [Display(Name = "大于等于")]
        BigEqualThen = 12,
        [Display(Name = "小于")]
        SmallThen = 21,
        [Display(Name = "小于等于")]
        SmallEqualThen = 22,
        [Display(Name = "包含整数")]
        InWithInt = 31,
        [Display(Name = "包含小数")]
        InWithDecimal = 32,
        [Display(Name = "包含字符串")]
        InWithString = 33,
        [Display(Name = "左侧多位匹配")]
        LeftMatchLike = 41,
        [Display(Name = "右侧多位匹配")]
        RighMatchLike = 42,
        [Display(Name = "中央多位匹配")]
        PattenMatchLike = 43,
    }
}
