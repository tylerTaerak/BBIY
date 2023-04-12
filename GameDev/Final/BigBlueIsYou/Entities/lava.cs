namespace CS5410.Entities
{
    public class Lava
    {
        public static Entity create(int x, int y)
        {
            var lava = new Entity();

            lava.AddComponents(
                    new Components.Noun(Components.Objects.Lava),
                    new Components.Position(x, y),
                    new Components.Sprite(),
                    new Components.Property()
                    );

            return lava;
        }
    }
}
