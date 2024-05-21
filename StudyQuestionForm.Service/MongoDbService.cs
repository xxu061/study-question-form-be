using MongoDB.Driver;
using StudyQuestionForm.Domain;
using StudyQuestionForm.Repo;
using System.Linq;
using Path = StudyQuestionForm.Domain.Path;

namespace StudyQuestionForm.Service
{
    public class MongoDbService : IMongoDbService
    {
        private readonly ICountryRepo _countryRepo;
        private readonly IFormRepo _formRepo;
        private readonly IRecommendationRepo _recommendationRepo;
        private readonly IPathRepo _pathRepo;
        private readonly IApplicationRepo _applicationRepo;
        private readonly IStudyProgramRepo _studyProgramRepo;
        private readonly ISchoolRepo _schoolRepo;

        public MongoDbService(ICountryRepo repo, 
            IFormRepo formRepo, 
            IRecommendationRepo recommendationRepo, 
            IPathRepo pathRepo, 
            IApplicationRepo applicationRepo, 
            IStudyProgramRepo studyProgramRepo,
            ISchoolRepo schoolRepo)
        {
            _countryRepo = repo ?? throw new ArgumentNullException(nameof(CountryRepo));
            _formRepo = formRepo ?? throw new ArgumentNullException(nameof(FormRepo));
            _recommendationRepo = recommendationRepo ?? throw new ArgumentNullException(nameof(RecommendationRepo));
            _pathRepo = pathRepo ?? throw new ArgumentNullException(nameof(pathRepo));
            _applicationRepo = applicationRepo ?? throw new ArgumentNullException(nameof(ApplicationRepo));
            _studyProgramRepo = studyProgramRepo ?? throw new ArgumentNullException(nameof(StudyProgramRepo));
            _schoolRepo = schoolRepo ?? throw new ArgumentNullException(nameof(SchoolRepo));
        }

        public async Task<IList<Country>> GetCountry()
        {
            return await _countryRepo.GetCountry();
        }

        public async Task<IList<Recommendation>> GetRecommendation(PersonalDetailsForm form)
        {
            return await _recommendationRepo.GetRecommendation(form);
        }
        public async Task<IList<Major>> GetPaths(Application application)
        {
            var existingApplication = await _applicationRepo.GetApplication(application.Id.Value);

            var paths = await _pathRepo.GetPaths(existingApplication);
            foreach (var major in existingApplication.PreferredMajors)
            {
                var matchPaths = paths.Where(p => p.EligibleSchools.Any(e => e.School == major.SchoolName) && p.MinimumQualification.Contains(existingApplication.Qualification));
                var pathForSchool = paths.Where(p => p.EligibleSchools.Any(e => e.School == major.SchoolName));
                foreach (var path in matchPaths)
                {
                    path.DisqualifyReasons = ValidatePath(path, existingApplication);
                }
                major.Paths = matchPaths.ToList();
            }

            await UpdateApplication(existingApplication);

            return existingApplication.PreferredMajors;
        }

        private IList<string> ValidatePath(Path path, Application existingApplication)
        {
            var reasons = new List<string>();
            var isQualifield = CompareQualifications(path.MinimumQualification, existingApplication.Qualification);
            if (!string.IsNullOrEmpty(isQualifield))
            {
                reasons.Add(isQualifield);
            }
            if (path.MinimumScore > existingApplication.AverageGrade)
            {
                reasons.Add($"最低平均分要求：{path.MinimumScore} 申请人平均分：{existingApplication.AverageGrade}");
            }

            return reasons;
        }
        public async Task<Application> SetRecommendations(Guid id)
        {
            var existingApplication = await _applicationRepo.GetApplication(id);

            existingApplication.BestMatch = GetBestMatch(existingApplication);
            existingApplication.Targeting = GetTargeting(existingApplication);
            existingApplication.Conservative = GetConservative(existingApplication);

            await _applicationRepo.UpdateApplication(existingApplication);

            return existingApplication;
        }


        public async Task<Application> CreateApplication()
        {
            return await _applicationRepo.CreateApplication();
        }
        public async Task<Application> GetApplication(Guid id)
        {
            return await _applicationRepo.GetApplication(id);
        }

        public async Task UpdateApplication(Application application)
        {
            await _applicationRepo.UpdateApplication(application);
        }
        public async Task<IList<StudyProgram>> GetStudyPrograms(Application application)
        {
            var studyPrograms = await _studyProgramRepo.GetStudyProgram(application.State);
            var qualifiedMatches = studyPrograms.Where(p => p.MinimumScore <= application.AverageGrade 
                                                && CompareQualification(p.MinimumQualification, application.Qualification));
            if (qualifiedMatches.Any())
            {
                var matches = qualifiedMatches.Where(m => m.EligibleMajors.Intersect(application.PreferredMajors).Any());
                if (matches.Any())
                {
                    return matches.ToList();
                }
            }

            return new List<StudyProgram>();
        }

        public async Task<IList<School>> GetSchools(string state)
        {
            var schools = await _schoolRepo.GetSchools(state);

            return schools;
        }

        public async Task<IList<Major>> GetMajors(string state)
        {
            var schools = await _schoolRepo.GetSchools(state);

            return schools.SelectMany(s => s.OfferedMajors).OrderBy(m => m.Name).ToList();
        }

        private IList<Plan> GetPlan(Application application)
        {
            List<Plan> plans = new List<Plan>();
            foreach(var major in application.PreferredMajors)
            {
                plans.Add(new Plan { Major = major, 
                    Score = GetScore(application, major), 
                    Path = GetBestPath(major.Paths, application) });
            }

            return plans;
        }

        private Plan GetBestMatch(Application application)
        {
            var major = application.PreferredMajors
                .Where(m => m.TuitionFee + (GetBestPath(m.Paths, application) == null ? 0 : GetBestPath(m.Paths, application).TuitionFee) 
                < Convert.ToDecimal(application.Budget.Substring(1, application.Budget.Length - 1)))
                .OrderBy(m => m.QsRank)
                .ThenByDescending(m => m.TuitionFee)
                .FirstOrDefault();

            var path = GetBestPath(major.Paths, application);

            return new Plan { Major = major, Path = path, Score = GetScore(application, major) };
        }

        private Plan GetTargeting(Application application)
        {
            var major = application.PreferredMajors
                .OrderByDescending(m => m.QsRank)
                .ThenByDescending(m => m.TuitionFee)
                .FirstOrDefault();
            var path = GetBestPath(major.Paths, application);

            return new Plan { Major = major, Path = path, Score = GetScore(application, major) };
        }

        private Plan GetConservative(Application application)
        {
            var major = application.PreferredMajors
                .OrderByDescending(m => m.QsRank)
                .ThenByDescending(m => m.TuitionFee)
                .FirstOrDefault();
            var path = GetBestPath(major.Paths, application);

            return new Plan { Major = major, Path = path, Score = GetScore(application, major) };
        }

        private decimal GetScore(Application application, Major major)
        {
            decimal score = 0;
            var budget = Convert.ToDecimal(application.Budget.Substring(1, application.Budget.Length - 1));
           
            score += 1000 - major.QsRank.Value;
            var path = GetBestPath(major.Paths, application);
            var totalTuitionFee = major.TuitionFee.Value - (path == null ? 0 : path.TuitionFee);

            score += budget - totalTuitionFee;

            return score;
        }

        private Domain.Path GetBestPath(IList<Domain.Path> paths, Application application)
        {
            foreach(var path in paths)
            {
                path.DisqualifyReasons = ValidatePath(path, application);
            }
            if (paths != null && paths.Any())
            {
                var path = paths.Where(p => p.IsQualified)
                    .OrderByDescending(p => p.Selected)
                    .ThenByDescending(p => p.DurationInMonth)
                    .ThenByDescending(p => p.TuitionFee)
                    .FirstOrDefault();
                return path;
            }
            else
            {
                return null;
            }            
        }

        private Domain.Path GetTopPath(IList<Domain.Path> paths)
        {
            if (paths != null && paths.Any())
            {
                var path = paths.Where(p => p.Selected)
                    .OrderByDescending(p => p.DurationInMonth)
                    .ThenByDescending(p => p.TuitionFee)
                    .FirstOrDefault();
                return path;
            }
            else
            {
                return null;
            }
        }

        private static string CompareQualifications(IList<string> qualifications, string qualification) 
        {
            var mappedQualifications = qualifications.Select(q => new { value = q, mappedValue = MapQualification(q) }).OrderBy(q => q.mappedValue);
            
            var lowestQualification = mappedQualifications.FirstOrDefault();

            if(lowestQualification.mappedValue > MapQualification(qualification))
            {
                return $"最低学历要求：{lowestQualification.value} 申请人学历：{qualification}";
            }
            else
            {
                return null;
            }
        }

        private static bool CompareQualification(string qualification1, string qualification2)
        {
            return MapQualification(qualification1) <= MapQualification(qualification2);
        }

        private static int MapQualification(string qualification)
        {
            int result = 0;
            switch(qualification)
            {
                case "初一完成":
                result = 1;
                    break;
                case "初二完成":
                    result = 2;
                    break;
                case "初三完成":
                    result = 3;
                    break;
                case "高一完成":
                    result = 4;
                    break;
                case "高二完成":
                    result = 5;
                    break;
                case "高三完成":
                    result = 6;
                    break;
            }

            return result;
        }

        //private async Task<StudyProgram> MatchStudyProgram(Application application, Stage stage, IList<StudyProgram> studyPrograms)
        //{

        //    var schools = await _schoolRepo.GetSchools(application.State);

        //    var matchSchools = schools
        //        .Where(s => s.OfferedMajors
        //        .Join(application.PreferredMajors, all => all.Name, preferred => preferred.Name, (all, preferred) => all).Any());

        //    matchSchools ??= application.PreferredSchools;

        //    var program = studyPrograms.FirstOrDefault(p => p.Country == application.Country
        //                                                 //&& p.Level == stage.Level
        //                                                 && p.MinimumIelts < application.Ielts
        //                                                 && p.EligibleMajors.Join(matchSchools, all => all, eligible => eligible.Name, (all, eligible) => all).Any()
        //                                                 //&& 
        //                                                 );
        //    return program;
        //}
    }

    public interface IMongoDbService
    {
        Task<IList<Country>> GetCountry();
        Task<Application> CreateApplication();
        Task UpdateApplication(Application application);
        Task<IList<Recommendation>> GetRecommendation(PersonalDetailsForm form);
        Task<IList<Major>> GetPaths(Application application);
        Task<IList<StudyProgram>> GetStudyPrograms(Application application);
        Task<Application> GetApplication(Guid id);
        Task<Application> SetRecommendations(Guid id);
    }
}
