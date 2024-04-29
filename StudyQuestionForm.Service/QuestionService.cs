using MongoDB.Driver;
using StudyQuestionForm.Domain;
using StudyQuestionForm.Repo;
using System.Reflection;
using System.Text.Json;

namespace StudyQuestionForm.Service
{
    public class QuestionService : IQuestionService
    {
        private readonly ICountryRepo _countryRepo;
        private readonly IFormRepo _formRepo;
        private readonly IRecommendationRepo _recommendationRepo;
        private readonly IPathRepo _pathRepo;
        private readonly IApplicationRepo _applicationRepo;
        private readonly IStudyProgramRepo _studyProgramRepo;
        private readonly ISchoolRepo _schoolRepo;
        private readonly IQuestionRepo _questionRepo;

        public QuestionService(ICountryRepo repo,
            IFormRepo formRepo,
            IRecommendationRepo recommendationRepo,
            IPathRepo pathRepo,
            IApplicationRepo applicationRepo,
            IStudyProgramRepo studyProgramRepo,
            ISchoolRepo schoolRepo,
            IQuestionRepo questionRepo)
        {
            _countryRepo = repo ?? throw new ArgumentNullException(nameof(CountryRepo));
            _formRepo = formRepo ?? throw new ArgumentNullException(nameof(FormRepo));
            _recommendationRepo = recommendationRepo ?? throw new ArgumentNullException(nameof(RecommendationRepo));
            _pathRepo = pathRepo ?? throw new ArgumentNullException(nameof(pathRepo));
            _applicationRepo = applicationRepo ?? throw new ArgumentNullException(nameof(ApplicationRepo));
            _studyProgramRepo = studyProgramRepo ?? throw new ArgumentNullException(nameof(StudyProgramRepo));
            _schoolRepo = schoolRepo ?? throw new ArgumentNullException(nameof(SchoolRepo));
            _questionRepo = questionRepo ?? throw new ArgumentNullException(nameof(QuestionRepo));
        }

        public async Task<IList<Question>> GetInitialQuestions()
        {
            return await _questionRepo.GetInitialQuestions();
        }

        public async Task<IList<Question>> AnswerQuestions(Application application)
        {
            if (application.Id == null)
            {
                var app = await _applicationRepo.CreateApplication();
                application.Id = app.Id;
                await _applicationRepo.UpdateApplication(application);
            }
            else
            {
                var existingApplication = await _applicationRepo.GetApplication(application.Id.Value);
                existingApplication = await MapApplicationQuestions(existingApplication, application);
                await _applicationRepo.UpdateApplication(existingApplication);
            }

            var lastQuestion = application?.Questions?.LastOrDefault();
            var moreQuestions = await _questionRepo.GetQuestions(lastQuestion.RelatedQuestionIds, lastQuestion.Answer.Value, lastQuestion.FilterAnswer.HasValue ? lastQuestion.FilterAnswer.Value : false);

            return moreQuestions;
        }

        public async Task<Application> ChoosePath(Guid id, IList<Major> majors)
        {
            var existingApplication = await _applicationRepo.GetApplication(id);
            existingApplication.PreferredMajors = majors;
            await _applicationRepo.UpdateApplication(existingApplication);

            return existingApplication;
        }

        private async Task<Application> MapApplicationQuestions(Application existing, Application application)
        {
            foreach (var question in application.Questions)
            {
                var property = existing.GetType().GetProperty(char.ToUpper(question.MappedProperty[0]) + question.MappedProperty.Substring(1));

                if (property != null)
                {
                    var value = await GetValueForType(property.PropertyType, question.Answer?.Value?.ToString());
                    property.SetValue(existing, value, null);
                }
            }

            return existing;
        }

        private async Task<object?> GetValueForType(Type propertyType, string value)
        {
            JsonSerializerOptions options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }
            else if (propertyType == typeof(string))
            {
                return value;
            }
            else if (propertyType == typeof(int) || propertyType == typeof(int?))
            {
                return Convert.ToInt32(value);
            }
            else if (propertyType == typeof(bool) || propertyType == typeof(bool?))
            {
                return Convert.ToBoolean(value);
            }
            else if (propertyType == typeof(decimal) || propertyType == typeof(decimal?))
            {
                return Convert.ToDecimal(value);
            }
            else if (propertyType == typeof(Major))
            {
                var major = JsonSerializer.Deserialize<Major>(value, options);
                if (major != null)
                {
                    return new List<Major>() { major };
                }
                else
                {
                    return new List<Major>();
                }

            }
            else if (propertyType == typeof(IList<Major>))
            {
                try
                {
                    var majors = JsonSerializer.Deserialize<IList<Major>>(value, options);
                    return majors;
                }
                catch (Exception)
                {
                    var major = (await _schoolRepo.GetAllMajors()).Where(m => m.Name == value).ToList();
                    return major;
                }                
            }
            else
            {
                throw new InvalidOperationException("Unsupported property type.");
            }
        }

        public async Task<IList<Major>> GetMajorSuggestions(string criteria)
        {
            var majors = await _schoolRepo.GetAllMajors();
            var result = Enumerable.Empty<Major>();
            result = criteria switch
            {
                "qs" => [.. majors.OrderByDescending(m => m.QsRank)],
                "employmentRate" => [.. majors.OrderByDescending(m => m.EmployerPreference)],
                "startSalary" => [.. majors.OrderByDescending(m => m.AveragePay)],
                "internationalStudent" => [.. majors.OrderByDescending(m => m.StudentPopularity)],
                "employerPreferred" => [.. majors.OrderByDescending(m => m.EmployerPreference)],
                _ => [.. majors],
            };
            return result.Take(5).ToList();
        }

        public async Task<IList<Major>> GetMajors()
        {
            var majors = await _schoolRepo.GetAllMajors();
            return majors;
        }
    }

    public interface IQuestionService
    {
        Task<IList<Question>> GetInitialQuestions();
        Task<IList<Question>> AnswerQuestions(Application application);
        Task<IList<Major>> GetMajorSuggestions(string criteria);
        Task<Application> ChoosePath(Guid id, IList<Major> majors);
        Task<IList<Major>> GetMajors();
    }
}
