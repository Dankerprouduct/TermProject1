using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loki2D.Core.Component;
using Loki2D.Core.Scene;
using Loki2D.Core.Utilities;
using Loki2D.Core.Utilities.MathHelper;
using Loki2D.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.X3DAudio;
using TermProject1.AI;
using TermProject1.AI.Actions;
using TermProject1.World;
using Action = TermProject1.AI.Actions.Action;

namespace TermProject1.Entity
{
    public class EnemyMech: Mech
    {
        public static bool ShowID { get; set; }
        Timer TurnTimer = new Timer(250);

        public List<Point> CurrentPath = new List<Point>();
        private int _currentMove = 0;

        private Point _originPosition;
        public Action DecidedAction { get; set; }

        private Animation _animation;
        private bool executedAction = false;


        private List<List<string>> Animations = new List<List<string>>()
        {

            new List<string>()
            {
                "ogre_idle_anim_f0",
                "ogre_idle_anim_f1",
                "ogre_idle_anim_f2",
                "ogre_idle_anim_f3",
            },
            new List<string>()
            {
                "orc_shaman_idle_anim_f0",
                "orc_shaman_idle_anim_f1",
                "orc_shaman_idle_anim_f2",
                "orc_shaman_idle_anim_f3",
            },
            new List<string>()
            {
                "big_demon_idle_anim_f0",
                "big_demon_idle_anim_f1",
                "big_demon_idle_anim_f2",
                "big_demon_idle_anim_f3",
            }
        };
        public EnemyMech()
        {
            Tag = "EnemyMech";

            Speed = MathUtils.Random.Next(2, 7);
            Health = MathUtils.Random.Next(12, 22);

            GetComponent<RenderComponent>().TextureName = "big_demon_idle_anim_f0";
            GetComponent<TransformComponent>().Position = Vector2.Zero;

            GetComponent<RenderComponent>().UsesCustomOrigin = false;
            //GetComponent<RenderComponent>().Color = Color.Red;


            var center = TextureManager.CenterOfImage(TextureManager.Instance.GetTexture(GetComponent<RenderComponent>().TextureName));
            GetComponent<RenderComponent>().Origin = new Vector2(center.X, TextureManager.Instance.GetTexture(GetComponent<RenderComponent>().TextureName).Height );

            _animation = new Animation(GetComponent<RenderComponent>(), MathUtils.Random.RandomValue(Animations), 100);

            TurnManager.Instance.NewTurn += (sender, args) =>
            {

                var decidedAction = MathUtils.Random.RandomValue(Actions);
                DecidedAction = decidedAction;
                DecidedAction.CompletedAction += (o, eventArgs) =>
                {
                    FinishTurn();
                    executedAction = false;
                };

                if(!executedAction)
                    CurrentPath = GetPath(FindPosition(DecidedAction.Range));
            };
            TurnManager.Instance.NewRound += (sender, args) =>
            {
                _originPosition = TilePosition;

            };

            GetComponent<RenderComponent>().OnDraw += (sender, args) =>
            {
                if (ShowID)
                {
                    args.SpriteBatch.DrawString(GUIManager.Instance.GetFont("MenuFont"),
                        Name, GetComponent<TransformComponent>().Position, Color.White, 0f, Vector2.Zero, 1f,
                        SpriteEffects.None,
                        1f);
                    
                }
            };

            Actions = ActionDictionary.GetRandomActions(MathUtils.Random.Next(1,4), MathUtils.Random.Next(2, 3 + (2 * (TurnManager.Round - 1)))); 
        }

        public Point FindPosition(int range)
        {
            var currentMap = GameManager.Instance.GetBattleMap();
            var playerPosition = Player.Instance.TilePosition;

            Point target = TilePosition;
            
            // check x axis

            var xTarget = playerPosition.X - range;
            for (int x = xTarget; x < playerPosition.X; x++)
            {
                target = new Point(xTarget, playerPosition.Y);
                if (currentMap.InBounds(target))
                {
                    return target;
                }
            }

            xTarget = playerPosition.X + range; 
            // x max
            for (int x = xTarget; x > playerPosition.X; x--)
            {
                target = new Point(xTarget, playerPosition.Y);
                if (currentMap.InBounds(target))
                {
                    return target;
                }
            }

            return target;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            _animation.Update(gameTime);

            GUIManager.Instance.Update(gameTime);
            if (TurnState == TurnManager.TurnState.InProgress && !executedAction)
            {

                if (TurnTimer.Update(gameTime.ElapsedGameTime.Milliseconds))
                {
                    if (CurrentPath.Any())
                    {
                        var position = CurrentPath[0];
                        
                        if (position.X == TilePosition.X + 1)
                        {
                            MoveRight();
                            _currentMove++; 
                        }
                        if (position.X == TilePosition.X - 1)
                        {
                            MoveLeft();
                            _currentMove++; 
                        }
                        if (position.Y == TilePosition.Y  - 1)
                        {
                            MoveUp();
                            _currentMove++;
                        }
                        if (position.Y == TilePosition.Y + 1)
                        {
                            MoveDown();
                            _currentMove++;
                        }

                        var dist = Vector2.Distance(_originPosition.ToVector2(), new Point(TilePosition.X, TilePosition.Y).ToVector2());
                        if (dist >= Speed)
                        {
                            //_currentMove = 0;
                            FinishTurn();
                        }

                        CurrentPath.RemoveAt(0);
                    }
                    else
                    {
                        // completed path
                        if (CanUseAction(DecidedAction.Range, Player.Instance))
                        {
                            DecidedAction.ExecuteAction(this, Player.Instance);
                            executedAction = true; 
                        }

                        _currentMove = 0;
                        
                    }
                }
            }

            if (Died)
            {
                TurnManager.Instance.Remove(this);
                Destroy();
                TurnState = TurnManager.TurnState.Completed;
            }

            // check for existing particles
            if (executedAction)
            {
                var particles = SceneManagement.Instance.CurrentScene.GetEntitiesByTag("Projectile");
                if (particles.Count <= 0)
                {
                    executedAction = false;
                    FinishTurn();
                }
            }
        }

        public List<Point> GetPath(Point target)
        {
            var map = GameManager.Instance.GetBattleMap();
            var grid = new SquareGrid(map.Width + 1, map.Height + 1);

            var otherMechs = SceneManagement.Instance.CurrentScene.GetEntitiesByTag(Tag);

            grid.walls.Add(Player.Instance.TilePosition);
            foreach (var mech in otherMechs)
            {
                if (mech != this)
                {
                    grid.walls.Add(((Mech) mech).TilePosition); 
                }
            }

            var astar = new Pathfinding(grid, TilePosition, target);

            return astar.Path;
        }
    }
}
