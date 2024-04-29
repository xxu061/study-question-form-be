using StudyQuestionForm.Repo;
using StudyQuestionForm.Service;

var builder = WebApplication.CreateBuilder(args);
var corsPolicy = "cors";
builder.Services.AddCors(o => o.AddPolicy(corsPolicy, builder =>
{
    builder.WithOrigins("*")
           .AllowAnyMethod()
           .AllowAnyHeader();
}));
// Add services to the container.
builder.Services.AddScoped<IMongoDbService, MongoDbService>();
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped<IQuestionRepo, QuestionRepo>();
builder.Services.AddScoped<ICountryRepo, CountryRepo>();
builder.Services.AddScoped<IFormRepo, FormRepo>();
builder.Services.AddScoped<IRecommendationRepo, RecommendationRepo>();
builder.Services.AddScoped<IPathRepo, PathRepo>();
builder.Services.AddScoped<IApplicationRepo, ApplicationRepo>();
builder.Services.AddScoped<IStudyProgramRepo, StudyProgramRepo>();
builder.Services.AddScoped<ISchoolRepo, SchoolRepo>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseCors(corsPolicy);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
