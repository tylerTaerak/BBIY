namespace CS5410.Entities
{
    public class Baba
    {
        public static Entity create(int x, int y)
        {
            var bb = new Entity();

            bb.AddComponents(
                    new Components.Noun(Components.Objects.Baba),
                    new Components.Position(x, y),
                    new Components.Sprite(Components.RenderLayer.Top),
                    new Components.Property()
                    );

            return bb;
        }
    }
}
