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
        private int currentGame;
        MiniGameTest miniGame = new MiniGameTest();
        private bool noHoldDown = false;

        public MiniGamesManager()
        {
            currentGame = 0;
        }

        public void ChangeGame()
        {
            KeyboardState key = Keyboard.GetState();

            if (key.IsKeyDown(Keys.M) && noHoldDown == false)
            {
                currentGame += 1;
                noHoldDown = true;
            }
        }

        public void DrawNextGame(SpriteBatch spriteBatch)
        {
            switch (currentGame)
            {
                case 1:
                    miniGame.DrawEmil(spriteBatch);
                    foreach (GameObject game in GameWorld.Instance.GameObjects)
                    {
                        if (game.Tag == "Tile")
                        {
                            GameWorld.Instance.UnloadGame(game);
                        }
                    }
                    break;
                case 2:
                    //Lobby lobby = new Lobby();
                    break;
            }
            if (currentGame < 5)
            {
                noHoldDown = false;
            }
           
        }

    }
}
