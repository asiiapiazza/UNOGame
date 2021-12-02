using UnoGame.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;

using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using UnoGame.Models;

namespace UnoGame.Views
{

    public class PlayerView
    {
        //è la rappresentazione del giocatore e in questo caso ha un riferimento alla socket al client associato
        private Socket _socket;
        private StreamReader _reader;
        private StreamWriter _writer;
        private GameController _controller;
        public List<Card> _hand = new List<Card>();


        public PlayerView()
        {

        }

 

        //ho bisogno della socket e del controller di riferimento
        public PlayerView(Socket socket, GameController controller)
        {
            _controller = controller;
            _socket = socket;
            _reader = new StreamReader(new NetworkStream(_socket));
            _writer = new StreamWriter(new NetworkStream(_socket));
            _writer.AutoFlush = true;
        }


        //eseguo la funzione Run
        //la rappresentazione del giocatore nel server fa le stesse cose del giocatore nel client
        //ovvero aspetta i messaggi da parte del client, messaggi di tipo mossa
        //la view deve notificare il controller
        public void Run()
        {
            while (true)
            {

                //aspetta messaggio del client
                var data = _reader.ReadLine();
                var message = JsonSerializer.Deserialize<Message>(data);


                //da sostituire switchcase
                switch (message.Type)
                {
           


                    case TypeMessage.START_TURN:
                        _controller.distribuiteCards(this);
                        _controller.discardCard(message);   
                        
                        break;


                    default:
                        Console.WriteLine($"{message.Type} not supported");
                        break;
                }
            }
        }

        //scrivere sulla socket il messaggio serializzato
        public void SendMessage(Message message)
        {
            _writer.WriteLine(JsonSerializer.Serialize(message));
        }

        internal void sendmessage(Message message)
        {
            throw new NotImplementedException();
        }
    }

}
