using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDataAccess.Model
{
    [BsonIgnoreExtraElements]
    public class MdTaxSlabDetail
    {
        [BsonId]
        public ObjectId _id { get; set; }

        [BsonElement("Id")]
        public int Id { get; set; }

        [BsonElement("TaxSlabId")]
        public int TaxSlabId { get; set; }

        [BsonElement("SlabFromAmount")]
        public int? SlabFromAmount { get; set; }

        [BsonElement("SlabToAmount")]
        public int? SlabToAmount { get; set; }

        [BsonElement("Percentage")]
        public int Percentage { get; set; }
    }
}
