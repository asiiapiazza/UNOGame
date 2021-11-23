using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Utilis
{
   public class Menu
    {

        //visione del gioco da parte del client
        public int Print()
        {


            //printHeader();
            string[] menuItems = { "2 PLAYERS", "3 PLAYERS", "4 PLAYERS", "RULES", "EXIT", "JOIN" };
            var menu = new Menu();
            int option = menu.PrintOptions(menuItems);
            return option;
        }

        public int NumberOfPlayers(int n)
        {
            int Nplayers = 0;
            switch (n)
            {
                case 0:
                    Nplayers = 2;
                    break;
                case 1:
                    Nplayers = 3;
                    break;
                case 2:
                    Nplayers = 4;
                    break;
                default:
                    Nplayers = 2;
                    break;
            }

            return Nplayers;
        }


        private int PrintOptions(string[] menuItems)
        {
            int cursorPosition = 0;
            while (true)
            {
                Console.WriteLine();

                for (int i = 0; i < menuItems.Length; i++)
                {
                    if (i == cursorPosition)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                    }

                    Console.WriteLine(menuItems[i]);
                }

                ConsoleKeyInfo cki = Console.ReadKey(true);

                if (cki.Key == ConsoleKey.DownArrow)
                {
                    if (cursorPosition + 1 == menuItems.Length)
                    {
                        cursorPosition = 0;
                    }
                    else
                    {
                        cursorPosition++;
                    }
                }
                else if (cki.Key == ConsoleKey.UpArrow)
                {
                    if (cursorPosition - 1 < 0)
                    {
                        cursorPosition = menuItems.Length - 1;
                    }
                    else
                    {
                        cursorPosition--;
                    }
                }
                else if (cki.Key == ConsoleKey.Enter)
                {
                    Console.Clear();
                    break;
                }

                Console.Clear();
            }

            return cursorPosition;
        }
        private void printHeader()
        {

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("  _    _                _____                      _ ");
            Console.WriteLine(" | |  | |              / ____|                    | |");
            Console.WriteLine(" | |  | |_ __   ___   | |  __  __ _ _ __ ___   ___| |");
            Console.WriteLine(@" | |  | | '_ \ / _ \  | | |_ |/ _` | '_ ` _ \ / _ \ |");
            Console.WriteLine(" | |__| | | | | (_) | | |__| | (_| | | | | | |  __/_|");
            Console.WriteLine(@"  \____/|_| |_|\___/   \_____|\__,_|_| |_| |_|\___(_)");
            Console.ResetColor();

        }


    }
}
