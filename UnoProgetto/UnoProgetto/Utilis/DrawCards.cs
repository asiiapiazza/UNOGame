using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnoGame.Models;

namespace Client.Utilis
{
    class DrawCard
    {
        private void printDeck(Deck deck)
        {
            for (int i = 0; i <deck.Count; i++)
            {
                printCard(deck.listOfCards[i]);
            }
        }

        private void printCard(Card card)
        {

        }
    }

}
