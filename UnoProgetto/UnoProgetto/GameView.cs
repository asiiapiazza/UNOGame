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
            printPlayersDeck(nOpponentsCards[0], nOpponentsCards[1]);


            //stampo carta scartata
            printDiscardedCard(lastDiscaredCard);

            //stampo mazzo pesca
            printDrawCard();

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
        /// 
        /// </summary>
        /// <param name="nOpponentsCards"></param>
        /// <param name="hand"></param>
        /// <param name="lastDiscaredCard"></param>
        internal void GameVision(List<int> nOpponentsCards, List<Card> hand, Card lastDiscaredCard)
        {

            Console.Clear();

            playerHand = hand;


            //stampo mano giocatore avversario
            //query dove trovo numero di carte dei giocatori avversari
            //problema: cosa succede se l'avversario ha piu di 7 carte? gli stampo a fianco il numero di carte piu di 7
            printPlayersDeck(nOpponentsCards[0], nOpponentsCards[1]);

            //stampo carta scartata
            printDiscardedCard(lastDiscaredCard);

            //stampo mazzo pesca
            printDrawCard();

            //visione del mio deck

            drawCards.printPlayerHand(hand);

            Console.WriteLine("Wait your turn!");
        }

        internal void MyHandView(List<Card> hand)
        {
            drawCards.printPlayerHand(hand);

        }

        /// <summary>
        /// selezione della carta tramite keybindings
        /// </summary>
        /// 

        internal int SelectCard(bool alreadyDiscarded)
        {
            int index = controller.selectCard(playerHand, alreadyDiscarded);
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
            drawCards.printOpponentVertHand(nCardHorzOpp);
        }


        /// <summary>
        /// ha perso, stampa sconfitta
        /// </summary>
        /// <param name="view"></param>
        internal void hasLost()
        {
            Console.Clear();
            Console.WriteLine("Hai perso");
        }


        /// <summary>
        /// ha vinto, stampa vittoria
        /// </summary>
        /// <param name="view"></param>
        internal void hasWon()
        {
            Console.Clear();
           Console.WriteLine("Hai vinto");
        }

   
    }
}
