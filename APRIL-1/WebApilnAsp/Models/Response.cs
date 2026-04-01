namespace WebApilnAsp.Models;

public class Response
{
    public string? Status { get; set; }
    public string? Message { get; set; }
    public IEnumerable<string>? Errors { get; set; }
}
