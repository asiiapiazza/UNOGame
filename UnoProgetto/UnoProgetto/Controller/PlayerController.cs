using Client.Utilis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnoGame;
using UnoGame.Models;

namespace Client.Controller
{
    public class PlayerController
    {
        DrawCard draw = new DrawCard();
        internal int selectCard(List<Card> hand, bool alreadyDiscarded)
        {

            int n = 10;
            int sumPos = 33;

            //da dove devo iniziare a stampare il mio deck
            int startCardIndex = 0;
            int viewedCardPosition = 0;
            int lastCardIndex = 0;

            Console.SetCursorPosition(sumPos, 43);
            Console.WriteLine("*");
      
            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.RightArrow)
                {
                    if (lastCardIndex != hand.Count-1 || lastCardIndex !=0 )
                    {
                        if (viewedCardPosition+1 < hand.Count && viewedCardPosition != 6) 
                        {
                            
                            Console.SetCursorPosition(sumPos, 43);
                            Console.WriteLine(" ");
                            draw.indexCard(sumPos + n, 43);
                            lastCardIndex++;
                            viewedCardPosition++;
                            sumPos += n;
      

                        }
                      
               
                    }
           
                    //se ho piu di 7 carte e sono alla fine della mia view del deck (quindi posizione 6)
                    //scorro ristampo tutto il deck partendo da posizione deck[1]
                    if (hand.Count > 7 && viewedCardPosition == 6 && lastCardIndex != hand.Count-1)
                    {

                        startCardIndex++;
                        lastCardIndex++;
                        draw.printPlayerScrollView(hand, startCardIndex , startCardIndex + 7);
                    }

                }
                else if (key.Key == ConsoleKey.LeftArrow)
                {
                    if (viewedCardPosition != 0)
                    {
                        if (lastCardIndex > 0 && viewedCardPosition != 0)
                        {
                            
                            Console.SetCursorPosition(sumPos, 43);
                            Console.WriteLine(" ");
                            draw.indexCard(sumPos - n, 43);
                            lastCardIndex--;
                            viewedCardPosition--;            
                            sumPos -= n;
                        }
                    }
                    

                    if (hand.Count > 7 && viewedCardPosition == 0 && lastCardIndex>0 && startCardIndex > 0 && lastCardIndex != 0)
                    {
                        startCardIndex--;
                        lastCardIndex--;
                        draw.printPlayerScrollView(hand, startCardIndex, startCardIndex +7 );
                    }
                }
            
                else if (key.Key == ConsoleKey.Enter)
                {
                    return lastCardIndex;
                }

                else if (key.Key == ConsoleKey.P && !alreadyDiscarded )
                {
                    
                    alreadyDiscarded = true;
                    Client.SendMessage(new Message { Type = TypeMessage.DRAW_CARD });
                    return -1;
               
                }
            }
        }
    }
}
