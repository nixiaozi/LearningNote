using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDbTest
{
    public enum MatchType
    {
        Equal,
        Big,
        Small,
        BigEqual,
        SmallEqual,
        SmallThan,
        BigThan,
        Like, // String Only like
    }
}
