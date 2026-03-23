using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeFirst.Models;

public class Post
{
    [Key]
    public int Id { get; set; }
    public DateTime PublishedAt { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
}