using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loki2D.Core.Scene;
using Loki2D.Core.Utilities;
using Loki2D.Core.Utilities.MathHelper;
using Loki2D.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TermProject1.Entity;
using TermProject1.World.Generators;

namespace TermProject1.World
{
    public class GameManager
    {
        public static GameManager Instance;
        private BattleMapGenerator.BattleMap _currentBattleMap;

        public static Dictionary<int, string> TextureDictionary = new Dictionary<int, string>();

        public GameManager()
        {
            Instance = this;
            TextureLoader.Instance = new TextureLoader();
            TurnManager.Instance = new TurnManager();
            ParticleManager.Instance = new ParticleManager(1000);

            _currentBattleMap = BattleMapGenerator.GenerateBattleMap(7, 7);
            _currentBattleMap.PlaceTiles();

            var width = ((float)_currentBattleMap.Width) / 2; 
            var height =((float)_currentBattleMap.Height * 25) / 2;

            var player = new Player(Vector2.Zero);
            player.SetTilePosition(6, 3);

            var mechList = new List<Mech>();
            mechList.Add(player);
            for (int i = 0; i < 2; i++)
            {
                var randomY = MathUtils.Random.Next(0, 4); 
                var mech = new EnemyMech();
                mech.SetTilePosition(1, randomY);

                SceneManagement.Instance.CurrentScene.AddEntity(mech);
                mechList.Add(mech);
            }

            TurnManager.Instance.AddMechList(mechList);
            TurnManager.Instance.CreateTurnOrder();

            SceneManagement.Instance.CurrentScene.AddEntity(player);
            Camera.position = new Vector2(width, height);
            Camera.Instance.SetZoom(2);


        }



        public BattleMapGenerator.BattleMap GetBattleMap()
        {
            return _currentBattleMap; 
        }

        public void SetBattleMap(BattleMapGenerator.BattleMap map)
        {
            _currentBattleMap?.Destroy();
            _currentBattleMap = map; 
        }

        public void Update(GameTime gameTime)
        {
            TurnManager.Instance.Update(gameTime);
            ParticleManager.Instance.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
