namespace Logins
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Logging.AddEventLog();
            builder.Services.AddSingleton<PersonService>();

            var app = builder.Build();

            app.Logger.LogDebug("debug-message");
            app.Logger.LogInformation("info-message");
            app.Logger.LogWarning("warn-message");
            app.Logger.LogError("error-message");
            app.Logger.LogCritical("critical-message");

            var logger = app.Services.GetRequiredService<ILogger<Program>>();
            var personService = app.Services.GetRequiredService<PersonService>();

            logger.LogInformation("=== Starting PersonService Demo ===");

            logger.LogInformation("1. Calling GetAllPersons()");
            var allPersons = personService.GetAllPersons();
            foreach (var person in allPersons)
            {
                logger.LogInformation($"Found person: ID={person.Id}, Name={person.Name}");
            }

            logger.LogInformation("2. Calling GetPersonById(2)");
            var person2 = personService.GetPersonById(2);
            if (person2 != null)
                logger.LogInformation($"Person found: ID={person2.Id}, Name={person2.Name}");
            else
                logger.LogWarning("Person with ID 2 not found!");

            logger.LogInformation("3. Calling AddPerson()");
            var newPerson = new Person { Id = 4, Name = "David" };
            personService.AddPerson(newPerson);
            logger.LogInformation($"Added new person: ID={newPerson.Id}, Name={newPerson.Name}");

            var updatedList = personService.GetAllPersons();
            logger.LogInformation($"Total persons now: {updatedList.Count()}");

            logger.LogInformation("4. Testing GetPersonById(999)");
            var missingPerson = personService.GetPersonById(999);
            if (missingPerson == null)
                logger.LogWarning("Person ID 999 not found (expected)");

            logger.LogInformation("=== Demo Complete ===");

            app.MapGet("/", () => "PersonService Demo - Check console logs!");

            app.Run();
        }
    }
}
