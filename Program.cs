using Microsoft.EntityFrameworkCore;
using DataBaseWork.Data;
using DataBaseWork.Controllers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Получаем строку подключения с проверкой
var connectionString = builder.Configuration.GetConnectionString("MySQLConnection");
if (string.IsNullOrEmpty(connectionString))
{
    throw new ApplicationException("MySQL connection string is not configured. Please check your appsettings.json");
}

// Настройка DbContext с обработкой ошибок и автоматическими повторными попытками
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        connectionString,
        new MySqlServerVersion(new Version(8, 0, 35)),
        mysqlOptions =>
        {
            mysqlOptions.EnableRetryOnFailure();
        }));

builder.Services.AddTransient<QueriesController>();

var app = builder.Build();

// Проверка подключения к БД при старте приложения
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();

    try
    {
        var dbContext = services.GetRequiredService<AppDbContext>();
        if (dbContext.Database.CanConnect())
        {
            logger.LogInformation("Database connection successful");
        }
        else
        {
            logger.LogWarning("Database connection test failed");
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while connecting to the database");
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();