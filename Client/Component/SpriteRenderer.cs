using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceRTS;
using System;
using System.Collections.Generic;
using System.Text;

namespace NotAGame.Component
{
    class SpriteRenderer : Component
    {
        public Texture2D Sprite { get; set; }
        public Vector2 Origin { get; set; }
        public float Scale { get; set; }
        public Rectangle Rectangle { get; set; }
        public SpriteFont _font { get; set; }
        public String text { get; set; }

        public void SetSpriteName(string spriteName)
        {
           Sprite = GameWorld.Instance.Content.Load<Texture2D>(spriteName);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite, GameObject.transform.Position, Rectangle, Color.White, 0f, Origin, Scale, SpriteEffects.None, 1);
        }
        public override void DrawString(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_font, text, GameObject.transform.Position, Color.Black, 0, Origin, Scale, SpriteEffects.None, 1f);
        }


        public override string ToString()
        {
            return "SpriteRenderer";
        }

    }
}
