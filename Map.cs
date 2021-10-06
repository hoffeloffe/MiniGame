using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceRTS
{
    internal class Map
    {
        private int row = 30;
        private int col = 17;
        private int gridSize = 65;
        private List<GameObject> grid;

        public Map()
        {
            grid = new List<GameObject>();
            mapMaker();
        }

        public void LoadContent(ContentManager content)
        {
            foreach (GameObject go in grid)
            {
                go.LoadContent(content);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (GameObject go in grid)
            {
                go.Draw(spriteBatch);
            }
        }

        private void mapMaker()
        {
            for (int x = 0; x < row; x++)
            {
                for (int y = 0; y < col; y++)
                {
                    grid.Add(new Tile(x, y, gridSize));
                }
            }
        }
    }
}