using Microsoft.Xna.Framework;
using SpaceRTS;
using System;
using System.Collections.Generic;
using System.Text;

namespace NotAGame.Component
{
    class Player : Component
    {
        private float speed;

        public Player()
        {
            this.speed = 100;
        }

        public override void Awake()
        {
            //    GameObject.transform.Position = new Vector2(GameWorld.Instance.GraphicsDevice.Viewport.Width / 2,
            //GameWorld.Instance.GraphicsDevice.Viewport.Height);
        }

        public override void Start()
        {
            SpriteRenderer sr = new SpriteRenderer();
            sr.SetSpriteName("Emil");

        }

        public override string ToString()
        {
            return "Player";
        }
    }
}
