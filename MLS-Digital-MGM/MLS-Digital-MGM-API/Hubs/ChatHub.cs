// using System.Collections.Concurrent;
// using DataStore.Core.Models;
// using DataStore.Persistence.Interfaces;
// using Microsoft.AspNetCore.SignalR;

// namespace MLS_Digital_MGM_API.Hubs;

// public class ChatHub : Hub
// {
//     private readonly IRepositoryManager repositoryManager;
//     private static readonly ConcurrentDictionary<int, ConcurrentQueue<Message>> ThreadMessages = new();

//     public ChatHub(IRepositoryManager repositoryManager)
//     {
//         this.repositoryManager = repositoryManager;
//     }

//     public async Task JoinThread(int threadId)
//     {
//         // Check if the thread exists
//         var thread = await repositoryManager.ThreadRepository.GetByIdAsync(threadId);
//         if (thread == null)
//         {
//             await Clients.Caller.SendAsync("Error", "Thread does not exist");
//             return;
//         }

//         await Groups.AddToGroupAsync(Context.ConnectionId, threadId.ToString());

//         // Load and send previous messages when joining the thread
//         var previousMessages = await repositoryManager.MessageRepository.GetAllAsync(
//             m => m.ThreadId == threadId,
//             query => query.OrderByDescending(m => m.Timestamp),
//             5
//         );

//         // Now we can use LINQ methods on the 'messages' variable
//         var orderedMessages = previousMessages.OrderByDescending(m => m.Timestamp).ToList();

//         await Clients.Caller.SendAsync("LoadPreviousMessages", previousMessages);
//     }

//     public async Task LeaveThread(int threadId)
//     {
//         await Groups.RemoveFromGroupAsync(Context.ConnectionId, threadId.ToString());
//     }

//     public async Task SendMessageToThread(int threadId, string messageText)
//     {
//         //get the current user id

//         var user = Context.UserIdentifier;
//         var userDb = await repositoryManager.UserRepository.GetSingleUser(user);

//         if(userDb == null)
//         {
//             await Clients.Caller.SendAsync("Error", "You are not authorized to send messages");
//             return;
//         }

//         //
        
//         // Check if the thread exists
//         var thread = await repositoryManager.ThreadRepository.GetByIdAsync(threadId);
//         if (thread == null)
//         {
//             await Clients.Caller.SendAsync("Error", "Thread does not exist");
//             return;
//         }

//         var message = new Message
//         {
//             ThreadId = threadId,
//             Content = messageText,
//             Timestamp = DateTime.UtcNow,
//             CreatedById = Context.UserIdentifier // You'll need to set this appropriately
//         };

//         await repositoryManager.MessageRepository.AddAsync(message);
//         await repositoryManager.UnitOfWork.CommitAsync();

//         await Clients.Group(threadId.ToString()).SendAsync("ReceiveMessage", message);
//     }

//     // Other methods...
// }