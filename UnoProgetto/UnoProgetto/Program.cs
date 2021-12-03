
using Client.Controller;
using Client.Utilis;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using UnoGame;
using UnoGame.Models;
using Type = UnoGame.TypeMessage;

namespace Client
{
    class Game
    {
    
        private static void Main(string[] args)
        {
            //STAMPA DEL MENU
            Console.CursorVisible = false;
            Menu menu = new Menu();
            Client client = new Client();
            PlayerController controller = new PlayerController();

            int option =  menu.Print();

            //se premo le altre opzioni
            
            switch (option)
            {
                case 0:
                    //se premo opzione 0, quindi 3 players
                    client.StartClient();
                    break;

                case 1:
                    Environment.Exit(0);
                    break;
         


            }


        }



    }
}
