using UnoGame.Controllers;
using UnoGame.Models;
using UnoGame.Views;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;


namespace UnoGame
{
    class Server
    {
        public static void Main(string[] args)
        {
            int nP = 3;
            var model = new GameModel();
            var controller = new GameController(model);
            var ipe = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080);
            var listener = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(ipe);
            listener.Listen(10);
            Console.WriteLine("Server listening on port 8080");

            //N tasks a seconda del numero di player
            var tasks = new Task[nP];
    
            for (int i = 0; i < nP; i++)
            {
  
                var socket = listener.Accept();
                Console.WriteLine($"Player {i} connected");

                var view = new PlayerView(socket, controller);
                model.AddView(view);
                controller.AddView(view);

                //viene passato il metodo per la ricezione dei messaggi
                tasks[i] = Task.Run(view.Run);
            }

            controller.Start();

            //attendo che tutti i task finiscano
            Task.WaitAll(tasks);
        }
    }
}
