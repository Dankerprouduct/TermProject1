using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Direct3D9;
using TermProject1.Entity;

namespace TermProject1.AI.Actions
{
    public class Action
    {
        public event EventHandler CompletedAction;
        public string Name { get; set; }
        /// <summary>
        /// The range of this attack
        /// </summary>
        public int Range { get; set; }

        private int _uses;
        public int Uses
        {
            get => _uses;
            set
            {
                _uses = value;
                UsesLeft = _uses;
            }
        }

        public int UsesLeft { get; set; } 

        public Action(string name, int range)
        {
            Name = name;
            Range = range;
            UsesLeft = Uses;
        }

        /// <summary>
        /// The mech this action is being applied to
        /// </summary>
        /// <param name="mech"></param>
        public virtual void ExecuteAction(Mech parent, Mech mech)
        {
            Console.WriteLine($"Executing {Name} action on {mech}");
            CompletedAction?.Invoke(this, null);
            UsesLeft--;
            Player.Instance.PlayerUi.UpdateUI();
        }


        public virtual Action Clone()
        {
            return new Action(Name, Range);
        }
    }
}
