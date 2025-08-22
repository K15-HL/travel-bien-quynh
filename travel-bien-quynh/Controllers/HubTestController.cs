using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using travel_bien_quynh.Hubs;

[ApiController]
[Route("api/[controller]")]
public class HubTestController : ControllerBase
{
    private readonly IHubContext<BookingHub> _hubContext;

    public HubTestController(IHubContext<BookingHub> hubContext)
    {
        _hubContext = hubContext;
    }

    [HttpGet("send")]
    public async Task<IActionResult> SendMessage()
    {
        await _hubContext.Clients.All.SendAsync("ReceiveNotification", "Test notification from server");
        return Ok(new { message = "Notification sent" });
    }
}
