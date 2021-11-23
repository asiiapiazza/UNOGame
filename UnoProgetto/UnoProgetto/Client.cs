using Client.Utilis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using UnoGame;
using System.Threading.Tasks;
using Type = UnoGame.Type;

namespace Client
{
    class Client
    {

        public static GameView view;

        //rendere la view una variabile utilizzabile da tutte le classi del client

        public static void StartClient()
        {

            //ip endpoint, sereve un IP e una porta. In questo caso local host
            var ipe = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080);

            //creo una nuova socket che prende l'indirizzo ip dell'endpoint. Le socket possono essere di diversi tipi
            //stream vuol dire che i dati vengono mandati con bit e il protocollo tcp
            var socket = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            //il client è attivo quindi si connette alla socket
            socket.Connect(ipe);


            //inizializzo un reader e un writer per scrivere sulla socket
            var reader = new StreamReader(new NetworkStream(socket));

            //stream reader che legge direttamente dalla SOCKET
            var writer = new StreamWriter(new NetworkStream(socket));
            writer.AutoFlush = true;



            //deve aspettare che il server gli dica che si sia connesso un altro client
            //while true, deve continuare sempre a cercare qualche messaggio del server



            while (true)
            {
                //aggiunto dopo, leggere cosa mi dice il server. Operazione bloccante, se non c'è niente rimane in atesa
                var socketMessage = reader.ReadLine();

                //deserializzo con la classe MESSAGE
                var message = JsonSerializer.Deserialize<Message>(reader.ReadLine());

                //operazione da fare in base al tipo

                switch (message.Type)
                {

                    case Type.START:
                        view.Start();
                        break;
                    case Type.DRAW_CARDS:
                        view.UpdateDecks();
                        break;
                    case Type.WIN:
                        //devo passare il giocatore del CLIENT
                        view.hasWin();
                        break;
                    case Type.LOSE:
                        view.hasLost();
                        break;

                    default:
                        Console.WriteLine($"{message.Type} not supported");
                        break;
                }
            }
        }


    }
}
