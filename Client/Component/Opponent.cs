using Microsoft.Xna.Framework;
using NotAGame.Command_Pattern;
using SpaceRTS;
using System;
using System.Collections.Generic;
using System.Text;

namespace NotAGame.Component
{
    class Opponent : Component
    {
        private Vector2 Position = new Vector2(200, 200);
        SpriteRenderer sr;

        public Opponent()
        {

        }
        public Opponent(Color color)
        {
            sr = (SpriteRenderer)GameObject.GetComponent("SpriteRenderer");
            sr.Color = color;
        }
        public Opponent(int x, int y)
        {
            //Position = new Vector2(x, y);
        }
        public override void Awake()
        {
            //GameObject.transform.Position = new Vector2(975, 500);
        }
        public override void Start()
        {
            sr = (SpriteRenderer)GameObject.GetComponent("SpriteRenderer");
            sr.SetSpriteName("Sprites/dog_stand");
        }
        public override void Update(GameTime gametime)
        {
            //sr.GameObject.transform.Position = Position;
            base.Update(gametime);
        }

        public override string ToString()
        {
            return "Opponent";
        }
    }
}
