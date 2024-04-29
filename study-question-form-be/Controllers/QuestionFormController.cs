using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudyQuestionForm.Domain;
using StudyQuestionForm.Service;

namespace study_question_form_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionFormController : ControllerBase
    {
        private readonly IMongoDbService _service;
        public QuestionFormController(IMongoDbService service) 
        {
            _service = service;
        }

        [HttpGet("country")]
        public async Task<IList<Country>> GetCountry()
        {
            return await _service.GetCountry();
        }

        [HttpPost("createApplication")]
        public async Task<Application> CreateApplication()
        {
            var application = await _service.CreateApplication();

            return application;
        }

        [HttpPost("updateApplication")]
        public async Task<Application> UpdateApplication(Application application)
        {
            await _service.UpdateApplication(application);

            return application;
        }

        [HttpGet("getApplication")]
        public async Task<Application> GetApplication(Guid id)
        {
            return await _service.GetApplication(id);
        }

        [HttpPost("path")]
        public async Task<IList<Major>> GetPath(Application application)
        {
            return await _service.GetPaths(application);
        }

        [HttpPost("studyPrograms")]
        public async Task<IList<StudyProgram>> GetStudyPrograms(Application application)
        {
            return await _service.GetStudyPrograms(application);
        }

        //[HttpPost("report")]
        //public async Task<Application> GetReport(Application application)
        //{

        //    await _service.UpdateApplication(application);
        //}
    }
}
