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
        private Random rnd = new Random();
        private Color yourColor;
        private List<int> playersId = new List<int>();
        private string name = "NoName";
        private List<string> names = new List<string>() { "Jeff", "John", "Joe", "Jack", "Jim", "Peter", "Paul", "Ticky", "Tennis", "Egg Salad", "Dingus", "Fred", "Mango", "Cupcake", "Snowball", "Dragonborn" };
        private Player player;
        private string som;
        public List<GameObject> opponents = new List<GameObject>();
        public int playerID;
        public int totalPoints = 0;
        public int changeInTotalPoints = 0;
        public Vector2 currentPosition;
        public Vector2 changeInPosition;

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
        private List<Player> players = new List<Player>();

        public static bool changeGame = false;
        private MiniGamesManager gameManager;
        public static Texture2D Emil;

        private Client client = new Client();
        private string serverMessage;
        private List<List<string>> playerInfomationList = new List<List<string>>();
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
            gameManager = new MiniGamesManager();
            lobby = new Lobby();

            // #region GameObjects - Player, texts, add to gameObjects

            #region Component

            playerGo = new GameObject();
            player = new Player();
            playerGo.AddComponent(player);
            playerGo.AddComponent(new SpriteRenderer());
            gameObjects.Add(playerGo);

            yourColor = new Color(rnd.Next(256), rnd.Next(256), rnd.Next(256));
            Random r = new Random();
            int index = r.Next(names.Count);
            name = names[index];

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

            #region Server Thread

            sendThread = new Thread(() => client.SendData());
            reciveThread = new Thread(() => ReceiveThread());
            sendThread.IsBackground = true;
            reciveThread.IsBackground = true;
            sendThread.Start();
            reciveThread.Start();

            #endregion Server Thread

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Emil = Content.Load<Texture2D>("Emil");
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
            gameManager.ChangeGame();

            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.Update(gameTime);
            }
            int opponentCounter = 0;
            foreach (GameObject opponent in opponents)
            {
                opponent.Update(gameTime);
                opponentCounter++;
            }
            Debug.WriteLine("Counter: " + opponentCounter);

            #region Client/Server

            #region Server Beskeder

            string superservermessage;
            superservermessage = serverMessage;

            if (superservermessage != null && superservermessage != serverMessageIsTheSame) //if not empty or same
            {
                #region Create Opponent GameObjects Equal to total opponents (virker med dig selv, men ikke med flere spillere endnu)

                if (currentPosition != changeInPosition)
                {
                    client.cq.Enqueue("PO" + currentPosition);
                    currentPosition = changeInPosition;
                }
                if (superservermessage.StartsWith("PO"))
                {
                    superservermessage.Remove(0, 2);
                }

                if (superservermessage.StartsWith("ID"))
                {
                    //Din ID
                    superservermessage.Remove(0, 2);
                    playerID = Convert.ToInt32(superservermessage);
                }

                //Send Dine Totale Points til serveren.
                if (totalPoints != changeInTotalPoints)
                {
                    client.cq.Enqueue("TP" + totalPoints);
                    changeInTotalPoints = totalPoints;
                }

                //Modtag Alles Totale Points
                if (superservermessage.StartsWith("TP"))
                {
                    superservermessage.Remove(0, 2);
                }
                //Send Dine Minigame points til serveren.
                if (superservermessage.StartsWith("MP"))
                {
                    superservermessage.Remove(0, 2);
                }
                //Modtage alles Minigame points til serveren.
                if (superservermessage.StartsWith("MP"))
                {
                    superservermessage.Remove(0, 2);
                }

                //Send Done
                if (superservermessage.StartsWith("DO"))
                {
                    superservermessage.Remove(0, 2);
                }
                //Modtage Done
                if (superservermessage.StartsWith("DO"))
                {
                    superservermessage.Remove(0, 2);
                }
                //Send Fail
                if (superservermessage.StartsWith("FA"))
                {
                    superservermessage.Remove(0, 2);
                }
                //Modtage Fails
                if (superservermessage.StartsWith("FA"))
                {
                    superservermessage.Remove(0, 2);
                }
                //Send Username
                if (superservermessage.StartsWith("US"))
                {
                    superservermessage.Remove(0, 2);
                }
                //Modtage Username
                if (superservermessage.StartsWith("US"))
                {
                    superservermessage.Remove(0, 2);
                }
                //Send Color
                if (superservermessage.StartsWith("CO"))
                {
                    superservermessage.Remove(0, 2);
                }
                //Modtage Color
                if (superservermessage.StartsWith("CO"))
                {
                    superservermessage.Remove(0, 2);
                }

                if (opponents.Count < playerInfomationList.Count)//er opponents mindre end antallet af array strenge? tilføj ny opponent.
                {
                    while (opponents.Count < playerInfomationList.Count)
                    {
                        rnd = new Random();
                        //Color randomColor = new Color(rnd.Next(256), rnd.Next(256), rnd.Next(256));
                        GameObject oppObj = new GameObject();
                        SpriteRenderer oppSpr = new SpriteRenderer();
                        Opponent oppOpp = new Opponent();
                        //oppSpr.Color = randomColor;
                        oppObj.AddComponent(oppSpr);
                        oppObj.AddComponent(oppOpp);
                        //Adding opponents and playerId at the same time should help us keep track of who is who, because their positions in the lists are the same...
                        opponents.Add(oppObj);
                        playersId.Add(Convert.ToInt32(playerInfomationList[playerInfomationList.Count - 1][0]));
                        oppSpr.Font = Content.Load<SpriteFont>("Fonts/Arial24");
                        oppSpr.hasLabel = true;
                        oppSpr.Text = playerInfomationList[playerInfomationList.Count - 1][0] + " ";
                        oppObj.Awake();
                        oppObj.Start();
                    }
                }
                if (opponents.Count > playerInfomationList.Count)//er opponents mindre end antallet af array strenge? tilføj ny opponent.
                {
                    Debug.WriteLine("Oops, somebody disconnected");
                }
                foreach (int id in playersId)
                {
                    UpdatePos(id);
                    UpdateColor(id);
                    UpdateName(id);
                }

                #endregion Create Opponent GameObjects Equal to total opponents (virker med dig selv, men ikke med flere spillere endnu)

                serverMessageIsTheSame = superservermessage;
            }

            #endregion Server Beskeder

            #endregion Client/Server

            // position + message + totalPoints +  minigamePoints + done + failed username + color;
            //                  position,                                                        message,     totalPoints, minigamePoints + done + failed username + color;
            client.cq.Enqueue(playerGo.transform.ReturnPosition(playerGo).ToString() + "@" + "messageTest" + "@" + "1" + "@" + "9" + "@" + "false" + "@" + "false" + "@" + name + "@" + yourColor);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkGray);
            _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.Draw(_spriteBatch);
            }
            foreach (GameObject opponent in opponents)
            {
                opponent.Draw(_spriteBatch);
            }

            gameManager.DrawNextGame(_spriteBatch);

            base.Draw(gameTime);
            _spriteBatch.End();
        }

        public void UnloadGame(GameObject go)
        {
            if (go.Tag == "Tiles")
            {
                gameObjects.Remove(go);
            }
        }

        #region Thread Method

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
                client.SendDataOnce(playerGo.transform.ReturnPosition(playerGo).ToString());
            }
        }

        #endregion Thread Method

        public void UpdatePos(int id)
        {
            string som = playerInfomationList[id][1].ToString();
            string cleanString = som.Replace("{X:", "");
            cleanString = cleanString.Replace("Y:", "");
            cleanString = cleanString.Replace("}", "");
            cleanString = cleanString.Replace(".", ",");
            string[] xyVals = cleanString.Split(' ');
            float XPos = float.Parse(xyVals[0]);
            float YPos = float.Parse(xyVals[1]);
            string client0Message = som + " anyway, X: " + XPos + ", og Y: " + YPos;
            Debug.WriteLine(client0Message);
            opponents[id].transform.Position = new Vector2(XPos, YPos);
        }

        public void UpdateColor(int id)
        {
            string som = playerInfomationList[id][8].ToString();
            string cleanString = som.Replace("{R:", "");
            cleanString = cleanString.Replace("G:", "");
            cleanString = cleanString.Replace("B:", "");
            cleanString = cleanString.Replace("A:255}", "");
            cleanString = cleanString.Replace(".", ",");
            string[] xyVals = cleanString.Split(' ');
            int R = Convert.ToInt32(xyVals[0]);
            int G = Convert.ToInt32(xyVals[1]);
            int B = Convert.ToInt32(xyVals[2]);
            //string client0Message = som + " anyway, X: " + XPos + ", og Y: " + YPos;
            //Debug.WriteLine(client0Message);
            SpriteRenderer srr = (SpriteRenderer)opponents[id].GetComponent("SpriteRenderer");
            Color newColor = new Color(R, G, B);
            srr.Color = newColor;
            //srr.Color = new Color(R, G, B);
            Debug.WriteLine(srr.Color);
        }

        public void UpdateName(int id)
        {
            string som = playerInfomationList[id][8].ToString();
            SpriteRenderer srr = (SpriteRenderer)opponents[id].GetComponent("SpriteRenderer");
            srr.Text = playerInfomationList[id][0] + " " + playerInfomationList[id][7];
            //srr.Color = new Color(R, G, B);
            Debug.WriteLine(srr.Color);
        }
    }
}