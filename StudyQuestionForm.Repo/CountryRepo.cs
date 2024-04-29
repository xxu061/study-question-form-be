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
    public class CountryRepo : MongoDbRepo, ICountryRepo
    {
        public CountryRepo(IConfiguration config, ILogger<MongoDbRepo> logger) : base(config, logger)
        {
        }

        public async Task<IList<Country>> GetCountry()
        {
            var collection = InitialiseCollection<IList<Country>>("country");

            return await (await collection.FindAsync<Country>(Builders<IList<Country>>.Filter.Empty)).ToListAsync();
        }
    }

    public interface ICountryRepo
    {
        Task<IList<Country>> GetCountry();
    }
}
