﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace NotAGame.Component
{
    class Tile : Component
    {
        private Vector2 position;
        private int x, y;

        public Tile()
        {
            //color = Color.White;
            //scale = new Vector2(1, 1);
        }

        public override void Awake()
        {
            position = new Vector2(x, y);
        }

        public override void Start()
        {
            SpriteRenderer sr = (SpriteRenderer)GameObject.GetComponent("SpriteRenderer");
            //sr.SetSpriteName("Emil");
            //sr.GameObject.transform.Position = position;
            //sr.Scale = 1;
        }

        public override string ToString()
        {
            return "Tile";
        }
    }
}
