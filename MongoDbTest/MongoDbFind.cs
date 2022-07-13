using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDbTest
{
    public static class MongoDbFind
    {
        public static dynamic GetEqualData(IMongoCollection<BsonDocument> collection)
        {
            var documents = collection.Find(new BsonDocument()).ToList();

            var filter = Builders<BsonDocument>.Filter.Exists("i");
            //filter.Lt("aaa", 1);
            //filter.Eq("i", 10);
            var filter1 = Builders<BsonDocument>.Filter.Eq("aaa", "1");

            var test = collection.Find(filter1).ToList();

            //var theFilter = filter1 & filter2 & filter3;
            #region MatchDetail -- 已注释
            List<MatchDetail> details = new List<MatchDetail>
            {
                new MatchDetail
                {
                    DataName="aaa",
                    MatchType=MatchType.Small,
                    DataValue=30
                },
                new MatchDetail
                {
                    DataName="aaa",
                    MatchType=MatchType.Small,
                    DataValue=20
                }
            };

            var FilterDefinitions = GetMatchFilter(details);
            var theFilter = FilterDefinitions[0];
            for (var i = 1; i < FilterDefinitions.Count; i++)
            {
                theFilter = theFilter & FilterDefinitions[i];
            }


            var test1 = collection.Find(theFilter).ToList();
            #endregion

            var update = Builders<BsonDocument>.Update.Set("i", 110);


            return (dynamic)new object();

        }


        public static List<FilterDefinition<BsonDocument>> GetMatchFilter(List<MatchDetail> details)
        {
            List<FilterDefinition<BsonDocument>> result =new List<FilterDefinition<BsonDocument>>();

            details.ForEach(x =>
            {
                result.Add(GetTheMatchFilter(x));
            });

            return result;
        }

        public static FilterDefinition<BsonDocument> GetTheMatchFilter(MatchDetail match)
        {
            switch (match.MatchType)
            {
                case MatchType.Small:
                    return Builders<BsonDocument>.Filter.Lt(match.DataName, match.DataValue);
                default:
                    break;
            }

            return null;
        }


    }
}
