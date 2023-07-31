namespace CS5410.Entities
{
    public class Wall
    {
        public static Entity create(int x, int y)
        {
            var wall = new Entity();

            wall.AddComponents(
                    new Components.Noun(Components.Objects.Wall),
                    new Components.Position(x, y),
                    new Components.Sprite(Components.RenderLayer.Middle),
                    new Components.Property()
                    );

            return wall;
        }
    }
}
