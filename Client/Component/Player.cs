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
            this.speed = 900;
        }

        public override void Awake()
        {
            GameObject.transform.Position = new Vector2(975, 500);
        }

        public override void Start()
        {
            SpriteRenderer sr = (SpriteRenderer)GameObject.GetComponent("SpriteRenderer");
            sr.SetSpriteName("Emil");
            sr.Scale = new Vector2(1, 1);
            sr.Layerdepth = 0.9f;
            
        }

        public override string ToString()
        {
            return "Player";
        }

        public void ChangeInput()
        {
            MiniGamesManager.Instance.currentGame++;
        }

        public void Move(Vector2 velocity)
        {
            if (velocity != Vector2.Zero)
            {
                velocity.Normalize();
            }
            velocity *= speed;
            Vector2 translater = velocity * GameWorld.Instance.DeltaTime;
            //Casting coordinates to prevent crazy long float numbers
            translater.X = (int)translater.X;
            translater.Y = (int)translater.Y;
            GameObject.transform.Translate(translater);
        }
    }
}