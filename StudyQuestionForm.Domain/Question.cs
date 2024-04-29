using MongoDB.Bson.Serialization.Attributes;

namespace StudyQuestionForm.Domain
{
    [BsonIgnoreExtraElements]
    public class Question
    {
        [BsonElement("questionId")]
        public int QuestionId { get; set; }
        [BsonElement("questionText")]
        public string? QuestionText { get; set; }
        [BsonElement("questionType")]
        public string? QuestionType { get; set; }
        [BsonElement("type")]
        public string? Type { get; set; }
        [BsonElement("relatedQuestionIds")]
        public IList<int>? RelatedQuestionIds { get; set; }
        [BsonElement("answer")]
        public Answer? Answer { get; set; }
        [BsonElement("isInitialQuestion")]
        public bool? IsInitialQuestion { get; set; }
        [BsonElement("mappedProperty")]
        public string? MappedProperty { get; set; }
        [BsonElement("options")]
        public IList<string>? Options { get; set; }
        [BsonElement("relatedAnswerValue")]
        public string? RelatedAnswerValue { get; set; }

        [BsonElement("filterAnswer")]
        public bool? FilterAnswer { get; set; }
        [BsonElement("externalSource")]
        public string? ExternalSource { get; set; }
    }
}
