using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using UnoGame.Models;
using UnoGame.Views;

namespace UnoGame.Controllers
{
    public class GameController
    {
        //array di giocatore
        private PlayerView[] _views;
        private int _turn;

        public GameModel _model;

        public GameController(GameModel model)
        {
            _model = model;
            _views = new PlayerView[3];
            _turn = 0;
        }

        public void AddView(PlayerView view)
        {
            _views[_turn] = view;
            NextTurn();
        }


        /// <summary>
        /// metodo per mischiare il mazzo deck iniziale
        /// </summary>
        public void shuffleDeck()
        {
           
        }

        public void reverseOrder()
        {
           
        }

        public void checkDrawDeck()
        {

        }

        public void isWinner()
        {
         
        }

        public void drawCards(PlayerView currentView, Message message, GameModel model)
        {
            if (currentView != _views[_turn])
                throw new InvalidOperationException();



        }

        public void checkCardValidity(PlayerView currentView)
        {
            //controllo se la view  del player che passo è uguale alla view
            if (currentView != _views[_turn])
                throw new InvalidOperationException();
            


        }

        /// <summary>
        /// metodo per la distribuzione delle carte delle carte iniziali 
        /// la distribuzione è random
        /// </summary>
        public void distruibiteCards()
        {
            
        }

        public void isLoser()
        {

        }

        /// <summary>
        /// questo metodo prende il numero di giocatori che partecipano
        /// e incrementa il turno del giocatore successivo
        /// </summary>
        private void NextTurn()
        {
            
            //3 giocatore
            if (_turn ==2)
            {
                _turn = 0;
            }
            else
            {
                _turn += 1;
            }


        }

        public void Start()
        {

            foreach (var virtualView in _views)
            {
                //serializzazione, manda messaggio al server di iniziare il gioco
                virtualView.SendMessage(new Message { Type = Type.START });
            }

 
        }
            
    }
}
