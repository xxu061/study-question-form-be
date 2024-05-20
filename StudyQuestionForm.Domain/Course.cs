using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyQuestionForm.Domain
{
    [BsonIgnoreExtraElements]
    public class Course
    {
        [BsonElement("name")]
        public string? Name { get; set; }
        [BsonElement("description")]
        public string? Description { get; set; }
        [BsonElement("year")]
        public int? Year {  get; set; }
    }
}
