using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TermProject1.Entity;

namespace TermProject1.AI.Actions
{
    public class DamageAction: Action
    {
        public int Damage { get; set; }
        public DamageAction(string name, int range, int damage) : base(name, range)
        {
            Damage = damage; 
        }

        public override void ExecuteAction(Mech parent, Mech mech)
        {
            base.ExecuteAction(parent, mech);
        }
    }
}
