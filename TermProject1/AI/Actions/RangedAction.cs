using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loki2D.Core.Component;
using Loki2D.Core.Scene;
using Loki2D.Core.Utilities.MathHelper;
using Microsoft.Xna.Framework;
using TermProject1.Entity;
using TermProject1.World;

namespace TermProject1.AI.Actions
{
    public class RangedAction: DamageAction
    {
        public int ProjectileCount { get; set; }


        public RangedAction(string name, int range, int damage, int projectileCount) : base(name, range, damage)
        {
            ProjectileCount = projectileCount; 
        }

        public override void ExecuteAction(Mech parent, Mech mech)
        {

            int spawned = 0; 
            for (int i = 0; i < ProjectileCount; i++)
            {
                var projectile = new Projectile(
                    parent.GetComponent<TransformComponent>().Position, 
                    mech.GetComponent<TransformComponent>().Position,
                    "fireball");
                
                projectile.ReachedTarget += (sender, args) =>
                {
                    spawned++; 
                    mech.TakeDamage(Damage);

                    if (spawned == ProjectileCount)
                    {
                        base.ExecuteAction(parent, mech);
                    }
                };

                SceneManagement.Instance.CurrentScene.AddEntity(projectile); 
            }

            base.ExecuteAction(parent, mech);

        }

        public override Action Clone()
        {
            return new RangedAction(Name, Range, Damage, ProjectileCount)
            {
                Uses =  Uses
            };
        }
    }

    public class Projectile : Loki2D.Core.GameObject.Entity
    {
        public event EventHandler ReachedTarget;
        private Vector2 _target;
        private float _speed;
        private Vector2 _velocity; 
        private float _angle; 
        public Projectile(Vector2 position, Vector2 target, string textureKey)
        {
            Tag = "Projectile"; 
            _target = target;

            AddComponent(new TransformComponent(this, position));
            AddComponent(new RenderComponent(textureKey));
            GetComponent<RenderComponent>().CanDraw = false;

            GetComponent<RenderComponent>().RenderLayer = 80;

            var delta = target - position;
            delta.Normalize();

            _angle = (float) Math.Atan2(delta.Y, delta.X);
            GetComponent<RenderComponent>().Rotation = MathHelper.ToDegrees(_angle);

            _speed = (float)MathUtils.Random.Next(1, 3) / 3;

            var xComponent = (float) Math.Cos(_angle) * _speed;
            var yComponent = (float) Math.Sin(_angle) * _speed;
            

            _velocity = new Vector2(xComponent, yComponent);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _velocity *= 1.05f;
            GetComponent<TransformComponent>().Position += _velocity;

            var distance = Vector2.Distance(GetComponent<TransformComponent>().Position, _target);
            
            var particle = new Particle();
            var angle = (float)MathUtils.Random.Next(-30, 30);
            angle = angle - 180 - MathHelper.ToDegrees(_angle);

            particle.Colors = new List<Color>()
            {
                Color.White,
                Color.Red,
                Color.Orange,
                Color.Yellow
            };
            var force = MathUtils.Random.Next(2, 10);
            particle.ForceAngle = angle;
            particle.Scale = 10;
            particle.ScaleRate = .95f;
            particle.Force = force;
            particle.FadeRate = (float)MathUtils.Random.Next(80, 100) / 100;
            particle.Dampening = (float)MathUtils.Random.Next(1, 3) / 3;
            particle.SetPosition(GetComponent<TransformComponent>().Position);

            ParticleManager.Instance.AddParticle(particle);

            if (distance <= 10)
            {
                ReachedTarget?.Invoke(this, null);
                Destroy();
            }
        }
    }
}
