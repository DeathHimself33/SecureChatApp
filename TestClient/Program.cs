using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("<<<<< CLIENT >>>>>");
            Console.WriteLine(">> WRITE 'exit' FOR EXITING PROGRAM");
            TcpClient client = new TcpClient();
            client.Connect("127.0.0.1", 5011);
            NetworkStream stream = client.GetStream();

            while (true)
            {
                Console.Write(">> ");
                string message = Console.ReadLine();
                if(message == "exit")
                {
                    break;
                }

                //Send Message
                byte[] sendBuffer = Encoding.UTF8.GetBytes(message);
                stream.Write(sendBuffer, 0, sendBuffer.Length);
                Console.WriteLine($">> YOU: {message}");

                //Receive Message
                byte[] receiveBuffer = new byte[1024];
                int bytesRead = stream.Read(receiveBuffer, 0, receiveBuffer.Length);
                string response = Encoding.UTF8.GetString(receiveBuffer, 0, bytesRead);
                Console.WriteLine($">> SERVER: {response}");
            }
        }
    }
}
