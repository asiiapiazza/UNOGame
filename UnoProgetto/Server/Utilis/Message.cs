using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnoGame
{
    
    public class Message
    {

        public Type Type { get; set; }
        public string Body { get; set; }
        public int NPlayers { get; set; }


    }

    public enum Type
    {
        START,
        CARD_NUMBER,
        JOLLY,
        DRAW_CARDS,
        WIN,
        LOSE
    }
}
