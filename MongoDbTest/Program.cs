// See https://aka.ms/new-console-template for more information
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDbTest;

Console.WriteLine("Hello, World!");

// 下面是数据库连接字符串
//var settings = MongoClientSettings.FromConnectionString("mongodb+srv://leo:As1234566@cluster0.kb4bi.mongodb.net/LeoTest?retryWrites=true&w=majority");
var settings = MongoClientSettings.FromConnectionString("mongodb://admin:123456@localhost:27017/?authSource=admin");
settings.ServerApi = new ServerApi(ServerApiVersion.V1);
var client = new MongoClient(settings);
// var thesession = client.StartSession(); -- 这个是来开启一个事务的

var database = client.GetDatabase("admin");  // 这个是默认数据库

var collection = database.GetCollection<BsonDocument>("MyTest");

// await collection.InsertOneAsync(new BsonDocument("Name", "IDK"));

MongoDbFind.GetEqualData(collection);

var result = collection.InsertOneAsync(new BsonDocument("Name", "IDK"));
result.Wait();

var list = await collection.Find(new BsonDocument("Name", "IDK"))
    .ToListAsync();

foreach (var document in list)
{
    Console.WriteLine(document["Name"]);
}

