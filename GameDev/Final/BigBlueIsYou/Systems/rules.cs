using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;

namespace CS5410.Systems
{
    public class Rule
    {
        public Rule(string noun, string app)
        {
            Noun = noun;
            Application = app;
        }

        public string Noun
        {
            get;
            private set;
        }

        public string Application
        {
            get;
            private set;
        }
    }

    public class RulesSystem : System
    {
        private List<Rule> m_rules;

        public List<Rule> Rules
        {
            get { return m_rules; }
        }

        public RulesSystem()
            : base(
                    /* add required components here */
                    typeof(Components.Text),
                    typeof(Components.Position)
                  )
        {
            m_rules = new List<Rule>();

        }

        public override void update(GameTime gameTime)
        {
            /* update logic goes here */

            m_rules.Clear();

            foreach (Entities.Entity entity in m_entities.Values)
            {
                var text = entity.GetComponent<Components.Text>();
                if (text.Word == "is")
                {
                    /* check up down left right for other words */
                    var pos = entity.GetComponent<Components.Position>();
                    var neighbors = new Entities.Entity[4];
                    foreach (Entities.Entity neighbor in m_entities.Values)
                    {
                        var neighborPos = neighbor.GetComponent<Components.Position>();
                        if (neighborPos.CurrentPosition.Item1 == pos.CurrentPosition.Item1 - 1 &&
                                neighborPos.CurrentPosition.Item2 == pos.CurrentPosition.Item2)
                        {
                            neighbors[0] = neighbor;
                        }

                        if (neighborPos.CurrentPosition.Item1 == pos.CurrentPosition.Item1 &&
                                neighborPos.CurrentPosition.Item2 == pos.CurrentPosition.Item2 - 1)
                        {
                            neighbors[2] = neighbor;
                        }

                        if (neighborPos.CurrentPosition.Item1 == pos.CurrentPosition.Item1+1 &&
                                neighborPos.CurrentPosition.Item2 == pos.CurrentPosition.Item2)
                        {
                            neighbors[1] = neighbor;
                        }

                        if (neighborPos.CurrentPosition.Item1 == pos.CurrentPosition.Item1 &&
                                neighborPos.CurrentPosition.Item2 == pos.CurrentPosition.Item2+1)
                        {
                            neighbors[3] = neighbor;
                        }
                    }

                    if (neighbors[0] != null && neighbors[1] != null)
                    {
                        // rule from left to right
                        var comp1 = neighbors[0].GetComponent<Components.Text>();
                        var comp2 = neighbors[1].GetComponent<Components.Text>();

                        m_rules.Add(new Rule(comp1.Word, comp2.Word));
                    }
                    if (neighbors[2] != null && neighbors[3] != null)
                    {
                        // rule from top to bottom
                        var comp1 = neighbors[2].GetComponent<Components.Text>();
                        var comp2 = neighbors[3].GetComponent<Components.Text>();

                        m_rules.Add(new Rule(comp1.Word, comp2.Word));
                    }
                }
            }
        }
    }
}
