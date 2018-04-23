using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace WCF_TCP_Chat
{
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(ISendToClient))]
    interface IChatService
    {
        [OperationContract(IsOneWay = true)]
        void ClientConnect(ChatUser user);       

        [OperationContract(IsOneWay = true)]
        void SendNewMessage(ChatMessage newMessage);        

        [OperationContract(IsOneWay = true)]
        void RemoveUser(ChatUser user);
    }
}
