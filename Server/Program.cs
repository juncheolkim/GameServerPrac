﻿using System.Net;
using System.Net.Sockets;
using System.Text;
using ServerCore;

namespace Server
{
    internal class Program
    {
        static Listener _listener = new Listener();

        static void Main(string[] args)
        {
            // DNS (Domain Name System)
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

            _listener.Init(endPoint, () => { return new ClientSession(); });
            Console.WriteLine("Listening ...");
            while (true)
            {
                ;
            }

        }
    }
}