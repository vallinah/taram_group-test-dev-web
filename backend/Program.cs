using backend.Repositories;

var builder = WebApplication.CreateBuilder(args);


// Services MVC / Controllers
builder.Services.AddControllers();


// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Dependency Injection
builder.Services.AddSingleton<IContactRepository, InMemoryContactRepository>();


var app = builder.Build();


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