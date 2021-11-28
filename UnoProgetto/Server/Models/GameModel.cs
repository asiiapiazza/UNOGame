using System;
using System.Collections.Generic;
using System.Linq;
using UnoGame.Views;

namespace UnoGame.Models
{
    public class GameModel
    {
        public List<Card> UnoHand { get; set; } 
        public List<Card> DiscardedHand { get; set; } 
        public List<Card>[] PlayersHand { get; set ; }
        public List<PlayerView> Views { get; set; }


        public GameModel()
        {
            initalizeUnoDeck();
            initializePlayersHand();
            Views = new List<PlayerView>();
            DiscardedHand = new List<Card>();
        }


        //FATTO
        private void initalizeUnoDeck()
        {

            UnoHand = new List<Card>()
            {
                //CARTE BLU
                new Card(Type.ONE, Color.BLUE),
                new Card(Type.TWO, Color.BLUE),
                new Card(Type.THREE, Color.BLUE),
                new Card(Type.FOUR, Color.BLUE),
                new Card(Type.FIVE, Color.BLUE),
                new Card(Type.SIX, Color.BLUE),
                new Card(Type.SEVEN, Color.BLUE),
                new Card(Type.EIGHT, Color.BLUE),
                new Card(Type.NINE, Color.BLUE),
                new Card(Type.ONE, Color.BLUE),
                new Card(Type.TWO, Color.BLUE),
                new Card(Type.THREE, Color.BLUE),
                new Card(Type.FOUR, Color.BLUE),
                new Card(Type.FIVE, Color.BLUE),
                new Card(Type.SIX, Color.BLUE),
                new Card(Type.SEVEN, Color.BLUE),
                new Card(Type.EIGHT, Color.BLUE),
                new Card(Type.NINE, Color.BLUE),
                new Card(Type.ZERO, Color.BLUE),


                //CARTE ROSSE
                new Card(Type.ONE, Color.RED),
                new Card(Type.TWO, Color.RED),
                new Card(Type.THREE, Color.RED),
                new Card(Type.FOUR, Color.RED),
                new Card(Type.FIVE, Color.RED),
                new Card(Type.SIX, Color.RED),
                new Card(Type.SEVEN, Color.RED),
                new Card(Type.EIGHT, Color.RED),
                new Card(Type.NINE, Color.RED),
                new Card(Type.ONE, Color.RED),
                new Card(Type.TWO, Color.RED),
                new Card(Type.THREE, Color.RED),
                new Card(Type.FOUR, Color.RED),
                new Card(Type.FIVE, Color.RED),
                new Card(Type.SIX, Color.RED),
                new Card(Type.SEVEN, Color.RED),
                new Card(Type.EIGHT, Color.RED),
                new Card(Type.NINE, Color.RED),
                new Card(Type.ZERO, Color.RED),


                //CARTE VERDI
                new Card(Type.ONE, Color.GREEN),
                new Card(Type.TWO, Color.GREEN),
                new Card(Type.THREE, Color.GREEN),
                new Card(Type.FOUR, Color.GREEN),
                new Card(Type.FIVE, Color.GREEN),
                new Card(Type.SIX, Color.GREEN),
                new Card(Type.SEVEN, Color.GREEN),
                new Card(Type.EIGHT, Color.GREEN),
                new Card(Type.NINE, Color.GREEN),

                new Card(Type.ONE, Color.GREEN),
                new Card(Type.TWO, Color.GREEN),
                new Card(Type.THREE, Color.GREEN),
                new Card(Type.FOUR, Color.GREEN),
                new Card(Type.FIVE, Color.GREEN),
                new Card(Type.SIX, Color.GREEN),
                new Card(Type.SEVEN, Color.GREEN),
                new Card(Type.EIGHT, Color.GREEN),
                new Card(Type.NINE, Color.GREEN),

                new Card(Type.ZERO, Color.GREEN),

                //CARTE GIALLI
                new Card(Type.ONE, Color.YELLOW),
                new Card(Type.TWO, Color.YELLOW),
                new Card(Type.THREE, Color.YELLOW),
                new Card(Type.FOUR, Color.YELLOW),
                new Card(Type.FIVE, Color.YELLOW),
                new Card(Type.SIX, Color.YELLOW),
                new Card(Type.SEVEN, Color.YELLOW),
                new Card(Type.EIGHT, Color.YELLOW),
                new Card(Type.NINE, Color.YELLOW),

                new Card(Type.ONE, Color.YELLOW),
                new Card(Type.TWO, Color.YELLOW),
                new Card(Type.THREE, Color.YELLOW),
                new Card(Type.FOUR, Color.YELLOW),
                new Card(Type.FIVE, Color.YELLOW),
                new Card(Type.SIX, Color.YELLOW),
                new Card(Type.SEVEN, Color.YELLOW),
                new Card(Type.EIGHT, Color.YELLOW),
                new Card(Type.NINE, Color.YELLOW),

                new Card(Type.ZERO, Color.YELLOW),

                //PESCA DUE
                new Card(Type.DRAW_TWO, Color.BLUE),
                new Card(Type.DRAW_TWO, Color.BLUE),
                new Card(Type.DRAW_TWO, Color.RED),
                new Card(Type.DRAW_TWO, Color.RED),
                new Card(Type.DRAW_TWO, Color.GREEN),
                new Card(Type.DRAW_TWO, Color.GREEN),
                new Card(Type.DRAW_TWO, Color.YELLOW),
                new Card(Type.DRAW_TWO, Color.YELLOW),

                //INVERTI O CAMBIO GIRO
                new Card(Type.INVERT_TURN, Color.BLUE),
                new Card(Type.INVERT_TURN, Color.BLUE),
                new Card(Type.INVERT_TURN, Color.RED),
                new Card(Type.INVERT_TURN, Color.RED),
                new Card(Type.INVERT_TURN, Color.GREEN),
                new Card(Type.INVERT_TURN, Color.GREEN),
                new Card(Type.INVERT_TURN, Color.YELLOW),
                new Card(Type.INVERT_TURN, Color.YELLOW),

                //SALTA O STOP_TURN
                new Card(Type.STOP_TURN, Color.BLUE),
                new Card(Type.STOP_TURN, Color.BLUE),
                new Card(Type.STOP_TURN, Color.RED),
                new Card(Type.STOP_TURN, Color.RED),
                new Card(Type.STOP_TURN, Color.GREEN),
                new Card(Type.STOP_TURN, Color.GREEN),
                new Card(Type.STOP_TURN, Color.YELLOW),
                new Card(Type.STOP_TURN, Color.YELLOW),

                //JOLLI O CAMBIO COLORE
                new Card(Type.JOLLY, Color.NONE),
                new Card(Type.JOLLY, Color.NONE),
                new Card(Type.JOLLY, Color.NONE),
                new Card(Type.JOLLY, Color.NONE),

                //JOLLI PESCA 4 O CAMBIO COLORE
                new Card(Type.DRAW_FOUR, Color.NONE),
                new Card(Type.DRAW_FOUR, Color.NONE),
                new Card(Type.DRAW_FOUR, Color.NONE),
                new Card(Type.DRAW_FOUR, Color.NONE),

             };
            var rnd = new Random();
            var randomized = UnoHand.OrderBy(a => Guid.NewGuid()).ToList();
            UnoHand = randomized;
        }


        //FATTO
        private void initializePlayersHand()
        {
            PlayersHand = new List<Card>[3];
            PlayersHand[0] = new List<Card>();
            PlayersHand[1] = new List<Card>();
            PlayersHand[2] = new List<Card>();
        }


        //FATTO
        public void playSelectedCard(Card card, List<Card> playerHand)
        {
            //1 carta rimossa dal mazzo del giocatore
            playerHand.Remove(card);
            DiscardedHand.Add(card);  
        }

        //FATTO
        public void drawFromDrawHand(List<Card> playerHand)
        {
            //2 pesco dal mazzo pesca e aggiungo al giocatore
            var card = UnoHand.Last();
            UnoHand.Remove(card);
            playerHand.Add(card);
        }


        //da capire
        private void Notify(Message msg)
        {
            foreach (var view in Views)
            {
               
                view.SendMessage(msg);
            }
        }


        public void AddView(PlayerView view)
        {
            Views.Add(view);
        }
    }

}
