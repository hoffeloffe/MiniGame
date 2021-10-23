using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
                spriteBatch.Draw(GameWorld.Emil, new Vector2(500, 970), null, Color.White, 01f, Vector2.Zero, 1, SpriteEffects.None, 0);
        }
    }
}
