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
            try
            {
                stream.Write(sendBuffer, 0, sendBuffer.Length);
                Console.WriteLine($">> YOU: {message}");
            }
            catch(Exception ex)
            {
                Console.Clear();
                Console.WriteLine("CLIENT DISCONNECTED!");
                return;
            }

            //Receive message
            byte[] receiveBuffer = new byte[1024];
            int bytesRead = stream.Read(receiveBuffer, 0, receiveBuffer.Length);
            if(bytesRead == 0)
            {
                return;
            }
            string response = Encoding.UTF8.GetString(receiveBuffer, 0, bytesRead);
            Console.WriteLine($">> CLIENT: {response}");
        }
    }
}
