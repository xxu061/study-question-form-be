using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyQuestionForm.Domain
{
    public class StudyProgram
    {
        [BsonElement("country")]
        public string Country { get; set; }
        [BsonElement("state")]
        public string State { get; set; }
        public Level Level { get; set; }
        [BsonElement("name")]
        public string? Name { get; set; }
        [BsonElement("description")]
        public string? Description { get; set; }
        [BsonElement("minimumScore")]
        public decimal MinimumScore { get; set; }
        [BsonElement("minimumQualification")]
        public string MinimumQualification { get; set; }
        [BsonElement("tuitionFee")]
        public decimal TuitionFee { get; set; }
        public int DurationInMonth { get; set; }
        public int StartMonth { get; set; }
        public int EndMonth { get; set; }
        public string? Campus { get; set; }
        public decimal MinimumIelts { get; set; }
        [BsonElement("eligibleMajors")]
        public IList<Major>? EligibleMajors { get; set; }

    }
}
