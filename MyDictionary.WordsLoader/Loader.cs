using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using  MyDictionary.Core;
namespace MyDictionary.WordsLoader
{
    public class Loader
    {
        public static void Load(string file)
        {
            var wordCollection = ConnectToMongo();
            var words = new List<Word>();

            using (var stream = new StreamReader(file))
            {
                char firstLetter = ' ';
                while (stream.EndOfStream == false)
                {

                    var line = stream.ReadLine();
                    if (firstLetter != line[0])
                    {
                        firstLetter = line[0];
                        Console.WriteLine(firstLetter);
                    }

                    var firstWordEndINdex = line.IndexOf(";", StringComparison.Ordinal);
                    if (firstWordEndINdex > -1)
                    {
                        var word = line.Substring(0, firstWordEndINdex);
                        var subWords = line.Substring(firstWordEndINdex, line.Length - 1);
                        words.Add(new Word(word, subWords));
                    }
                    else
                    {
                        words.Add(new Word(line, ""));
                    }

                }
            }

            wordCollection.InsertMany(words);

        }

        private static IMongoCollection<Word> ConnectToMongo()
        {
            MongoClient client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("MyDictionary");
            var collectionExists = CollectionExistsAsync("Words", database).GetAwaiter().GetResult();
            if (collectionExists == false)
            {

                database.CreateCollection("Words");
                database.GetCollection<Word>("Words").Indexes.CreateOne(new IndexKeysDefinitionBuilder<Word>().Ascending(_ => _.Item).Text(_ => _.Item));
            }

            return database.GetCollection<Word>("Words");
        }

        public static async Task<bool> CollectionExistsAsync(string collectionName, IMongoDatabase database)
        {
            var filter = new BsonDocument("name", collectionName);
            //filter by collection name
            var collections = await database.ListCollectionsAsync(new ListCollectionsOptions { Filter = filter });
            //check for existence
            return await collections.AnyAsync();
        }
    }
}
