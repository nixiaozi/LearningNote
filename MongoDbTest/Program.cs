// See https://aka.ms/new-console-template for more information
using MongoDB.Bson;
using MongoDB.Driver;

Console.WriteLine("Hello, World!");

// 下面是数据库连接字符串
var settings = MongoClientSettings.FromConnectionString("mongodb+srv://leo:As1234566@cluster0.kb4bi.mongodb.net/LeoTest?retryWrites=true&w=majority");
settings.ServerApi = new ServerApi(ServerApiVersion.V1);
var client = new MongoClient(settings);
var database = client.GetDatabase("test");  // 这个是默认数据库

var collection = database.GetCollection<BsonDocument>("bar");

await collection.InsertOneAsync(new BsonDocument("Name", "IDK"));

var list = await collection.Find(new BsonDocument("Name","Jack"))
    .ToListAsync();

foreach (var document in list)
{
    Console.WriteLine(document["Name"]);
}

