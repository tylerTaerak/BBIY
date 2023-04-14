using Microsoft.Xna.Framework;

namespace CS5410.Systems
{
    public class CollisionSystem : System
    {
        public bool Win
        {
            get;
            private set;
        }

        public CollisionSystem()
            : base(
                    typeof(Components.Position),
                    typeof(Components.Property)
                  )
        {
            Win = false;
        }

        public override void update(GameTime gameTime)
        {
            foreach (Entities.Entity entity in m_entities.Values)
            {
                collide(entity);
            }
        }

        private void collide(Entities.Entity entity)
        {
            var entPos = entity.GetComponent<Components.Position>();
            var entProp = entity.GetComponent<Components.Property>();
            foreach (var other in m_entities.Values)
            {
                var otherPos = entity.GetComponent<Components.Position>();
                var otherProp = entity.GetComponent<Components.Property>();

                if (otherPos.CurrentPosition == entPos.CurrentPosition && other.Id != entity.Id)
                {
                    // then they are colliding
                    // check if push or if you
                    if (otherProp.hasProperty(Components.Properties.Push) || otherProp.hasProperty(Components.Properties.You))
                    {
                        var (x, y) = otherPos.CurrentPosition;

                        // push recursively
                        switch (entPos.Facing)
                        {
                            case Components.Direction.Up:
                                otherPos.Facing = Components.Direction.Up;
                                y -= 1;
                                break;
                            case Components.Direction.Down:
                                otherPos.Facing = Components.Direction.Down;
                                y += 1;
                                break;
                            case Components.Direction.Left:
                                otherPos.Facing = Components.Direction.Left;
                                x -= 1;
                                break;
                            case Components.Direction.Right:
                                otherPos.Facing = Components.Direction.Right;
                                x += 1;
                                break;
                        }

                        collide(other);
                    }
                    if (otherProp.hasProperty(Components.Properties.Stop))
                    {
                        // stop, pushing to previous position
                        var (x, y) = entPos.CurrentPosition;

                        // basically, push colliding entity backwards
                        switch (entPos.Facing)
                        {
                            case Components.Direction.Up:
                                entPos.Facing = Components.Direction.Down;
                                y += 1;
                                break;
                            case Components.Direction.Down:
                                entPos.Facing = Components.Direction.Up;
                                y -= 1;
                                break;
                            case Components.Direction.Left:
                                entPos.Facing = Components.Direction.Right;
                                x += 1;
                                break;
                            case Components.Direction.Right:
                                x -= 1;
                                break;
                        }

                        collide(entity);
                    }
                    if (otherProp.hasProperty(Components.Properties.Win))
                    {

                        if (entProp.hasProperty(Components.Properties.You))
                        {
                            Win = true;
                        }
                    }
                    if (otherProp.hasProperty(Components.Properties.Sink))
                    {
                        // sink, removing colliding tile and sink tile

                        m_entities.Remove(entity.Id);
                        m_entities.Remove(other.Id);
                    }
                    if (otherProp.hasProperty(Components.Properties.Defeat))
                    {
                        // defeat, removing player tile

                        if (entProp.hasProperty(Components.Properties.You))
                        {
                            m_entities.Remove(entity.Id);
                        }
                    }
                }
            }
        }
    }
}
