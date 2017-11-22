using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDataAccess.Model
{
    [BsonIgnoreExtraElements]
    public class MdTaxSlab
    {
        [BsonId]
        public ObjectId _id { get; set; }

        [BsonElement("Id")]
        public int Id { get; set; }

        [BsonElement("FromYear")]
        public int FromYear { get; set; }

        [BsonElement("ToYear")]
        public int ToYear { get; set; }

        [BsonElement("Category")]
        public string Category { get; set; }
    }
}
