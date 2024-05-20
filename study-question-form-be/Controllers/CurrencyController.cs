using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudyQuestionForm.Domain;

namespace study_question_form_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private List<Currency> _currencies = new List<Currency>()
        {
            new Currency()
            {
                Country = "新西兰",
                ConvertFactor = 4.417,
                CurrencyName = "新西兰元"
            },
                        new Currency()
            {
                Country = "澳大利亚",
                ConvertFactor = 4.8288,
                CurrencyName = "澳大利亚元"
            }
        };
        [HttpGet("convert")]
        public string Get(string country, double amount) 
        {
            Currency currency = _currencies.FirstOrDefault(c => c.Country == country);

            return string.Format("根据{0}实时汇率买入价格{1}（每日更新），学费约为{2}", currency.CurrencyName, currency.ConvertFactor, amount * currency.ConvertFactor);
        }
    }
}
