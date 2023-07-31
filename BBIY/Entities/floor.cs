namespace CS5410.Entities
{
    public class Floor
    {
        public static Entity create(int x, int y)
        {
            var floor = new Entity();

            floor.AddComponents(
                    new Components.Noun(Components.Objects.Floor),
                    new Components.Position(x, y),
                    new Components.Sprite(Components.RenderLayer.Bottom),
                    new Components.Property()
                    );

            return floor;
        }
    }
}
