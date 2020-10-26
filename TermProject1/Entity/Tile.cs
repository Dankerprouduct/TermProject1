using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loki2D.Core.Component;
using Loki2D.Systems;
using Microsoft.Xna.Framework;

namespace TermProject1.Entity
{
    public class Tile: Loki2D.Core.GameObject.Entity
    {
        public TransformComponent TransformComponent;
        public RenderComponent RenderComponent;

        public Tile(string textureName, Vector2 position)
        {
            TransformComponent = new TransformComponent(this, position);
            RenderComponent = new RenderComponent(textureName);
            RenderComponent.RenderLayer = 0;
            AddComponent(TransformComponent);
            AddComponent(RenderComponent);


            var center = TextureManager.CenterOfImage(TextureManager.Instance.GetTexture(GetComponent<RenderComponent>().TextureName));
            GetComponent<RenderComponent>().Origin = new Vector2(center.X, center.Y - 10);
        }

        
    }
}
