using System.ComponentModel.DataAnnotations;

namespace AZURE_MVC.Models;

public class ContainerModel
{
    [Required]
    public string Name { get; set; }
}