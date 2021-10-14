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
        public float Layerdepth { get; set; }
        //public Vector2 Position{ get; set; }

        public void SetSpriteName(string spriteName)
        {
           Sprite = GameWorld.Instance.Content.Load<Texture2D>(spriteName);

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite, GameObject.transform.Position, null, Color.White, 0f, Origin, Scale, SpriteEffects.None, Layerdepth);
        }

        public override string ToString()
        {
            return "SpriteRenderer";
        }

    }
}
