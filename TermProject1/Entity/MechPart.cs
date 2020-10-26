using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loki2D.Core.Component;
using Microsoft.Xna.Framework;

namespace TermProject1.Entity
{
    public class MechPart :Loki2D.Core.GameObject.Entity
    {
        public string Name { get; set; }
        public int MaxHP { get; set; }
        public string Action { get; set; }

        private TransformComponent _transformComponent;
        private RenderComponent _renderComponent; 
        public MechPart()
        {
            AddComponent(_transformComponent = new TransformComponent(this, Vector2.Zero));
            AddComponent(_renderComponent = new RenderComponent("")); 
            
            
        }

        public virtual void ExecuteAction()
        {

        }
    }
}
