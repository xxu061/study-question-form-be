using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyQuestionForm.Domain
{
    [BsonIgnoreExtraElements]
    public class School
    {
        [BsonElement("country")]
        public string? Country { get; set; }
        [BsonElement("state")]
        public string? State { get; set; }
        [BsonElement("offeredMajors")]
        public IList<Major>? OfferedMajors { get; set; }
        [BsonElement("name")]
        public string? Name { get; set; }
        [BsonElement("qsRank")]
        public int? QsRank { get; set; }
    }
}
