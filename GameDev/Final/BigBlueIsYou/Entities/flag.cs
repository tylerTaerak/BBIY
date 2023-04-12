namespace CS5410.Entities
{
    public class Flag
    {
        public static Entity create(int x, int y)
        {
            var flag = new Entity();

            flag.AddComponents(
                    new Components.Noun(Components.Objects.Flag),
                    new Components.Position(x, y),
                    new Components.Sprite(),
                    new Components.Property()
                    );

            return flag;
        }
    }
}
