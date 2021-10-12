using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;
using NotAGame;
using System.Linq;
using NotAGame.Component;

namespace SpaceRTS
{
    public class GameWorld : Game
    {
        #region Singleton
        private static GameWorld instance;

        public static GameWorld Instance 
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameWorld();
                }
                return instance;
            }
        }
        #endregion

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Lobby lobby;

        //private GameObject gameObjects;
        private List<GameObject> gameObjects;
        private Client client = new Client();
        private string serverMessage;
        private List<string> playerPositionList = new List<string>();
        private string playerPosition;
        private Color color;

        public GameWorld()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
        }

        protected override void Initialize()
        {
            gameObjects = new List<GameObject>();
            GameObject go = new GameObject();
            go.AddComponent(new Tile());
            go.AddComponent(new SpriteRenderer());
            gameObjects.Add(go);
            lobby = new Lobby();


            foreach (GameObject gameObject in gameObjects)
            {
                for (int i = 0; i < gameObjects.Count; i++)
                {
                    gameObject.Awake();
                }
            }
            client.Connect();

            foreach (var playerinfo in playerPositionList)
            {
                foreach (var item in playerinfo)
                {
                    //position
                    //playerPosition = item[0];
                    //color
                    //color = item[1];
                }
            }
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            foreach (GameObject go in gameObjects)
            {
                for (int i = 0; i < gameObjects.Count; i++)
                {
                    go.Start();
                }
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == Microsoft.Xna.Framework.Input.ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
                Exit();

            foreach (GameObject  gameObject in gameObjects)
            {
                gameObject.Update(gameTime);
            }

            serverMessage = Client.playerPositionList;

            playerPositionList = serverMessage.Split(',').ToList();

            for (int i = 0; i < playerPositionList.Count; i++)
            {
                playerPositionList[i].Split('c').ToList();
            }

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();

            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.Draw(_spriteBatch);
            }
            base.Draw(gameTime);
            _spriteBatch.End();
        }
    }
}