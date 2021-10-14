using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;
using NotAGame;
using System.Linq;
using NotAGame.Component;
using System.Threading;
using NotAGame.Command_Pattern;

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
        private Lobby lobby;
        private Player player;

        private List<GameObject> gameObjects = new List<GameObject>();
        public List<GameObject> GameObjects
        {
            get
            {
                return gameObjects;
            }

            set
            {
                gameObjects = value;
            }
        }

        private Client client = new Client();
        private string serverMessage;
        private List<string> playerInfomationList = new List<string>();
        private string playerPosition;
        private Color color;
        private Thread sendThread;

        private Thread reciveThread;

        public float DeltaTime { get; set; }

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

            GameObject go = new GameObject();

            player = new Player();

            go.AddComponent(player);

            //go.AddComponent(new Tile());

            go.AddComponent(new SpriteRenderer());

            gameObjects.Add(go);

            GameObject tile = new GameObject();
            tile.AddComponent(new Tile());
            tile.AddComponent(new SpriteRenderer());
            gameObjects.Add(tile);
            lobby = new Lobby();

            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.Awake();
            }

            sendThread = new Thread(() => client.SendData("1234"));

            reciveThread = new Thread(() => serverMessage = client.ReceiveData());
            sendThread.IsBackground = true;
            reciveThread.IsBackground = true;
            sendThread.Start();
            reciveThread.Start();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.Start();
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == Microsoft.Xna.Framework.Input.ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
            {
                Exit();
            }

            DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            InputHandler.Instance.Excute(player);

            foreach (GameObject  gameObject in gameObjects)
            {
                gameObject.Update(gameTime);
            }

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
            GraphicsDevice.Clear(Color.DarkGray);
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