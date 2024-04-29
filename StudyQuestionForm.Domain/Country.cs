using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace StudyQuestionForm.Domain
{
    [BsonIgnoreExtraElements()]
    public class Country
    {
        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("cities")]
        public IList<string> Cities { get; set; }
    }
}
