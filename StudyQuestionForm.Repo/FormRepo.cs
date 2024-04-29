using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StudyQuestionForm.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyQuestionForm.Repo
{
    public class FormRepo(IConfiguration config, ILogger<MongoDbRepo> logger) : MongoDbRepo(config, logger), IFormRepo
    {
        public async Task InsertForm(PersonalDetailsForm form)
        {
            var collection = InitialiseCollection<PersonalDetailsForm>("submitForm");
            await collection.InsertOneAsync(form);
        }
    }

    public interface IFormRepo
    {
        Task InsertForm(PersonalDetailsForm form);
    }
}
