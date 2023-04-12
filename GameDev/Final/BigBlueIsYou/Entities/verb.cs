namespace CS5410.Entities
{
    public class Verb
    {
        public static Entity create(string text, int x, int y)
        {
            var word = new Entity();

            word.AddComponents(
                    new Components.Text(text),
                    new Components.Position(x, y),
                    new Components.Sprite(),
                    new Components.Property()
                    );

            var props = word.GetComponent<Components.Property>();

            // all words can be pushed
            props.GameProperties |= (int)Components.Properties.Push;

            return word;
        }
    }
}
