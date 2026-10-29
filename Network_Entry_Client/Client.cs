using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Network_Entry_Client
{
    internal class Client
    {
        static void Main(string[] args)
        {
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint IPEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8888);

            try
            {
                clientSocket.Connect(IPEndPoint);
                Console.WriteLine("Введіть число від 1 до 100 (або 'exit' для виходу)");

                byte[] buffer = new byte[1024];

                while (true)
                {
                    Console.Write("> ");
                    string? input = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(input)) continue;

                    byte[] data = Encoding.UTF8.GetBytes(input);
                    clientSocket.Send(data);

                    if (input.ToLower() == "exit") break;

                    int bytes = clientSocket.Receive(buffer);
                    string response = Encoding.UTF8.GetString(buffer, 0, bytes);
                    Console.WriteLine(response);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }
            finally
            {
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
            }
        }
    }
}
