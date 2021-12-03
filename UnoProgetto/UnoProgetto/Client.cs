﻿using Client.Utilis;
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
        //view di un player

        //rendere la view una variabile utilizzabile da tutte le classi del client
        private Socket socket;
        private StreamReader reader;
        private StreamWriter writer;

        public  void StartClient()
        {
            var view = new GameView();

            //ip endpoint, sereve un IP e una porta. In questo caso local host
            var ipe = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080);

            //creo una nuova socket che prende l'indirizzo ip dell'endpoint. Le socket possono essere di diversi tipi
            //stream vuol dire che i dati vengono mandati con bit e il protocollo tcp
            socket = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            //il client è attivo quindi si connette alla socket
            socket.Connect(ipe);


            //inizializzo un reader e un writer per scrivere sulla socket
            reader = new StreamReader(new NetworkStream(socket));

            //stream reader che legge direttamente dalla SOCKET
            var writer = new StreamWriter(new NetworkStream(socket));
            writer.AutoFlush = true;


            Card selectedCard = new Card() ;


            while (true)
            {
                //aggiunto dopo, leggere cosa mi dice il server. Operazione bloccante, se non c'è niente rimane in atesa
                //try catch
                var data = reader.ReadLine();

                //deserializzo con la classe MESSAGE
                 var message = JsonSerializer.Deserialize<Message>(data);

                //operazione da fare in base al tipo

                switch (message.Type)
                {
                                       
                    case TypeMessage.START_TURN:

                        //mando tutto model = modificare le variabili in message
       
                        selectedCard = view.SelectionView(message.nOpponentCards, message.MyHand, message.lastDiscardeCard);          
                        writer.WriteLine(JsonSerializer.Serialize(new Message { Type = TypeMessage.START_TURN, Body = JsonSerializer.Serialize(selectedCard) }));
                        break;
            

                    case TypeMessage.MODEL_UPDATE:
                        view.View(message.nOpponentCards, message.MyHand, message.lastDiscardeCard);
                        break;

                    case TypeMessage.WAITING_TURN:

                        view.View(message.nOpponentCards, message.MyHand, message.lastDiscardeCard);
                        break;

                    case TypeMessage.DRAW_CARD:
                        selectedCard = JsonSerializer.Deserialize<Card>(message.Body);

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

        public void SendMessage(Message message)
        {
            writer.WriteLine(JsonSerializer.Serialize(message));
        }



    }
}
