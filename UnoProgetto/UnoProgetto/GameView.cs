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

        /// <summary>
        ///  il suo mazzo, la carta scartata all’inizio, una carta scoperta che rappresenta il mazzo,
        ///  il numero di carte degli avversari/carte degli avversi coperte 
        /// </summary>
        internal void Start(GameModel model)
        {

            //visione del mio deck
            drawCards.printPlayerHand(model.PlayersHand[0]);

            printPlayersDeck();

            //stampo carta scartata
            printDiscardedCard(model.DiscardedHand.Last());
            printDrawCard();
        }


      
        /// <summary>
        /// selezione della carta tramite keybindings
        /// </summary>
        internal Card SelectCard()
        {
            //TESTING
            //Card card = new Card(UnoGame.Models.Type.FIVE, Color.YELLOW);
            //return card;
            return null;
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
        internal void printPlayersDeck()
        {
            drawCards.printOpponentHand(7);
            drawCards.printOpponentHorizHand(7);
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
        internal void UpdateHandView()
        {
           
        }
    }
}
