using ClassToSql.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassToSql
{
    public static class TableJoinHelper
    {
        public static string TableJoinSqlString(string FirstTableStr,string SecondTableStr,
            TableJoinType joinType,string MatchName)
        {
            var joinStr = "";
            switch (joinType)
            {
                case TableJoinType.InnerJoin:
                    joinStr = " inner join ";
                    break;
                case TableJoinType.LeftJoin:
                    joinStr = " left join ";
                    break;
                case TableJoinType.RightJoin:
                    joinStr = " right join ";
                    break; 
                default:
                    throw new Exception("无效的TableJoinType类型");

            }
            var onStr =string.Format(" join_table_base.{0}=join_table_new.{0} ",MatchName);
            var TableSqlStr=@"
                        ({0}) as join_table_base
                        {1}
                        ({2}) as join_table_new
                     on {3}
                    ";
            return string.Format(TableSqlStr, FirstTableStr, joinStr, SecondTableStr, onStr);

        }

        public static bool CheckTableJoinVaild<F,C>(List<string> listOne,List<string> listTwo,out string matchName)
        {
            matchName = "";
            var matchs = listOne.Where(s => listTwo.Contains(s));
            if (matchs.Count() == 1)
            {
                matchName=matchs.First();
                return true;
            }
            else if (matchs.Count()>1)
            {
                throw new Exception("两个表"+typeof(F).Name+"、"+ typeof(C).Name + "字段有太多重复项");
            }
            else
            {
                throw new Exception("两个表" + typeof(F).Name + "、" + typeof(C).Name + "找不到相同的可连接字段");
            }
        }

    }
}
