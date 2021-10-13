using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using SpaceRTS;

namespace NotAGame.Component
{
    class Text : Component
    {
        private Vector2 position;
        private Color color;
        private SpriteFont font;
        private string text;
        SpriteRenderer sr;

        public Text(int x, int y, string text)
        {
            position = new Vector2(x, y);
            color = Color.White;
            this.text = text;
            font = GameWorld.Instance.Content.Load<SpriteFont>("Fonts/Hands");
        }

        public override void Awake()
        {
            base.Awake();
        }

        public override void Start()
        {
            SpriteRenderer sr = (SpriteRenderer)GameObject.GetComponent("SpriteRenderer");
            sr.Scale = 1;
        }
        public override void Draw(SpriteBatch spritebatch)
        {
            spritebatch.DrawString(font, text, position, Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
        }
    }
}