using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyQuestionForm.Domain
{
    public class LocationDto
    {
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
    }
}
