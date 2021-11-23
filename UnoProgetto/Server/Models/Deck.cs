using System.Collections.Generic;

namespace UnoGame.Models
{
     public class Deck
     {
        public int Count { get; set; }
        public List<Card> listOfCards { get; set; } = new List<Card>();

        public Deck(List<Card> deck)
        {
            this.listOfCards = deck;
            Count = deck.Count;
        }

    }
}
