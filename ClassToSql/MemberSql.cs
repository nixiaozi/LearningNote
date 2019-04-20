using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ClassToSql
{
    public class MemberSql
    {
        public Guid ID { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public DateTime CreateDate { get; set; }



        public static MemberSql operator +(MemberSql a, MemberSql b){
            a.Age = a.Age + b.Age;
            return a;
        }


        public string SqlSelect(Expression<MemberSql> express)
        {


            return "";
        }
    }
}
