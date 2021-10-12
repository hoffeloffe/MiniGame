﻿using Microsoft.Xna.Framework;
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
        private int gridSize = 65;
        private List<GameObject> grid;

        public Lobby()
        {
            grid = new List<GameObject>();
            LobbyMaker();
        }

        //public void LoadContent(ContentManager content)
        //{
        //    foreach (GameObject go in grid)
        //    {
        //        go.Awake();
        //    }
        //}

        //public void Draw(SpriteBatch spriteBatch)
        //{
        //    foreach (GameObject go in grid)
        //    {
        //        go.Start();
        //    }
        //}

        private void LobbyMaker()
        {
            GameObject go = new GameObject();
            for (int x = 0; x < row; x++)
            {
                for (int y = 0; y < col; y++)
                {
                    go.AddComponent(new Tile());
                    grid.Add(go);
                }
            }
        }
    }
}