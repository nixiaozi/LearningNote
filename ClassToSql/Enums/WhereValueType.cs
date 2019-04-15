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
        [Display(Name = "包含")]
        WithInList = 31,
        [Display(Name = "左侧多位匹配")]
        MatchLike = 41,
        [Display(Name = "仅为空")]
        OnlyNull =51,
        [Display(Name = "左侧多位匹配")]
        NotNull =52,
    }
}
