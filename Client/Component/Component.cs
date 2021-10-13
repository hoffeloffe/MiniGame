using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceRTS;
using System;
using System.Collections.Generic;
using System.Text;

namespace NotAGame.Component
{
    public class Component
    {
        public bool IsEnabled { get; set; }
        public bool IsText { get; set; }
        public GameObject GameObject { get; set; }

        public virtual void Awake()
        {

        }

        public virtual void Start()
        {

        }

        public virtual void Update(GameTime gametime)
        {

        }

        public virtual void Draw(SpriteBatch spritebatch)
        {

        }
        public virtual void DrawString(SpriteBatch spritebatch)
        {

        }
    }
}
