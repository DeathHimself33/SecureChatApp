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
            int port = 5000;
            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            while (true)
            {
                //Trazi klijenta
                Console.Write("Waiting for a connection...");
                TcpClient client = listener.AcceptTcpClient();
                Console.WriteLine("Connected!");

                //Startuj Thread da handluje klijenta
                Thread t = new Thread(HandleClient);
                t.Start(client);
            }

        }
        static void HandleClient(object obj)
        {
            TcpClient client = (TcpClient)obj;
            NetworkStream stream = client.GetStream();

            //Posalji poruku
            byte[] sendBuffer = Encoding.UTF8.GetBytes("Is anybody there?");
            stream.Write(sendBuffer, 0, sendBuffer.Length);

            //Primaj poruku
            byte[] receiveBuffer = new byte[1024];
            int bytesReceived = stream.Read(receiveBuffer, 0, receiveBuffer.Length);
            string data = Encoding.UTF8.GetString(receiveBuffer, 0, bytesReceived);
        }
    }
}
