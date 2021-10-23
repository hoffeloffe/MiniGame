using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NotAGame.MiniGames;
using SpaceRTS;
using System;
using System.Collections.Generic;
using System.Text;

namespace NotAGame
{
    class MiniGamesManager
    {
        private int currentGame = 0;
        MiniGameTest miniGame = new MiniGameTest();
        private bool noHoldDown = false;

        public MiniGamesManager()
        {

        }

        public void ChangeGame()
        {
            KeyboardState key = Keyboard.GetState();

            if (key.IsKeyDown(Keys.M) && noHoldDown == false)
            {
                ++currentGame;
                noHoldDown = true;
            }
        }

        public void DrawNextGame(SpriteBatch spriteBatch)
        {
            
            if (currentGame < 5)
            {
                switch (currentGame)
                {
                    case 1:
                        miniGame.DrawEmil(spriteBatch);
                        break;
                    case 2:
                        Lobby lobby = new Lobby();
                        break;
                }
                noHoldDown = false;
            }
            else
            {
                currentGame = 0;
            }
        }

    }
}
