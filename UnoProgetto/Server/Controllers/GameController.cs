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
        public List<PlayerView> opponentviews;

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
            opponentviews = _views.Where(t => t != _views[_turn]).ToList();

            //restituisce il numero di carte che hanno gli avversari
            var nOpponentsCards = oppponentsViewCards(opponentviews);

            //messaggio di startare il turno (scelta della carta)
            _views[_turn].SendMessage(new Message { Type = TypeMessage.START_TURN, MyHand = _views[_turn]._hand, nOpponentCards = nOpponentsCards, lastDiscardeCard = _model.DiscardedHand.Last(), alreadyDiscarded = false });


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
            opponentviews = new List<PlayerView>();
            int opponentTurn = 0;

            switch (_turn)
            {
                //turno del giocatore 0
                case 0:

                    //view giocatore 1
                    opponentTurn = _turn + 1;
                    opponentviews = _views.Where(t => t != _views[opponentTurn]).ToList();
                    nOpponentsCards = oppponentsViewCards(opponentviews);
                    _views[opponentTurn].SendMessage(new Message { Type = TypeMessage.WAITING_TURN, MyHand = _views[opponentTurn]._hand, nOpponentCards = nOpponentsCards, lastDiscardeCard = _model.DiscardedHand.Last(), alreadyDiscarded = false });

                    //view giocatore 2
                    opponentTurn = _turn + 2;
                    opponentviews = _views.Where(t => t != _views[opponentTurn]).ToList();
                    nOpponentsCards = oppponentsViewCards(opponentviews);
                    _views[opponentTurn].SendMessage(new Message { Type = TypeMessage.WAITING_TURN, MyHand = _views[opponentTurn]._hand, nOpponentCards = nOpponentsCards, lastDiscardeCard = _model.DiscardedHand.Last(), alreadyDiscarded = false });
                    break;


                //turno del giocatore 1
                case 1:

                    //view giocatore 2
                    opponentTurn = _turn + 1;
                    opponentviews = _views.Where(t => t != _views[opponentTurn]).ToList();
                    nOpponentsCards = oppponentsViewCards(opponentviews);
                    _views[opponentTurn].SendMessage(new Message { Type = TypeMessage.WAITING_TURN, MyHand = _views[opponentTurn]._hand, nOpponentCards = nOpponentsCards, lastDiscardeCard = _model.DiscardedHand.Last(), alreadyDiscarded = false });

                    //view giocatore 0
                    opponentTurn = _turn - 1;
                    opponentviews = _views.Where(t => t != _views[opponentTurn]).ToList();
                    nOpponentsCards = oppponentsViewCards(opponentviews);
                    _views[opponentTurn].SendMessage(new Message { Type = TypeMessage.WAITING_TURN, MyHand = _views[opponentTurn]._hand, nOpponentCards = nOpponentsCards, lastDiscardeCard = _model.DiscardedHand.Last(), alreadyDiscarded = false });


                    break;

                //turno del giocatore 2
                case 2:

                    //view giocatore 1
                    opponentTurn = _turn - 1;
                    opponentviews = _views.Where(t => t != _views[opponentTurn]).ToList();
                    nOpponentsCards = oppponentsViewCards(opponentviews);
                    _views[opponentTurn].SendMessage(new Message { Type = TypeMessage.WAITING_TURN, MyHand = _views[opponentTurn]._hand, nOpponentCards = nOpponentsCards, lastDiscardeCard = _model.DiscardedHand.Last(), alreadyDiscarded = false });

                    //view giocatore 0
                    opponentTurn = _turn - 2;
                    opponentviews = _views.Where(t => t != _views[opponentTurn]).ToList();
                    nOpponentsCards = oppponentsViewCards(opponentviews);
                    _views[opponentTurn].SendMessage(new Message { Type = TypeMessage.WAITING_TURN, MyHand = _views[opponentTurn]._hand, nOpponentCards = nOpponentsCards, lastDiscardeCard = _model.DiscardedHand.Last(), alreadyDiscarded = false });

                    break;
                default:
                    Console.WriteLine("This turn doesn't exist");
                    break;
            }
        }






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
            var card = new Card();
            var rnd = new Random();
            var index = 0;
            do
            {
                index = rnd.Next(0, _model.UnoHand.Count - 1);
                card = _model.UnoHand[index];
                if (card.Type == Models.Type.DRAW_TWO)
                {
                    drawNCards(2);
                }
                else if (card.Type == Models.Type.STOP_TURN)
                {
                    altCard();
                }
                else if (card.Type == Models.Type.INVERT_TURN)
                {
                    invertTurnOrder();
                }
                else
                {
                    _model.UnoHand.Remove(card);
                }
            } while (card.Type == Models.Type.DRAW_FOUR);


            _model.DiscardedHand.Add(card);


        }


        /// <summary>
        /// distribuisce 7 carte dal mazzo base ad ogni mazzo dei giocatori
        /// </summary>
        private void starterHands()
        {
            Card cardFromUnoHand;
            var rnd = new Random();
            int index = 0;
            for (int i = 0; i < _views.Count(); i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    //prendo la carta in cima al mazzo coperta
                    index = rnd.Next(0, _model.UnoHand.Count - 1);
                    cardFromUnoHand = _model.UnoHand[index];

                    //rimuovo carta dal deck
                    _model.UnoHand.Remove(cardFromUnoHand);


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
        private void distribuiteCards()
        {
            _lastDiscardedCard = _model.DiscardedHand.Last();

            if (_lastDiscardedCard.Type == Models.Type.DRAW_TWO)
            {
                drawNCards(2);
            }
            else if (_lastDiscardedCard.Type == Models.Type.STOP_TURN)
            {

                altCard();
            }
            else if (_lastDiscardedCard.Type == Models.Type.INVERT_TURN)
            {

                invertTurnOrder();
            }
            else if (_lastDiscardedCard.Type == Models.Type.DRAW_FOUR)
            {
                drawNCards(4);
            }
        }

        /// <summary>
        /// Metodo pesca N carte 
        /// </summary>
        /// <param name="nCardsToDraw">numero carte da pescare</param>
        private void drawNCards(int nCardsToDraw)
        {
            //al giocatore dopo, faccio pescare
            nextTurn();
            for (int i = 0; i < nCardsToDraw; i++)
            {
                _model.drawFromDrawDeck(_turn);
            }

            //ritorno al turno originale
            invertTurnOrder();
        }



        /// <summary>
        /// Metodo per scartare una propria carta dal proprio mazzo
        /// </summary>
        /// <param name="selectedCard"></param>
        public void discardCard(Message message)
        {

            var card = JsonSerializer.Deserialize<Card>(message.Body);

            if (card != null)
            {
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



        }


        //FATTO
        bool checkDiscardedCard(Card selectedCard, List<Card> playerHand)
        {

            _lastDiscardedCard = _model.DiscardedHand.Last();
            bool discardedBool = false;


            if (selectedCard.Color == _lastDiscardedCard.Color || selectedCard.Type == _lastDiscardedCard.Type || _lastDiscardedCard.Type == Models.Type.JOLLY || selectedCard.Type == Models.Type.DRAW_FOUR || selectedCard.Type == Models.Type.JOLLY)
            {
                discardedBool = true;
                _model.discardCardFromMyHand(selectedCard, _turn);

                //qui controllo se il player deve pescare qualche carta prima
                distribuiteCards();
                Console.WriteLine($"Player {_turn} has discarded card {selectedCard.Color} {selectedCard.Type}");
            }

            else if (selectedCard.Type == Models.Type.CHANGE_COLOR)
            {

                discardedBool = true;
            }
            else
            {

                discardedBool = false;
                Console.WriteLine($"Player {_turn} Draw/Choose another card: can't discard card {selectedCard.Color} {selectedCard.Type}");
            }

            return discardedBool;
        }



        public void isWinner(List<Card> hand)
        {
            //controllare se il mazzo del giocatore è vuoto
            if (hand.Count == 0)
            {
                Console.WriteLine("You Won!");
            }
        }
        public void invertTurnOrder()
        {
            if (_turn == 0)
            {
                _turn = 2;

            }
            else
            {

                _turn -= 1;
            }
        }


        public void altCard()
        {
            nextTurn();
        }

        public void checkDrawDeck()
        {

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
            opponentviews = _views.Where(t => t != _views[_turn]).ToList();
            List<int> nOpponentsCards = oppponentsViewCards(opponentviews);

            opponentsViews();
            //se turno è uguale a 1, so che il player 
            _views[_turn].SendMessage(new Message { Type = TypeMessage.START_TURN, MyHand = _views[_turn]._hand, lastDiscardeCard = _model.DiscardedHand.Last(), nOpponentCards = nOpponentsCards });

        }

        internal void Draw()
        {
            var drewCard = _model.drawFromDrawDeck(_turn);


            //inccongruenza: non dovrei mandargli il numero di carte del giocatore perche non cambia
            opponentviews = _views.Where(t => t != _views[_turn]).ToList();
            List<int> nOpponentsCards = oppponentsViewCards(opponentviews);

            //controllo se la carta pescata posso scartarla o meno (DA VEDERE)
            bool canIDiscard = false;

            while (!canIDiscard)
            {
                for (int i = 0; i < _views[_turn]._hand.Count; i++)
                {
                    if (_views[_turn]._hand[i].Color == drewCard.Color || _views[_turn]._hand[i].Type == drewCard.Type || _views[_turn]._hand[i].Type == Models.Type.CHANGE_COLOR || _views[_turn]._hand[i].Type == Models.Type.DRAW_FOUR || _views[_turn]._hand[i].Type == Models.Type.JOLLY)
                    {
                        canIDiscard = true;

                    }

                }
            }
            


            if (canIDiscard)
            {
                _views[_turn].SendMessage(new Message { Type = TypeMessage.START_TURN, MyHand = _views[_turn]._hand, nOpponentCards = nOpponentsCards, lastDiscardeCard = _model.DiscardedHand.Last(), alreadyDiscarded = true });
                opponentsViews();

            }
            else
            {
                _views[_turn].SendMessage(new Message { Type = TypeMessage.WAITING_TURN, MyHand = _views[_turn]._hand, nOpponentCards = nOpponentsCards, lastDiscardeCard = _model.DiscardedHand.Last(), alreadyDiscarded = false });

                Console.WriteLine($"Player {_turn} can't do any other move");
                nextTurn();
                StartTurn();
            }
            

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
        }
    }
}
