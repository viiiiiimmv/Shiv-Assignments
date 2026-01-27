namespace Logins
{
    public class PersonService
    {
        public readonly ILogger<PersonService> _logger;

        public PersonService(ILogger<PersonService> logger)
        {
            _logger = logger;
        }

        List<Person> persons = new List<Person>()
        {
            new Person(){ Id=1, Name="Alice"},
            new Person(){ Id=2, Name="Bob"},
            new Person(){ Id=3, Name="Charlie"},
        };
        public IEnumerable<Person> GetAllPersons()
        {
            _logger.LogInformation("Fetching all persons");
            return persons;
        }

        public Person? GetPersonById(int id)
        {
            _logger.LogInformation("Fetching person with ID: {Id}", id);
            foreach (Person person in persons)
            {
                if (person.Id == id)
                {
                    return person;
                }
            }
            return null;
        }

        public void AddPerson(Person person)
        {
            _logger.LogInformation("Adding new person with ID: {Id}", person.Id);
            persons.Add(person);
        }
    }
}