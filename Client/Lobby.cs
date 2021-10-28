using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NotAGame.Component;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceRTS
{
    internal class Lobby
    {
        private int row = 30;
        private int col = 17;
        private List<GameObject> grid;

        public Lobby()
        {
            grid = new List<GameObject>();
            //GameWorld.Instance.Games.AddRange(grid);
        }

        public void LobbyMaker()
        {
            for (int x = 0; x < row; x++)
            {
                for (int y = 0; y < col; y++)
                {
                    GameObject tileObject = new GameObject();
                    SpriteRenderer sr = new SpriteRenderer();
                    //Add Component
                    tileObject.AddComponent(sr);
                    tileObject.AddComponent(new Tile());
                    tileObject.AddComponent(new MiniGame());
                    
                    //Draw settings
                    sr.SetSpriteName("Tile");
                    tileObject.transform.Position = new Vector2(x * 64, y * 64);
                    sr.Scale = new Vector2(1, 1);
                    sr.Layerdepth = 01f;

                    GameWorld.Instance.GameObjects.Add(tileObject);
                    //grid.Add(tileObject);
                }
            }
        }
    }
}
