using Microsoft.AspNetCore.SignalR;
using SignalRWebApp237.Services;

namespace SignalRWebApp237.Hubs
{
    public class MessageHub : Hub
    {
        private readonly IFileService _fileService;
        private static List<(string room, List<string> users)> rooms = new List<(string room, List<string> users)>();

        public MessageHub(IFileService fileService)
        {
            _fileService = fileService;
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("ReceiveConnectInfo", "User Connected");
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await Clients.Others.SendAsync("ReceiveDisconnectInfo", "User Disconnected");
        }

        public async Task SendMessage(string message, double data)
        {
            await Clients.All.SendAsync("ReceiveMessage", message + "'s Offer is ", data);
        }

        public async Task JoinRoom(string room, string user)
        {
            var findRoom = rooms.FirstOrDefault(r => r.room == room);
            if (findRoom != default && findRoom.users.Count > 3) {
                await Clients.Client(Context.ConnectionId).SendAsync("ReceiveFullRoomInfo", user);
                return;
            }
            if (findRoom == default)
            {
                rooms.Add((room, new List<string> { user }));
            }
            else
            {
                findRoom.users.Add(user);  
            }
            await Groups.AddToGroupAsync(Context.ConnectionId, room);
            await Clients.OthersInGroup(room).SendAsync("ReceiveJoinInfo", user);

        }
        public async Task LeaveRoom(string room, string user)
        {
            var findRoom = rooms.FirstOrDefault(r => r.room == room);

            if (findRoom != default)
            {
                findRoom.users.Remove(user);

                if (findRoom.users.Count == 0)
                {
                    rooms.Remove(findRoom);
                }

                await Groups.RemoveFromGroupAsync(Context.ConnectionId, room);

                await Clients.OthersInGroup(room).SendAsync("ReceiveLeaveInfo", user);
            }
        }

        public async Task SendMessageRoom(string room,string user)
        {
            await Clients.OthersInGroup(room).SendAsync("ReceiveInfoRoom", user, await _fileService.Read(room));
        }
    }
}
