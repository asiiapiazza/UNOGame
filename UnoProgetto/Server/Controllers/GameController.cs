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
      
        private PlayerView[] _views;
        public int _turn;
        public GameModel _model;
        private Card _lastDiscardedCard;
    

        public GameController(GameModel model)
        {
            _model = model;
            _views = new PlayerView[3];
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
            var rnw = new Random();
            _turn = rnw.Next(0, 2);


            //metodo che distribuisce 7 carte ad ogni giocatore della view
            starterHands();

            //scelgo la prima carta dal deck scarta
            firstDiscardedCard();

         
            //LINQ per trovare gli avversari
            var opponentviews = _views.Where(t => t != _views[_turn]).ToList();

            //restituisce il numero di carte che hanno gli avversari
            var nOpponentsCards = oppponentsViewCards(opponentviews);

            //messaggio di startare il turno (scelta della carta)
            _views[_turn].SendMessage(new Message { Type = TypeMessage.START_TURN, MyHand = _views[_turn]._hand, nOpponentCards = nOpponentsCards, lastDiscardeCard = _model.DiscardedHand.Last() });


            opponentsViews();
       


        }

        /// <summary>
        /// metodo che mi restituisce il numero di carte che hanno gli avversari
        /// </summary>
        /// <param name="playerViews"></param>
        /// <returns></returns>
        internal List<int> oppponentsViewCards(List<PlayerView> playerViews)
        {
            List<int> nCardsOpponents = new List<int>();
            foreach (var item in playerViews)
            {
                nCardsOpponents.Add(item._hand.Count);
            }

            return nCardsOpponents;
        }

        //il giocatore del turno vede i mazzi avversari coperti, ma gli avversasi non vedono il mazzo degli avversari coperti. Utilizzo metodo separato
        private void opponentsViews()
        {
            List<int> nOpponentsCards = new List<int>();
            var opponentviews = new List<PlayerView>();
            int opponentTurn = 0;
        
            switch (_turn)
            {
                //turno del giocatore 0
                case 0:

                    //view giocatore 1
                    opponentTurn = _turn + 1;
                     opponentviews = _views.Where(t => t != _views[opponentTurn]).ToList();
                    nOpponentsCards = oppponentsViewCards(opponentviews);
                    _views[opponentTurn].SendMessage(new Message { Type = TypeMessage.WAITING_TURN,  MyHand = _views[opponentTurn]._hand, nOpponentCards = nOpponentsCards, lastDiscardeCard = _model.DiscardedHand.Last() });

                    //view giocatore 2
                    opponentTurn = _turn + 2;
                    opponentviews = _views.Where(t => t != _views[opponentTurn]).ToList();
                    nOpponentsCards = oppponentsViewCards(opponentviews);
                    _views[opponentTurn].SendMessage(new Message { Type = TypeMessage.WAITING_TURN,  MyHand = _views[opponentTurn]._hand, nOpponentCards = nOpponentsCards, lastDiscardeCard = _model.DiscardedHand.Last() });
                    break;


                //turno del giocatore 1
                case 1:

                    //view giocatore 2
                    opponentTurn = _turn + 1;
                    opponentviews = _views.Where(t => t != _views[opponentTurn]).ToList();
                    nOpponentsCards = oppponentsViewCards(opponentviews);
                    _views[opponentTurn].SendMessage(new Message { Type = TypeMessage.WAITING_TURN,  MyHand = _views[opponentTurn]._hand, nOpponentCards = nOpponentsCards, lastDiscardeCard = _model.DiscardedHand.Last() });

                    //view giocatore 0
                    opponentTurn = _turn - 1;
                    opponentviews = _views.Where(t => t != _views[opponentTurn]).ToList();
                    nOpponentsCards = oppponentsViewCards(opponentviews);
                    _views[opponentTurn].SendMessage(new Message { Type = TypeMessage.WAITING_TURN, MyHand = _views[opponentTurn]._hand, nOpponentCards = nOpponentsCards, lastDiscardeCard = _model.DiscardedHand.Last() });


                    break;

                //turno del giocatore 2
                case 2:

                    //view giocatore 1
                    opponentTurn = _turn - 1;
                    opponentviews = _views.Where(t => t != _views[opponentTurn]).ToList();
                    nOpponentsCards = oppponentsViewCards(opponentviews);
                    _views[opponentTurn].SendMessage(new Message { Type = TypeMessage.WAITING_TURN,  MyHand = _views[opponentTurn]._hand, nOpponentCards = nOpponentsCards, lastDiscardeCard = _model.DiscardedHand.Last() });

                    //view giocatore 0
                    opponentTurn = _turn - 2;
                    opponentviews = _views.Where(t => t != _views[opponentTurn]).ToList();
                    nOpponentsCards = oppponentsViewCards(opponentviews);
                    _views[opponentTurn].SendMessage(new Message { Type = TypeMessage.WAITING_TURN, MyHand = _views[opponentTurn]._hand, nOpponentCards = nOpponentsCards, lastDiscardeCard = _model.DiscardedHand.Last() });

                    break;
                default:
                    break;
            }
        
           
         

        

            
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
        /// 
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
                    cardFromUnoHand = _model.UnoHand[_model.UnoHand.Count-1];

                    //rimuovo carta dal deck
                    _model.UnoHand.RemoveAt(_model.UnoHand.Count-1);


                    //aggiungo carta al deck del giocatore
                    _model.Views[i]._hand.Add(cardFromUnoHand);

        
                    
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
                _model.drawFromDrawHand( _turn);
            }
        }
 

      
        /// <summary>
        /// metodo per scartare una propria carta dal proprio mazzo
        /// </summary>
        /// <param name="selectedCard"></param>
        public void discardCard(Message message)
        {
           
            var card = JsonSerializer.Deserialize<Card>(message.Body);
            var playerHand = message.MyHand;

            
            bool discarded = checkDiscardedCard(card, playerHand);

            
            //se la carta che vuole sccartare non  va bene, riprova a scartare
            if (!discarded)
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
        bool checkDiscardedCard(Card selectedCard, List<Card> playerHand)
        {
       
            _lastDiscardedCard = _model.DiscardedHand.Last();
            
            bool discardedBool = false;

               
                if (selectedCard.Color == _lastDiscardedCard.Color || selectedCard.Type == _lastDiscardedCard.Type || _lastDiscardedCard.Type== Models.Type.JOLLY || selectedCard.Type == Models.Type.DRAW_FOUR || selectedCard.Type == Models.Type.JOLLY)
                {
                    discardedBool = true;
                    _model.discardCardFromMyHand(selectedCard, _turn);
                    Console.WriteLine("Card discarded");
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
            
            //else
            //{
            //    _model.drawFromDrawHand(playerHand);
            //    Console.WriteLine("Player drew from draw deck");
            //}
            

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
            List<int> nOpponentsCards = oppponentsViewCards(opponentviews);

            opponentsViews();  
            //se turno è uguale a 1, so che il player 
            _views[_turn].SendMessage(new Message { Type = TypeMessage.START_TURN, MyHand = _views[_turn]._hand, lastDiscardeCard = _model.DiscardedHand.Last(), nOpponentCards = nOpponentsCards });
     
        }

        internal void drawFromDrawDeck(Message message)
        {

            var card = JsonSerializer.Deserialize<Card>(message.Body);
            _model.drawFromDrawHand(_turn);


        }
        private void nextTurn()
        {


            if (_turn == 2)
            {
                _turn = 0;

            }
            else
            {

                _turn += 1;
            }

            //_turn = (_turn + 1) % 2;


        }
    }
}
