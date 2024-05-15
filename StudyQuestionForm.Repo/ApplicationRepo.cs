using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using StudyQuestionForm.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyQuestionForm.Repo
{
    public class ApplicationRepo(IConfiguration config, ILogger<MongoDbRepo> logger) : MongoDbRepo(config, logger), IApplicationRepo
    {
        public async Task<Application> CreateApplication()
        {
            Application application = new()
            {
                Id = Guid.NewGuid(),
                DateCreated = DateTime.Now,
            };

            var collection = InitialiseCollection<Application>("application");
            await collection.InsertOneAsync(application);
            return application;
        }

        public async Task<Application> GetApplication(Guid id)
        {
            var collection = InitialiseCollection<Application>("application");
            var application = (await collection.FindAsync(Builders<Application>.Filter.Eq(a => a.Id, id))).FirstOrDefault();
            return application;
        }

        public async Task UpdateApplication(Application application)
        {
            var collection = InitialiseCollection<Application>("application");

            await collection.UpdateOneAsync(Builders<Application>.Filter.Eq(a => a.Id, application.Id),
                Builders<Application>.Update.Set(a => a.Path, application.Path)
                                            .Set(a => a.City, application.City)
                                            .Set(a => a.Country, application.Country)
                                            .Set(a => a.Age, application.Age)
                                            .Set(a => a.Qualification, application.Qualification)
                                            .Set(a => a.Gender, application.Gender)
                                            .Set(a => a.AverageGrade, application.AverageGrade)
                                            .Set(a => a.PreferredMajors, application.PreferredMajors)
                                            .Set(a => a.State, application.State)
                                            .Set(a => a.PreferredSchools, application.PreferredSchools)
                                            .Set(a => a.Ielts, application.Ielts)
                                            .Set(a => a.Budget, application.Budget)
                                            .Set(a => a.Questions, application.Questions)
                                            .Set(a => a.StartPeriod, application.StartPeriod)
                                            .Set(a => a.Name, application.Name)
                                            .Set(a => a.BestMatch, application.BestMatch)
                                            .Set(a => a.Targeting, application.Targeting)
                                            .Set(a => a.Conservative, application.Conservative)
                                            .Set(a => a.TargetQualification, application.TargetQualification)
                );
        }
    }

    public interface IApplicationRepo
    {
        Task<Application> CreateApplication();
        Task UpdateApplication(Application application);
        Task<Application> GetApplication(Guid id);
    }
}
