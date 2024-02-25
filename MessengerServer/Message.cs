using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerServer
{


    internal class Message
    {

      public string SenderAccountNumber,TextContent;


        public Message(string SenderAccountNumber, string TextContent)
        {
            this.SenderAccountNumber = SenderAccountNumber;            
            this.TextContent = TextContent; 
        }


    }
}
