using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loki2D.Core.Component;
using Loki2D.Core.Scene;
using Loki2D.Core.Utilities.MathHelper;
using Microsoft.Xna.Framework;
using TermProject1.AI.Actions;
using TermProject1.Entity;
using TermProject1.World.Generators;

namespace TermProject1.World
{
    public class TurnManager
    {
        public static TurnManager Instance;
        public static int Round = 1; 
        public event EventHandler NewTurn; 
        public event EventHandler NewRound; 
        public enum TurnState
        {
            InProgress,
            Completed,
            Waiting
        }

        public Mech CurrentMech { get; set; }
        public List<Mech> MechQueue = new List<Mech>();
        public TurnManager()
        {
            Instance = this; 
        }

        public void AddMechList(List<Mech> mechs)
        {
            foreach (var mech in mechs)
            {
                MechQueue.Add(mech);
            }
        }

        public void CreateTurnOrder()
        {
            MechQueue = MechQueue.OrderBy(i => i.Speed).Reverse().ToList();
            NewRound?.Invoke(this, null);
        }

        public void Reset()
        {
            CreateTurnOrder();
            foreach (var mech in MechQueue)
            {
                mech.TurnState = TurnState.Waiting;
            }

            CurrentMech = null;
        }

        public void Remove(Mech mech)
        {
            MechQueue.Remove(mech);
            if (CurrentMech == mech)
                CurrentMech = null;
        }

        public void Update(GameTime gameTime)
        {

            

            foreach (var mech in MechQueue)
            {
                if (mech == null)
                {
                    MechQueue.Remove(mech); 
                }
            }

            if (CurrentMech == null)
            {
                CurrentMech = MechQueue[0];
                CurrentMech.TurnState = TurnState.InProgress;
                NewTurn?.Invoke(this, null);
            }

            //Console.WriteLine($"{CurrentMech} - State: {CurrentMech.TurnState}  Render: {CurrentMech.GetComponent<RenderComponent>()}");

            if (CurrentMech.TurnState == TurnState.Completed)
            {


                NewTurn?.Invoke(this, null);
                var nextMech = MechQueue.FirstOrDefault(i => i.TurnState == TurnState.Waiting);
                
                if (nextMech != null)
                {
                    CurrentMech = nextMech;
                    CurrentMech.TurnState = TurnState.InProgress;

                }
                else
                {
                    bool reset = true;
                    foreach (var mech in MechQueue)
                    {
                        if (mech.TurnState != TurnState.Completed)
                        {
                            reset = false;
                        }
                    }

                    if (reset)
                    {
                        Reset();
                    }

                    if (MechQueue.Count <= 1)
                    {
                        if (MechQueue[0] is Player)
                        {
                            NextRound();
                        }
                    }
                }
            }

        }

        public void NextRound()
        {
            var width = MathUtils.Random.Next(5, 10);
            var height = MathUtils.Random.Next(5, 10);
            var map = BattleMapGenerator.GenerateBattleMap(width, height);
            map.PlaceTiles();

            Player.Instance.SetTilePosition(width - 1, height / 2);

            Player.Instance.Actions.AddRange(ActionDictionary.GetRandomActions(MathUtils.Random.Next(1, 3)));
            Player.Instance.Health += MathUtils.Random.Next(3 + Round -1 , 8 + (2 * (Round - 1))); 
            Player.Instance.RegisterActions();
            GameManager.Instance.SetBattleMap(map);

            var mechCount = MathUtils.Random.Next(2, 5);

            Mech.MechNumber = 1;
            Round++;
            for (int i = 0; i < mechCount; i++)
            {
                var randomY = MathUtils.Random.Next(MathUtils.Random.Next(0,2), height - 1);
                var mech = new EnemyMech();
                mech.SetTilePosition(1, randomY);

                SceneManagement.Instance.CurrentScene.AddEntity(mech);
                MechQueue.Add(mech);
            }
        }
    }
}
