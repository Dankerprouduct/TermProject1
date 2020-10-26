using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loki2D.Core.Component;
using Loki2D.Core.Utilities;
using Loki2D.Core.Utilities.MathHelper;
using Loki2D.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TermProject1.World
{
    public class Particle: Loki2D.Core.GameObject.Entity
    {
        public float Dampening { get; set; } = .99f;

        public Vector2 Velocity { get; set; }
        public float Fade { get; set; } = 1; 
        public float FadeRate { get; set; } = 1;
        public float Rotation { get; set; } = 0; 
        public List<Color> Colors = new List<Color>();
        public float ForceAngle { get; set; }
        public float Force { get; set; }
        public string Text { get; set; }
        public Color CurrentColor { get; set; }

        public float Scale { get; set; } = 1;
        public float ScaleRate { get; set; } = 1; 

        public Particle()
        {
            AddComponent(new TransformComponent(this, Vector2.Zero)); 
            AddComponent(new RenderComponent("particle"));
            GetComponent<RenderComponent>().Scale = 5;
            GetComponent<RenderComponent>().RenderLayer = 80;

            CanUpdate = true;

            GetComponent<RenderComponent>().OnDraw += (sender, args) =>
            {
                if (!string.IsNullOrEmpty(Text))
                {
                    args.SpriteBatch.DrawString(GUIManager.Instance.GetFont("MenuFont"), Text,
                        GetComponent<TransformComponent>().Position, Color.White * Fade, 0f, Vector2.Zero,
                        Vector2.One, SpriteEffects.None, 1f);
                }
            };
        }

        public void Enable()
        {
            CanUpdate = true;
            GetComponent<RenderComponent>().CanDraw = true;
        }

        public void Disable()
        {
            CanUpdate = false;
            GetComponent<RenderComponent>().CanDraw = false;
            Colors.Clear();
            Scale = 1;
            ScaleRate = 1; 
        }

        public void SetPosition(Vector2 position)
        {
            GetComponent<TransformComponent>().Position = position; 
        }

        public void AddForce(float angle, float force, float dampening = .85f, float rotation = 0f, float fade = 1f)
        {
            ForceAngle = angle; 
            var radians = MathHelper.ToRadians(angle);
            var xComponent = (float) Math.Cos(radians) * force; 
            var yComponent = (float) Math.Sin(radians) * force;

            Force = force; 
            Velocity = new Vector2(xComponent, yComponent);
            Dampening = dampening;

            Fade = 1;
            FadeRate = fade;
            Rotation = MathHelper.ToRadians(rotation);

            if (Colors.Count > 0)
            {
                CurrentColor = RandomHelper.RandomValue(MathUtils.Random, Colors);
            }
            else
            {
                CurrentColor = Color.White;
            }
            Text = "";
            Enable();
        }

        public override void Update(GameTime gameTime)
        {
            Velocity *= Dampening;
            Scale *= ScaleRate;
            Fade *= FadeRate;
            GetComponent<TransformComponent>().Position += Velocity;
            GetComponent<RenderComponent>().Rotation += Rotation;
            GetComponent<RenderComponent>().Scale = Scale;
            
            GetComponent<RenderComponent>().Color =  CurrentColor * Fade; 
            
            if (Velocity.Length() <= .1f)
            {
                Disable();
            }

            base.Update(gameTime);
        }
    }
}
