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
        private SpriteFont font = GameWorld.Instance.Content.Load<SpriteFont>("Fonts/Hands");
        private string text = "eggu";
        SpriteRenderer sr;
        Vector2 textSize;
        public Text()
        {

        }

        public Text(int x, int y, string text)
        {
            //SpriteRenderer sr = (SpriteRenderer)GameObject.GetComponent("SpriteRenderer");
        }
        public override void Awake()
        {
            //textSize = font.MeasureString(text);
            sr = (SpriteRenderer)GameObject.GetComponent("SpriteRenderer");
        }

        public override void Start()
        {
        }
        #region SpriteName Methods
        public void SetSpriteName(string spriteName)
        {

            sr.Sprite = GameWorld.Instance.Content.Load<Texture2D>(spriteName);
        }
        public void SetText(string text)
        {

            sr.Text = text;
        }
        public void SetText(string font, string text)
        {

            sr.Font = GameWorld.Instance.Content.Load<SpriteFont>("Fonts/" + font);
            sr.Text = text;
        }
        public void SetText(string font, string text, int x, int y, float scale, float rotation, Color color)
        {
            sr = (SpriteRenderer)GameObject.GetComponent("SpriteRenderer");
            sr.Font = GameWorld.Instance.Content.Load<SpriteFont>("Fonts/" + font);
            sr.FontOut = GameWorld.Instance.Content.Load<SpriteFont>("Fonts/HandsOut");
            sr.Text = text;
            sr.ScaleText = scale;
            sr.IsText = true;
            sr.Rotation = rotation;
            sr.IsText = true;
            sr.Color = color;
            GameObject.transform.Position = new Vector2(x, y);
        }
        #endregion

       
        //public override void Update(GameTime gametime)
        //{
        //    //textSize = font.MeasureString(text);
        //    //textSize.X = 4f;
        //    base.Update(gametime);
        //}
        public override void Update(GameTime gametime)
        {
            if (sr.IsText == true)
            {
                
            }
            base.Update(gametime);
        }
        public override string ToString()
        {
            return "Text";
        }
    }
}