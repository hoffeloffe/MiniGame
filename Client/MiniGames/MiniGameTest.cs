using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NotAGame.Component;
using SpaceRTS;
using System;
using System.Collections.Generic;
using System.Text;

namespace NotAGame.MiniGames
{
    class MiniGameTest
    {
        public MiniGameTest()
        {
        }

        public void DrawEmil(SpriteBatch spriteBatch)
        {
            GameObject emilGO = new GameObject();
            SpriteRenderer sr = new SpriteRenderer();
            emilGO.AddComponent(sr);
            emilGO.AddComponent(new MiniGame());

            sr.Scale = 1;
            sr.SetSpriteName("Emil");
            sr.Layerdepth = 0;
            emilGO.transform.Position = new Vector2(500, 970);
            GameWorld.Instance.Games.Add(emilGO);
            
        }
    }
}
