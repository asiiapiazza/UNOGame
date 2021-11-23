using UnoGame.Controllers;
using UnoGame.Models;
using UnoGame.Views;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;


namespace UnoGame
{
    class Program
    {
        private static void Main(string[] args)
        {
           
            var model = new GameModel();
            var controller = new GameController(model);
            var ipe = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080);
            var listener = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(ipe);
            listener.Listen(10);
            Console.WriteLine("Server listening on port 8080");

            //ho bisogno di dude task: uno per la comunicazione con client0 e una con client1
            //creo un array di tasks
            var tasks = new Task[1];

            //ciclo dove ascolto per due volte
            
            for (int i = 0; i < 1; i++)
            {
                //si blocca finche un client non si è connesso alla socket
                //appena un client si connette alla socket, non è bloccante e passo alla riga di codice successiva
                var socket = listener.Accept();
                Console.WriteLine($"Player {i} connected");

                //virtualview = visione che ho io del client, sto creando la classe player
                var view = new PlayerView(socket, controller);

                //aggiungo questa view al mio model
                model.AddView(view);

                //aggiungo al controller che il player
                controller.AddView(view);

                //assegno il task. questa visione virtuale del giocatore si mette in attesa pronta
                //ad ascoltare i messaggi da parte del client
                tasks[i] = Task.Run(view.Run);
            }

            //faccio partire il controller
            controller.Start();

            //attendo che tutti i task finiscano
            Task.WaitAll(tasks);
        }
    }
}
