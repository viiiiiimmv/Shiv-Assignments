using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeFirst.Models;

public class Course_1
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; set; }
    
    [Required]
    [Column("STitle",TypeName = "varchar(150)")]
    public string Title { get; set; }
    
    [Required]
    [MaxLength(220)]
    public string Description { get; set; }
    
    public float FullPrice { get; set; }
    public Author_1 Author { get; set; }
    
    [ForeignKey("Author")]
    public int AuthorId { get; set; }
    
}