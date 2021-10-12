using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace NotAGame.Component
{
    class Tile : Component
    {
        private int x;
        private int y;
        private Vector2 position;


        public Tile()
        {
            //color = Color.White;
            //scale = new Vector2(1, 1);
        }

        public override void Awake()
        {
            base.Awake();
        }

        public override void Start()
        {
            SpriteRenderer sr =  (SpriteRenderer)GameObject.GetComponent("SpriteRenderer");
            sr.GameObject.transform.Position = position;
            sr.Scale = 1;
        }

        public String ComponentName()
        {
            return "Tile";
        }
    }
}
