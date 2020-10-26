using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loki2D.Core.Component;
using Loki2D.Core.Scene;
using Loki2D.Core.Utilities;
using Loki2D.Core.Utilities.MathHelper;
using Loki2D.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TermProject1.AI.Actions;
using TermProject1.World;
using TermProject1.World.Generators;
using Action = TermProject1.AI.Actions.Action;

namespace TermProject1.Entity
{
    public class Player: Mech
    {
        public static Player Instance { get; set; }
        private Point _originPosition;
        public PlayerUI PlayerUi; 
        private List<Loki2D.Core.GameObject.Entity> Enemies = new List<Loki2D.Core.GameObject.Entity>();
        private bool _executingAction;
        private int _actionID;
        private Animation _animation; 

        
        public Player(Vector2 position) : base()
        {
            Instance = this; 

            GetComponent<RenderComponent>().TextureName = "wizzard_f_idle_anim_f0";
            
            GetComponent<TransformComponent>().Position = position;

            GetComponent<RenderComponent>().UsesCustomOrigin = false;

            var center = TextureManager.CenterOfImage(TextureManager.Instance.GetTexture(GetComponent<RenderComponent>().TextureName));
            GetComponent<RenderComponent>().Origin = new Vector2(center.X - 0, center.Y + 15);

            _animation = new Animation(GetComponent<RenderComponent>(), new List<string>()
            {
                "wizzard_f_idle_anim_f0",
                "wizzard_f_idle_anim_f1",
                "wizzard_f_idle_anim_f2",
                "wizzard_f_idle_anim_f3",
            }, 100);

            PlayerUi = new PlayerUI();

            Speed = 4;
            TurnManager.Instance.NewRound += (sender, args) =>
            {
                _originPosition = TilePosition;
                GameManager.Instance.GetBattleMap().ClearOverlay();

                if (TurnState == TurnManager.TurnState.InProgress)
                {
                    GameManager.Instance.GetBattleMap().SetOverlay(Color.LimeGreen, _originPosition, Speed);
                }
            };

            TurnManager.Instance.NewTurn += (sender, args) =>
            {
                GameManager.Instance.GetBattleMap().ClearOverlay();

                if (TurnState == TurnManager.TurnState.InProgress)
                {
                    GameManager.Instance.GetBattleMap().SetOverlay(Color.LimeGreen, _originPosition, Speed);
                }
            };



            Actions.Add(ActionDictionary.GetAction("Bopity Boo"));
            Actions.Add(ActionDictionary.GetAction("Magic Missile"));
            Actions.Add(ActionDictionary.GetAction("Magic Missiles"));

            RegisterActions();
        }

        public void RegisterActions()
        {
            foreach (var action in Actions)
            {
                action.CompletedAction += (sender, args) =>
                {
                    FinishTurn();
                };
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _animation.Update(gameTime);
            if (_executingAction)
            {
                ExecuteAction();
            }

            CheckActions();

            PlayerUi.Update(gameTime);

            if (TurnState == TurnManager.TurnState.InProgress)
            {
                if (PlayerUi.CurrentDisplay == PlayerUI.Display.None)
                {
                    EnemyMech.ShowID = false;
                    _executingAction = false;
                    CheckInput();
                }
            }


            

            base.Update(gameTime);
        }

        public void CheckActions()
        {
            if (PlayerUi.CurrentDisplay == PlayerUI.Display.Actions && !_executingAction)
            {

                if (InputManager.KeyPressed(Keys.D1))
                {
                    if (Actions.Count >= 1)
                    {
                        Enemies = SceneManagement.Instance.CurrentScene.GetEntitiesByTag("EnemyMech");


                        EnemyMech.ShowID = true;
                        _executingAction = true;
                        _actionID = 0; 
                    }
                }
                if (InputManager.KeyPressed(Keys.D2))
                {
                    if (Actions.Count >= 2)
                    {
                        Enemies = SceneManagement.Instance.CurrentScene.GetEntitiesByTag("EnemyMech");

                        EnemyMech.ShowID = true;
                        _executingAction = true;
                        _actionID = 1;
                    }
                }
                if (InputManager.KeyPressed(Keys.D3))
                {
                    if (Actions.Count >= 3)
                    {
                        Enemies = SceneManagement.Instance.CurrentScene.GetEntitiesByTag("EnemyMech");

                        EnemyMech.ShowID = true;
                        _executingAction = true;
                        _actionID = 2;

                    }
                }
                if (InputManager.KeyPressed(Keys.D4))
                {
                    if (Actions.Count >= 4)
                    {
                        Enemies = SceneManagement.Instance.CurrentScene.GetEntitiesByTag("EnemyMech");

                        EnemyMech.ShowID = true;
                        _executingAction = true;
                        _actionID = 3;
                    }
                }
                if (InputManager.KeyPressed(Keys.D5))
                {
                    Console.WriteLine($"Action count: {Actions.Count}");
                    if (Actions.Count >= 5)
                    {
                        Enemies = SceneManagement.Instance.CurrentScene.GetEntitiesByTag("EnemyMech");

                        EnemyMech.ShowID = true;
                        _executingAction = true;
                        _actionID = 4;
                    }
                }
                if (InputManager.KeyPressed(Keys.D6))
                {
                    if (Actions.Count >= 6)
                    {
                        Enemies = SceneManagement.Instance.CurrentScene.GetEntitiesByTag("EnemyMech");

                        EnemyMech.ShowID = true;
                        _executingAction = true;
                        _actionID = 5;
                    }
                }
                if (InputManager.KeyPressed(Keys.D7))
                {
                    if (Actions.Count >= 7)
                    {
                        Enemies = SceneManagement.Instance.CurrentScene.GetEntitiesByTag("EnemyMech");

                        EnemyMech.ShowID = true;
                        _executingAction = true;
                        _actionID = 6;
                    }
                }
                if (InputManager.KeyPressed(Keys.D8))
                {
                    if (Actions.Count >= 8)
                    {
                        Enemies = SceneManagement.Instance.CurrentScene.GetEntitiesByTag("EnemyMech");

                        EnemyMech.ShowID = true;
                        _executingAction = true;
                        _actionID = 7; 
                    }
                }
                if (InputManager.KeyPressed(Keys.D9))
                {
                    if (Actions.Count >= 9)
                    {
                        Enemies = SceneManagement.Instance.CurrentScene.GetEntitiesByTag("EnemyMech");

                        EnemyMech.ShowID = true;
                        _executingAction = true;
                        _actionID = 8;
                    }
                }
                if (InputManager.KeyPressed(Keys.D0))
                {
                    if (Actions.Count >= 10)
                    {
                        Enemies = SceneManagement.Instance.CurrentScene.GetEntitiesByTag("EnemyMech");

                        EnemyMech.ShowID = true;
                        _executingAction = true;
                        _actionID = 9;
                    }
                }
            }

            
        }

        public void ExecuteAction()
        {
            GameManager.Instance.GetBattleMap().SetOverlay(new Color(255, 86, 112), TilePosition, Actions[_actionID].Range);

            if (InputManager.KeyPressed(Keys.D1))
            {
                var mech = Enemies.FirstOrDefault(i => i.Name == "1") as EnemyMech;
                if (mech == null)
                    return;

                if (!CanUseAction(Actions[_actionID].Range, mech))
                {
                    return;
                }
                Actions[_actionID].ExecuteAction(this, mech);
                PlayerUi.CurrentDisplay = PlayerUI.Display.None;

                EnemyMech.ShowID = false;
                _executingAction = false;

                
            }
            if (InputManager.KeyPressed(Keys.D2))
            {
                var mech = Enemies.FirstOrDefault(i => i.Name == "2") as EnemyMech;
                if (mech == null)
                    return;

                if (!CanUseAction(Actions[_actionID].Range, mech))
                {
                    return;
                }
                Actions[_actionID].ExecuteAction(this, mech);
                PlayerUi.CurrentDisplay = PlayerUI.Display.None;

                EnemyMech.ShowID = false;
                _executingAction = false;

            }
            if (InputManager.KeyPressed(Keys.D3))
            {
                var mech = Enemies.FirstOrDefault(i => i.Name == "3") as EnemyMech;
                if (mech == null)
                    return;

                if (!CanUseAction(Actions[_actionID].Range, mech))
                {
                    return;
                }
                Actions[_actionID].ExecuteAction(this, mech);
                PlayerUi.CurrentDisplay = PlayerUI.Display.None;

                EnemyMech.ShowID = false;
                _executingAction = false;

            }
            if (InputManager.KeyPressed(Keys.D4))
            {
                var mech = Enemies.FirstOrDefault(i => i.Name == "4") as EnemyMech;
                
                if(mech == null)
                    return;

                if (!CanUseAction(Actions[_actionID].Range, mech))
                {
                    return;
                }
                Actions[_actionID].ExecuteAction(this, mech);
                PlayerUi.CurrentDisplay = PlayerUI.Display.None;

                EnemyMech.ShowID = false;
                _executingAction = false;

            }
            if (InputManager.KeyPressed(Keys.D5))
            {
                var mech = Enemies.FirstOrDefault(i => i.Name == "5") as EnemyMech;

                if (mech == null)
                    return;
                if (!CanUseAction(Actions[_actionID].Range, mech))
                {
                    return;
                }
                Actions[_actionID].ExecuteAction(this, mech);
                PlayerUi.CurrentDisplay = PlayerUI.Display.None;

                EnemyMech.ShowID = false;
                _executingAction = false;

            }
            if (InputManager.KeyPressed(Keys.D6))
            {
                var mech = Enemies.FirstOrDefault(i => i.Name == "6") as EnemyMech;
                if (mech == null)
                    return;

                if (!CanUseAction(Actions[_actionID].Range, mech))
                {
                    return;
                }
                Actions[_actionID].ExecuteAction(this, mech);
                PlayerUi.CurrentDisplay = PlayerUI.Display.None;

                EnemyMech.ShowID = false;
                _executingAction = false;

            }
            if (InputManager.KeyPressed(Keys.D7))
            {
                var mech = Enemies.FirstOrDefault(i => i.Name == "7") as EnemyMech;
                if (mech == null)
                    return;

                if (!CanUseAction(Actions[_actionID].Range, mech))
                {
                    return;
                }
                Actions[_actionID].ExecuteAction(this, mech);
                PlayerUi.CurrentDisplay = PlayerUI.Display.None;

                EnemyMech.ShowID = false;
                _executingAction = false;

            }
            if (InputManager.KeyPressed(Keys.D8))
            {
                var mech = Enemies.FirstOrDefault(i => i.Name == "8") as EnemyMech;
                if (mech == null)
                    return;

                if (!CanUseAction(Actions[_actionID].Range, mech))
                {
                    return;
                }
                Actions[_actionID].ExecuteAction(this, mech);
                PlayerUi.CurrentDisplay = PlayerUI.Display.None;

                EnemyMech.ShowID = false;
                _executingAction = false;

            }
            if (InputManager.KeyPressed(Keys.D9))
            {
                var mech = Enemies.FirstOrDefault(i => i.Name == "9") as EnemyMech;
                if (mech == null)
                    return;

                if (!CanUseAction(Actions[_actionID].Range, mech))
                {
                    return;
                }
                Actions[_actionID].ExecuteAction(this, mech);
                PlayerUi.CurrentDisplay = PlayerUI.Display.None;

                EnemyMech.ShowID = false;
                _executingAction = false;

            }
            if (InputManager.KeyPressed(Keys.D0))
            {
                var mech = Enemies.FirstOrDefault(i => i.Name == "10") as EnemyMech;
                if (mech == null)
                    return;

                if (!CanUseAction(Actions[_actionID].Range, mech))
                {
                    return;
                }
                Actions[_actionID].ExecuteAction(this, mech);
                PlayerUi.CurrentDisplay = PlayerUI.Display.None;

                EnemyMech.ShowID = false;
                _executingAction = false;

            }

            for (var index = 0; index < Actions.Count; index++)
            {
                var action = Actions[index];
                if (action.UsesLeft <= 0)
                {
                    Actions.RemoveAt(index);
                    PlayerUi.UpdateUI();
                }
            }
        }

        

        private void CheckInput()
        {
            var distance = Vector2.Distance(_originPosition.ToVector2(), TilePosition.ToVector2());
            
            if (InputManager.KeyPressed(Keys.W))
            {
                GameManager.Instance.GetBattleMap().SetOverlay(Color.White, TilePosition, Actions[_actionID].Range);
                GameManager.Instance.GetBattleMap().SetOverlay(Color.LimeGreen, _originPosition, Speed);
                var dist = Vector2.Distance(_originPosition.ToVector2(), new Point(TilePosition.X, TilePosition.Y - 1).ToVector2());
                if(dist <= Speed)
                    MoveUp();
            }

            if (InputManager.KeyPressed(Keys.S))
            {
                GameManager.Instance.GetBattleMap().SetOverlay(Color.White, TilePosition, Actions[_actionID].Range);
                GameManager.Instance.GetBattleMap().SetOverlay(Color.LimeGreen, _originPosition, Speed);
                var dist = Vector2.Distance(_originPosition.ToVector2(), new Point(TilePosition.X, TilePosition.Y + 1).ToVector2());
                
                if (dist <= Speed)
                    MoveDown();
            }

            if (InputManager.KeyPressed(Keys.A))
            {
                GameManager.Instance.GetBattleMap().SetOverlay(Color.White, TilePosition, Actions[_actionID].Range);
                GameManager.Instance.GetBattleMap().SetOverlay(Color.LimeGreen, _originPosition, Speed);
                var dist = Vector2.Distance(_originPosition.ToVector2(), new Point(TilePosition.X - 1, TilePosition.Y).ToVector2());

                if (dist <= Speed)
                    MoveLeft();
            }

            if (InputManager.KeyPressed(Keys.D))
            {
                GameManager.Instance.GetBattleMap().SetOverlay(Color.White, TilePosition, Actions[_actionID].Range);
                GameManager.Instance.GetBattleMap().SetOverlay(Color.LimeGreen, _originPosition, Speed);
                var dist = Vector2.Distance(_originPosition.ToVector2(), new Point(TilePosition.X + 1, TilePosition.Y).ToVector2());

                if (dist <= Speed)
                    MoveRight();
            }

            if (InputManager.KeyPressed(Keys.Space))
            {
                GameManager.Instance.GetBattleMap().SetOverlay(Color.White, TilePosition, Actions[_actionID].Range);
                FinishTurn();
            }
        }

        public void ShowRange()
        {
            GameManager.Instance.GetBattleMap().SetOverlay(Color.LimeGreen, _originPosition, Speed);
        }
    }
}
