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
    public class RecommendationRepo(IConfiguration config, ILogger<MongoDbRepo> logger) : MongoDbRepo(config, logger), IRecommendationRepo
    {
        public async Task<IList<Recommendation>> GetRecommendation(PersonalDetailsForm form)
        {
            var collection = InitialiseCollection<Recommendation>("recommendation-cn");

            FilterDefinition<Recommendation> filter = Builders<Recommendation>.Filter.Eq(r => r.SchoolName, form.PreferredSchool);

            return await (await collection.FindAsync<Recommendation>(filter)).ToListAsync();
        }
    }

    public interface IRecommendationRepo
    {
        Task<IList<Recommendation>> GetRecommendation(PersonalDetailsForm form);
    }
}
