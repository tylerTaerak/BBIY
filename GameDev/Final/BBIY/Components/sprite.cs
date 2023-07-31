namespace CS5410.Components
{
    public enum RenderLayer
    {
        Bottom,
        Middle,
        Top
    }

    public class Sprite : Component
    {
        public RenderLayer Layer
        {
            get;
            set;
        }

        public Sprite(RenderLayer layer)
        {
            Layer = layer;
        }

        public override Sprite Copy()
        {
            return new Sprite(Layer);
        }
    }
}
