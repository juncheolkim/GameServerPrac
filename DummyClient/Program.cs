﻿using System.Net;
using System.Net.Sockets;
using System.Text;

namespace DummyClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // DNS (Domain Name System)
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

            // 휴대폰 설정
            Socket socket = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
            // 문지기한테 입장 문의
            socket.Connect(endPoint);
            Console.WriteLine($"Conneted To {socket.RemoteEndPoint.ToString()}");

            // 보낸다
            byte[] sendBuffer = Encoding.UTF8.GetBytes("Hello World!");
            int sendBytes = socket.Send(sendBuffer);

            // 받는다
            byte[] recvBuff = new byte[1024];
            int recvBytes = socket.Receive(recvBuff);
            string recvData = Encoding.UTF8.GetString(recvBuff, 0, recvBytes);
            Console.WriteLine($"[from server]: {recvData}");

            // 나간다
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}