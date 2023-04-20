using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;

namespace CS5410.Systems
{
    public class RuleApplicatorSystem : System
    {
        private Dictionary<string, Components.Objects> m_objectNames;
        private Dictionary<string, Components.Properties> m_propertyNames;

        private RulesSystem m_rules;
        private RenderParticleSystem m_particles;

        private Components.Objects? m_you; // can be null when starting
        private Components.Objects? m_win; // can be null when starting

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

            m_you = null;
            m_win = null;
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
                           if (m_you == null)
                           {
                                m_you = m_objectNames[rule.Noun];
                           }
                           else if (m_objectNames[rule.Noun] != m_you.Value)
                           {
                               m_you = m_objectNames[rule.Noun];
                               m_particles.changeIsYou(m_you.Value);
                           }

                       }
                       if (m_propertyNames[rule.Application] == Components.Properties.Win)
                       {
                           if (m_win == null)
                           {
                               m_win = m_objectNames[rule.Noun];
                               m_particles.changeIsWin(m_win.Value);
                           }
                           if (m_objectNames[rule.Noun] != m_win.Value)
                           {
                               m_win = m_objectNames[rule.Noun];
                               m_particles.changeIsWin(m_win.Value);
                           }
                       }
                       comp.Add(m_propertyNames[rule.Application]);
                   }
                }
            }
        }
    }
}
