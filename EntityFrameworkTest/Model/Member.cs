using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkTest.Model
{
    public class Member
    {
        public Guid ID { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public DateTime CreateDate { get; set; }


    }

    
}
