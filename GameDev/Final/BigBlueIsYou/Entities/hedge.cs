namespace CS5410.Entities
{
    public class Hedge
    {
        public static Entity create(int x, int y)
        {
            var hedge = new Entity();

            hedge.AddComponents(
                    new Components.Noun(Components.Objects.Hedge),
                    new Components.Position(x, y),
                    new Components.Sprite(),
                    new Components.Property()
                    );

            return hedge;
        }
    }
}
