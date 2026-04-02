using System.Collections;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DBFirst.Views.Northwind;

public class SpainCustomers : PageModel, IEnumerable
{
    public void OnGet()
    {
        
    }

    public IEnumerator GetEnumerator()
    {
        throw new NotImplementedException();
    }
}