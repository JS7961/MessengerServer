using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatsonTcp;

namespace MessengerServer
{




    internal class Server
    {





        public delegate void ServerDelegate();
        public   ServerDelegate MessageReceived;


        public   bool Gestartet;

          
          
        public   WatsonTcpServer server1;

        Dictionary<Guid, string> IpListe;

        Dictionary<string, Account> AccountList;

        


        public Server(Dictionary<Guid, string> IpListe, Dictionary<string, Account> AccountList)
        {
            Gestartet = false;
            this.IpListe = IpListe;
            this.AccountList = AccountList;
            //Init();
        }


        public  void Init()
        {


            server1 = new WatsonTcpServer("127.0.0.1",1000);

            


            server1.Events.ClientConnected += ClientConnected;

            server1.Events.ClientDisconnected += ClientDisconnected;


            server1.Events.MessageReceived += ClientMessageReceived;



        }

        public void Start()
        {
            server1.Start();
            Gestartet = true;

        }


        public void Stop()
        {
            server1.Stop();
            Gestartet = false;
        }


          void ClientConnected(object sender, ConnectionEventArgs e)
          {
            Console.WriteLine("\n Verbunden");
           
          }

          void ClientDisconnected(object sender, DisconnectionEventArgs e)
          {
            if (IpListe.ContainsKey(e.Client.Guid) == true)
            {
                AccountList[IpListe[e.Client.Guid]].CurrentIp = string.Empty;
                AccountList[IpListe[e.Client.Guid]].AccountGuid = Guid.Empty;
            }
            
        }

          void ClientMessageReceived(object? sender, MessageReceivedEventArgs e)
          {
            string ausgabe = Encoding.UTF8.GetString(e.Data);


            InvestigateRequest(e.Client.IpPort, e.Client.Guid ,ausgabe);
            //MessageReceived.Invoke();

          }




          bool Send(string IpundPort,Guid Guid1, string wort)
          {
            

            if (server1.IsClientConnected(Guid1) == true)
            {
                server1.SendAsync(Guid1, wort );
                return true;
            }
            else
            {
                return false;
            }

          }

        


        void DeliverMessages(string ReceiverAccountNumber)
        {
            bool Erg = false;

            if (AccountList.ContainsKey(ReceiverAccountNumber) == true)
            {
                if (AccountList[ReceiverAccountNumber].AccountGuid != Guid.Empty)
                {

                    while(AccountList[ReceiverAccountNumber].MessageList.Count > 0)
                    {
                        Erg = Send(AccountList[ReceiverAccountNumber].CurrentIp, AccountList[ReceiverAccountNumber].AccountGuid, "GetMessage1414\n" + AccountList[ReceiverAccountNumber].MessageList[0].SenderAccountNumber + "\n" + AccountList[ReceiverAccountNumber].MessageList[0].TextContent + "\n");

                        if (Erg == true)
                        {
                            AccountList[ReceiverAccountNumber].MessageList.RemoveAt(0);
                        }
                        else
                        {
                            AccountList[ReceiverAccountNumber].AccountGuid = Guid.Empty;
                            break;
                        }
                    }
                    


                }
            }
        }


          void InvestigateRequest(string SenderIp, Guid ClientGuid, string ausgabe)
          {
            Console.WriteLine(ausgabe);
            Console.WriteLine("\n\n\n");

            string zeile = "\n";
            int zeileanfang = 0;
            List<string> Nachricht = new List<string>();

            if (ausgabe[ausgabe.Length - 1] != zeile[0])
            {
                ausgabe += zeile;
            }

            for (int i = 0; i < ausgabe.Length; i++)
            {

                if (ausgabe[i] == zeile[0])
                {
                    Nachricht.Add(ausgabe.Substring(zeileanfang, (i - zeileanfang)));
                    zeileanfang = i + 1;

                }

            }

            


            if (Nachricht[0] == "SendMessage1313")
            {
                string Text = "";
                string SenderAccountNumber = Nachricht[1];
                string ReceiverAccountNumber = Nachricht[2];
                string SenderPassword = Nachricht[3];

                for (int i = 4; i < Nachricht.Count; i++)
                {
                    Text += Nachricht[i];
                }

                /* if (IpListe.ContainsKey(AccountNumber) == true)
                 {
                     IpListe[AccountNumber] = SenderIp;
                 }
                 else
                 {
                     IpListe.Add(AccountNumber, SenderIp);
                 }*/

                if (AccountList.ContainsKey(SenderAccountNumber) == true)
                {

                    if (AccountList[SenderAccountNumber].Password == SenderPassword)
                    {

                        if (AccountList.ContainsKey(ReceiverAccountNumber) == true)
                        {
                            AccountList[ReceiverAccountNumber].MessageList.Add(new Message(SenderAccountNumber, Text));

                            DeliverMessages(ReceiverAccountNumber);

                        }
                        
                    }
                }
            }


            if (Nachricht[0] == "GetMessage1313")
            {
                string Text = "";              
                string ReceiverAccountNumber = Nachricht[1];
                string ReceiverPassword = Nachricht[2];
                bool Erg=false;
                

                if (AccountList.ContainsKey(ReceiverAccountNumber) == true)
                {
                    if (AccountList[ReceiverAccountNumber].Password == ReceiverPassword)
                    {

                        if (AccountList[ReceiverAccountNumber].MessageList.Count > 0)
                        {
                            Erg = Send(SenderIp,ClientGuid, "GetMessage1414\n" + AccountList[ReceiverAccountNumber].MessageList[0].SenderAccountNumber + "\n" + AccountList[ReceiverAccountNumber].MessageList[0].TextContent + "\n");

                            if (Erg == true)
                            {
                                AccountList[ReceiverAccountNumber].MessageList.RemoveAt(0);
                            }
                        }
                    }
                }
                

            }




            if (Nachricht[0] == "CreateAccount100")
            {
                string Text = "";
                string AccountNumber = Nachricht[1];
                string Password = Nachricht[2];
                bool Erg = false;


                if (AccountList.ContainsKey(AccountNumber) == true)
                {
                    Send(SenderIp,ClientGuid, "CreateAccount100\n"+AccountNumber+"\n"+"Fail\n");
                }
                else
                {
                    AccountList.Add(AccountNumber, new Account(Password) );
                    Send(SenderIp,ClientGuid, "CreateAccount100\n" + AccountNumber + "\n" + "Success\n");
                }
            }


            if (Nachricht[0] == "Login101")
            {                
                string AccountNumber = Nachricht[1];
                string Password = Nachricht[2];                


                if (AccountList.ContainsKey(AccountNumber) == true)
                {
                    if ( AccountList[AccountNumber].Password == Password )
                    {
                        AccountList[AccountNumber].AccountGuid = ClientGuid;

                        if (IpListe.ContainsKey(ClientGuid) == false)
                        {
                            IpListe.Add(ClientGuid, AccountNumber);
                        }
                        else
                        {
                            IpListe[ClientGuid] = AccountNumber;
                        }
                        

                        

                        Send(SenderIp,ClientGuid, "Login101\n" + AccountNumber + "\n" + "Success\n");

                        DeliverMessages(AccountNumber);
                    }
                }
                else
                {                    
                    Send(SenderIp,ClientGuid, "Login101\n" + AccountNumber + "\n" + "Fail\n");
                }
            }

















        }












    }
}
