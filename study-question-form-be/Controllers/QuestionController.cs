using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudyQuestionForm.Domain;
using StudyQuestionForm.Service;

namespace study_question_form_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService _service;
        public QuestionController(IQuestionService service)
        {
            _service = service;
        }

        [HttpGet("initialQuestions")]
        public async Task<IList<Question>> GetInitialQuestions()
        {
            return await _service.GetInitialQuestions();
        }

        [HttpPost("answerQuestions")]
        public async Task<IList<Question>> AnswerQuestions(Application application)
        {
            return await _service.AnswerQuestions(application);
        }

        [HttpPost("choosePath")]
        public async Task<Application> ChoosePath(Application application)
        {
            return await _service.ChoosePath(application.Id.Value, application.PreferredMajors);
        }

        [HttpGet("majorSuggestions")]
        public async Task<IList<Major>> GetMajorSuggestions(string criteria)
        {
            return await _service.GetMajorSuggestions(criteria);
        }

        [HttpGet("majors")]
        public async Task<IList<Major>> GetMajors()
        {
            return await _service.GetMajors();
        }
    }
}
