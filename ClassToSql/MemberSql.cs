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


    }

    public class WorkSql
    {
        public Guid ID { get; set; }

        public Guid MemberId { get; set; }

        public string WorkName { get; set; }

        public string WorkLocation { get; set; }

        public int WorkTimes { get; set; }


    }
}
