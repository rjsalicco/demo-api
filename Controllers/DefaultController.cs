using System.Net.WebSockets;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Default.Controllers;

[ApiController]
[Route("[controller]")]
public class DefaultController : ControllerBase
{
    private readonly ILogger<DefaultController> _logger;

    public DefaultController(ILogger<DefaultController> logger)
    {
        _logger = logger;
    }

    [Route("/ws")]
    public async Task Get()
    {
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            await Echo(webSocket);
        }
        else
        {
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
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
        catch (ArgumentNullException ane)
        {
            _logger.LogInformation(ane.Message);
        }

        return claimValue;
    }

    private static async Task Echo(WebSocket webSocket)
    {
        var buffer = new byte[1024 * 4];
        var receiveResult = await webSocket.ReceiveAsync(
            new ArraySegment<byte>(buffer), CancellationToken.None);

        while (!receiveResult.CloseStatus.HasValue)
        {
            await webSocket.SendAsync(
                new ArraySegment<byte>(buffer, 0, receiveResult.Count),
                receiveResult.MessageType,
                receiveResult.EndOfMessage,
                CancellationToken.None);

            receiveResult = await webSocket.ReceiveAsync(
                new ArraySegment<byte>(buffer), CancellationToken.None);
        }

        await webSocket.CloseAsync(
            receiveResult.CloseStatus.Value,
            receiveResult.CloseStatusDescription,
            CancellationToken.None);
    }

}