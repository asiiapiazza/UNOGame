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
  
        Menu menu = new Menu();

        public GameView()
        {
            
        }

        public void Start()
        {
            
        }

        public void PrintCard(Card card)
        {

        }

        public void SelectCard()
        {

        }
        

        /// <summary>
        /// stampa mazzo del giocatore/scarto/pescare
        /// </summary>
        /// <param name="deck"></param>
        public void PrintDeck(Deck deck)
        {

        }
        

        /// <summary>
        /// metodo per updatare i deck di scarto e del giocatore quando pesca una carta/scarti/
        /// </summary>
        public void UpdateDecks()
        {

        }


        /// <summary>
        /// ha perso, stampa sconfitta
        /// </summary>
        /// <param name="view"></param>
        public void hasLost()
        {

        }

        public void hasWin()
        {
           
        }

        public void connectedPlayers()
        {

        }
    }
}
