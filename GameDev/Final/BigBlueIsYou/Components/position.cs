namespace CS5410.Components
{
    public class Position : Component
    {
        public (int, int) CurrentPosition
        {
            get;
            set;
        }

        public Position(int initX, int initY)
        {
            CurrentPosition = (initX, initY);
        }
    }
}
