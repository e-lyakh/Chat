using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace WCF_TCP_Chat
{
    [DataContract]
    class ChatUser
    {
        [DataMember]
        public string UserName { get; set; }
        public override string ToString()
        {
            return UserName;
        }
    }
}
