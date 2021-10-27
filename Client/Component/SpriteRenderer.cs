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
        public Random rnd = new Random();
        public Color Color { get; set; } = Color.White;
        public float Rotation { get; set; }
        public bool Spin { get; set; } = false;
        public float Layerdepth { get; set; } = 0.5f;

        #region Text/Font Properties
        public SpriteFont Font { get; set; } = GameWorld.Instance.Content.Load<SpriteFont>("Fonts/Arial");
        public SpriteFont FontOut { get; set; } = GameWorld.Instance.Content.Load<SpriteFont>("Fonts/HandsOut");
        public string Text { get; set; } = "Undefined";
        public Color Color2 { get; set; } = Color.Black;
        public bool hasOutline { get; set; } = false;
        public bool hasShadow { get; set; } = false;
        public bool hasLabel { get; set; } = false;
        #endregion

        public void SetSpriteName(string spriteName)
        {
            Sprite = GameWorld.Instance.Content.Load<Texture2D>(spriteName);
            //Color = new Color(rnd.Next(256), rnd.Next(256), rnd.Next(256));
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite, GameObject.transform.Position, null, Color, 0f, Origin, Scale, SpriteEffects.None, Layerdepth);

            if (hasLabel)//Draw text above a sprite.
            {
                spriteBatch.DrawString(Font, Text, new Vector2(GameObject.transform.Position.X + 15, GameObject.transform.Position.Y + -40f), Color.White, Rotation, Origin, Scale, SpriteEffects.None, Layerdepth);
            }
        }

        public override void DrawString(SpriteBatch spriteBatch)
        {
            if (hasOutline) //outdated
            {
                spriteBatch.DrawString(FontOut, Text, GameObject.transform.Position, Color2, Rotation, Origin, Scale, SpriteEffects.None, Layerdepth);
            }
            if (hasShadow) //Draws a black font with offset before the main font.
            {
                spriteBatch.DrawString(Font, Text, new Vector2(GameObject.transform.Position.X + 2, GameObject.transform.Position.Y + 2f), Color2, Rotation, Origin, Scale, SpriteEffects.None, 0.6f);
            }
            spriteBatch.DrawString(Font, Text, GameObject.transform.Position, Color, Rotation, Origin, Scale, SpriteEffects.None, Layerdepth);
        }

        public override void Update(GameTime gametime)
        {
            //Color = new Color(rnd.Next(256), rnd.Next(256), rnd.Next(256));
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