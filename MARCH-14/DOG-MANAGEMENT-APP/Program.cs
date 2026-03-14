using DOG_MANAGEMENT_APP.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace DOG_MANAGEMENT_APP;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var sqliteConnectionString = BuildSqliteConnectionString(builder.Configuration, builder.Environment.ContentRootPath);

        // Add services to the container.
        builder.Services.AddControllersWithViews();
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(sqliteConnectionString));

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            dbContext.Database.EnsureCreated();
            EnsureDogTableColumns(dbContext);
        }

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseAuthorization();

        app.MapStaticAssets();
        app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
            .WithStaticAssets();

        app.Run();
    }

    private static void EnsureDogTableColumns(AppDbContext dbContext)
    {
        using var connection = dbContext.Database.GetDbConnection();
        connection.Open();

        var existingColumns = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        using (var command = connection.CreateCommand())
        {
            command.CommandText = "PRAGMA table_info(\"Dogs\");";

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                existingColumns.Add(reader.GetString(1));
            }
        }

        if (!existingColumns.Contains("Description"))
        {
            using var command = connection.CreateCommand();
            command.CommandText = "ALTER TABLE \"Dogs\" ADD COLUMN \"Description\" TEXT NOT NULL DEFAULT '';";
            command.ExecuteNonQuery();
        }

        if (!existingColumns.Contains("ImagePath"))
        {
            using var command = connection.CreateCommand();
            command.CommandText = "ALTER TABLE \"Dogs\" ADD COLUMN \"ImagePath\" TEXT NULL;";
            command.ExecuteNonQuery();
        }
    }

    private static string BuildSqliteConnectionString(ConfigurationManager configuration, string contentRootPath)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") ?? "Data Source=dogs.db";
        var connectionStringBuilder = new SqliteConnectionStringBuilder(connectionString);

        if (!Path.IsPathRooted(connectionStringBuilder.DataSource))
        {
            connectionStringBuilder.DataSource = Path.Combine(contentRootPath, connectionStringBuilder.DataSource);
        }

        return connectionStringBuilder.ToString();
    }
}
