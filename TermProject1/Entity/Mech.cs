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
using SharpDX.MediaFoundation;
using SharpDX.X3DAudio;
using TermProject1.World;
using TermProject1.World.Generators;
using Action = TermProject1.AI.Actions.Action;

namespace TermProject1.Entity
{
    public class Mech : Loki2D.Core.GameObject.Entity
    {
        public static int MechNumber = 0;
        public List<Action> Actions = new List<Action>();

        public bool Died = false;
        public int Speed { get; set; } = 2;
        public int Health { get; set; } = 20; 
        public TurnManager.TurnState TurnState { get; set; }

        public Point TilePosition { get; set; }
        private RenderComponent _renderComponent;
        private TransformComponent _transformComponent;
        public Mech()
        {
            AddComponent(_transformComponent = new TransformComponent(this, Vector2.Zero));
            AddComponent(_renderComponent = new RenderComponent());
            GetComponent<RenderComponent>().CanDraw = true;
            GetComponent<RenderComponent>().RenderLayer = 75; 

            TurnState = TurnManager.TurnState.Waiting;

            Name = MechNumber.ToString();
            MechNumber++;

            

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            //CanUpdate = ActiveTurn; 
        }

        public void TakeDamage(int damage)
        {
            var damageText = $"-{damage}";
            var particle = new Particle();
            particle.Dampening = .999f;
            particle.FadeRate = .99f;
            particle.Fade = 1;
            particle.Force = MathUtils.Random.Next(1, 3);
            particle.ForceAngle = -90;
            particle.Text = damageText;
            particle.GetComponent<TransformComponent>().Position = GetComponent<TransformComponent>().Position;

            ParticleManager.Instance.AddParticle(particle);


            Health -= damage;
            if (Health <= 0)
            {
                TurnManager.Instance.Remove(this);
                Died = true; 
                Destroy();
                
                Console.WriteLine("Destroyed Entity!");
            }
        }

        public void SetTilePosition(int x, int y)
        {
            TilePosition = new Point(x,y);

            var posX = (x - y) * (BattleMapGenerator.TileWidth / 2);
            var posY = (x + y) * (BattleMapGenerator.TileHeight / 2);

            //var positionX = (x * BattleMapGenerator.TileWidth); 
            //var positionY = (y * BattleMapGenerator.TileHeight);

            GetComponent<TransformComponent>().Position = new Vector2(posX, posY);
        }

        public void SetTilePosition(Point position)
        {
            SetTilePosition(position.X, position.Y);
        }

        public void MoveUp()
        {

            if (!GameManager.Instance.GetBattleMap().InBounds(new Point(TilePosition.X, TilePosition.Y - 1)))
                return;

            TilePosition =  new Point(TilePosition.X, TilePosition.Y - 1);
            SetTilePosition(TilePosition);
        }

        public bool CanUseAction(float range, Mech mech)
        {
            var dist = Vector2.Distance(TilePosition.ToVector2(), mech.TilePosition.ToVector2());
            return (dist <= range); 
        }

        public void MoveDown()
        {
            if (!GameManager.Instance.GetBattleMap().InBounds(new Point(TilePosition.X , TilePosition.Y + 1)))
                return;

            TilePosition = new Point(TilePosition.X, TilePosition.Y + 1);
            SetTilePosition(TilePosition);
        }
        public void MoveLeft()
        {
            if(!GameManager.Instance.GetBattleMap().InBounds(new Point(TilePosition.X - 1, TilePosition.Y)))
                return;

            TilePosition = new Point(TilePosition.X - 1, TilePosition.Y);
            SetTilePosition(TilePosition);
        }
        public void MoveRight()
        {
            if (!GameManager.Instance.GetBattleMap().InBounds(new Point(TilePosition.X + 1, TilePosition.Y)))
                return;

            TilePosition = new Point(TilePosition.X + 1, TilePosition.Y);
            SetTilePosition(TilePosition);
        }

        public void FinishTurn()
        {
            TurnState = TurnManager.TurnState.Completed;
        }

        public void MoveRandom()
        {
            var number = MathUtils.Random.Next(1, 4);

            switch (number)
            {
                case 1:
                {
                    MoveUp();
                    break;
                }
                case 2:
                {
                    MoveDown();
                    break;
                }
                case 3:
                {
                    MoveLeft();
                    break;
                }
                case 4:
                {
                    MoveRight();
                    break;
                }
            }
        }
    }
}
