using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Books.DTOs;
using Books.Entities;
using Books.Extensions;
using Books.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;


namespace Books.SignalR
{
    [Authorize]
    public class MessageHub : Hub
    {
       
        private readonly IMessageRepository _messageRepository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IHubContext<PresenceHub> _presenceHub;
        private readonly PresenceTracker _tracker;

        public MessageHub(IMessageRepository messageRepository,
        IMapper mapper, IUserRepository userRepository, 
        IHubContext<PresenceHub> presenceHub, PresenceTracker tracker)
        {
            _tracker = tracker;
            _presenceHub = presenceHub;
            _userRepository = userRepository;
            _mapper = mapper;
            _messageRepository = messageRepository;
        }
        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var otherUser = httpContext.Request.Query["user"].ToString();
            var groupName = GetGroupName(Context.User.GetUsername(), otherUser);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            var group= await AddToGroup( groupName);
            await Clients.Group(groupName).SendAsync("UpdatedGroup", group);
            var messages = await _messageRepository.GetMessageThread(Context.User.GetUsername(), otherUser);
            await Clients.Caller.SendAsync("ReceiveMessageThread", messages);

        }
        private string GetGroupName(string caller, string other)
        {
            var stringCompare = string.CompareOrdinal(caller, other) < 0;
            return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
          var group=  await RemoveFromMessageGroup();
          await Clients.Group(group.Name).SendAsync("UpdatedGroup",group);
            await base.OnDisconnectedAsync(exception);

        }
        public async Task SendMessage(CreateMessageDto createMessageDto)
        {
            var username = Context.User.GetUsername();
            if (username == createMessageDto.RecipientUsername.ToLower())
                throw new HubException("Can't send messages");

            var sender = await _userRepository.GetUserByUsernameAsync(username);
            var recipient = await _userRepository.GetUserByUsernameAsync(createMessageDto.RecipientUsername);
            if (recipient == null) throw new HubException("not found");

            var message = new Message
            {
                Sender = sender,
                Recipient = recipient,
                SenderUsername = sender.UserName,
                RecipientUsername = recipient.UserName,
                Content = createMessageDto.Content
            };
            var groupName = GetGroupName(sender.UserName, recipient.UserName);
            var group = await _messageRepository.GetMessageGroup(groupName);
            if (group.Connections.Any(x => x.Username == recipient.UserName))
            {
                message.DateRead = DateTime.UtcNow;
            }
            else
            {
                var connections = await _tracker.GetConnectionsForUser(recipient.UserName);
                if(connections != null)
                {
                    await _presenceHub.Clients.Clients(connections).SendAsync("NewMessageReceived",
                        new{username = sender.UserName, knownas = sender.KnownAs});
                }
            }

            _messageRepository.AddMessage(message);
            if (await _messageRepository.SaveAllAsync())
            {
                await Clients.Group(groupName).SendAsync("NewMessage", _mapper.Map<MessageDto>(message));
            }

        }
        private async Task<Group> AddToGroup( string groupName)
        {
            var group = await _messageRepository.GetMessageGroup(groupName);
            var connection = new Connection(Context.ConnectionId, Context.User.GetUsername());
            if (group == null)
            {
                group = new Group(groupName);
                _messageRepository.AddGroup(group);

            }
            group.Connections.Add(connection);
            if( await _messageRepository.SaveAllAsync()) return group;
            throw new HubException("Failed to join group");

        }

        private async Task<Group> RemoveFromMessageGroup()
        {
            var group = await _messageRepository.GetGroupForConnection(Context.ConnectionId);
            var connection =  group.Connections.FirstOrDefault(x=>x.ConnectionId==Context.ConnectionId);
            _messageRepository.RemoveConnection(connection);
            if( await _messageRepository.SaveAllAsync()) return group;
            throw new HubException("Failed to remove from group");   
        }
     

        }
    }
