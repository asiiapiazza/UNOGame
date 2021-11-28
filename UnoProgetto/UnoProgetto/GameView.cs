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

        //devo fare scegliere al giocatore quanti giocatori giocano
        //dare dimensione massima console 


        DrawCard drawCard = new DrawCard();


        /// <summary>
        ///  il suo mazzo, la carta scartata all’inizio, una carta scoperta che rappresenta il mazzo,
        ///  il numero di carte degli avversari/carte degli avversi coperte 
        /// </summary>
        internal void Start()
        {


        }

        /// <summary>
        /// stampa carta scartata
        /// </summary>
        /// <param name="card"></param>
        internal void PrintCard(Card card)
        {
            
        }

        /// <summary>
        /// stampa 
        /// </summary>
        internal void SelectCard()
        {

        }


        /// <summary>
        /// stampa mazzo del giocatore/scarto/pescare
        /// </summary>
        /// <param name="deck"></param>
        
        //stampa deck del giocatore
        internal void printPlayerDeck()
        {
            //drawCard.printDeck();
        }

        //stampa il deck di carte scartate
        internal void printDiscardedDeck()
        {
            //drawCard.printDeck();
        }

        //stampa il deck da cui pescare
        internal void printDrawDeck()
        {
            //drawCard.printDeck();
        }

        //stampa il deck (nascosto) degli altri giocatori
        internal void printPlayersDeck()
        {
            //ma deve sapere quante carte stampare
            //drawCard.coveredCard();
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

        internal void UpdateDecks()
        {
            throw new NotImplementedException();
        }
    }
}
