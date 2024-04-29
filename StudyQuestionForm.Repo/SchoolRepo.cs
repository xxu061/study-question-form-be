using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using StudyQuestionForm.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyQuestionForm.Repo
{
    public class SchoolRepo(IConfiguration config, ILogger<MongoDbRepo> logger) : MongoDbRepo(config, logger), ISchoolRepo
    {
        public async Task<IList<Major>> GetAllMajors()
        {
            var collection = InitialiseCollection<Major>("major");

            FilterDefinition<Major> filter = Builders<Major>.Filter.Empty;
            var majors = (await collection.FindAsync<Major>(filter));
            return majors.ToList();
        }

        public async Task<IList<School>> GetSchools(string state)
        {
            var collection = InitialiseCollection<School>("school");
            var majorCollection = InitialiseCollection<Major>("major");
            FilterDefinition<School> filter = Builders<School>.Filter.Eq(r => r.State, state);
            var schools = (await collection.FindAsync<School>(filter)).ToList();
            var majors = (await majorCollection.FindAsync<Major>(Builders<Major>.Filter.Empty)).ToList();
            foreach (var school in schools)
            {
                school.OfferedMajors = majors.Where(m => m.SchoolName == school.Name).ToList();
            }

            return schools;
        }
    }

    public interface ISchoolRepo
    {
        Task<IList<School>> GetSchools(string state);
        Task<IList<Major>> GetAllMajors();
    }
}
