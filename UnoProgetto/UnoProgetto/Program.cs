
using Client.Controller;
using Client.Utilis;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using UnoGame;
using Type = UnoGame.Type;

namespace Client
{
    class FirstStart
    {
        public static int Nplayers = 2;

        private static void Main(string[] args)
        {
            //STAMPA DEL MENU
            Console.CursorVisible = false;
            Menu menu = new Menu();
            int option = 0;

            while (option != 3 || option != 4)
            {
                option = menu.Print();
                Nplayers = menu.NumberOfPlayers(option);
            }

            Client.StartClient();

        }



    }
}
