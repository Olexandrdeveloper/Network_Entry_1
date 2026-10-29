using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Network_Entry
{
    internal class Server
    {
        static void Main(string[] args)
        {
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint IPEndPoint = new IPEndPoint(IPAddress.Any, 8888);
            serverSocket.Bind(IPEndPoint);

            serverSocket.Listen();
            Socket clientSocket = serverSocket.Accept();

            Random rand = new Random();
            int randomNum = rand.Next(1, 101);
            int attempts = 0;

            byte[] buffer = new byte[1024];

            while (true)
            {
                int bytesRead = clientSocket.Receive(buffer);
                if (bytesRead == 0) break;

                string message = Encoding.UTF8.GetString(buffer, 0, bytesRead).Trim();

                if (message.ToLower() == "exit")
                {
                    clientSocket.Send(Encoding.UTF8.GetBytes("Гра завершена!"));
                    break;
                }

                if (!int.TryParse(message, out int guess))
                {
                    clientSocket.Send(Encoding.UTF8.GetBytes("Помилка: Введіть ціле число від 1 до 100."));
                    continue;
                }

                attempts++;

                string response;
                if (guess < randomNum)
                    response = "Більше";
                else if (guess > randomNum)
                    response = "Менше";
                else
                {
                    response = $"Вгадано! Кількість спроб: {attempts}. Загадено нове число від 1 до 100, граємо далі!";
                    randomNum = rand.Next(1, 101);
                    attempts = 0;
                }

                clientSocket.Send(Encoding.UTF8.GetBytes(response));
            }

            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
            serverSocket.Close();
        }
    }
}
