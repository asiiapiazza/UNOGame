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
            var rnw = new Random();
            _turn = rnw.Next(0, 2);
            starterHands();
            firstDiscardedCard();
            //serializzazione, manda messaggio al server di INIZIARE IL GIOCO

            var opponentviews = _views.Where(t => t != _views[_turn]).ToList();

            foreach (var item in opponentviews)
            {
                item.SendMessage(new Message { Type = TypeMessage.WAITING_TURN, Body = JsonSerializer.Serialize(_model), MyHand = item._hand});


            }

            _views[_turn].SendMessage(new Message { Type = TypeMessage.START_TURN, Body = JsonSerializer.Serialize(_model), MyHand = _views[_turn]._hand });


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
                _model.drawFromDrawHand(_model.Views[_turn]._hand);
            }
        }
 

      
        /// <summary>
        /// metodo per scartare una propria carta dal proprio mazzo
        /// </summary>
        /// <param name="selectedCard"></param>
        public void discardCard(Message message)
        {
           
            var index = JsonSerializer.Deserialize<int>(message.Body);
            var playerHand = message.MyHand;

            bool discarded = checkDiscardedCard(index, playerHand);

            //se la carta che vuole sccartare non  va bene, riprova a scartare
            if (discarded != true)
            {
                StartTurn();
            }
            else
            {
              

                nextTurn();
                StartTurn();
            }
          

        }

   
        //FATTO
        bool checkDiscardedCard(int indexCard, List<Card> playerHand)
        {
       
            _lastDiscardedCard = _model.DiscardedHand.Last();
            var selectedCard = playerHand[indexCard];
            bool discardedBool = false;

            if (selectedCard.Color == _lastDiscardedCard.Color || selectedCard.Type == _lastDiscardedCard.Type)
            {
                discardedBool = true;
                _model.discardCardFromMyHand(indexCard, playerHand);
            }
            else if (selectedCard.Type == Models.Type.JOLLY)
            {
                discardedBool = true;
                _model.discardCardFromMyHand(indexCard, playerHand);
            }
            else if (selectedCard.Type == Models.Type.CHANGE_COLOR)
            {

                discardedBool = true;
            }
            else 
            {

                discardedBool = false;
                Console.WriteLine("Draw/Choose another card!");
            }

            return discardedBool;
        }


        //DA FARE
        public void isWinner(List<Card> hand)
        {
            //controllare se il mazzo del giocatore è vuoto
            if (hand.Count == 0)
            {
                Console.WriteLine("You Won!");
            }
        }


       
        //DA FARE
        public void invertTurnOrder()
        {
            
        }


        public void altCard()
        {
            _turn++;
        }

        public void checkDrawDeck()
        {
            //usare _model
            //se il mazzo da cui pescare è finito
            //richiamo il metodo che aggiunge il deck delle carte scartate
            //tranne l'ultima che ho scartato
            //stampare l'ultima carta del mazzo scarti

            if (_model.UnoHand.Count == 0)
            {
                _lastDiscardedCard = _model.DiscardedHand.Last();
                var rnd = new Random();
                var randomized = _model.UnoHand.OrderBy(a => Guid.NewGuid()).ToList();
                _model.DiscardedHand.Remove(_lastDiscardedCard);
                _model.UnoHand = _model.DiscardedHand;
                _model.UnoHand = randomized;
            }
        }

    
     
        private void StartTurn()
        {
            var opponentviews = _views.Where(t => t != _views[_turn]).ToList();

            foreach (var item in opponentviews)
            {
                //arriva messaggio update e waiting
                item.SendMessage(new Message { Type = TypeMessage.MODEL_UPDATE, Body = JsonSerializer.Serialize(_model), MyHand = item._hand });
             
            }

            _views[_turn].SendMessage(new Message { Type = TypeMessage.START_TURN, Body = JsonSerializer.Serialize(_model), MyHand = _views[_turn]._hand });
     
        }


        private void nextTurn()
        {


            //if (_turn == 2)
            //{
            //    _turn = 0;

            //}
            //else
            //{

            //    _turn += 1;
            //}

            _turn = (_turn + 1) % 2;


        }
    }
}
