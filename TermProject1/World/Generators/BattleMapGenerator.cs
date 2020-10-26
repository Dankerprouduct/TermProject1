using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loki2D.Core.Component;
using Loki2D.Core.Scene;
using Loki2D.Core.Utilities.MathHelper;
using Loki2D.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TermProject1.Entity;

namespace TermProject1.World.Generators
{
    /// <summary>
    /// Generator for Battle Maps
    /// </summary>
    public static class BattleMapGenerator
    {

        /// <summary>
        /// Data structure for Battle Maps
        /// </summary>
        public class BattleMap
        {
            /// <summary>
            /// Width of Map
            /// </summary>
            public  int Width { get; set; }
            
            /// <summary>
            /// Height of Map 
            /// </summary>
            public int Height { get; set; }

            /// <summary>
            /// 2D array of tile values
            /// </summary>
            public int[,] TileIDs { get; set; }
            public Tile[,] Tiles { get; set; }

            public void PlaceTiles()
            {
                Tiles = new Tile[Width, Height];
                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        var posX = (x - y) * (BattleMapGenerator.TileWidth / 2);
                        var posY = (x + y) * (BattleMapGenerator.TileHeight / 2);
                        var tile = new Tile(TextureLoader.Instance.TextureDictionary[TileIDs[x, y]],
                            new Vector2(posX, posY));

                        tile.GetComponent<RenderComponent>().RenderLayer = Width * y + x;

                        SceneManagement.Instance.CurrentScene.AddEntity(tile);
                        Tiles[x, y] = tile;
                    }
                }
            }

            public void SetOverlay(Color color, Point origin, float range)
            {
                if(Tiles == null)
                    return;
                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        var tile = Tiles[x, y];
                        var dist = Vector2.Distance(origin.ToVector2(), new Point(x, y).ToVector2());

                        var lastColor = tile.GetComponent<RenderComponent>().Color;
                        if (dist <= range)
                        {
                            tile.GetComponent<RenderComponent>().Color = color;
                        }
                        else
                        {
                            tile.GetComponent<RenderComponent>().Color = lastColor;
                        }
                    }
                }
            }

            public void ClearOverlay()
            {
                if(Tiles == null)
                    return;
                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        var tile = Tiles[x, y];
                        tile.GetComponent<RenderComponent>().Color = Color.White;
                    }
                }
            }

            public bool InBounds(Point point)
            {
                return (point.X >= 0 && point.X < Width && point.Y >= 0 && point.Y < Height) ;
            }

            public void Destroy()
            {
                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        var tile = Tiles[x, y];
                        tile.Destroy();
                    }
                }
            }
        }

        public static int TileWidth = 50;
        public static int TileHeight = 26; 

            public static BattleMap GenerateBattleMap(int width, int height)
        {
            var map = new BattleMap();
            map.Width = width;
            map.Height = height;
            map.TileIDs = new int[width, height];

            //Initialize Map
            map = InitializeMap(map);

            // Generate Obstacles
            map = CreateObstacles(map);

            return map; 
        }

        /// <summary>
        /// Initializes a BattleMap with '0's
        /// </summary>
        /// <param name="map"></param>
        /// <returns></returns>
        private static BattleMap InitializeMap(BattleMap map)
        {
            for (int y = 0; y < map.Height; y++)
            {
                for (int x = 0; x < map.Width; x++)
                {
                    map.TileIDs[x, y] = 0;
                }
            }

            return map; 
        }

        /// <summary>
        /// Creates obstacles for a BattleMap
        /// </summary>
        /// <param name="map"></param>
        /// <returns></returns>
        private static BattleMap CreateObstacles(BattleMap map)
        {
            for (int y = 0; y < map.Height; y++)
            {
                for (int x = 0; x < map.Width; x++)
                {
                    if (MathUtils.Random.Next(0, 100) % 13 == 0)
                    {
                        map.TileIDs[x, y] =1;
                    }
                }
            }

            return map;
        }
    }
}
