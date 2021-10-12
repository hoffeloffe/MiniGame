using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Windows.Forms;

using System.Diagnostics;
using NotAGame;
using System.Linq;
using System.Threading;

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

        #endregion Singleton

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Map map;

        //private GameObject gameObjects;
        private List<GameObject> gameObjects;

        private Client client = new Client();
        private string serverMessage;
        private List<string> playerInfomationList = new List<string>();
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
            map = new Map();

            foreach (GameObject go in gameObjects)
            {
                for (int i = 0; i < gameObjects.Count; i++)
                {
                    go.Awake();
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
            Thread sendThread = new Thread(() => client.SendData("1234"));

            Thread reciveThread = new Thread(() => serverMessage = client.ReceiveData());

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == Microsoft.Xna.Framework.Input.ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
            {
                Exit();
                sendThread.Abort();
                reciveThread.Abort();
            }

            sendThread.Start();
            reciveThread.Start();

            if (serverMessage != null)
            {
                playerInfomationList.AddRange(serverMessage.Split(','));

                for (int i = 0; i < playerInfomationList.Count; i++)
                {
                    playerInfomationList[i].Split('c').ToList();
                    playerInfomationList[0][0].ToString();
                }
            }

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            map.Draw(_spriteBatch);
            base.Draw(gameTime);
            _spriteBatch.End();
        }
    }
}