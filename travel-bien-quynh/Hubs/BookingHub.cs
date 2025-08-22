using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace travel_bien_quynh.Hubs
{
    public class BookingHub : Hub
    {
        public async Task SendBookingNotification(string userName, string service)
        {
            await Clients.All.SendAsync("ReceiveBooking", userName, service);
        }
    }
}
