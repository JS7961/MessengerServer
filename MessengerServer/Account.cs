using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerServer
{


    



    internal class Account
    {

        public List<Message> MessageList;

        public string Password,CurrentIp;

        public Guid AccountGuid;

        public Account(string password)
        {
            MessageList = new List<Message>();
            Password = password;
            CurrentIp = string.Empty;
        }




        












    }







}
