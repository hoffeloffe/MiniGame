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

        private Client client = new Client();
        private string serverMessage;
        private List<string> playerInfomationList = new List<string>();
        private string playerPosition;
        private Color color;
        private Thread sendThread;

        private Thread reciveThread;



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

            go.AddComponent(new Tile());

            go.AddComponent(new SpriteRenderer());

            gameObjects.Add(go);

            //Tester tekst i spillet:
            //GameObject testTextWoohoo = new GameObject();
            //Text textext = new Text(3, 3, "Hello hello hello hello hellooo!");
            //testTextWoohoo.AddComponent(textext);
            //gameObjects.Add(testTextWoohoo);
            
            GameObject texty = new GameObject();
            
            SpriteRenderer textRender = new SpriteRenderer();
            textRender._font = Instance.Content.Load<SpriteFont>("Fonts/Hands");
            textRender.text = $"Hej med dig :)";
            texty.transform.Position = new Vector2(20, 20);
            //TextRenderer.MeasureText
            //_spriteBatch.DrawString(_font, $"Press to buy", new Vector2(0.1f, 0.075f), Color.Black, 0, new Vector2(0, 0), 1.5f, SpriteEffects.None, 0f);
            texty.AddComponent(textRender);
            
            gameObjects.Add(texty);


            //lobby = new Lobby();

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
            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.Draw(_spriteBatch);
            }
            base.Draw(gameTime);
            _spriteBatch.End();
        }
    }
}