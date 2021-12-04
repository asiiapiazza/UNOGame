using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnoGame.Models;

namespace UnoGame
{
    
    public class Message
    {

        public TypeMessage Type { get; set; }
        public string Body { get; set; }
        public List<Card> MyHand { get; set; }
        public Card lastDiscardeCard { get; set; }
        public List<int> nOpponentCards { get; set; }
        public bool alreadyDiscarded { get; set; }
    }


    //SELECT_CARD = NEXT_TURN
    public enum TypeMessage
    {
        START_GAME,
        DRAW_CARD,
        START_TURN,
        HAND_UPDATE_VIEW,
        WAITING_TURN,
        WIN,
        LOSE
    }
}
