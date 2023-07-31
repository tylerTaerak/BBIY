namespace CS5410.Entities
{
    public class Grass
    {
        public static Entity create(int x, int y)
        {
            var grass = new Entity();

            grass.AddComponents(
                    new Components.Noun(Components.Objects.Grass),
                    new Components.Position(x, y),
                    new Components.Sprite(Components.RenderLayer.Bottom),
                    new Components.Property()
                    );

            return grass;
        }
    }
}
