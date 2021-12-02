using Client.Controller;
using Client.Utilis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnoGame.Models;
using UnoGame.Views;

namespace Client
{
    public class GameView
    {

        //dare dimensione massima console 
        DrawCard drawCards = new DrawCard();
        PlayerController controller = new PlayerController();
        public List<Card> playerHand = new List<Card>();

        /// <summary>
        ///  il suo mazzo, la carta scartata all’inizio, una carta scoperta che rappresenta il mazzo,
        ///  il numero di carte degli avversari/carte degli avversi coperte 
        /// </summary>
        internal int View(GameModel model, List<Card> hand)
        {

            Console.Clear();

            playerHand = hand;


            //stampo mano giocatore avversario
            //query dove trovo numero di carte dei giocatori avversari
            //problema: cosa succede se l'avversario ha piu di 7 carte? gli stampo a fianco il numero di carte piu di 7

            var opponentHands = model.Views.Where(t => t._hand != hand).ToList();
            Card cc = new Card(UnoGame.Models.Type.EIGHT, Color.BLUE);
            opponentHands[0]._hand.Add(cc);

            Card cc1 = new Card(UnoGame.Models.Type.EIGHT, Color.RED);
            opponentHands[0]._hand.Add(cc1);

            Card cc2 = new Card(UnoGame.Models.Type.EIGHT, Color.YELLOW);
            opponentHands[0]._hand.Add(cc2);

            printPlayersDeck(opponentHands[0]._hand.Count, opponentHands[1]._hand.Count);



            //stampo carta scartata
            printDiscardedCard(model.DiscardedHand.Last());

            //stampo mazzo pesca
            printDrawCard();

            //visione del mio deck
            drawCards.printPlayerHand(hand);

            //selezione della carta
           
                int index = SelectCard();
                return index;
 
       
            
        }

        internal void Start(GameModel model, List<Card> hand)
        {

            Console.Clear();

            playerHand = hand;


            //stampo mano giocatore avversario
            //query dove trovo numero di carte dei giocatori avversari
            //problema: cosa succede se l'avversario ha piu di 7 carte? gli stampo a fianco il numero di carte piu di 7

            //var opponentHands = model.Views.Where(t => t._hand != hand).ToList();
            //printPlayersDeck(opponentHands[0]._hand.Count, opponentHands[1]._hand.Count);



            //stampo carta scartata
            printDiscardedCard(model.DiscardedHand.Last());

            //stampo mazzo pesca
            printDrawCard();

            //visione del mio deck

            drawCards.printPlayerHand(hand);


        }


        internal void WaitYourTurn()
        {
            Console.Clear();
            Console.WriteLine("Wait for your turn!");
        }
        /// <summary>
        /// selezione della carta tramite keybindings
        /// </summary>
        /// 

        internal int SelectCard()
        {
            int index = controller.selectCard(playerHand);
            return index;
        }


        /// <summary>
        /// stampa mazzo del giocatore/scarto/pescare
        /// </summary>
        /// <param name="deck"></param>
        
   

        //stampa il deck di carte scartate
        internal void printDiscardedCard(Card card)
        {
            drawCards.printCard(card, 63,15);
        }

        //stampa il deck da cui pescare
        internal void printDrawCard()
        {
            drawCards.coveredCard(72, 15);
        }

        //stampa il deck (nascosto) degli altri giocatori
        internal void printPlayersDeck(int nCardOpponent, int nCardHorzOpp)
        {
            drawCards.printOpponentHand(nCardOpponent);
            drawCards.printOpponentHorizHand(nCardHorzOpp);
        }


        /// <summary>
        /// ha perso, stampa sconfitta
        /// </summary>
        /// <param name="view"></param>
        internal void hasLost()
        {

        }


        /// <summary>
        /// ha vinto, stampa vittoria
        /// </summary>
        /// <param name="view"></param>
        internal void hasWon()
        {
           
        }


        /// <summary>
        /// metodo per l'update della gameview ogni qualvolta che avviente 
        /// un cambio del model (draw, discard card)
        /// </summary>
       

   
    }
}
