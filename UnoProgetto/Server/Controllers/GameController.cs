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
        private Card _lastDiscardedCard;
        int n = 0;

        public GameController(GameModel model)
        {
            _model = model;
            _views = new PlayerView[2];
            _turn = 0;

        }

        public void AddView(PlayerView view)
        {
          
            _views[_turn] = view;
            nextTurn();
        }



        //1) PRIMA OPERAZIONE ESEGUITA DEL SERVER
        //questi metodi vanno nella playerview
        //vengono richiamati dei metodi nel GameModel per la gestione dei deck       
        public void Start()
        {
            //metodo che distribuisce 7 carte ad ogni giocatore della view
            starterHands();
            firstDiscardedCard();
            //serializzazione, manda messaggio al server di INIZIARE IL GIOCO

            foreach (var view in _views)
            {

                view.SendMessage(new Message { Type = TypeCard.START, Body = JsonSerializer.Serialize<List<Card>>(view._hand) });
                view.SendMessage(new Message { Type = TypeCard.START, Body = JsonSerializer.Serialize<Card>(view._lastDiscardedCard) });
                n++;

            }

            //TESTING
            //manda messaggio di inizio al giocatore 0
            _model.Views[0].SendMessage(new Message { Type = TypeCard.START, Body = JsonSerializer.Serialize<GameModel>(_model)});

           
        }


        //DA FARE

        /// <summary>
        /// Metodo per la pesca dal deck UnoHand della prima carta scartata ad inizio game
        /// devo fare i controlli 
        /// . Nel caso in cui la prima carta sia una carta Azione, è necessario applicare le regole della sezione Funzioni delle carte
        ///; fa eccezione la carta "Jolly Pesca Quattro" che, se scoperta all'inizio del gioco, deve essere 
        ///rimessa a caso nel "mazzo pesca" e sostituita con un'altra.
        /// </summary>
        /// <returns></returns>
        private void firstDiscardedCard()
        {
            _model.DiscardedHand.Add(_model.UnoHand.Last());
            _model.UnoHand.Remove(_model.DiscardedHand.Last());
            
        }


        /// <summary>
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
                    cardFromUnoHand = _model.UnoHand.Last();

                    //aggiungo carta al deck del giocatore
                    _model.Views[i]._hand.Add(cardFromUnoHand);

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
            if (currentView != _views[_turn])
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
                _model.drawFromDrawHand(_model.Views[n]._hand);
            }
        }
 

      
        /// <summary>
        /// metodo per scartare una propria carta dal proprio mazzo
        /// </summary>
        /// <param name="selectedCard"></param>
        public void discardCard(Message message)
        {
           
            var selectedCard = JsonSerializer.Deserialize<Card>(message.Body);
            checkDiscardedCard(selectedCard);
            _model.discardCardFromMyHand(selectedCard, _model.Views[n]._hand);
     
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
            //controllare se il mazzo del giocatore è vuoto
        }


        //DA FARE
        //se posso scartare, tolgo dal deck del giocatore la carta e la agggiungo al
        //mazzo scarto. Se non posso scartare, pesco. Se ho pescato e non ho niente passo il turno
        public void checkCardAvailability(PlayerView currentView, Message message)
        {
            //controllo se la view  del player che passo è uguale alla view
            if (currentView != _views[_turn])
                throw new InvalidOperationException();
           

        }

        /// <summary>
        /// questo metodo prende il numero di giocatori che partecipano
        /// e incrementa il turno del giocatore successivo
        /// </summary>
        private void nextTurn()
        {
            
       
            if (_turn ==2)
            {
                _turn = 0;
                
            }
            else
            {

                _turn += 1;
            }


        }

        //DA FARE
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
