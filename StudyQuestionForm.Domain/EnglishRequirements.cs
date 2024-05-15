using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyQuestionForm.Domain
{
    [BsonIgnoreExtraElements]
    public class EnglishRequirements
    {
        [BsonElement("ielts")]
        public Ielts Ielts { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class Ielts
    {
        [BsonElement("listening")]
        public decimal Listening { get; set; }
        [BsonElement("overall")]
        public decimal Overall { get; set; }
        [BsonElement("speaking")]
        public decimal Speaking { get; set;}
        [BsonElement("reading")]
        public decimal Reading { get; set;}
        [BsonElement("writing")]
        public decimal Writing { get; set;}

    }
}
