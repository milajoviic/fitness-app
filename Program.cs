using FitnessApp.DataProvider;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
//dodavanje swagger-a:
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<UserWorkoutDP>();
builder.Services.AddSingleton<UserDP>();
builder.Services.AddSingleton<ExerciseDP>();
builder.Services.AddSingleton<BodyMetricsDP>();
builder.Services.AddSingleton<UserDietDP>();
builder.Services.AddSingleton<UserHealthDP>();
builder.Services.AddSingleton<PeriodDP>();


var app = builder.Build();

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
