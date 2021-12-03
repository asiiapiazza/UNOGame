using Client.Utilis;
using System;
using System.Collections.Generic;
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
        Client client = new Client();
        internal int selectCard(List<Card> hand)
        {

            //TEST SCORRIMENTO
            //Card cc = new Card(UnoGame.Models.Type.EIGHT, Color.BLUE);
            //hand.Add(cc);

            //Card cc1 = new Card(UnoGame.Models.Type.EIGHT, Color.RED);
            //hand.Add(cc1);

            //Card cc2 = new Card(UnoGame.Models.Type.EIGHT, Color.YELLOW);
            //hand.Add(cc2);


            int n = 10;
            int sumPos = 33;
            int startIndex = 0;
            int viewCardIndex = 0;
            int cardIndexList = 0;
            int lastPrintedCardIndex = hand.Count-1;
            bool alreadyDrew = false;
            Console.SetCursorPosition(sumPos, 43);
            Console.WriteLine("*");
            while (true)
            {
               
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.RightArrow)
                {
                    if (cardIndexList != hand.Count-1 || cardIndexList !=0 )
                    {
                        if (viewCardIndex <= hand.Count-1 && viewCardIndex != hand.Count - 1)
                        {
                            
                            Console.SetCursorPosition(sumPos, 43);
                            Console.WriteLine(" ");
                            draw.indexCard(sumPos + n, 43);
                            cardIndexList++;
                            viewCardIndex++;
                            sumPos += n;

                           
                        }
                      
               
                    }
           

                    //se ho piu di 7 carte e sono alla fine della mia view del deck (quindi posizione 6)
                    //scorro ristampo tutto il deck partendo da posizione deck[1]
                    if (hand.Count > 7 && viewCardIndex == hand.Count - 1 && cardIndexList != hand.Count-1)
                    {

                        draw.printPlayerScrollView(hand, startIndex + 1, cardIndexList+1);
                        lastPrintedCardIndex++ ;

                        startIndex++;
                        cardIndexList++;
             
                    }

                }
                else if (key.Key == ConsoleKey.LeftArrow)
                {
                    if (viewCardIndex != 0)
                    {
                        if (cardIndexList > 0 && viewCardIndex != 0)
                        {
                            
                            Console.SetCursorPosition(sumPos, 43);
                            Console.WriteLine(" ");
                            draw.indexCard(sumPos - n, 43);
                            cardIndexList--;
                            viewCardIndex--;
                            sumPos -= n;
                        }
                    }
                    

                    if (hand.Count > 7 && viewCardIndex == 0 && cardIndexList>0 && startIndex > 0 && cardIndexList != hand.Count-1)
                    {
                        //devo calcolare indice ultima carta da stampare
                        startIndex--;
                        cardIndexList--;
                        lastPrintedCardIndex--;
                        draw.printPlayerScrollView(hand, startIndex, lastPrintedCardIndex);
                    }

                }
            
                else if (key.Key == ConsoleKey.Enter)
                {
                    return cardIndexList;
                }

                else if (key.Key == ConsoleKey.P && !alreadyDrew)
                {
                    //invio il fatto che stato premuto p per
                    alreadyDrew = true;
                    client.SendMessage(new Message { Type = TypeMessage.DRAW_CARD});
                }
            }
        }
    }
}
