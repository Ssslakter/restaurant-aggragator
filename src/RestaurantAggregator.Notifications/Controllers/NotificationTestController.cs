using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using RestaurantAggregator.Core.Data.DTO;
using SignalRAuthenticationSample.Hubs;

namespace SignalRAuthenticationSample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationTestController : ControllerBase
    {
        private IHubContext<NotificationsHub> HubContext { get; }

        public NotificationTestController(IHubContext<NotificationsHub> hubContext)
        {
            HubContext = hubContext;
        }

        [HttpPost("send")]
        public async Task SendNotification(OrderStatusNotification notification)
        {
            await HubContext.Clients.User(notification.ClientId.ToString())
            .SendAsync("ReceiveMessage",
                        $"{DateTime.UtcNow:s} UTC: {notification.OrderNumber} {notification.Status}");
        }
    }
}
