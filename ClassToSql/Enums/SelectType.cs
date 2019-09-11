using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassToSql.Enums
{
    /// <summary>
    /// 选择类型
    /// </summary>
    public enum SelectType
    {
        /// <summary>
        /// 直接选择
        /// </summary>
        Directly,
        /// <summary>
        /// 聚合计算选择
        /// </summary>
        Count,
        /// <summary>
        /// 聚合求和选择
        /// </summary>
        Sum,
        /// <summary>
        /// 选择获取行号
        /// </summary>
        Row,
    }
}
