using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerServer
{
    internal class Main
    {


        Server Server1;

        Dictionary<Guid, string> IpListe ;

        Dictionary<string, Account> AccountList;


        public void Run()
        {

            IpListe = new Dictionary<Guid, string>();

            AccountList = new Dictionary<string, Account>();

            Server1 = new Server(IpListe, AccountList);

            string wort = "";


            Server1.Init();
            Server1.Start();

            Console.WriteLine("\n  Gestartet \n");



            while (true)
            {
            

                wort = Console.ReadLine();

                if (wort == "1")
                {
                    //PrintIpList();
                }

                if (wort == "2")
                {
                    PrintMList();
                }




            }



        }



       /* void PrintIpList()
        {
            Console.WriteLine("\n Liste: \n");
            foreach (KeyValuePair<string, string> author in IpListe)
            {
                Console.WriteLine("Key:  {0}, Value:  {1}",
                    author.Key, author.Value);
            }

            Console.WriteLine("\n\n");
        }*/


        void PrintMList()
        {
            Console.WriteLine("\n Liste: \n");
            
           
            var Al = AccountList.Values.ToList();
            var Kl = AccountList.Keys.ToList();

            for (int i = 0; i < Kl.Count; i++)
            {
                Console.WriteLine(Kl[i]+"     " + Al[i].Password);
                
                
                    for (int j = 0; j < Al[i].MessageList.Count; j++)
                    {
                        Console.WriteLine("\n\n " + Al[i].MessageList[j].SenderAccountNumber + "\n " + Al[i].MessageList[j].TextContent);
                    }
                
            }


            Console.WriteLine("\n\n");
        }






    }
}
