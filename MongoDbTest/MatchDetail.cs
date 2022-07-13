using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDbTest
{
    public class MatchDetail
    {
        public MatchType MatchType { get; set; } = MatchType.Equal;

        public string DataName { get; set; } = "";

        public MatchDataType DataType { get; set; } = MatchDataType.String;

        public dynamic DataValue { get; set; } = "";

    }
}
