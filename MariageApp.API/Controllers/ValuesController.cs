using System.Threading.Tasks;
using MariageApp.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MariageApp.API.Controllers;
[Authorize]
[ApiController]
[Route("[controller]")]
public class ValuesController : ControllerBase
{
    private readonly   DataContext _context;
    public ValuesController(DataContext context)
    {
        _context = context;

    }

  [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetValues()
    {

        var values= await _context.Values.ToListAsync();

        return Ok(values);
    }
    [AllowAnonymous]
    [HttpGet("{n}")]
    public async Task<IActionResult> GetValue(int n)
    {

       var value=await _context.Values.FirstOrDefaultAsync(x=>x.id==n);
       return Ok(value);

    }
}
