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
  
        private Socket _socket;
        private StreamReader _reader;
        private StreamWriter _writer;
        private GameController _controller;
        public List<Card> _hand = new List<Card>();


        public PlayerView()
        {

        }

        public PlayerView(Socket socket, GameController controller)
        {
            _controller = controller;
            _socket = socket;
            _reader = new StreamReader(new NetworkStream(_socket));
            _writer = new StreamWriter(new NetworkStream(_socket));
            _writer.AutoFlush = true;
        }

        //metodo che i task creati nella classe Server eseguono
        public void Run()
        {
            string data;
            Message message = new Message();
            while (true)
            {
            
                try
                {
                    //aspetta messaggio del client
                    data = _reader.ReadLine();
                    message = JsonSerializer.Deserialize<Message>(data);
                }
                catch (Exception)
                {
                    Console.WriteLine("Server Error: client disconnected");            
                    Environment.Exit(0);
                }


                switch (message.Type)
                {
           
                    case TypeMessage.START_TURN:                  
                        _controller.DiscardCard(message);                          
                        break;

                    case TypeMessage.DRAW_CARD:
                        _controller.Draw();
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



    }

}
