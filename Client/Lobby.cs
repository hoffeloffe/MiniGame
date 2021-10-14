using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NotAGame.Component;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceRTS
{
    class Lobby
    {
        private int row = 30;
        private int col = 17;
        private int gridSize = 65;
        private List<GameObject> grid;

        public Lobby()
        {
            grid = new List<GameObject>();
            LobbyMaker();
            GameWorld.Instance.GameObjects.AddRange(grid);
        }

        public void LobbyMaker()
        {
            for (int x = 0; x < row; x++)
            {
                for (int y = 0; y < col; y++)
                {
                    GameObject tileObject = new GameObject();
                    SpriteRenderer sr = new SpriteRenderer();
                    tileObject.AddComponent(sr);
                    tileObject.AddComponent(new Tile());
                    sr.SetSpriteName("Tile");
                    tileObject.transform.Position = new Vector2(x * 64, y * 64);
                    sr.Scale = 64;
                    sr.Layerdepth = 1;
                    grid.Add(tileObject);
                }
            }
        }
    }
}

//SpriteRenderer sr = (SpriteRenderer)GameObject.GetComponent("SpriteRenderer");
//sr.SetSpriteName("Tile");
//sr.Scale = 1;