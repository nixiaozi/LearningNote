using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkTest.Model
{
    public class Work
    {
        public Guid ID { get; set; }

        public Guid MemberId { get; set; }

        public string WorkName { get; set; }

        public string WorkLocation { get; set; }

        public int WorkTimes { get; set; }


    }
}
