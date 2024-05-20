using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyQuestionForm.Domain
{
    public class Currency
    {
        public string Country {  get; set; }
        public string CurrencyName { get; set; }
        public double ConvertFactor { get; set; }
    }
}
