using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace WCF_TCP_Chat
{
    [DataContract]
    class ChatMessage
    {
        [DataMember]
        public ChatUser User { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public DateTime Date { get; set; }
        public override string ToString()
        {
            return $"({Date.ToString()}) {User} says: {Message}";
        }
        public ChatMessage()
        {
            Date = DateTime.Now;
        }
        public ChatMessage(ChatUser user, string message)
        {
            User = user;
            Message = message;
            Date = DateTime.Now;
        }
    }
}
