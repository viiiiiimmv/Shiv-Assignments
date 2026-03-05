/*
This coding question evaluates interfaces and collection management.

Requirements:

1. Create a Movie class with:
     - Title
     - Genre
     - Year
2. Create MovieLibrary class that:
     - Adds movie
     - Removes movie
     - Searches by title
     - Displays all movies
3. Demonstrate usage in Main().
*/

using System;
using System.Collections.Generic;
using System.Linq;

namespace MovieLibraryApp
{
     public class Movie
     {
          public string Title { get; set; }
          public string Genre { get; set; }
          public int Year { get; set; }

          public Movie(string title, string genre, int year)
          {
               Title = title;
               Genre = genre;
               Year = year;
          }
     }

     public class MovieLibrary
     {
          private List<Movie> movies = new List<Movie>();

          public void AddMovie(Movie movie)
          {
               // TODO
          }

          public void RemoveMovie(string title)
          {
               // TODO
          }

          public Movie SearchByTitle(string title)
          {
               // TODO
               return null;
          }

          public void DisplayAll()
          {
               // TODO
          }
     }

     class Program
     {
          static void Main(string[] args)
          {
               MovieLibrary library = new MovieLibrary();

               library.AddMovie(new Movie("Inception", "Sci-Fi", 2010));
               library.AddMovie(new Movie("Interstellar", "Sci-Fi", 2014));

               library.DisplayAll();

               Console.ReadLine();
          }
     }
}