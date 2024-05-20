using MongoDB.Bson.Serialization.Attributes;

namespace StudyQuestionForm.Domain
{
    [BsonIgnoreExtraElements]
    public class Plan
    {
        [BsonElement("major")]
        public Major Major { get; set; }
        [BsonElement("score")]
        public decimal Score { get; set; }
        [BsonElement("path")]
        public Path Path { get; set; }

        public decimal TotalTuitionFee
        {
            get
            {
                if (Major != null && Path != null)
                {
                    return Major.TuitionFee.Value + Path.TuitionFee;
                }
                else { return 0; }
            }
        }
        public int TotalDurationInMonth
        {
            get
            {
                if (Major != null && Path != null)
                {
                    return Major.DurationInMonth + Path.DurationInMonth;
                }
                else { return 0; }
            }
        }
    }
}
