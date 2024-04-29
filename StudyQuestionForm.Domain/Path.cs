using MongoDB.Bson.Serialization.Attributes;

namespace StudyQuestionForm.Domain
{
    [BsonIgnoreExtraElements]
    public class Path
    {
        [BsonElement("country")] 
        public string Country { get; set; }
        [BsonElement("state")]
        public string State { get; set; }
        [BsonElement("minimumScore")]
        public decimal MinimumScore { get; set; }
        [BsonElement("minimumQualification")]
        public IList<string>? MinimumQualification { get; set; }
        [BsonElement("eligibleSchools")]
        public IList<SchoolFaculty>? EligibleSchools { get; set; }
        [BsonElement("name")]
        public string Name { get; set; }
        [BsonElement("tuitionFee")]
        public decimal TuitionFee { get; set; }
        [BsonElement("durationInMonth")]
        public int DurationInMonth { get; set; }
        public bool IsQualified { get { return DisqualifyReasons == null || !DisqualifyReasons.Any(); } }
        public IList<string>? DisqualifyReasons { get; set; }
    }
}
