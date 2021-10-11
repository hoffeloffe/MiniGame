using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace NotAGame.Component
{
    class SpriteRenderer : Component
    {
        public Texture2D Sprite { get; set; }
        public Vector2 Origin { get; set; }

        public void SetSpriteName(string spriteName)
        {
           
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

        }

        public String ComponentName()
        {
            return "SpriteRenderer";
        }

    }
}
