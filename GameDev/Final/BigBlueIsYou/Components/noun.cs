namespace CS5410.Components
{
    public enum Objects
    {
        BigBlue,
        Wall,
        Rock,
        Flag,
        Floor,
        Grass,
        Water,
        Lava,
        Hedge
    }

    public class Noun : Component
    {
        public Objects Object
        {
            get;
            set; // if a "noun is noun" rule is set, the noun can change
        }

        public Noun(Objects obj)
        {
            Object = obj;
        }
    }
}
