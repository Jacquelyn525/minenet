using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;


namespace MvcWeb.Hubs;


[SignalRHub]
[AllowAnonymous]
public class ChatHub : Hub {

  [AllowAnonymous]
  public async Task SendMessage(string user, string message) {
    await Clients.All.SendAsync("ReceiveMessage", user, message);
  }
}
