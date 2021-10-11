//using System;
//using System.Collections.Generic;
//using System.Text;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;

//namespace SpaceRTS
//{
//    internal class Tile : GameObject
//    {
//        private int x;
//        private int y;

//        public Tile(int x, int y, int tileSize)
//        {
//            position = new Vector2(x * tileSize, y * tileSize);
//            color = Color.White;
//            scale = new Vector2(1, 1);
//        }

//        public override void LoadContent(ContentManager content)
//        {
//            sprite = content.Load<Texture2D>("Tile");
//        }

//        public override void OnCollision(GameObject other)
//        {
//        }

//        public override void Update(GameTime gameTime)
//        {
//        }
//    }
//}