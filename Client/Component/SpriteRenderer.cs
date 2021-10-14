using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceRTS;
using System;
using System.Collections.Generic;
using System.Text;

namespace NotAGame.Component
{
    internal class SpriteRenderer : Component
    {
        public Texture2D Sprite { get; set; } = GameWorld.Instance.Content.Load<Texture2D>("Sprites/missing");
        public Vector2 Origin { get; set; }
        public float Scale { get; set; } = 1f;
        public Color Color { get; set; } = Color.White;
        public string Text { get; set; } = "Undefined";
        public float Rotation { get; set; }
        public bool Spin { get; set; } = false;
        public float Layerdepth { get; set; }

        //Font Properties
        public SpriteFont Font { get; set; } = GameWorld.Instance.Content.Load<SpriteFont>("Fonts/Arial");

        public SpriteFont FontOut { get; set; } = GameWorld.Instance.Content.Load<SpriteFont>("Fonts/HandsOut");
        public Color Color2 { get; set; } = Color.Black;
        public bool hasOutline { get; set; } = false;
        public bool hasShadow { get; set; } = false;

        //public Vector2 Position{ get; set; }
        public void SetSpriteName(string spriteName)
        {
            Sprite = GameWorld.Instance.Content.Load<Texture2D>(spriteName);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite, GameObject.transform.Position, null, Color.White, 0f, Origin, Scale, SpriteEffects.None, Layerdepth);
        }

        public override void DrawString(SpriteBatch spriteBatch)
        {
            if (hasOutline)
            {
                spriteBatch.DrawString(FontOut, Text, GameObject.transform.Position, Color2, Rotation, Origin, Scale, SpriteEffects.None, 1f);
            }
            if (hasShadow)
            {
                spriteBatch.DrawString(Font, Text, new Vector2(GameObject.transform.Position.X + 2, GameObject.transform.Position.Y + 2f), Color2, Rotation, Origin, Scale, SpriteEffects.None, 1f);
            }
            spriteBatch.DrawString(Font, Text, GameObject.transform.Position, Color, Rotation, Origin, Scale, SpriteEffects.None, 1f);
        }

        public override void Update(GameTime gametime)
        {
            if (Spin == true)
            {
                //Continuously rotates the sprite
                Rotation = Rotation - 0.015f;
            }
            base.Update(gametime);
        }

        public override string ToString()
        {
            return "SpriteRenderer";
        }
    }
}