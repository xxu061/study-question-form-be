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
    public class StudyProgramRepo(IConfiguration config, ILogger<MongoDbRepo> logger) : MongoDbRepo(config, logger), IStudyProgramRepo
    {
        public async Task<IList<StudyProgram>> GetStudyProgram(string state)
        {
            var collection = InitialiseCollection<StudyProgram>("study-program");

            FilterDefinition<StudyProgram> filter = Builders<StudyProgram>.Filter.Eq(r => r.State, state);

            return await (await collection.FindAsync<StudyProgram>(filter)).ToListAsync();
        }
    }

    public interface IStudyProgramRepo
    {
        Task<IList<StudyProgram>> GetStudyProgram(string state);
    }
}
