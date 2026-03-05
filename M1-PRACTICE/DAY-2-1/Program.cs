namespace DAY_2_1;

public interface IFilm
{
    string Title { get; set; } 
    string Director { get; set; } 
    int Year { get; set; }
}

public interface IFilmLibrary
{
    void AddFilm(IFilm film);
    void RemoveFilm(string title);
    List<IFilm> GetFilms();
    List<IFilm> SearchFilms(string query);
    int GetTotalFilmCount();
}

public class Film : IFilm
{
    public string Title { get; set; } = string.Empty;
    public string Director { get; set; } = string.Empty;
    public int Year { get; set; }

    public override string ToString()
    {
        return $"\"{Title}\" by {Director} ({Year})";
    }
}

public class FilmLibrary : IFilmLibrary
{
    private List<IFilm> _films = new List<IFilm>();

    public void AddFilm(IFilm film)
    {
        _films.Add(film); 
        Console.WriteLine($"Added: {film}");
    }

    public void RemoveFilm(string title)
    {
        var film = _films.FirstOrDefault(f => f.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        if (film != null)
        {
            _films.Remove(film);
            Console.WriteLine($"Removed: {film}");
        }
        else
        {
            Console.WriteLine($"Film '{title}' not found.");
        }
    }

    public List<IFilm> GetFilms()
    {
        return new List<IFilm>(_films);  // Return copy
    }

    public List<IFilm> SearchFilms(string query)
    {
        return _films.Where(f =>
            f.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
            f.Director.Contains(query, StringComparison.OrdinalIgnoreCase) ||
            f.Year.ToString().Contains(query)).ToList();
    }

    public int GetTotalFilmCount()
    { 
        return _films.Count;
    }
}

internal class Program
{
    static void Main(string[] args)
    {
            // Create library
        IFilmLibrary library = new FilmLibrary();

            // Add sample films
        library.AddFilm(new Film { Title = "Inception", Director = "Christopher Nolan", Year = 2010 });
        library.AddFilm(new Film { Title = "The Godfather", Director = "Francis Ford Coppola", Year = 1972 }); 
        library.AddFilm(new Film { Title = "Pulp Fiction", Director = "Quentin Tarantino", Year = 1994 }); 
        library.AddFilm(new Film { Title = "Schindler's List", Director = "Steven Spielberg", Year = 1993 });
        library.AddFilm(new Film { Title = "Dunkirk",          Director = "Christopher Nolan",     Year = 2017 });
        library.AddFilm(new Film { Title = "Memento",          Director = "Christopher Nolan",     Year = 2000 });
        library.AddFilm(new Film { Title = "The Dark Knight",  Director = "Christopher Nolan",     Year = 2008 });
        // duplicate title/director pair for Distinct demo
        library.AddFilm(new Film { Title = "Inception",        Director = "Christopher Nolan",     Year = 2010 });
        Console.WriteLine($"\nTotal films: {library.GetTotalFilmCount()}");

            // List all films
        Console.WriteLine("\n--- All Films ---");
        foreach (var film in library.GetFilms())
        {
            Console.WriteLine(film);
        }

            // Search example
        Console.WriteLine("\n--- Search 'Nolan' ---");
        var nolanFilms = library.SearchFilms("Nolan");
        foreach (var film in nolanFilms)
        {
            Console.WriteLine(film);
        }

            // Remove example
        library.RemoveFilm("Pulp Fiction");
        Console.WriteLine($"\nTotal after removal: {library.GetTotalFilmCount()}");

        Console.WriteLine("\n--- Final Films ---");
        foreach (var film in library.GetFilms())
        {
            Console.WriteLine(film);
        }

        Console.ReadKey();
    }
}