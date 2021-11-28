using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnoGame
{
    
    public class Message
    {

        public TypeCard Type { get; set; }
        public string Body { get; set; }


    }

    public enum TypeCard
    {
        DRAW_CARDS,
        START,
        SAID_UNO,
        CARD_NUMBER,
        JOLLY,
        MODEL_UPDATE,
        WIN,
        LOSE
    }
}
