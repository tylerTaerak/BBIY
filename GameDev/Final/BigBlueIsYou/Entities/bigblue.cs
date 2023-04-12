namespace CS5410.Entities
{
    public class BigBlue
    {
        public static Entity create(int x, int y)
        {
            var bb = new Entity();

            bb.AddComponents(
                    new Components.Noun(Components.Objects.BigBlue),
                    new Components.Position(x, y),
                    new Components.Sprite(),
                    new Components.Property()
                    );

            return bb;
        }
    }
}
