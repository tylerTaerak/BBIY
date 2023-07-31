namespace CS5410.Entities
{
    public class Water
    {
        public static Entity create(int x, int y)
        {
            var water = new Entity();

            water.AddComponents(
                    new Components.Noun(Components.Objects.Water),
                    new Components.Position(x, y),
                    new Components.Sprite(Components.RenderLayer.Bottom),
                    new Components.Property()
                    );

            return water;
        }
    }
}
