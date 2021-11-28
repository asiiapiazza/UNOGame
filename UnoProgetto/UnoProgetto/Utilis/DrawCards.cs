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

        private void printCard(Card card, int top, int left)
        {

        }
        
        public void coveredCard()
        {
            Console.WriteLine();
        }


        //il giocatore vede la carta girata: deck avversari e carda da cui pescare


    }

}
