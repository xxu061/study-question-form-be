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
    public class QuestionRepo(IConfiguration config, ILogger<MongoDbRepo> logger, ISchoolRepo _schoolRepo) : MongoDbRepo(config, logger), IQuestionRepo
    {
        public async Task<IList<Question>> GetInitialQuestions()
        {
            var collection = InitialiseCollection<Question>("question");

            FilterDefinition<Question> filter = Builders<Question>.Filter.Eq(r => r.IsInitialQuestion, true);

            var result = await (await collection.FindAsync<Question>(filter)).ToListAsync();

            return result;
        }

        public async Task<IList<Question>> GetQuestions(IList<int> questionId, object value, bool filterAnswer)
        {
            var collection = InitialiseCollection<Question>("question");
            List<FilterDefinition<Question>> filters = [Builders<Question>.Filter.In(r => r.QuestionId, questionId)];

            if (filterAnswer)
            {
                filters.Add(Builders<Question>.Filter.Eq(r => r.RelatedAnswerValue, value));
            }

            FilterDefinition<Question> filter = Builders<Question>.Filter.And(filters);


            var questions = await (await collection.FindAsync<Question>(filter)).ToListAsync();

            foreach(var question in questions)
            {
                if (!string.IsNullOrEmpty(question.ExternalSource))
                {
                    switch(question.ExternalSource)
                    {
                        case "major":
                            question.Options = (await _schoolRepo.GetAllMajors()).Select(m => m.Name).ToList(); 
                            break;
                    }
                }
            }

            return questions;
        }
    }

    public interface IQuestionRepo
    {
        Task<IList<Question>> GetInitialQuestions();
        Task<IList<Question>> GetQuestions(IList<int> questionId, object value, bool filterAnswer);
    }
}
