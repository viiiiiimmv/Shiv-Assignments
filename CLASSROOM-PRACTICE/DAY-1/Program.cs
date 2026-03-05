namespace DAY_1;
using System;
using System.Collections.Generic;

/* ================= INTERFACES ================= */

public interface ISong
{
    string Title { get; set; }
    string Artist { get; set; }
    int Duration { get; set; }
}

public interface IPlaylist
{
    void AddSong(ISong song);
    void RemoveSong(string title);
    List<ISong> GetSongs();
    List<ISong> SearchSongs(string query);
    int GetTotalSongCount();
    int GetTotalDuration();
}

/* =================================================
   IMPLEMENT BELOW CLASSES YOURSELF

   class Song : ISong
   class Playlist : IPlaylist
   ================================================= */

class Song : ISong
{
    public string Title { get; set; }
    public string Artist { get; set; }
    public int Duration { get; set; }

    public Song()
    {
        Title = "";
        Artist = "";
        Duration = 0;
    }

    public Song(string title, string artist, int duration)
    {
        Title = title;
        Artist = artist;
        Duration = duration;
    }
}

class Playlist : IPlaylist
{
    private List<ISong> songs = new List<ISong>();

    public void AddSong(ISong song)
    {
        songs.Add(song);
    }

    public void RemoveSong(string title)
    {
        songs.RemoveAll(s => s.Title.Equals(title,StringComparison.OrdinalIgnoreCase));
    }

    public List<ISong> GetSongs()
    {
        return songs;
    }

    public List<ISong> SearchSongs(string query)
    {
        return songs.Where(s =>
            s.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
            s.Artist.Contains(query, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public int GetTotalSongCount()
    {
        return songs.Count;
    }

    public int GetTotalDuration()
    {
        return songs.Sum(s => s.Duration);
    }
    
}


/* ================= MAIN FUNCTION ================= */

class Program
{
    static void Main(string[] args)
    {
        IPlaylist playlist = new Playlist();

        playlist.AddSong(new Song("Believer", "Imagine Dragons", 204));
        playlist.AddSong(new Song("Perfect", "Ed Sheeran", 210));
        playlist.AddSong(new Song("Thunder", "Imagine Dragons", 187));
        playlist.AddSong(new Song("Shape of You", "Ed Sheeran", 230));

        Console.WriteLine("Total Songs:");
        Console.WriteLine(playlist.GetTotalSongCount());

        Console.WriteLine("\nAll Songs:");
        foreach (var s in playlist.GetSongs())
        {
            Console.WriteLine($"{s.Title} - {s.Artist} ({s.Duration}s)");
        }

        Console.WriteLine("\nSearch Result (Imagine):");
        var results = playlist.SearchSongs("Imagine");

        foreach (var s in results)
        {
            Console.WriteLine(s.Title);
        }

        playlist.RemoveSong("Perfect");

        Console.WriteLine("\nAfter Removing 'Perfect':");
        foreach (var s in playlist.GetSongs())
        {
            Console.WriteLine(s.Title);
        }

        Console.WriteLine("\nTotal Duration:");
        Console.WriteLine(playlist.GetTotalDuration());
    }
}