using backend.Repositories;
using backend.Services;

var builder = WebApplication.CreateBuilder(args);


// Services MVC / Controllers
builder.Services.AddControllers();


// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Dependency Injection
builder.Services.AddSingleton<IContactRepository, InMemoryContactRepository>();
builder.Services.AddSingleton<IImportService, ImportService>();


builder.Services.AddCors(options =>
{
    options.AddPolicy("VuePolicy", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors("VuePolicy");

// HTTP pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();


// Active les controllers
app.MapControllers();


app.Run();