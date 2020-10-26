using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TermProject1.Entity;

namespace TermProject1.AI.Actions
{
    public class MeleeAction: DamageAction
    {
        public MeleeAction(string name, int damage) : base(name, 1, damage)
        {

        }

        public override void ExecuteAction(Mech parent, Mech mech)
        {
            mech?.TakeDamage(Damage);

            base.ExecuteAction(parent, mech);
        }

        public override Action Clone()
        {
            return new MeleeAction(Name, Damage)
            {
                Uses =  Uses
            };
        }
    }
}
