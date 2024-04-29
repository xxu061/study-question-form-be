using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyQuestionForm.Domain
{
    [BsonIgnoreExtraElements]
    public class Answer
    {
        [BsonElement("value")]
        public dynamic? Value { get; set; }
        [BsonElement("type")]
        public string? Type { get; set; }
    }
}
