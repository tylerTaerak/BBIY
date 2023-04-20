using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;

namespace CS5410.Systems
{
    public class RuleApplicatorSystem : System
    {
        private Dictionary<string, Components.Objects> m_objectNames;
        private Dictionary<string, Components.Properties> m_propertyNames;

        private RulesSystem m_rules;
        private RenderParticleSystem m_particles;

        private List<Components.Objects> m_you; // can be null when starting
        private List<Components.Objects> m_win; // can be null when starting

        private bool m_firstRound; // we don't want to emit particles on the first update

        /* defined in GameState.loadContent; play a sound when IS WIN changes */
        public SoundEffect ChangeWin
        {
            get;
            set;
        }

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

            m_you = new List<Components.Objects>();
            m_win = new List<Components.Objects>();

            m_firstRound = true;
        }

        public override void update(GameTime gameTime)
        {
            /* after generating rules for this cycle, apply them */

            List<Components.Objects> tempYou = new List<Components.Objects>();
            List<Components.Objects> tempWin = new List<Components.Objects>();

            bool playSound = false;

            foreach (Entities.Entity entity in m_entities.Values)
            {
                var comp = entity.GetComponent<Components.Property>();

               if (entity.GetComponent<Components.Noun>().Object == Components.Objects.Hedge)
               {
                   continue; // hedges always act as a border to the level
               }

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
                           if (!m_you.Contains(m_objectNames[rule.Noun]))
                           {
                               if (!m_firstRound)
                               {
                                   m_particles.changeIsYou(m_objectNames[rule.Noun]);
                               }
                           }

                           tempYou.Add(m_objectNames[rule.Noun]);
                       }
                       if (m_propertyNames[rule.Application] == Components.Properties.Win)
                       {
                           if (!m_win.Contains(m_objectNames[rule.Noun]))
                           {
                               if (!m_firstRound)
                               {
                                   m_particles.changeIsWin(m_objectNames[rule.Noun]);

                                   playSound = true;
                               }
                           }

                           tempWin.Add(m_objectNames[rule.Noun]);
                       }
                       comp.Add(m_propertyNames[rule.Application]);
                   }
                }
            }

            if (playSound)
            {
               ChangeWin.Play();
            }

            m_you.Clear();
            m_you.AddRange(tempYou);

            m_win.Clear();
            m_win.AddRange(tempWin);

            m_firstRound = false;

        }
    }
}
