using System;

namespace MyDictionary.Core
{
    public class Word
    {
        public Word(string item,string subItems)
        {
            this.Id = Guid.NewGuid().ToString();
            this.Item = item;
            this.SubItems = subItems;
        }

        [MongoDB.Bson.Serialization.Attributes.BsonElement]
        public string  Id { get; set; }
        [MongoDB.Bson.Serialization.Attributes.BsonElement]
        public string Item { get; set; }
        [MongoDB.Bson.Serialization.Attributes.BsonElement]
        public string SubItems { get; set; }
    }
}