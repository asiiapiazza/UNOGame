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

        
        DrawCard drawCards = new DrawCard();
        PlayerController controller = new PlayerController();
        public List<Card> playerHand = new List<Card>();
        

        /// <summary>
        /// Metodo per la selezione e la stampa della carta. Richiamato al turno del giocatore.
        /// </summary>
        internal Card SelectionView(List<int> nOpponentsCards,List<Card> hand, Card lastDiscaredCard, bool alreadyDiscarded)
        {

            Console.Clear();
           playerHand = hand;

            //stampo mani COPERTE giocatori avversari
            PrintPlayersDeck(nOpponentsCards[0], nOpponentsCards[1]);

            //stampo carta scartata
            PrintDiscardedCard(lastDiscaredCard);

            //stampo mazzo pesca
            PrintDrawCard();

            //visione del mio deck
            drawCards.printPlayerHand(hand);

            //selezione della carta
            int index = SelectCard(alreadyDiscarded);

            if (index>=0)
            {
                Card card = hand[index];
                return card;
            }
            else
            {
                //se mi ritorna nulla vuol dire che ho PESCATO
                return null;
            }
          
    
        }

        /// <summary>
        /// Metodo per la stampa della carta ma non la selezione. Richiamato al turno del giocatore.
        /// </summary>
        internal void GameVision(List<int> nOpponentsCards, List<Card> hand, Card lastDiscaredCard)
        {

            Console.Clear();
            playerHand = hand;

            PrintPlayersDeck(nOpponentsCards[0], nOpponentsCards[1]);

            //stampo carta scartata
            PrintDiscardedCard(lastDiscaredCard);

            //stampo mazzo pesca
            PrintDrawCard();

            //visione del mio deck
            drawCards.printPlayerHand(hand);

            Console.WriteLine("Wait your turn!");
        }

        internal void MyHandView(List<Card> hand)
        {
            drawCards.printPlayerHand(hand);

        }

       
        internal int SelectCard(bool alreadyDiscarded)
        {
            int index = controller.selectCard(playerHand, alreadyDiscarded);
            return index;
        }



        internal void PrintDiscardedCard(Card card)
        {
            drawCards.printCard(card, 63,15);
        }

        internal void PrintDrawCard()
        {
            drawCards.coveredCard(72, 15);
        }


        //stampa le mani (nascosti) degli altri giocatori
        internal void PrintPlayersDeck(int nCardOpponent, int nCardHorzOpp)
        {
            drawCards.printOpponentHand(nCardOpponent);
            drawCards.printOpponentVertHand(nCardHorzOpp);
        }


        internal void HasLost()
        {
            Console.Clear();
            Console.WriteLine("Sadly, you lost!");
            Console.WriteLine("[Press any key to exit!]");
            Console.Read();
            Environment.Exit(0);
        }



        internal void HasWon()
        {
           Console.Clear();
           Console.WriteLine("Congrats, you won!");
           Console.WriteLine("[Press any key to exit!]");
           Console.Read();
           Environment.Exit(0);
        }

   
    }
}
