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

            //ip endpoint, sereve un IP e una porta. In questo caso local host
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
            catch (Exception ex)
            {
                Console.WriteLine("Wait for the server!");
            }
            


            Card selectedCard = new Card();
            bool alreadyDiscarded = false;

            //deve aspettare che il server gli dica che si sia connesso un altro client
            //while true, deve continuare sempre a cercare qualche messaggio del server

            Console.WriteLine("Wait for other players!");


            string data = "";
            Message message = new Message();

            while (true)
            {
                //aggiunto dopo, leggere cosa mi dice il server. Operazione bloccante, se non c'è niente rimane in atesa
                //try catch
                try
                {
                    if (data != null)
                    {

                        data = reader.ReadLine();
                    }

                    //deserializzo con la classe MESSAGE
                    message = JsonSerializer.Deserialize<Message>(data);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("");
                }

                //operazione da fare in base al tipo

                switch (message.Type)
                {
                                       
                    case TypeMessage.START_TURN:

                        alreadyDiscarded = message.alreadyDiscarded;

                        //al metodo passo anche se il giocatore ha pescato o meno. Il booleano viene passato al metodo di selezione carta e pesca
                        //e non potrà accedere alla funzione pesca P
                        selectedCard = view.SelectionView(message.nOpponentCards, message.MyHand, message.lastDiscardeCard, alreadyDiscarded);

                        //se viene restituita una carta, viene mandato un messaggio di tipo startTurn dove la carta viene scartata
                        if (selectedCard != null)
                        {
                            writer.WriteLine(JsonSerializer.Serialize(new Message { Type = TypeMessage.START_TURN, Body = JsonSerializer.Serialize(selectedCard)}));

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
                        //devo passare il giocatore del CLIENT
                        view.hasWon();
                        break;

                    case TypeMessage.LOSE:
                        view.hasLost();
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
