using System.ComponentModel.DataAnnotations;

namespace WebApilnAsp.Security;

public class JwtOptions
{
    public const string SectionName = "JWT";

    [Required]
    public string ValidAudience { get; set; } = string.Empty;

    [Required]
    public string ValidIssuer { get; set; } = string.Empty;

    [Required]
    [MinLength(32)]
    public string Secret { get; set; } = string.Empty;

    [Range(1, 24)]
    public int ExpiryHours { get; set; } = 2;
}
