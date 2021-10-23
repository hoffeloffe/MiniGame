using Microsoft.Xna.Framework;
using NotAGame.Command_Pattern;
using SpaceRTS;
using System;
using System.Collections.Generic;
using System.Text;

namespace NotAGame.Component
{
    internal class Player : Component
    {


        private float speed;

        public Player()
        {
            InputHandler.Instance.player = this;
            this.speed = 600;
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
            sr.Layerdepth = 0;
            
        }

        public override string ToString()
        {
            return "Player";
        }

        public void ChangeInput()
        {
            switch (GameWorld.changeGame)
            {
                case false:
                    GameWorld.changeGame = true;
                    break;
                case true:
                    GameWorld.changeGame = false;
                    break;
            }
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
    }
}