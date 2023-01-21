using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DemoAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class DefaultController : ControllerBase
{
    private readonly ILogger<DefaultController> _logger;

    public DefaultController(ILogger<DefaultController> logger)
    {
        _logger = logger;
    }

    [Route("/GetAdmin")]
    public String GetAdmin()
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        return "Admin";
    }

    [Route("/GetRepresentative")]
    public String GetRepresentative()
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        return "Representative";
    }

    [Route("/GetEither")]
    public String GetEither()
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        return "Admin or Representative";
    }

    [Route("/GetAnyClaim/{claim}")]
    public String GetAny(string claim)
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        string? claimValue = "";
        
        try
        {
            claimValue = identity?.FindFirst(claim)?.Value;
        }
        catch(ArgumentNullException ane)
        {
            _logger.LogInformation(ane.Message);
        }

        return claimValue;
    }

}