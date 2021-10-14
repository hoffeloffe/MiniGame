using Microsoft.Xna.Framework;
using NotAGame.Command_Pattern;
using SpaceRTS;
using System;
using System.Collections.Generic;
using System.Text;

namespace NotAGame.Component
{
    class Player : Component
    {
        private float speed;
        private Vector2 startPosition;

        public Player()
        {
            InputHandler.Instance.player = this;
            this.speed = 500;
        }

        public override void Awake()
        {
            GameObject.transform.Position = new Vector2(975, 500);
        }

        public override void Start()
        {
            SpriteRenderer sr = (SpriteRenderer)GameObject.GetComponent("SpriteRenderer");
            sr.SetSpriteName("Emil");
            sr.Scale = 1;
            sr.Layerdepth = 1;
        }

        public void Move(Vector2 velocity)
        {
            if (velocity != Vector2.Zero)
            {
                velocity.Normalize();
            }
            velocity *= speed;
            GameObject.transform.Translate(velocity * GameWorld.Instance.DeltaTime);
        }

        public override string ToString()
        {
            return "Player";
        }
    }
}
