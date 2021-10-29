using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NotAGame.Component;
using SpaceRTS;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace NotAGame.MiniGames
{
    public class GameSSP
    {
        #region fields
        private Rectangle rectRock;
        private Rectangle rectPaper;
        private Rectangle rectScissor;

        private bool choiceRock = false;
        private bool choicePaper = false;
        private bool choiceScissor = false;

        private Rectangle rectRock2;
        private Rectangle rectPaper2;
        private Rectangle rectScissor2;

        private Rectangle rectReady;
        private bool playerReady;
        private string choicePlayer;

        private int botAI;
        private string choiceAI;
        private bool rockAI = false;
        private bool paperAI = false;
        private bool scissorAI = false;

        private bool noHoldDown = false;
        private Random rnd;

        private float textLayerDepth = 0;
        private float panelDepth = 0.2f;
        private int timer = 5;
        private Thread timerThread;
        
        #endregion

        private Vector2 mousePosition;
        public GameSSP()
        {
            DrawGame();
            timerThread = new Thread(new ThreadStart(Timer));
        }

        public void Update(GameTime gameTime)
        {
            MouseState mouse = Mouse.GetState();
            mousePosition = new Vector2(mouse.X, mouse.Y);

            #region Mouse Input
            if (new Rectangle(mouse.X, mouse.Y, 1, 1).Intersects(rectRock) || new Rectangle(mouse.X, mouse.Y, 1, 1).Intersects(rectPaper) ||
                new Rectangle(mouse.X, mouse.Y, 1, 1).Intersects(rectScissor) || new Rectangle(mouse.X, mouse.Y, 1, 1).Intersects(rectRock2) ||
                new Rectangle(mouse.X, mouse.Y, 1, 1).Intersects(rectPaper2) || new Rectangle(mouse.X, mouse.Y, 1, 1).Intersects(rectScissor2) ||
                new Rectangle(mouse.X, mouse.Y, 1, 1).Intersects(rectReady))
            {
                
                Mouse.SetCursor(MouseCursor.FromTexture2D(GameWorld.mouseSprite, 1,1));

                if (mouse.LeftButton == ButtonState.Pressed && new Rectangle(mouse.X, mouse.Y, 1, 1).Intersects(rectRock))
                {
                    choiceRock = true;
                    choicePaper = false;    
                    choiceScissor = false;
                    choicePlayer = "ROCK";
                }
                if (mouse.LeftButton == ButtonState.Pressed && new Rectangle(mouse.X, mouse.Y, 1, 1).Intersects(rectPaper))
                {
                    choicePaper = true;
                    choiceRock = false;
                    choiceScissor = false;
                    choicePlayer = "PAPER";
                }
                if (mouse.LeftButton == ButtonState.Pressed && new Rectangle(mouse.X, mouse.Y, 1, 1).Intersects(rectScissor))
                {
                    choiceScissor = true;
                    choicePaper = false;
                    choiceRock = false;
                    choicePlayer = "SCISSOR";
                }
                if (mouse.LeftButton == ButtonState.Pressed && new Rectangle(mouse.X, mouse.Y, 1, 1).Intersects(rectReady) && noHoldDown == false)
                {
                    noHoldDown = true;
                    playerReady = true;
                    rnd = new Random();
                    botAI = rnd.Next(1,3);
                    switch (botAI)
                    {
                        case 1:
                            rockAI = true;
                            paperAI = false;
                            scissorAI = false;
                            choiceAI = "ROCK";
                            break;
                        case 2:
                            paperAI = true;
                            rockAI = false;
                            scissorAI = false;
                            choiceAI = "PAPER";
                            break;
                        case 3:
                            scissorAI = true;
                            rockAI = false;
                            paperAI = false;
                            choiceAI = "SCISSOR";
                            break;
                    }
                    timerThread.IsBackground = true;
                    timerThread.Start();
                }
            }
            else
            {
                Mouse.SetCursor(MouseCursor.Arrow);
            }
            #endregion
        }

        public void DrawText(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(GameWorld.smallFont, "Rock", new Vector2(285, 785), Color.Yellow, 0, Vector2.Zero, 1, SpriteEffects.None, textLayerDepth);
            spriteBatch.DrawString(GameWorld.smallFont, "Paper", new Vector2(475, 785), Color.Yellow, 0, Vector2.Zero, 1, SpriteEffects.None, textLayerDepth);
            spriteBatch.DrawString(GameWorld.smallFont, "Scissor", new Vector2(665, 785), Color.Yellow, 0, Vector2.Zero, 1, SpriteEffects.None, textLayerDepth);
            spriteBatch.DrawString(GameWorld.smallFont, "Ready", new Vector2(475, 875), Color.Yellow, 0, Vector2.Zero, 1, SpriteEffects.None, textLayerDepth);


            if (choiceRock == true)
            {
                spriteBatch.DrawString(GameWorld.font, "ROCK", new Vector2(335, 440), Color.Yellow, 0, Vector2.Zero, 2, SpriteEffects.None, textLayerDepth);
                choiceScissor = false;
                choicePaper = false;
            }
            if (choicePaper == true)
            {
                spriteBatch.DrawString(GameWorld.font, "PAPER", new Vector2(310, 440), Color.Yellow, 0, Vector2.Zero, 2, SpriteEffects.None, textLayerDepth);
                choiceRock = false;
                choiceScissor = false;
            }
            if (choiceScissor == true)
            {
                spriteBatch.DrawString(GameWorld.font, "SCISSOR", new Vector2(385, 480), Color.Yellow, 0, Vector2.Zero, 1, SpriteEffects.None, textLayerDepth);
                choiceRock = false;
                choicePaper = false;
            }
            if (playerReady == true)
            {
                if (rockAI == true)
                {
                    spriteBatch.DrawString(GameWorld.font, "ROCK", new Vector2(1265, 440), Color.DarkMagenta, 0, Vector2.Zero, 2, SpriteEffects.None, textLayerDepth);
                    paperAI = false;
                    scissorAI = false;
                    if (choicePlayer == choiceAI)
                    {
                        spriteBatch.DrawString(GameWorld.font, "DRAW", new Vector2(835, 220), Color.GhostWhite, 0, Vector2.Zero, 1, SpriteEffects.None, textLayerDepth);
                    }
                    if (choicePlayer == "PAPER")
                    {
                        spriteBatch.DrawString(GameWorld.font, "YOU WON", new Vector2(835, 220), Color.GhostWhite, 0, Vector2.Zero, 1, SpriteEffects.None, textLayerDepth);
                    }
                    else
                    {
                        spriteBatch.DrawString(GameWorld.font, "YOU LOST", new Vector2(835, 220), Color.GhostWhite, 0, Vector2.Zero, 1, SpriteEffects.None, textLayerDepth);
                    }
                }
                if (paperAI == true)
                {
                    spriteBatch.DrawString(GameWorld.font, "PAPER", new Vector2(1240, 440), Color.DarkMagenta, 0, Vector2.Zero, 2, SpriteEffects.None, textLayerDepth);
                    rockAI = false;
                    scissorAI = false;
                    if (choicePlayer == choiceAI)
                    {
                        spriteBatch.DrawString(GameWorld.font, "DRAW", new Vector2(835, 220), Color.GhostWhite, 0, Vector2.Zero, 1, SpriteEffects.None, textLayerDepth);
                        if (choicePlayer == "ROCK")
                        {
                            spriteBatch.DrawString(GameWorld.font, "YOU WON", new Vector2(835, 220), Color.GhostWhite, 0, Vector2.Zero, 1, SpriteEffects.None, textLayerDepth);
                        }
                        else
                        {
                            spriteBatch.DrawString(GameWorld.font, "YOU LOST", new Vector2(835, 220), Color.GhostWhite, 0, Vector2.Zero, 1, SpriteEffects.None, textLayerDepth);
                        }
                    }
                }
                if (scissorAI == true)
                {
                    spriteBatch.DrawString(GameWorld.font, "SCISSOR", new Vector2(1315, 440), Color.DarkMagenta, 0, Vector2.Zero, 1, SpriteEffects.None, textLayerDepth);
                    paperAI = false;
                    rockAI = false;
                    if (choicePlayer == choiceAI)
                    {
                        spriteBatch.DrawString(GameWorld.font, "DRAW", new Vector2(835, 220), Color.GhostWhite, 0, Vector2.Zero, 1, SpriteEffects.None, textLayerDepth);
                        if (choicePlayer == "PAPER")
                        {
                            spriteBatch.DrawString(GameWorld.font, "YOU WON", new Vector2(835, 220), Color.GhostWhite, 0, Vector2.Zero, 1, SpriteEffects.None, textLayerDepth);
                        }
                        else
                        {
                            spriteBatch.DrawString(GameWorld.font, "YOU LOST", new Vector2(835, 220), Color.GhostWhite, 0, Vector2.Zero, 1, SpriteEffects.None, textLayerDepth);
                        }
                    }
                }
            }

            if (playerReady == true)
            {
                spriteBatch.DrawString(GameWorld.smallFont, timer.ToString(), new Vector2(1800, 100), Color.DarkSlateGray);
                
            }
        }

        public void DrawGame()
        {
            #region GO
            GameObject go = new GameObject();

            SpriteRenderer sr = new SpriteRenderer();
            go.AddComponent(sr);
            go.AddComponent(new MiniGame());
            sr.SetSpriteName("panel_blue");
            sr.Scale = new Vector2(5, 5);
            sr.Layerdepth = panelDepth;
            go.transform.Position = new Vector2(275 , 250);
            GameWorld.Instance.GameObjects.Add(go);


            GameObject go1 = new GameObject();
            SpriteRenderer sr1 = new SpriteRenderer();
            go1.AddComponent(sr1);
            go1.AddComponent(new MiniGame());
            sr1.SetSpriteName("panel_blue");
            sr1.Scale = new Vector2(5, 5);
            sr1.Layerdepth = panelDepth;
            go1.transform.Position = new Vector2(1200, 250);
            GameWorld.Instance.GameObjects.Add(go1);


            GameObject rock = new GameObject();
            SpriteRenderer sr2 = new SpriteRenderer();
            rock.AddComponent(sr2);
            sr2.SetSpriteName("buttonLong_blue");
            sr2.Scale = new Vector2(1, 2);
            sr2.Layerdepth = panelDepth;
            rock.transform.Position = new Vector2(230, 760);
            GameWorld.Instance.GameObjects.Add(rock);
            rectRock = new Rectangle(230, 750, 190, 100);

            GameObject paper = new GameObject();
            SpriteRenderer paperSR = new SpriteRenderer();
            paper.AddComponent(paperSR);
            paperSR.SetSpriteName("buttonLong_blue");
            paperSR.Scale = new Vector2(1, 2);
            paperSR.Layerdepth = panelDepth;
            paper.transform.Position = new Vector2(425, 760);
            GameWorld.Instance.GameObjects.Add(paper);
            rectPaper = new Rectangle(425, 760, 190, 100);

            GameObject scissors = new GameObject();
            SpriteRenderer scissorsSR = new SpriteRenderer();
            scissors.AddComponent(scissorsSR);
            scissorsSR.SetSpriteName("buttonLong_blue");
            scissorsSR.Scale = new Vector2(1, 2);
            scissorsSR.Layerdepth = panelDepth;
            scissors.transform.Position = new Vector2(620, 760);
            GameWorld.Instance.GameObjects.Add(scissors);
            rectScissor = new Rectangle(620, 760, 190, 100);


            GameObject rock2 = new GameObject();
            SpriteRenderer rockSR2 = new SpriteRenderer();
            rock2.AddComponent(rockSR2);
            rockSR2.SetSpriteName("buttonLong_blue");
            rockSR2.Scale = new Vector2(1, 2);
            rockSR2.Layerdepth = panelDepth;
            rock2.transform.Position = new Vector2(1155, 760);
            GameWorld.Instance.GameObjects.Add(rock2);
            rectRock2 = new Rectangle(1155, 760, 190, 100);

            GameObject paper2 = new GameObject();
            SpriteRenderer paperSR2 = new SpriteRenderer();
            paper2.AddComponent(paperSR2);
            paperSR2.SetSpriteName("buttonLong_blue");
            paperSR2.Scale = new Vector2(1, 2);
            paperSR2.Layerdepth = panelDepth;
            paper2.transform.Position = new Vector2(1350, 760);
            GameWorld.Instance.GameObjects.Add(paper2);
            rectPaper2 = new Rectangle(1350, 760, 190, 100);

            GameObject scissors2 = new GameObject();
            SpriteRenderer scissorsSR2 = new SpriteRenderer();
            scissors2.AddComponent(scissorsSR2);
            scissorsSR2.SetSpriteName("buttonLong_blue");
            scissorsSR2.Scale = new Vector2(1, 2);
            scissorsSR2.Layerdepth = panelDepth;
            scissors2.transform.Position = new Vector2(1545, 760);
            GameWorld.Instance.GameObjects.Add(scissors2);
            rectScissor2 = new Rectangle(1545, 760, 190, 100);

            GameObject ready = new GameObject();
            SpriteRenderer readySR = new SpriteRenderer();
            ready.AddComponent(readySR);
            readySR.SetSpriteName("buttonLong_blue");
            readySR.Scale = new Vector2(1, 1);
            readySR.Layerdepth = panelDepth;
            ready.transform.Position = new Vector2(425, 870);
            GameWorld.Instance.GameObjects.Add(ready);
            rectReady = new Rectangle(425, 870, 190, 100);
            #endregion
        }


        private void Timer()
        {
            while (timer != 0)
            {
                Thread.Sleep(1000);
                timer -= 1;

                if (timer == 0)
                {
                    playerReady = false;
                    MiniGamesManager.startOver = true;
                }
            }
        }
    }
}
