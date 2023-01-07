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

    [Authorize(Policy = "Admin")]
    [Route("/GetAdmin")]
    public String GetAdmin()
    {
        return "Admin";
    }

    [Authorize(Policy = "Representative")]
    [Route("/GetRepresentative")]
    public String GetRepresentative()
    {
        return "Representative";
    }

    [Authorize(Policy = "Either")]
    [Route("/GetEither")]
    public String GetEither()
    {
        return "Admin or Representative";
    }

    [Authorize]
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