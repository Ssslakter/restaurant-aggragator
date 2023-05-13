using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace SignalRAuthenticationSample.Hubs;

[Authorize]
public class NotificationsHub : Hub
{
}