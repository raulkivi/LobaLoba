using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Lobabot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemController : ControllerBase
    {
        private readonly IHubContext<LogHub> _hubContext;

        public SystemController(IHubContext<LogHub> hubContext)
        {
            _hubContext = hubContext;
        }

        // POST api/system/log
        [HttpPost("log")]
        public async Task<IActionResult> PostLog([FromBody] string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return BadRequest("Message cannot be empty.");
            }

            // Send the message to all connected clients
            await _hubContext.Clients.All.SendAsync("ReceiveLog", message);

            return Ok(new { status = "Message sent", message });
        }
    }
}
