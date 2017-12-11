using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using MyDictionary.Core;
using Wox.Plugin;

namespace MyDictionary.WoxPlugin
{
    public class MyDictionary : IPlugin
    {
        private MongoClient client;
        private IMongoDatabase database;
        private IMongoCollection<Word> words;

        public void Init(PluginInitContext context)
        {
            MongoClient client = new MongoClient("mongodb://localhost:27017");
            database = client.GetDatabase("MyDictionary");
            words = database.GetCollection<Word>("Words");
        }

        public List<Result> Query(Query query)
        {
            List<Result> results = new List<Result>();
            if (query.FirstSearch.Any())
            {
                var o = query.FirstSearch.Replace('u', 'ó');
                var u = query.FirstSearch.Replace('ó', 'u');
                var r = query.FirstSearch.Replace("ż", "rz");
                var z = query.FirstSearch.Replace("rz", "ż");
                var h = query.FirstSearch.Replace("ch", "h");
                var ch = query.FirstSearch.Replace("h", "ch");

                results = words.Find(x =>
                    x.Item.StartsWith(query.FirstSearch) ||
                    x.Item.StartsWith(o) ||
                    x.Item.StartsWith(u) ||
                    x.Item.StartsWith(r) ||
                    x.Item.StartsWith(z) ||
                    x.Item.StartsWith(h) ||
                    x.Item.StartsWith(ch)
                ).Limit(10)
                    .ToList()
                    .Select(x => new Result()
                    {
                        Title = x.Item,
                        SubTitle = x.SubItems
                    }).ToList();
            }
            return results;
        }
    }
}
