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

        public TypeCard Type { get; set; }
        public string Body { get; set; }


    }


    //SELECT_CARD = NEXT_TURN
    public enum TypeCard
    {
        DRAW_CARDS,
        START,
        NEXT_TURN,
        SAID_UNO,
        WIN,
        LOSE
    }
}
