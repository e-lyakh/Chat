using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Threading;

namespace WCF_TCP_Chat
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, 
        ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    class ChatService : IChatService
    {
        public ChatService()
        {            
            callbackList = new Dictionary<string, ISendToClient>();
            connectedUsers = new List<ChatUser>();            
        }
        
        private Dictionary<string, ISendToClient> callbackList;
        private List<ChatUser> connectedUsers;             

        public void ClientConnect(ChatUser user)
        {
            var exists =
               from ChatUser e in connectedUsers
               where e.UserName == user.UserName
               select e;

            if(exists.Count()==0)
            {
                connectedUsers.Add(user);
                Console.WriteLine("({0}) New user connected - {1}", DateTime.Now.ToString(), user.UserName);
                ISendToClient callback = OperationContext.Current.GetCallbackChannel<ISendToClient>();                
                callbackList.Add(user.UserName, callback);                
                ThreadPool.QueueUserWorkItem(InitUsersList, callback);
                ThreadPool.QueueUserWorkItem(SendNewUserInfo, user);
                ChatMessage enterMessage = new ChatMessage()
                {
                    User = user,
                    Date = DateTime.Now,
                    Message = "I connected"
                };
                SendNewMessage(enterMessage);
            }            
        }        
        private void InitUsersList(object callback)
        {
            if (connectedUsers.Count() > 1)
                (callback as ISendToClient).GetAllConnected(connectedUsers.ToArray());
        }
        private void SendNewUserInfo(object user)
        {
            ChatUser u = user as ChatUser;
            foreach (var cb in callbackList)               
                cb.Value.NewUserConnected(u);
        }

        public void SendNewMessage(ChatMessage newMessage)
        {
            ISendToClient callback = OperationContext.Current.GetCallbackChannel<ISendToClient>();            
            ThreadPool.QueueUserWorkItem(DeliverMessage, newMessage);
                                   
            Console.WriteLine($"{newMessage.ToString()}");
        }

        private void DeliverMessage(object newMessage)
        {
            ChatMessage message = newMessage as ChatMessage;
            foreach (var cb in callbackList)               
                cb.Value.DeliverNewMessage(message);            
        }        

        public void RemoveUser(ChatUser user)
        {
            var r = connectedUsers.First(u => u.UserName == user.UserName);
            connectedUsers.Remove(r);
            foreach(var cb in callbackList)
            {
                if (cb.Key == user.UserName)
                {
                    callbackList.Remove(user.UserName);
                    break;
                }                    
            }
            foreach (var cb in callbackList)
            {
                cb.Value.GetAllConnected(connectedUsers.ToArray());
            }

            ChatMessage exitMessage = new ChatMessage()
            {
                User = user,
                Date = DateTime.Now,
                Message = "I disconnected"
            };
            SendNewMessage(exitMessage);
            Console.WriteLine($"({DateTime.Now.ToString()}) User {user.UserName} logged out");
        }
    }
}
