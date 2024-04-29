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
    public class PathRepo(IConfiguration config, ILogger<MongoDbRepo> logger) : MongoDbRepo(config, logger), IPathRepo
    {
        public async Task<IList<Domain.Path>> GetPaths(Application application)
        {
            var collection = InitialiseCollection<Domain.Path>("path");

            FilterDefinition<Domain.Path> countryFilter = Builders<Domain.Path>.Filter.Eq(r => r.Country, application.Country);
            //FilterDefinition<Domain.Path> scoreFilter = Builders<Domain.Path>.Filter.Gte(r => r.MinimumScore, application.AverageGrade);
            //FilterDefinition<Domain.Path> qualificationFilter = Builders<Domain.Path>.Filter.Eq(r => r.Name, "标准预科（Standard Foundation）+本科");
            var paths = await (await collection.FindAsync<Domain.Path>(countryFilter)).ToListAsync();
           
            return paths;
        }
    }

    public interface IPathRepo
    {
        Task<IList<Domain.Path>> GetPaths(Application application);
    }
}
