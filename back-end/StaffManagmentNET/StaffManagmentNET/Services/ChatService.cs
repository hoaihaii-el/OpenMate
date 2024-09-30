using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using StaffManagmentNET.Hubs;
using StaffManagmentNET.Models;
using StaffManagmentNET.Repositories;
using StaffManagmentNET.Responses;
using StaffManagmentNET.ViewModels;

namespace StaffManagmentNET.Services
{
    public class ChatService : IChatRepo
    {
        private readonly DataContext _context;

        public ChatService(DataContext context)
        {
            _context = context;
        }


        public async Task AddNewRoom(string input)
        {
            var roomName = "";
            var users = input.Split(',', StringSplitOptions.TrimEntries);
            foreach (var userID in users)
            {
                var user = await _context.Staffs.FindAsync(userID);
                if (user == null)
                {
                    throw new Exception("User not found!");
                }

                roomName += user.StaffName + (users.Length > 2 ? ";" : "");
                if (users.Length <= 2) break;
            }

            if (roomName.Length > 20)
            {
                roomName = roomName.Substring(0, 20) + "...";
            }

            var newRoom = new ChatRoom
            {
                RoomID = Guid.NewGuid().ToString(),
                RoomName = roomName,
            };
            _context.ChatRooms.Add(newRoom);

            foreach (var userID in users)
            {
                _context.ChatRoomsDetail.Add(new ChatRoomDetail
                {
                    RoomID = newRoom.RoomID,
                    StaffID = userID
                });
            }

            await _context.SaveChangesAsync();
        }

        public async Task ChangeRoomName(string roomID, string newName)
        {
            var room = await _context.ChatRooms.FindAsync(roomID);

            if (room == null)
            {
                throw new Exception("Not found!");
            }

            if (newName.Length > 20)
            {
                newName = newName.Substring(0, 20) + "...";
            }
            room.RoomName = newName;
            _context.ChatRooms.Update(room);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<MessageResponse>> GetAllMessage(string roomID)
        {
            var messages = await _context.Messages.Where(m => m.RoomID == roomID)
                .OrderByDescending(m => m.SendTime)
                .ToListAsync();

            var result = new List<MessageResponse>();
            foreach (var message in messages)
            {
                var sender = await _context.Staffs.FindAsync(message.SenderID);
                result.Add(new MessageResponse
                {
                    RoomID = message.RoomID,
                    Content = message.Content,
                    MessageType = message.MessageType,
                    ResourceURL = message.ResourceURL,
                    SenderID = message.SenderID,
                    SenderName = sender!.StaffName
                });
            }
            return result;
        }

        public async Task<IEnumerable<ChatRoom>> GetAllRoom(string staffID)
        {
            var roomDetails = await _context.ChatRoomsDetail
                .Where(r => r.StaffID == staffID)
                .ToListAsync();

            var rooms = new List<ChatRoom>();
            var sendTime = new List<DateTime>();

            foreach (var detail in roomDetails)
            {
                var room = await _context.ChatRooms.FindAsync(detail.RoomID);
                if (room == null) continue;

                var send = await _context.Messages
                    .Where(m => m.RoomID == detail.RoomID)
                    .OrderByDescending(m => m.SendTime)
                    .Select(m => m.SendTime)
                    .FirstOrDefaultAsync();

                if (sendTime.Count == 0)
                {
                    sendTime.Add(send);
                    rooms.Add(room);
                    continue;
                }

                for (int i = 0; i < sendTime.Count; i++)
                {
                    if (send > sendTime[i])
                    {
                        sendTime.Insert(i, send);
                        rooms.Insert(i, room);
                        break;
                    }
                }

                sendTime.Add(send);
                rooms.Add(room);
            }

            return rooms;
        }

        public async Task<IEnumerable<ChatRoomDetail>> GetRoomDetail(string roomID)
        {
            var rooms = await _context.ChatRoomsDetail
                .Where(r => r.RoomID == roomID)
                .ToListAsync();
            return rooms;
        }

        public async Task SendMessage(MessageVM vm)
        {
            var newMess = new Message
            {
                MessageID = Guid.NewGuid().ToString(),
                Content = vm.Content,
                MessageType = vm.MessageType,
                ResourceURL = vm.ResourceURL,
                SenderID = vm.SenderID,
                RoomID = vm.RoomID,
                SendTime = DateTime.Now
            };

            _context.Messages.Add(newMess);
            await _context.SaveChangesAsync();
        }
    }
}
