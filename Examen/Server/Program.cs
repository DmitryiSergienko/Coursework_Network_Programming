using DataLayer.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Добавляем сервисы в контейнер DI (Регистрация DbContext)
builder.Services.AddDbContext<DataLayer.Models.DataBaseContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ModelsConnection")));

builder.Services.AddDbContext<DataLayer.Procedures.DataBaseContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ProceduresConnection")));

builder.Services.AddDbContext<DataLayer.Views.DataBaseContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ViewsConnection")));

// Регистрируем сервис
builder.Services.AddScoped<Service>();

// Добавляем контроллеры (API)
builder.Services.AddControllers();

// Добавляем Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Examen API",
        Version = "v1"
    });
});

// CORS - чтобы WPF клиент мог обращаться к серверу
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()  // Разрешает запросы с любого домена (для разработки)
              .AllowAnyMethod()  // Разрешает любые методы (GET, POST, PUT и т.д.)
              .AllowAnyHeader(); // Разрешает любые заголовки
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Examen API V1");
});

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.MapControllers();

app.Run();