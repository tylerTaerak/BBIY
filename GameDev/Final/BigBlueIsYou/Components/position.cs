namespace CS5410.Components
{
    public enum Direction
    {
        Stopped,
        Up,
        Down,
        Left,
        Right
    }
    public class Position : Component
    {
        public (int, int) CurrentPosition
        {
            get;
            set;
        }

        public Direction Facing
        {
            get;
            set;
        }

        public Position(int initX, int initY)
        {
            CurrentPosition = (initX, initY);
            Facing = Direction.Stopped; // nothing will be moved unless input
        }
    }
}
