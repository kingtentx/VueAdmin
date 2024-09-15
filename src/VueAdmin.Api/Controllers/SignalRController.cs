using VueAdmin.Api.SingnalR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace VueAdmin.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SignalRController: ControllerBase
    {
        private readonly IHubContext<ChatHub> _hubContext;

        public SignalRController(IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpPost("messages")]
        public async Task SendMessage(string user, string message)
        {
            // 通过 HubContext 调用客户端的方法
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
