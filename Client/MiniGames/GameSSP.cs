using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NotAGame.Component;
using SpaceRTS;
using System;
using System.Collections.Generic;
using System.Text;

namespace NotAGame.MiniGames
{
    class GameSSP
    {
        private Rectangle rectRock;
        private Rectangle rectPaper;
        private Rectangle rectScissor;

        private bool choiceRock = false;
        private bool choicePaper = false;
        private bool choiceScissor = false;

        private Rectangle rectRock2;
        private Rectangle rectPaper2;
        private Rectangle rectScissor2;

        private Vector2 mousePosition;
        public GameSSP()
        {
            DrawTest();

        }

        public void Update()
        {
            MouseState mouse = Mouse.GetState();
            mousePosition = new Vector2(mouse.X, mouse.Y);

            if (new Rectangle(mouse.X, mouse.Y, 1, 1).Intersects(rectRock) || new Rectangle(mouse.X, mouse.Y, 1, 1).Intersects(rectPaper) ||
                new Rectangle(mouse.X, mouse.Y, 1, 1).Intersects(rectScissor) || new Rectangle(mouse.X, mouse.Y, 1, 1).Intersects(rectRock2) ||
                new Rectangle(mouse.X, mouse.Y, 1, 1).Intersects(rectPaper2) || new Rectangle(mouse.X, mouse.Y, 1, 1).Intersects(rectScissor2))
            {
                
                Mouse.SetCursor(MouseCursor.FromTexture2D(GameWorld.mouseSprite, 1,1));
                if (mouse.LeftButton == ButtonState.Pressed && new Rectangle(mouse.X, mouse.Y, 1, 1).Intersects(rectRock))
                {
                    choiceRock = true;

                }
                if (mouse.LeftButton == ButtonState.Pressed && new Rectangle(mouse.X, mouse.Y, 1, 1).Intersects(rectPaper))
                {
                    choicePaper = true;
                }
                if (mouse.LeftButton == ButtonState.Pressed && new Rectangle(mouse.X, mouse.Y, 1, 1).Intersects(rectScissor))
                {
                    choiceScissor = true;
                }
            }
            else
            {
                Mouse.SetCursor(MouseCursor.Arrow);
            }


        }

        public void DrawChoice(SpriteBatch spriteBatch)
        {
            if (choiceRock == true)
            {
                spriteBatch.DrawString(GameWorld.font, "ROCK", new Vector2(250, 440), Color.White, 0, Vector2.Zero, 2, SpriteEffects.None, 0);
            }
            if (choicePaper == true)
            {

            }
            if (choiceScissor == true)
            {

            }
        }

        public void DrawTest()
        {
            GameObject go = new GameObject();

            SpriteRenderer sr = new SpriteRenderer();
            go.AddComponent(sr);
            go.AddComponent(new MiniGame());
            sr.SetSpriteName("panel_blue");
            sr.Scale = new Vector2(5, 5);
            sr.Layerdepth = 0;
            go.transform.Position = new Vector2(275 , 250);
            GameWorld.Instance.GameObjects.Add(go);


            GameObject go1 = new GameObject();
            SpriteRenderer sr1 = new SpriteRenderer();
            go1.AddComponent(sr1);
            go1.AddComponent(new MiniGame());
            sr1.SetSpriteName("panel_blue");
            sr1.Scale = new Vector2(5, 5);
            sr1.Layerdepth = 0;
            go1.transform.Position = new Vector2(1200, 250);
            GameWorld.Instance.GameObjects.Add(go1);


            GameObject rock = new GameObject();
            SpriteRenderer sr2 = new SpriteRenderer();
            rock.AddComponent(sr2);
            sr2.SetSpriteName("buttonLong_blue");
            sr2.Scale = new Vector2(1, 2);
            sr2.Layerdepth = 0;
            rock.transform.Position = new Vector2(230, 760);
            GameWorld.Instance.GameObjects.Add(rock);
            rectRock = new Rectangle(230, 750, 190, 100);

            GameObject paper = new GameObject();
            SpriteRenderer paperSR = new SpriteRenderer();
            paper.AddComponent(paperSR);
            paperSR.SetSpriteName("buttonLong_blue");
            paperSR.Scale = new Vector2(1, 2);
            paperSR.Layerdepth = 0;
            paper.transform.Position = new Vector2(425, 760);
            GameWorld.Instance.GameObjects.Add(paper);
            rectPaper = new Rectangle(425, 760, 190, 100);

            GameObject scissors = new GameObject();
            SpriteRenderer scissorsSR = new SpriteRenderer();
            scissors.AddComponent(scissorsSR);
            scissorsSR.SetSpriteName("buttonLong_blue");
            scissorsSR.Scale = new Vector2(1, 2);
            scissorsSR.Layerdepth = 0;
            scissors.transform.Position = new Vector2(620, 760);
            GameWorld.Instance.GameObjects.Add(scissors);
            rectScissor = new Rectangle(620, 760, 190, 100);


            GameObject rock2 = new GameObject();
            SpriteRenderer rockSR2 = new SpriteRenderer();
            rock2.AddComponent(rockSR2);
            rockSR2.SetSpriteName("buttonLong_blue");
            rockSR2.Scale = new Vector2(1, 2);
            rockSR2.Layerdepth = 0;
            rock2.transform.Position = new Vector2(1155, 760);
            GameWorld.Instance.GameObjects.Add(rock2);
            rectRock2 = new Rectangle(1155, 760, 190, 100);

            GameObject paper2 = new GameObject();
            SpriteRenderer paperSR2 = new SpriteRenderer();
            paper2.AddComponent(paperSR2);
            paperSR2.SetSpriteName("buttonLong_blue");
            paperSR2.Scale = new Vector2(1, 2);
            paperSR2.Layerdepth = 0;
            paper2.transform.Position = new Vector2(1350, 760);
            GameWorld.Instance.GameObjects.Add(paper2);
            rectPaper2 = new Rectangle(1350, 760, 190, 100);

            GameObject scissors2 = new GameObject();
            SpriteRenderer scissorsSR2 = new SpriteRenderer();
            scissors2.AddComponent(scissorsSR2);
            scissorsSR2.SetSpriteName("buttonLong_blue");
            scissorsSR2.Scale = new Vector2(1, 2);
            scissorsSR2.Layerdepth = 0;
            scissors2.transform.Position = new Vector2(1545, 760);
            GameWorld.Instance.GameObjects.Add(scissors2);
            rectScissor2 = new Rectangle(1545, 760, 190, 100);
        }
    }
}
