namespace CS5410.Entities
{
    public class Rock
    {
        public static Entity create(int x, int y)
        {
            var rock = new Entity();

            rock.AddComponents(
                    new Components.Noun(Components.Objects.Rock),
                    new Components.Position(x, y),
                    new Components.Sprite(),
                    new Components.Property()
                    );

            return rock;
        }
    }
}
