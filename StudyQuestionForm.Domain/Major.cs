using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyQuestionForm.Domain
{
    [BsonIgnoreExtraElements]
    public class Major
    {
        [BsonElement("name")]
        public string? Name { get; set; }
        [BsonElement("faculty")]
        public string? Faculty { get; set; }
        [BsonElement("employerPreference")]
        public int? EmployerPreference { get; set; }
        [BsonElement("studentPopularity")]
        public int? StudentPopularity { get; set; }
        [BsonElement("averagePay")]
        public int? AveragePay { get; set; }
        [BsonElement("qsRank")]
        public int? QsRank { get; set; }
        [BsonElement("schoolName")]
        public string? SchoolName { get; set; }
        [BsonElement("degree")]
        public string Degree { get; set; }

        public string DisplayName
        {
            get
            {
                return Name + " " + SchoolName;
            }
        }

        public IList<Domain.Path> Paths { get; set; }
        [BsonElement("selectedPaths")]
        public IList<Domain.Path> SelectedPaths { get; set; }
    }
}
