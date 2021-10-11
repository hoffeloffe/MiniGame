using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace NotAGame.Component
{
    class Tile : Component
    {
        private int x;
        private int y;
        private Vector2 position;


        public Tile(int x, int y, int tileSize)
        {
            position = new Vector2(x * tileSize, y * tileSize);
            //color = Color.White;
            //scale = new Vector2(1, 1);
        }

        public override void Awake()
        {
            base.Awake();
        }

        public override void Start()
        {
            SpriteRenderer sr = new SpriteRenderer();
            sr.Scale = 1;
        }

        public String ComponentName()
        {
            return "Tile";
        }
    }
}
