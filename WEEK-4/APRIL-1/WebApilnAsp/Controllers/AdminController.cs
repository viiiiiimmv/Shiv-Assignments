using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApilnAsp.Security;

namespace WebApilnAsp.Controllers;


[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = AppPolicies.AdminOnly)]
public class AdminController : ControllerBase
{
    // GET
    [HttpGet("employees")]
    public IEnumerable<string> Get()
    {
        return new List<string>{"John", "Steve", "Nancy"};
    }
    
}
