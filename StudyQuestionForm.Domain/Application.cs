using MongoDB.Bson.Serialization.Attributes;

namespace StudyQuestionForm.Domain
{
    public class Application
    {
        [BsonElement("id")]
        public Guid? Id { get; set; }
        [BsonElement("country")]
        public string? Country { get; set; }
        [BsonElement("state")]
        public string? State { get; set; }
        [BsonElement("name")] 
        public string? Name { get; set;}
        [BsonElement("city")]
        public string? City { get; set; }
        [BsonElement("age")]
        public int? Age { get; set; }
        [BsonElement("level")]
        public string? Qualification { get; set; }
        [BsonElement("gender")]
        public Gender? Gender { get; set; }
        [BsonElement("averageGrade")]
        public decimal? AverageGrade { get; set; }
        [BsonElement("preferredSchools")]
        public IList<School>? PreferredSchools { get; set; }
        [BsonElement("preferredMajors")]
        public IList<Major>? PreferredMajors { get; set; }
        [BsonElement("ielts")]
        public decimal? Ielts { get; set; }
        [BsonElement("path")] 
        public Path? Path { get; set; }
        [BsonElement("questions")]
        public IList<Question>? Questions { get; set; }
        [BsonElement("budget")]
        public string? Budget { get; set; }
        [BsonElement("dateCreated")]
        public DateTime DateCreated { get; set; }
        [BsonElement("targetQualification")]
        public string? TargetQualification { get; set; }
        [BsonElement("startPeriod")]
        public string? StartPeriod { get; set; }
        [BsonElement("bestMatch")]
        public Plan? BestMatch { get; set; }
        [BsonElement("targeting")]
        public Plan? Targeting { get; set; }
        [BsonElement("conservative")]
        public Plan? Conservative { get; set; }
    }
}
