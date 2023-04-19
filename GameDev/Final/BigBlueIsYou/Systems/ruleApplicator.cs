using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace CS5410.Systems
{
    public class RuleApplicatorSystem : System
    {
        private Dictionary<string, Components.Objects> m_objectNames;
        private Dictionary<string, Components.Properties> m_propertyNames;

        private RulesSystem m_rules;
        private RenderParticleSystem m_particles;

        private List<Rule> m_youRules;
        private List<Rule> m_winRules;

        public RuleApplicatorSystem(RulesSystem rules, RenderParticleSystem particles)
            : base(
                    typeof(Components.Noun),
                    typeof(Components.Property)
                  )
        {
            m_objectNames = new Dictionary<string, Components.Objects>();
            m_objectNames.Add("bigblue", Components.Objects.BigBlue);
            m_objectNames.Add("wall", Components.Objects.Wall);
            m_objectNames.Add("rock", Components.Objects.Rock);
            m_objectNames.Add("flag", Components.Objects.Flag);
            m_objectNames.Add("floor", Components.Objects.Floor);
            m_objectNames.Add("grass", Components.Objects.Grass);
            m_objectNames.Add("water", Components.Objects.Water);
            m_objectNames.Add("lava", Components.Objects.Lava);
            m_objectNames.Add("hedge", Components.Objects.Hedge);

            m_propertyNames = new Dictionary<string, Components.Properties>();
            m_propertyNames.Add("you", Components.Properties.You);
            m_propertyNames.Add("win", Components.Properties.Win);
            m_propertyNames.Add("defeat", Components.Properties.Defeat);
            m_propertyNames.Add("push", Components.Properties.Push);
            m_propertyNames.Add("sink", Components.Properties.Sink);
            m_propertyNames.Add("stop", Components.Properties.Stop);

            m_rules = rules;
            m_particles = particles;

            m_youRules = new List<Rule>();
            m_winRules = new List<Rule>();
        }

        public override void update(GameTime gameTime)
        {
            /* after generating rules for this cycle, apply them */

            foreach (Entities.Entity entity in m_entities.Values)
            {
                var comp = entity.GetComponent<Components.Property>();

                comp.Clear();
                foreach (Rule rule in m_rules.Rules)
                {
                   /* check to make sure the subject is a noun */
                   if (!m_objectNames.ContainsKey(rule.Noun))
                   {
                       continue;
                   }

                   /* check to make sure that rule applies to entity */
                   if (entity.GetComponent<Components.Noun>().Object != m_objectNames[rule.Noun])
                   {
                       continue;
                   }



                   // apply rule
                   if (m_objectNames.ContainsKey(rule.Application))
                   {
                       // change noun to another noun

                       var noun = entity.GetComponent<Components.Noun>();
                       noun.Object = m_objectNames[rule.Application];
                   }
                   else
                   {
                       // apply new Property
                       if (m_propertyNames[rule.Application] == Components.Properties.You)
                       {
                           if (!m_youRules.Contains(rule) && m_youRules.Count != 0)
                           {
                               // particles for changing you
                               m_particles.changeIsYou(m_objectNames[rule.Noun]);
                           }
                       }
                       if (m_propertyNames[rule.Application] == Components.Properties.Win)
                       {
                           if (!m_winRules.Contains(rule))
                           {
                               // particles for changing win
                               m_particles.changeIsWin(m_objectNames[rule.Noun]);
                           }
                       }
                       comp.Add(m_propertyNames[rule.Application]);
                   }
                }
            }
        }
    }
}
