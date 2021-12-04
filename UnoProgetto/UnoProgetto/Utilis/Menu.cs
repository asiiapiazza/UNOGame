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

            printHeader();
            string[] menuItems = {"3 PLAYERS", "EXIT" };
            var menu = new Menu();
            int option = menu.PrintOptions(menuItems);
            return option;
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
                Console.SetCursorPosition(0, 0);
                printHeader();
            }

            return cursorPosition;
        }
        internal void printHeader()
        {

            Console.ForegroundColor = ConsoleColor.Magenta;
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
