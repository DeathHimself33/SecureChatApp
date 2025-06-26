using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
namespace SecureChatServer
{
    class Program
    {
        static TcpListener listener;
        public static void Main(string[] args)
        {
            Console.WriteLine("<<<<< SERVER >>>>>");
            Console.WriteLine(">> WAITING FOR A CONNECTION...");
            int port = 5011;
            listener = new TcpListener(IPAddress.Any, port);
            listener.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            listener.Start();
            while (true)
            {
                //Looking for client
                TcpClient client = listener.AcceptTcpClient();
                Console.WriteLine(">> CONNECTED");

                //Handle client with thread
                Thread t = new Thread(HandleClient);
                t.Start(client);
            }

        }
        static void HandleClient(object obj)
        {
            TcpClient client = (TcpClient)obj;
            NetworkStream stream = client.GetStream();

            //Send message
            string message = "Is anybody there?";
            byte[] sendBuffer = Encoding.UTF8.GetBytes(message);
            Console.WriteLine($">> YOU: {message}");
            stream.Write(sendBuffer, 0, sendBuffer.Length);

            //Receive message
            byte[] receiveBuffer = new byte[1024];
            int bytesReceived = stream.Read(receiveBuffer, 0, receiveBuffer.Length);
            string data = Encoding.UTF8.GetString(receiveBuffer, 0, bytesReceived);
            Console.WriteLine($">> CLIENT: {data}");
        }
    }
}
