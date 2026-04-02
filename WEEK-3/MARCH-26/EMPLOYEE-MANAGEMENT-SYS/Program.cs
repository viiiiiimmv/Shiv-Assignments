using Microsoft.EntityFrameworkCore;
using WEBAPI_DEMO;
using WEBAPI_DEMO.Data;

Console.WriteLine("Program entry.");
var builder = WebApplication.CreateBuilder(args);
Console.WriteLine("Builder created.");

// Add services to the container.
builder.Services.AddProblemDetails();
builder.Services.AddScoped<IEmployee, EmployeeService>();
builder.Services.AddControllers();
builder.Services.AddRazorPages();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var databaseProvider = builder.Configuration["Database:Provider"];

    if (string.Equals(databaseProvider, "SqlServer", StringComparison.OrdinalIgnoreCase))
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnection"));
        return;
    }

    options.UseSqlite(builder.Configuration.GetConnectionString("SqliteConnection"));
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

Console.WriteLine("Service registration completed.");
Console.WriteLine("Building web application...");
var app = builder.Build();

Console.WriteLine("Starting Employee Management API...");
await InitializeDatabaseAsync(app);
Console.WriteLine("Database initialization completed.");

app.UseExceptionHandler();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthorization();
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Employee Management API v1");
    options.RoutePrefix = "swagger";
    options.DocumentTitle = "Employee Management API";
});

app.MapGet("/health", () => Results.Ok(new { status = "Healthy" }))
    .WithName("HealthCheck");

app.MapGet("/Home/Index", () => Results.Redirect("/"));
app.MapGet("/Home/Index2", () => Results.Redirect("/"));

app.MapControllers();
app.MapRazorPages();
app.MapFallbackToPage("/Index");

app.Run();

static async Task InitializeDatabaseAsync(WebApplication app)
{
    await using var scope = app.Services.CreateAsyncScope();
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>()
        .CreateLogger("DatabaseInitialization");

    try
    {
        Console.WriteLine("Initializing database provider...");
        if (string.Equals(configuration["Database:Provider"], "SqlServer", StringComparison.OrdinalIgnoreCase))
        {
            await context.Database.MigrateAsync();
            Console.WriteLine("SQL Server database is ready.");
            return;
        }

        await context.Database.EnsureCreatedAsync();
        Console.WriteLine("SQLite database is ready.");
    }
    catch (Exception exception)
    {
        logger.LogError(exception, "Failed to initialize the configured database provider.");
        throw;
    }
}
