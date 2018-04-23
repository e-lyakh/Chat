using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace WCF_TCP_Chat
{
    interface ISendToClient
    {
        [OperationContract(IsOneWay = true)]
        void NewUserConnected(ChatUser user);

        [OperationContract(IsOneWay = true)]
        void GetAllConnected(ChatUser[] users);

        [OperationContract(IsOneWay = true)]
        void DeliverNewMessage(ChatMessage message);
    }
}
