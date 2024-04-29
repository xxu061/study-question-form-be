using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyQuestionForm.Domain
{
    [BsonIgnoreExtraElements]
    public class PersonalDetailsForm
    {
        [BsonElement("country")]
        public string Country { get; set; }
        [BsonElement("city")]
        public string State { get; set; }
        [BsonElement("age")]
        public int Age { get; set; }
        [BsonElement("level")]
        public string Level { get; set; }
        [BsonElement("gender")]
        public Gender Gender { get; set; }
        [BsonElement("averageGrade")]
        public decimal AverageGrade { get; set; }
        [BsonElement("preferredSchool")]
        public string PreferredSchool { get; set; }
        [BsonElement("preferredMajor")]
        public string PreferredMajor { get; set; }
        [BsonElement("ielts")]
        public decimal Ielts {  get; set; }

    }
}
