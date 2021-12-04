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
        public List<PlayerView> _opponentViews;
        public bool _clockWise = true;
        public int _turn;
        public GameModel _model;
        private PlayerView[] _views;
        private Card _lastDiscardedCard;
        public Random rnd = new Random();
        private int opponentTurn = 0;

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
 
        public void Start()
        {
            _turn = rnd.Next(0, 2);

            //metodo che distribuisce 7 carte ad ogni giocatore della view
            StarterHands();

            //scelgo la prima carta iniziale
            FirstDiscardedCard();

            //Linq per trovare gli avversari rispetto al giocatore del primo turno
            _opponentViews = _views.Where(t => t != _views[_turn]).ToList();

            //restituisce il numero di carte che hanno gli avversari
            var nOpponentsCards = OppponentsViewCards(_opponentViews);

            //messaggio di startare il turno (scelta della carta)
            _views[_turn].SendMessage(new Message { Type = TypeMessage.START_TURN, MyHand = _views[_turn]._hand, nOpponentCards = nOpponentsCards, lastDiscardeCard = _model.DiscardedHand.Last(), alreadyDiscarded = false });

            //metodo per la stampa della view degli altri avversari (N carte coperte)
            OpponentsViews(new Message { Type = TypeMessage.WAITING_TURN, MyHand = _views[opponentTurn]._hand, nOpponentCards = nOpponentsCards, lastDiscardeCard = _model.DiscardedHand.Last(), alreadyDiscarded = false });

        }

        /// <summary>
        /// Metodo che mi restituisce il numero di carte che hanno gli avversari, salvandoli in una lista
        /// </summary>
        /// <param name="playerViews"></param>
        /// <returns></returns>
        internal List<int> OppponentsViewCards(List<PlayerView> playerViews)
        {
            List<int> nCardsOpponents = new List<int>();
            foreach (var item in playerViews)
            {
                nCardsOpponents.Add(item._hand.Count);
            }

            return nCardsOpponents;
        }

        /// <summary>
        /// Metodo per mandare messaggi agli opponenti
        /// </summary>
        private void OpponentsViews(Message message)
        {
            List<int> nOpponentsCards = new List<int>();
            _opponentViews = new List<PlayerView>();
     
            switch (_turn)
            {
                //turno del giocatore 0
                case 0:

                    //view giocatore 1
                    opponentTurn = _turn + 1;

                    //il valore di opponent cambia per tutti i turni. Nel caso in cui abbia bisogno di mandare START_TURN, devo riassegnare la mano.
                    message.MyHand = _views[opponentTurn]._hand;
                    _opponentViews = _views.Where(t => t != _views[opponentTurn]).ToList();
                    nOpponentsCards = OppponentsViewCards(_opponentViews);
                    _views[opponentTurn].SendMessage(message);

                    //view giocatore 2

                    opponentTurn = _turn + 2;
                    message.MyHand = _views[opponentTurn]._hand;
                    _opponentViews = _views.Where(t => t != _views[opponentTurn]).ToList();
                    nOpponentsCards = OppponentsViewCards(_opponentViews);
                    _views[opponentTurn].SendMessage(message); break;


                //turno del giocatore 1
                case 1:

                    //view giocatore 2
                    opponentTurn = _turn + 1;
                    message.MyHand = _views[opponentTurn]._hand;
                    _opponentViews = _views.Where(t => t != _views[opponentTurn]).ToList();
                    nOpponentsCards = OppponentsViewCards(_opponentViews);
                    _views[opponentTurn].SendMessage(message);

                    //view giocatore 0
                    opponentTurn = _turn - 1;
                    message.MyHand = _views[opponentTurn]._hand;
                    _opponentViews = _views.Where(t => t != _views[opponentTurn]).ToList();
                    nOpponentsCards = OppponentsViewCards(_opponentViews);
                    _views[opponentTurn].SendMessage(message);

                    break;

                //turno del giocatore 2
                case 2:

                    //view giocatore 1
                    opponentTurn = _turn - 1;
                    message.MyHand = _views[opponentTurn]._hand;
                    _opponentViews = _views.Where(t => t != _views[opponentTurn]).ToList();
                    nOpponentsCards = OppponentsViewCards(_opponentViews);
                    _views[opponentTurn].SendMessage(message);

                    //view giocatore 0
                    opponentTurn = _turn - 2;
                    
                    
                    message.MyHand = _views[opponentTurn]._hand;
                    _opponentViews = _views.Where(t => t != _views[opponentTurn]).ToList();
                    nOpponentsCards = OppponentsViewCards(_opponentViews);
                    _views[opponentTurn].SendMessage(message);
                    break;

                default:
                    Console.WriteLine("This turn doesn't exist");
                    break;
            }
        }


        /// <summary>
        /// Metodo per la pesca dal deck UnoHand della prima carta scartata ad inizio game
        /// Nel caso in cui la prima carta sia una carta Azione, è necessario applicare le regole della sezione Funzioni delle carte
        ///; fa eccezione la carta "Jolly Pesca Quattro" che, se scoperta all'inizio del gioco, deve essere 
        ///rimessa a caso nel "mazzo pesca" e sostituita con un'altra.
        /// </summary>
        /// <returns></returns>
        /// 
        private void FirstDiscardedCard()
        {
            var card = new Card();
            var index = 0;
            do
            {
                //la  carta è presa random
                index = rnd.Next(0, _model.UnoHand.Count - 1);
                card = _model.UnoHand[index];
                if (card.Type == Models.Type.DRAW_TWO)
                {
                    DrawNCards(2);
                }
                else if (card.Type == Models.Type.STOP_TURN)
                {
                    AltCard();
                }
                else if (card.Type == Models.Type.INVERT_TURN)
                {
                    InvertTurnOrder();
                }
                else
                {
                    _model.UnoHand.Remove(card);
                }
            } while (card.Type == Models.Type.DRAW_FOUR);

            //aggiungo la carta al mazzo di carte scartate
            _model.DiscardedHand.Add(card);

        }


        /// <summary>
        /// Metodo distribuzione 7 carte dal mazzo base ad ogni mazzo dei giocatori.
        /// Le carte sono prese randomicamente dal mazzo UnoHand non mischiato.
        /// </summary>
        private void StarterHands()
        {
            Card cardFromUnoHand;
            int index = 0;
            for (int i = 0; i < _views.Count(); i++)
            {
                for (int j = 0; j < 7; j++)
                {
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
        /// Metodo per il richiamo delle funzioni in base all'ultima carta scartata (+4,+2, cambio giro, salta turno)
        /// </summary>
        private void CheckActionCard()
        {
            _lastDiscardedCard = _model.DiscardedHand.Last();

            if (_lastDiscardedCard.Type == Models.Type.DRAW_TWO)
            {
                DrawNCards(2);
            }
            else if (_lastDiscardedCard.Type == Models.Type.STOP_TURN)
            {

                AltCard();
            }
            else if (_lastDiscardedCard.Type == Models.Type.INVERT_TURN)
            {

                InvertTurnOrder();
            }
            else if (_lastDiscardedCard.Type == Models.Type.DRAW_FOUR)
            {
                DrawNCards(4);
            }
        }

        /// <summary>
        /// Metodo pesca N carte 
        /// </summary>
        /// <param name="nCardsToDraw">numero carte da pescare</param>
        private void DrawNCards(int nCardsToDraw)
        {
          
            var tempTurn = _turn;

            //aumento il turno in modo di far pescare il giocatore dopo del player che ha scartatp
            NextTurn();

            CheckDrawDeck();
            for (int i = 0; i < nCardsToDraw; i++)
            {
                _model.DrawFromDrawDeck(_turn);
            }

            //ritorno al turno originale
            _turn = tempTurn;
        }


        /// <summary>
        /// Metodo per scartare una propria carta dal proprio mazzo
        /// </summary>
        /// <param name="selectedCard"></param>
        public void DiscardCard(Message message)
        {
            var card = JsonSerializer.Deserialize<Card>(message.Body);
            if (card != null)
            {
                var playerHand = message.MyHand;
                bool discarded = CheckDiscardedCard(card);


                if (!discarded)
                {
                    //il giocatore che ha pescato, dopo aver pescato può scartare la carta pescata/carta che aveva gia nel deck

                    StartTurn();
                }
                else
                {

                    NextTurn();
                    StartTurn();
                }
            }



        }

        /// <summary>
        /// Controllo della carta da scartare. Controllo se è possibile scartarla in base all'ultima carta scartata
        /// Restituisce false se non posso scartare, true se posso.
        /// </summary>
        /// <param name="selectedCard"> la carta selezionata</param>
        /// <returns></returns>
        bool CheckDiscardedCard(Card selectedCard)
        {

            _lastDiscardedCard = _model.DiscardedHand.Last();
            bool discardedBool;

            //possibili casi in cui una carta può essere scartata
            if (selectedCard.Color == _lastDiscardedCard.Color || selectedCard.Type == _lastDiscardedCard.Type || _lastDiscardedCard.Type == Models.Type.DRAW_FOUR || _lastDiscardedCard.Type == Models.Type.JOLLY || selectedCard.Type == Models.Type.DRAW_FOUR || selectedCard.Type == Models.Type.JOLLY)
            {
                discardedBool = true;

                //metodo per scartare la carta dal deck
                _model.DiscardCardFromMyHand(selectedCard, _turn);

                //controllo se il player deve pescare qualche carta prima
                CheckActionCard();

                //controllo se ho vinto
                IsWinner();

                //log message
                Console.WriteLine($"Player {_turn} has discarded card {selectedCard.Color} {selectedCard.Type}");
            }
            else if (selectedCard.Type == Models.Type.INVERT_TURN)
            {
                
                InvertTurnOrder();
                discardedBool = true;
            }
            else
            {
               
                Console.WriteLine($"Player {_turn} Draw/Choose another card: can't discard card {selectedCard.Color} {selectedCard.Type}");
                discardedBool = false;
            }

            return discardedBool;
        }


        
        public void IsWinner()
        {
            if (_views[_turn]._hand.Count == 0)
            {
                //invio messaggio vittoria al client
                _views[_turn].SendMessage(new Message { Type = TypeMessage.WIN });

                //invio messaggio sconfitta al tutti gli avversari
                OpponentsViews(new Message { Type = TypeMessage.LOSE });
                Console.WriteLine($"Player {_turn} won");
      

            }          

        }

  
        /// <summary>
        /// Metodo per controllare se il giocatore dopo aver pescato, può scartare o salta il turno.
        /// </summary>
        public void Draw()
        {
            CheckDrawDeck();
            var drewCard = _model.DrawFromDrawDeck(_turn);
            var lastDiscardedCard = _model.DiscardedHand.Last();

            //controllo se la carta pescata posso scartarla o meno
            bool canIDiscard = false;

            _opponentViews = _views.Where(t => t != _views[_turn]).ToList();
            List<int> nOpponentsCards = OppponentsViewCards(_opponentViews);


            //controllo della carta
            for (int i = 0; i < _views[_turn]._hand.Count && !canIDiscard; i++)
            {
                if (_views[_turn]._hand[i].Color == lastDiscardedCard.Color || _views[_turn]._hand[i].Type == lastDiscardedCard.Type || _views[_turn]._hand[i].Type == Models.Type.DRAW_FOUR || _views[_turn]._hand[i].Type == Models.Type.JOLLY)
                {
                    canIDiscard = true;

                }

            }

         
            if (canIDiscard)
            {
                //se posso scartare, rimando un messagio START_TURN allo stesso player
                _views[_turn].SendMessage(new Message { Type = TypeMessage.START_TURN, MyHand = _views[_turn]._hand, nOpponentCards = nOpponentsCards, lastDiscardeCard = _model.DiscardedHand.Last(), alreadyDiscarded = true });
                
                //rimando in wait gli avversari
                OpponentsViews(new Message { Type = TypeMessage.WAITING_TURN, MyHand = _views[opponentTurn]._hand, nOpponentCards = nOpponentsCards, lastDiscardeCard = _model.DiscardedHand.Last(), alreadyDiscarded = false });

            }

            else
            {
                //se non posso scartare, mando WAITING_TURN
                _views[_turn].SendMessage(new Message { Type = TypeMessage.WAITING_TURN, MyHand = _views[_turn]._hand, nOpponentCards = nOpponentsCards, lastDiscardeCard = _model.DiscardedHand.Last(), alreadyDiscarded = false });


                //log console
                Console.WriteLine($"Player {_turn} can't do any other move");

                NextTurn();
                StartTurn();
            }

        }

        /// <summary>
        /// Metodo per il controllo del mazzo draw. Se è finito, lo ripopolo con le carte scartate tranne l'ultima
        /// </summary>
        public void CheckDrawDeck()
        {
            if (_model.UnoHand.Count == 0)
            {
                _lastDiscardedCard = _model.DiscardedHand.Last();
                _model.DiscardedHand.Remove(_lastDiscardedCard);
                _model.UnoHand = _model.DiscardedHand;
            }
        }


        private void StartTurn()
        {
            _opponentViews = _views.Where(t => t != _views[_turn]).ToList();
            List<int> nOpponentsCards = OppponentsViewCards(_opponentViews);


            //Agli avversar viene mmandato un messaggio di tipo WAITING_TURN
            OpponentsViews(new Message { Type = TypeMessage.WAITING_TURN, MyHand = _views[opponentTurn]._hand, nOpponentCards = nOpponentsCards, lastDiscardeCard = _model.DiscardedHand.Last(), alreadyDiscarded = false });

            //Al giocatore del turno, viene mmandato un messaggio di tipo START_TURN
            _views[_turn].SendMessage(new Message { Type = TypeMessage.START_TURN, MyHand = _views[_turn]._hand, lastDiscardeCard = _model.DiscardedHand.Last(), nOpponentCards = nOpponentsCards });

        }

        private void NextTurn()
        {

            if (_clockWise)
            {
                if (_turn != 2)
                {
                    _turn++;
                }
                else
                {
                    _turn = 0;
                }
            }
            else
            {
                if (_turn != 0)
                {
                    _turn--;
                }
                else
                {
                    _turn = 2;
                }
            }
        }

        private void InvertTurnOrder()
        {
            _clockWise = !_clockWise;
        }
     
        private void AltCard()
        {
            NextTurn();
        }
    }
}
