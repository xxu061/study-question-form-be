using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyQuestionForm.Domain
{
    [BsonIgnoreExtraElements]
    public class SchoolFaculty
    {
        [BsonElement("school")]
        public string School { get; set; }
        [BsonElement("faculty")]
        public IList<string> Faculty { get; set; }
    }
}
