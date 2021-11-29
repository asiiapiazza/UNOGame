
using Client.Controller;
using Client.Utilis;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using UnoGame;
using UnoGame.Models;
using Type = UnoGame.TypeCard;

namespace Client
{
    class Game
    {
    
        private static void Main(string[] args)
        {
            //STAMPA DEL MENU
            Console.CursorVisible = false;
            Menu menu = new Menu();
            PlayerController controller = new PlayerController();

            int option =  menu.Print();

            //se premo le altre opzioni
            while (option != 0)
            {
                        
                
            }


            //se premo opzione 0, quindi 3 players
            Client.StartClient();

        }



    }
}
