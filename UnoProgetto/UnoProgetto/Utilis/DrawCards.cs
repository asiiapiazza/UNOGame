using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnoGame.Models;

namespace Client.Utilis
{
    public class DrawCard
    {
    
            
        
        //a seconda del numero di giocatori, la posizione del deck degli avversari cambia
        public void printDeck(List<Card> handsOfCards)
        {
            for (int i = 0; i < handsOfCards.Count; i++)
            {
                printCard(handsOfCards[i], 0, 0);
            }
        }

        private void printCard(Card card, int positionX, int positionY)
        {
            switch (card.Color)
            {
                case Color.RED:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case Color.BLUE:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                case Color.GREEN:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case Color.YELLOW:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
            }

            Console.SetCursorPosition(positionX, positionY);
            Console.WriteLine(" _____ ");

            Console.SetCursorPosition(positionX, positionY + 1);
            Console.WriteLine("|     |");

            Console.SetCursorPosition(positionX, positionY + 2);
            Console.WriteLine("|     |");

            Console.SetCursorPosition(positionX, positionY + 3);

            switch (card.Type)
            {
                case UnoGame.Models.Type.INVERT_TURN:
                    Console.WriteLine("| <-> |");
                    break;
                case UnoGame.Models.Type.DRAW_TWO:
                    Console.WriteLine("| +2  |");
                    break;
                case UnoGame.Models.Type.DRAW_FOUR:
                    Console.WriteLine("| +4  |");
                    break;
                case UnoGame.Models.Type.STOP_TURN:
                    Console.WriteLine("| ALT |");
                    break;
                case UnoGame.Models.Type.JOLLY:
                    Console.WriteLine("|JOLLY|");
                    break;
                case UnoGame.Models.Type.CHANGE_COLOR:
                    Console.WriteLine("|COLOR|");
                    break;
                case UnoGame.Models.Type.ZERO:
                    Console.WriteLine("|  0  |");
                    break;
                case UnoGame.Models.Type.ONE:
                    Console.WriteLine("|  1  |");
                    break;
                case UnoGame.Models.Type.TWO:
                    Console.WriteLine("|  2  |");
                    break;
                case UnoGame.Models.Type.THREE:
                    Console.WriteLine("|  3  |");
                    break;
                case UnoGame.Models.Type.FOUR:
                    Console.WriteLine("|  4  |");
                    break;
                case UnoGame.Models.Type.FIVE:
                    Console.WriteLine("|  5  |");
                    break;
                case UnoGame.Models.Type.SIX:
                    Console.WriteLine("|  6  |");
                    break;
                case UnoGame.Models.Type.SEVEN:
                    Console.WriteLine("|  7  |");
                    break;
                case UnoGame.Models.Type.EIGHT:
                    Console.WriteLine("|  8  |");
                    break;
                case UnoGame.Models.Type.NINE:
                    Console.WriteLine("|  9  |");
                    break;
            }

            Console.SetCursorPosition(positionX, positionY + 4);
            Console.WriteLine("|     |");


            Console.SetCursorPosition(positionX, positionY + 5);
            Console.WriteLine("|_____|");

            Console.ResetColor();
        }

        //il giocatore vede la carta girata: deck avversari e carta da cui pescare
        public void coveredCard(int positionX, int positionY)
        {
            Console.SetCursorPosition(positionX, positionY);
            Console.WriteLine(" _____ ");

            Console.SetCursorPosition(positionX, positionY + 1);
            Console.WriteLine("|     |");

            Console.SetCursorPosition(positionX, positionY + 2);
            Console.WriteLine("|     |");

            Console.SetCursorPosition(positionX, positionY + 3);
            Console.WriteLine("| UNO |");

            Console.SetCursorPosition(positionX, positionY + 4);
            Console.WriteLine("|     |");


            Console.SetCursorPosition(positionX, positionY + 5);
            Console.WriteLine("|_____|");

            Console.ResetColor();
        }


    }

}
