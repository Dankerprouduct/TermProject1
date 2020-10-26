using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Loki2D.Core.Component;
using Loki2D.Core.Scene;
using Loki2D.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TermProject1.World
{
    public class ParticleManager: SystemManager<ParticleManager>
    {
        private Particle[] _pool;
        public ParticleManager(int particlePool)
        {
            InitializePool(particlePool);
        }

        public void InitializePool(int poolSize)
        {
            _pool = new Particle[poolSize];
            for (int i = 0; i < _pool.Length; i++)
            {
                _pool[i] = new Particle();
                _pool[i].Disable();

                SceneManagement.Instance.CurrentScene.AddEntity(_pool[i]); 
            }
        }

        public void AddParticle(Vector2 position, float angle, float force, float dampening = .95f, float rotation = 0f, float fade = 1f)
        {

            for (int i = 0; i < _pool.Length; i++)
            {
                if (_pool[i].CanUpdate == false)
                {
                    _pool[i].SetPosition(position);
                    _pool[i].AddForce(angle, force, dampening, rotation, fade);
                    _pool[i].Enable();
                    return;
                }
            }
        }

        public void AddParticle(Particle particle)
        {
            for (int i = 0; i < _pool.Length; i++)
            {
                if (_pool[i].CanUpdate == false)
                {
                    _pool[i].Colors = particle.Colors;
                    _pool[i].Scale = particle.Scale;
                    _pool[i].ScaleRate = particle.ScaleRate; 
                    _pool[i].AddForce(particle.ForceAngle, particle.Force, particle.Dampening, particle.Rotation, particle.FadeRate);
                    _pool[i].SetPosition(particle.GetComponent<TransformComponent>().Position);
                    
                    _pool[i].Text = particle.Text; 

                    _pool[i].Enable();
                    return;
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            int poolCount = 0;
            for (int i = 0; i < _pool.Length; i++)
            {
                if (_pool[i].CanUpdate)
                {
                    poolCount++;
                }
            }

            //Console.WriteLine(poolCount);
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            
        }

        public override void OnDestroy()
        {
            
        }
    }
}
