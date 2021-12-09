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
using TypeMessage = UnoGame.TypeMessage;
using UnoGame.Models;

namespace Client
{
    public class Client
    {
 

        //rendere la view una variabile utilizzabile da tutte le classi del client
        private Socket socket;
        private StreamReader reader;
        public static StreamWriter writer;

        public  void StartClient()
        {
            var view = new GameView();
            var ipe = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080);

            //creo una nuova socket che prende l'indirizzo ip dell'endpoint. Le socket possono essere di diversi tipi
            //stream vuol dire che i dati vengono mandati con bit e il protocollo tcp
            socket = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                //il client è attivo quindi si connette alla socket
                socket.Connect(ipe);


                //inizializzo un reader e un writer per scrivere sulla socket
                reader = new StreamReader(new NetworkStream(socket));

                //stream reader che legge direttamente dalla SOCKET
                writer = new StreamWriter(new NetworkStream(socket));
                writer.AutoFlush = true;
            }
            catch (Exception )
            {
                Console.WriteLine("Wait for the server!");
            }
            

            Card selectedCard = new Card();
            bool alreadyDiscarded = false;

            Console.WriteLine("Wait for other players!");


            string data = "";
            Message message = new Message();

           
            while (true)
            {

                try
                {
                    if (reader != null && data != null)
                    {
                        data = reader.ReadLine();

                        message = JsonSerializer.Deserialize<Message>(data);
                    }

                }
                catch (Exception)
                {
                    Environment.Exit(0);
                }

                //operazione da fare in base al tipo
                switch (message.Type)
                {

                    case TypeMessage.START_TURN:

                        alreadyDiscarded = message.alreadyDiscarded;
                        selectedCard = view.SelectionView(message.nOpponentCards, message.MyHand, message.lastDiscardeCard, alreadyDiscarded);

                        //se viene restituita una carta, viene mandato un messaggio di tipo startTurn dove la carta viene scartata
                        if (selectedCard != null)
                        {
                            writer.WriteLine(JsonSerializer.Serialize(new Message { Type = TypeMessage.START_TURN, Body = JsonSerializer.Serialize(selectedCard) }));

                        }

                        break;


                    case TypeMessage.WAITING_TURN:
                        view.GameVision(message.nOpponentCards, message.MyHand, message.lastDiscardeCard);
                        break;


                    case TypeMessage.DRAW_CARD:
                        alreadyDiscarded = message.alreadyDiscarded;
                        selectedCard = view.SelectionView(message.nOpponentCards, message.MyHand, message.lastDiscardeCard, alreadyDiscarded);
                        writer.WriteLine(JsonSerializer.Serialize(new Message { Type = TypeMessage.START_TURN, Body = JsonSerializer.Serialize(selectedCard) }));
                        break;


                    case TypeMessage.WIN: 
                        view.HasWon();
                        break;

                    case TypeMessage.LOSE:
                        view.HasLost();
                        break;

                    default:
                        Console.WriteLine($"{message.Type} not supported");
                        break;
                }

            }

        }

        public static void SendMessage(Message message)
        {
            
            writer.WriteLine(JsonSerializer.Serialize(message));
        }


    }
}
