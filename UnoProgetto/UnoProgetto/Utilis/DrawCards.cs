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
      
        
        internal void PrintCard(Card card, int left, int top)
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

            Console.SetCursorPosition(left, top);
            Console.WriteLine(" _____ ");

            Console.SetCursorPosition(left, top + 1);
            Console.WriteLine("|     |");

            Console.SetCursorPosition(left, top + 2);
            Console.WriteLine("|     |");

            Console.SetCursorPosition(left, top + 3);

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

            Console.SetCursorPosition(left, top + 4);
            Console.WriteLine("|     |");


            Console.SetCursorPosition(left, top + 5);
            Console.WriteLine("|_____|");

            Console.ResetColor();
        }

        internal void PrintOpponentHand(int nCards)
        {
            int top = 0;
            int left = 30;

            if (nCards<8)
            {
                for (int i = 0; i < nCards; i++)
                {
                    CoveredCard(left, top);
                    left += 10;
                }
            }
            else
            {
                for (int i = 0; i < 7; i++)
                {
                    CoveredCard(left, top);
                    left += 10;
                }

                Console.SetCursorPosition(left, top + 5);
                Console.WriteLine("+" + (nCards - 7));
            }
         

        
            
        }

        internal void PrintOpponentVertHand(int nCards)
        {
            int left = 0;
            int top = 1;
       

            if (nCards <8)
            {

                for (int i = 0; i < nCards; i++)
                {
                    CoveredVerticalCard(left, top);
                    top += 5;
                }
                
            }
            else
            {
                for (int i = 0; i < 7; i++)
                {
                    CoveredVerticalCard(left, top);
                    top += 5;
                }

                //da definire posizione
                Console.SetCursorPosition(left, top + 5);
                Console.WriteLine("+" + (nCards - 7));
            }
        }

        internal void IndexCard(int left, int top)
        {
            Console.SetCursorPosition(left, top);
            Console.WriteLine("*");
        }

        internal void PrintPlayerScrollView(List<Card> hand, int startIndex, int endIndex)
        {

            int top = 37;
            int left = 30;
            for (int i = startIndex; i <endIndex; i++)
            {
                PrintCard(hand[i], left, top);
                left += 10;

            }

        }

        internal void PrintPlayerHand(List<Card> hand)
        {
            int top = 37;
            int left = 30;
            if (hand.Count > 7)
            {
                for (int i = 0; i < 7; i++)
                {
                    PrintCard(hand[i], left, top);
                    left += 10;


                }
            }
            else
            {
                for (int i = 0; i < hand.Count; i++)
                {
                    PrintCard(hand[i], left, top);
                    left += 10;


                }
            }
       
         
        }
        
        public void CoveredCard(int positionX, int positionY)
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

        internal void CoveredVerticalCard(int left, int top)
        {
            Console.SetCursorPosition(left, top);
            Console.WriteLine(" ----------");

            Console.SetCursorPosition(left, top + 1);
            Console.WriteLine("|    C     |");
            Console.SetCursorPosition(left, top + 2);
            Console.WriteLine("|    Z     |");

            Console.SetCursorPosition(left, top + 3);
            Console.WriteLine("|    O     |");

            Console.SetCursorPosition(left, top + 4);
            Console.WriteLine(" ----------");
        }


    }

}
