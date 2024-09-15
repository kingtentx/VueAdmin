using VueAdmin.Data;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.SignalR;

namespace VueAdmin.Api.SingnalR
{
    [EnableCors("allowAllCors")]
    public class ChatHub : Hub
    {
        public override async Task OnConnectedAsync()
        {         
            // 可以添加更多的逻辑，例如记录连接的用户信息等
            var connectionid = Context.ConnectionId;
            //var timespan = Context.GetHttpContext().Request.Query["timespan"];
            await base.OnConnectedAsync();
            Console.WriteLine($"{connectionid}  connect...");
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var connectionid = Context.ConnectionId;
            await base.OnDisconnectedAsync(exception);
            Console.WriteLine($"{connectionid} disconnect...");
        }

      
        public async Task SendMessage(string user, string message)
        {
            Console.WriteLine($"{user} - {message}");
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

    }
}
