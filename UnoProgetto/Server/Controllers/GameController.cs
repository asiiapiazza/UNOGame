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
        private int _player;
        public GameModel _model;
        private List<Card> _playerHand;
        private Card _lastDiscardedCard;

        public GameController(GameModel model)
        {
            _model = model;
            _views = new PlayerView[3];
            _player = 0;
            _playerHand = _model.PlayersHand[_player];
      
        }

        public void AddView(PlayerView view)
        {
            _views[_player] = view;
            nextTurn();
        }



        //1) PRIMA OPERAZIONE ESEGUITA DEL SERVER
        //questi metodi vanno nella playerview
        //vengono richiamati dei metodi nel GameModel per la gestione dei deck       
        public void Start()
        {
            //metodo che distribuisce 7 carte ad ogni giocatore della view
            starterHands();

            //serializzazione, manda messaggio al server di INIZIARE IL GIOCO
            //foreach (var view in _views)
            //{
            //   

            //}

            //TESTING
            //manda messaggio di inizio al giocatore 0
            _views[0].SendMessage(new Message { Type = TypeCard.START });
 

        }


        /// <summary>
        /// 
        /// distribuisce 7 carte dal mazzo base ad ogni mazzo dei giocatori
        /// </summary>
        private void starterHands()
        {
            Card cardFromUnoHand;

            for (int i = 0; i < _views.Count(); i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    //prendo la carta in cima al mazzo coperta
                    cardFromUnoHand = _model.UnoHand[0];

                    //aggiungo carta al deck del giocatore
                    _model.PlayersHand[i].Add(cardFromUnoHand);

                    //rimuovo carta dal deck
                    _model.UnoHand.Remove(cardFromUnoHand);
                }
            }
            
        }

        /// <summary>
        /// metodo per la pesca delle carte dal deck. Quando pesco +4/+2 o devo pescare una carta per forza
        /// </summary>
        /// <param name="currentView"></param>
        /// <param name="message"></param>
        /// <param name="model"></param>
        public void distribuiteCards(PlayerView currentView)
        {
            _lastDiscardedCard = _model.DiscardedHand.Last();
            if (currentView != _views[_player])
                throw new InvalidOperationException();
            if (_lastDiscardedCard.Type == Models.Type.DRAW_FOUR)
            {
                drawNCards(4);
            }
            else if (_lastDiscardedCard.Type == Models.Type.DRAW_TWO)
            {
                drawNCards(2);
            }
        }

        private void drawNCards(int nCardsToDraw)
        {
            for (int i = 0; i < nCardsToDraw; i++)
            {
                _model.drawFromDrawHand(_playerHand);
            }
        }
 

        public void checkCardValidity(PlayerView currentView, Message message)
        {
            //controllo se la view  del player che passo è uguale alla view
            if (currentView != _views[_player])
                throw new InvalidOperationException();
            //se posso scartare, tolgo dal deck del giocatore la carta e la agggiungo al
            //mazzo scarto. Se non posso scartare e ho gia pescato, passo il turno, se non ho pescato pesco
            //scartando o passando il turno
            
        }

        /// <summary>
        /// metodo per scartare una propria carta dal proprio mazzo
        /// </summary>
        /// <param name="selectedCard"></param>
        public void discardCard(Card selectedCard)
        {
            checkDiscardedCard(selectedCard);
            _model.discardCardFromMyHand(selectedCard, _playerHand);
     
        }



        //DA FARE
        void checkDiscardedCard(Card selectedCard)
        {
            //controllo della carta scartata e la carta del mazzo
            // _lastDiscardedCard = ultima carta scartata
        }


        //DA FARE
        public void isWinner()
        {

        }

        /// <summary>
        /// questo metodo prende il numero di giocatori che partecipano
        /// e incrementa il turno del giocatore successivo
        /// </summary>
        private void nextTurn()
        {
            
            //3 giocatore
            if (_player ==2)
            {
                _player = 0;
            }
            else
            {
                _player += 1;
            }


        }

        public void invertTurnOrder()
        {

        }

        public void checkDraw()
        {
            //usare _model
            //se il mazzo da cui pescare è finitp
            //richiamo il metodo che aggiunge il deck delle carte scartate
            //tranne l'ultima che ho scartato
        }


     


    }
}
