using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using WebApilnAsp.Data;
using WebApilnAsp.Models;
using WebApilnAsp.Security;
using WebApilnAsp.Services;

namespace WebApilnAsp;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        var jwtSection = Configuration.GetSection(JwtOptions.SectionName);
        var jwtOptions = jwtSection.Get<JwtOptions>()
                         ?? throw new InvalidOperationException(
                             $"Missing configuration section: {JwtOptions.SectionName}");
        var connectionString = GetRequiredConfigurationValue(
            Configuration.GetConnectionString("DefaultConnection"),
            "ConnectionStrings:DefaultConnection",
            "ConnectionStrings__DefaultConnection");

        EnsureConfigurationValue(jwtOptions.Secret, "JWT:Secret", "JWT__Secret");

        services.AddDbContext<EmpContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddOptions<JwtOptions>()
            .Bind(jwtSection)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<BootstrapAdminOptions>()
            .Bind(Configuration.GetSection(BootstrapAdminOptions.SectionName));

        services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            })
            .AddEntityFrameworkStores<EmpContext>()
            .AddDefaultTokenProviders();

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.Events = new JwtBearerEvents
                {
                    OnChallenge = async context =>
                    {
                        if (context.Response.HasStarted)
                        {
                            return;
                        }

                        context.HandleResponse();
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsJsonAsync(new Response
                        {
                            Status = "Error",
                            Message = "A valid bearer token is required to access this endpoint."
                        });
                    },
                    OnForbidden = async context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        await context.Response.WriteAsJsonAsync(new Response
                        {
                            Status = "Error",
                            Message = "You do not have permission to perform this action."
                        });
                    }
                };

                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtOptions.ValidIssuer,
                    ValidateAudience = true,
                    ValidAudience = jwtOptions.ValidAudience,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    NameClaimType = ClaimTypes.Name,
                    RoleClaimType = ClaimTypes.Role,

                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtOptions.Secret)
                    ),
                    ClockSkew = TimeSpan.Zero
                };
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy(AppPolicies.AuthenticatedUser, policy => policy.RequireAuthenticatedUser());
            options.AddPolicy(AppPolicies.AdminOnly, policy => policy.RequireRole(AppRoles.Admin));
            options.AddPolicy(AppPolicies.EmployeeRead, policy => policy.RequireRole(AppRoles.Admin, AppRoles.Hr));
            options.AddPolicy(AppPolicies.EmployeeWrite, policy => policy.RequireRole(AppRoles.Admin, AppRoles.Hr));
        });

        services.AddHttpContextAccessor();
        services.AddControllersWithViews();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(option =>
        {
            option.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Auth API",
                Version = "v1"
            });

            option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Paste only the JWT token here. Swagger UI adds the Bearer prefix automatically.",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer"
            });

            option.AddSecurityRequirement(_ => new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecuritySchemeReference("Bearer", null, null),
                    []
                }
            });
        });
        services.AddScoped<IEmployee, EmployeeService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddHttpClient("EmployeeAPI");
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (!IsEfDesignTime())
        {
            try
            {
                using var scope = app.ApplicationServices.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<EmpContext>();
                dbContext.Database.Migrate();

                IdentitySeeder.SeedIdentityAsync(app.ApplicationServices).GetAwaiter().GetResult();
            }
            catch (SqlException exception)
            {
                throw new InvalidOperationException(
                    "Unable to connect to SQL Server using ConnectionStrings:DefaultConnection. " +
                    "Set the connection string in user secrets or the ConnectionStrings__DefaultConnection environment variable, " +
                    "then make sure the Docker container named 'mssql' is running on port 1433.",
                    exception);
            }
        }

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });
    }

    private static bool IsEfDesignTime()
    {
        var commandLineArgs = Environment.GetCommandLineArgs();

        return AppDomain.CurrentDomain.GetAssemblies()
                   .Any(assembly => assembly.GetName().Name == "Microsoft.EntityFrameworkCore.Design")
               || commandLineArgs.Any(arg => arg.Contains("ef.dll", StringComparison.OrdinalIgnoreCase))
               || string.Equals(
                   Environment.GetEnvironmentVariable("DOTNET_EF_DESIGNTIME"),
                   "1",
                   StringComparison.Ordinal);
    }

    private static string GetRequiredConfigurationValue(string? value, string configPath, string environmentVariable)
    {
        if (string.IsNullOrWhiteSpace(value) || IsPlaceholderValue(value))
        {
            throw new InvalidOperationException(
                $"Set '{configPath}' in configuration or the '{environmentVariable}' environment variable before running the application.");
        }

        return value;
    }

    private static void EnsureConfigurationValue(string? value, string configPath, string environmentVariable)
    {
        _ = GetRequiredConfigurationValue(value, configPath, environmentVariable);
    }

    private static bool IsPlaceholderValue(string value)
    {
        return value.Contains("REPLACE_WITH", StringComparison.OrdinalIgnoreCase)
               || value.Contains("<set", StringComparison.OrdinalIgnoreCase);
    }

}
