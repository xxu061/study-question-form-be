using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyQuestionForm.Domain
{
    [BsonIgnoreExtraElements]
    public class Recommendation
    {
        [BsonElement]
        public string SchoolName { get; set; }
        [BsonElement]
        public string City { get; set; }
        [BsonElement]
        public string Country { get; set; }
        [BsonElement]
        public string Major {  get; set; }

    }
}
