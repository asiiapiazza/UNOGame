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

    }


    //SELECT_CARD = NEXT_TURN
    public enum TypeMessage
    {
        START_GAME,
        START_TURN,
        MODEL_UPDATE,
        WAITING_TURN,
        WIN,
        LOSE
    }
}
