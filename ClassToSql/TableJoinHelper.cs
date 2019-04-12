using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassToSql
{
    public static class TableJoinHelper
    {
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
