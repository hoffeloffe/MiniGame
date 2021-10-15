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
using System;

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
        private int masterCounter;
        private string som;
        public List<GameObject> opponents = new List<GameObject>();

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

        private GameObject playerGo;

        private Client client = new Client();
        private string serverMessage;
        private List<List<string>> playerInfomationList = new List<List<string>>();
        private string playerPosition;
        private Color color;
        private Thread sendThread;
        private Thread reciveThread;

        private readonly object recivelock = new object();
        private readonly object sendlock = new object();
        private string serverMessageIsTheSame;

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
            lobby = new Lobby();

            #region GameObjects

            playerGo = new GameObject();
            player = new Player();
            playerGo.AddComponent(player);
            playerGo.AddComponent(new SpriteRenderer());
            gameObjects.Add(playerGo);

            #region Tekst

            GameObject goText = new GameObject();
            SpriteRenderer cpSprite = new SpriteRenderer();
            Text CpText = new Text();
            goText.AddComponent(cpSprite);
            goText.AddComponent(CpText);
            CpText.SetText("Arial96", "MonoParty!", 620, 5, 1f, 0.12f, Color.MonoGameOrange);
            cpSprite.hasShadow = true;
            cpSprite.Color2 = Color.Black;
            gameObjects.Add(goText);
            //--------------
            goText = new GameObject();
            cpSprite = new SpriteRenderer();
            CpText = new Text();
            goText.AddComponent(cpSprite);
            goText.AddComponent(CpText);
            CpText.SetText("Arial24", "More text here!", 100, 190, 1f, -0.05f, Color.Black);
            gameObjects.Add(goText);
            //--------------
            goText = new GameObject();
            cpSprite = new SpriteRenderer();
            CpText = new Text();
            goText.AddComponent(cpSprite);
            goText.AddComponent(CpText);
            CpText.SetText("Hands", "Outline test.", 100, 250, 0.5f, 0, Color.White);
            cpSprite.hasOutline = true;
            cpSprite.Color2 = Color.Black;
            gameObjects.Add(goText);
            //--------------
            goText = new GameObject();
            cpSprite = new SpriteRenderer();
            CpText = new Text();
            goText.AddComponent(cpSprite);
            goText.AddComponent(CpText);
            CpText.SetText("Hands", "Shadow test.", 100, 330, 0.5f, 0, Color.White);
            cpSprite.hasShadow = true;
            gameObjects.Add(goText);
            //--------------
            goText = new GameObject();
            cpSprite = new SpriteRenderer();
            CpText = new Text();
            goText.AddComponent(cpSprite);
            goText.AddComponent(CpText);
            CpText.SetText("Hands", ":D", 125, 425, 0.5f, 0, Color.Green);
            cpSprite.hasOutline = false;
            cpSprite.hasShadow = true;
            cpSprite.Spin = true;
            gameObjects.Add(goText);
            //--------------
            goText = new GameObject();
            cpSprite = new SpriteRenderer();
            CpText = new Text();
            goText.AddComponent(cpSprite);
            goText.AddComponent(CpText);
            CpText.SetText("Hands", "Shadow test.", 100, 330, 0.5f, 0, Color.White);
            cpSprite.hasShadow = true;
            gameObjects.Add(goText);

            #endregion Tekst

            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.Awake();
            }
            foreach (GameObject opponent in opponents)
            {
                opponent.Awake();
            }

            #endregion Component

            #region Server

            sendThread = new Thread(() => SendThread());
            reciveThread = new Thread(() => ReceiveThread());
            sendThread.IsBackground = true;
            reciveThread.IsBackground = true;
            sendThread.Start();
            reciveThread.Start();

            #endregion Server

            base.Initialize();
        }

        public void ReceiveThread()
        {
            while (true)
            {
                serverMessage = client.ReceiveData();
            }
        }

        public void SendThread()
        {
            while (true)
            {
                client.SendDataOnce(playerGo.transform.ReturnPosition(playerGo).ToString() + "@" +  "messageTest" + "@" + "1");
            }
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.Start();
            }
            foreach (GameObject opponent in opponents)
            {
                opponent.Start();
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

            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.Update(gameTime);
            }

            #region Server Beskeder
            string superservermessage;
            superservermessage = serverMessage;

            if (superservermessage != null && superservermessage != serverMessageIsTheSame)
            {
                string[] array = superservermessage.Split('q');

                for (int i = 0; i < array.Length; i++)
                {
                    if (i + 1 > playerInfomationList.Count)
                    {
                        playerInfomationList.Add(array[i].Split('_').ToList());
                    }
                    else
                        playerInfomationList[i] = array[i].Split('_').ToList();
                }
                #region Create Opponent GameObjects Equal to total opponents (virker med dig selv, men ikke med flere spillere endnu)
                for (int i = 0; i < array.Length; i++)
                {
                    if (opponents.Count < playerInfomationList.Count)
                    {
                        Random rnd = new Random();
                        Color randomColor = new Color(rnd.Next(256), rnd.Next(256), rnd.Next(256));
                        GameObject oppObj = new GameObject();
                        SpriteRenderer oppSpr = new SpriteRenderer();
                        Opponent oppOpp = new Opponent();
                        oppSpr.Color = randomColor;
                        oppObj.AddComponent(oppSpr);
                        oppObj.AddComponent(oppOpp);
                        opponents.Add(oppObj);
                        oppObj.Awake();
                        oppObj.Start();
                    }
                    else if (opponents.Count > playerInfomationList.Count)
                    {
                        Debug.WriteLine("ERROR Code 28713");
                    }
                }
                #endregion
                #region Send position to each Opponent
                for (int i = 0; i < opponents.Count; i++)
                {
                    string som = playerInfomationList[i][1].ToString();
                    string cleanString = som.Replace("{X:", "");
                    cleanString = cleanString.Replace("Y:", "");
                    cleanString = cleanString.Replace("}", "");
                    string[] xyVals = cleanString.Split(' ');
                    float XPos = float.Parse(xyVals[0]);
                    float YPos = float.Parse(xyVals[1]);
                    string client0Message = som + " anyway, X: " + XPos + ", og Y: " + YPos;
                    Debug.WriteLine(client0Message);
                    opponents[i].transform.Position = new Vector2(XPos, YPos);
                }
                #endregion

                serverMessageIsTheSame = superservermessage;
            }
            #endregion

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
            foreach (GameObject opponent in opponents)
            {
                opponent.Draw(_spriteBatch);
            }

            base.Draw(gameTime);
            _spriteBatch.End();
        }
    }
}