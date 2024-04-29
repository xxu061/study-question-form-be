using MongoDB.Bson.Serialization.Attributes;

namespace StudyQuestionForm.Domain
{
    [BsonIgnoreExtraElements]
    public class Stage
    {
        [BsonElement("level")]
        public Level Level { get; set; }
        [BsonElement("name")]
        public string? Name { get; set; }
        [BsonElement("studyProgram")]
        public StudyProgram? StudyProgram { get; set; }

    }
}
