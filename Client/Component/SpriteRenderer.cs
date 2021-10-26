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
        public Rectangle Rectangle{ get; set; }
        public Vector2 Origin { get; set; }
        public Vector2 Scale { get; set; }
        public Color Color { get; set; } = Color.White;
        public float Layerdepth { get; set; }

        public float ScaleText { get; set; } = 01f;
        public Random rnd = new Random();
        public float Rotation { get; set; }
        public bool Spin { get; set; } = false;
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

            if (hasLabel)//Draw text above the sprite.
            {
                spriteBatch.DrawString(Font, Text, new Vector2(GameObject.transform.Position.X + 15, GameObject.transform.Position.Y + -40f), Color.White, Rotation, Origin, Scale, SpriteEffects.None, 1f);
            }
        }

        public override void DrawString(SpriteBatch spriteBatch)
        {
            if (hasOutline) //outdated
            {
                spriteBatch.DrawString(FontOut, Text, GameObject.transform.Position, Color2, Rotation, Origin, ScaleText, SpriteEffects.None, 1);
            }
            if (hasShadow) //Draws a black font with offset before the main font.
            {
                spriteBatch.DrawString(Font, Text, new Vector2(GameObject.transform.Position.X + 2, GameObject.transform.Position.Y + 2f), Color2, Rotation, Origin, ScaleText, SpriteEffects.None, 1);
            }
            spriteBatch.DrawString(Font, Text, GameObject.transform.Position, Color, Rotation, Origin, ScaleText, SpriteEffects.None, 1);
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