using Bipki.App.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Bipki.App.Features.Admin;

[Route("admin")]
public class AdminController: ControllerBase
{
    public ApplicationOptions appop;
    
    public AdminController(IOptions<ApplicationOptions> opt)
    {
        appop = opt.Value;
    }
    
    [HttpGet("huy")]
    public async Task<ActionResult<string>> GetHuy()
    {
        return "huy";
    }
}