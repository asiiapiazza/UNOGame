namespace UnoGame.Models
{

    public class Card
    {
        public Card()
        {
        }

        public Card(Type type, Color color)
        {
            this.Type = type;
            this.Color = color;
        }

        public Type Type { get; set; }
        public Color Color { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Card card &&
                   Type == card.Type &&
                   Color == card.Color;
        }
    }

    public enum Type
    {
        ZERO,
        ONE,
        TWO,
        THREE,
        FOUR,
        FIVE,
        SIX,
        SEVEN,
        EIGHT,
        NINE,
        DRAW_TWO,
        DRAW_FOUR,
        INVERT_TURN,
        STOP_TURN,
        JOLLY,
        CHANGE_COLOR
    }

    public enum Color
    {
        RED,
        YELLOW,
        BLUE,
        GREEN,
        NONE
    }




}
