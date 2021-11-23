namespace UnoGame.Models
{

    public class Card
    {
        Type type;
        Color color;

        public Card(Type type, Color color)
        {
            this.type = type;
            this.color = color;
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
