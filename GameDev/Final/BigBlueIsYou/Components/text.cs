namespace CS5410.Components
{
    public class Text : Component
    {
        public string Word
        {
            get;
            private set;
        }

        public Text(string text)
        {
            Word = text;
        }
    }
}
