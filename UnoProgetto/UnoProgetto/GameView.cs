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
        internal Card SelectionView(List<int> nOpponentsCards,List<Card> hand, Card lastDiscaredCard)
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

            //selezione della carta
            int index = SelectCard();
            Card card = hand[index];
        
            return card;
    
        }

        internal void View(List<int> nOpponentsCards, List<Card> hand, Card lastDiscaredCard)
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
            drawCards.printOpponentVertHand(nCardHorzOpp);
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
