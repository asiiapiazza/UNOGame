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
using TypeCard = UnoGame.TypeCard;
using UnoGame.Models;

namespace Client
{
    public class Client
    {
        //view di un player
      
        //rendere la view una variabile utilizzabile da tutte le classi del client

        public static void StartClient()
        {
            var view = new GameView();

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
                var data = reader.ReadLine();

                //deserializzo con la classe MESSAGE
                var message = JsonSerializer.Deserialize<Message>(data);

                //operazione da fare in base al tipo

                switch (message.Type)
                {

                    //messaggi per la parte grafica
                    case TypeCard.START:
                        view.Start();
                        break;

                        //SELEZIONE DELLA CARTA
                    case TypeCard.NEXT_TURN:
                        var selectedCard = view.SelectCard();
                        writer.WriteLine(JsonSerializer.Serialize(new Message { Type = TypeCard.NEXT_TURN, Card = selectedCard }));
                        break;
             
                    //PESCARE DELLE CARTE
                    case TypeCard.DRAW_CARDS:
                        view.UpdateDecks();
                        
                        break;
                    case TypeCard.WIN:
                        //devo passare il giocatore del CLIENT
                        view.hasWon();
                        break;
                    case TypeCard.LOSE:
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
