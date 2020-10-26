using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loki2D.Core.Utilities.MathHelper;

namespace TermProject1.AI.Actions
{
    public static class ActionDictionary
    {
        private static List<Action> Actions = new List<Action>()
        {
            // ranged
            new RangedAction("Bopity Boo", 3, 2, 2)
            {
                Uses = 99
            },
            new RangedAction("Magic Missiles", 6, 3, 15)
            {
                Uses = 3
            },
            new RangedAction("Magic Missile", 6, 10, 5)
            {
                Uses = 10
            },
            new RangedAction("Fireball", 3, 4, 1)
            {
                Uses = 15
            },

            //melee
            new MeleeAction("Starlight Punchout", 10),
            new MeleeAction("Mage Fists", 5)
            {
                Uses = 99
            },
            new MeleeAction("Void Touch", 3)
            {
                Uses = 10
            },
            new MeleeAction("Ogre Fists", 5)
            {
                Uses = 15
            },
        };

        public static Action GetAction(string name)
        {
            foreach (var action in Actions)
            {
                if (action.Name == name)
                    return action.Clone();
            }

            return new Action("NO DATA", 0);
        }

        public static List<Action> GetRandomActions(int count, int maxDamage = 100)
        {
            var list = new List<Action>();
            for (int i = 0; i < count; i++)
            {
                var action = GetAction(Actions[MathUtils.Random.Next(0, Actions.Count)].Name);

                if (action is DamageAction)
                {
                    if (((DamageAction) action).Damage > maxDamage)
                    {
                        ((DamageAction) action).Damage = maxDamage;
                    }
                }
                list.Add(action);
            }

            return list; 
        }
    }
}
