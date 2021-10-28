﻿using Microsoft.Xna.Framework;
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
using NotAGame.MiniGames;
using Microsoft.Xna.Framework.Media;

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
        public static Texture2D mouseSprite;
        public static SpriteFont font;
        public static SpriteFont smallFont;

        private Random rnd = new Random();
        private Color yourColor;
        private List<int> playersId = new List<int>();
        private string name = "NoName";
        private List<string> names = new List<string>() { "Jeff", "John", "Joe", "Jack", "Jim", "Peter", "Paul", "Ticky", "Tennis", "Egg Salad", "Dingus", "Fred", "Mango", "Cupcake", "Snowball", "Dragonborn" };
        private Player player;
        private string som;
        public List<GameObject> opponents = new List<GameObject>();
        private int OpponentGOBJCounter = 0;
        public int playerID;
        public int totalPoints = 0;
        public int changeInTotalPoints = 0;
        public Vector2 changeInPosition;

        private List<GameObject> gameObjects;
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

        #region Mini games manager fields
        private MiniGamesManager gameManager;
        public static bool changeGame = false;
        public static Texture2D Emil;
        #endregion

        #region server/client fields
        private Client client = new Client();
        private string serverMessage;
        private List<string[]> playerInfomationList = new List<string[]>();
        private int plInfoListCountIsTheSame = 0;
        private Color color;
        private Thread sendThread;
        private Thread reciveThread;

        private readonly object recivelock = new object();
        private readonly object sendlock = new object();
        private string serverMessageIsTheSame;
        #endregion

        private List<string> chatstring = new List<string>();
        public float DeltaTime { get; set; }

        private Song monsterSound;

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

            #region GameObjects - Player, texts, add to gameObjects

            gameManager = new MiniGamesManager();

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
            #endregion Componenent

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
            mouseSprite = Content.Load<Texture2D>("cursorGauntlet_grey");
            font = Content.Load<SpriteFont>("Fonts/Arial48");
            smallFont = Content.Load<SpriteFont>("Fonts/Arial24");
            monsterSound = Content.Load<Song>("MonsterVoice");
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
            MiniGamesManager.Instance.Update(gameTime);
            if (true)
            {
                MediaPlayer.Play(monsterSound);
                MediaPlayer.IsRepeating = true;
            }

            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.Update(gameTime);
            }

            foreach (GameObject opponent in opponents)
            {
                opponent.Update(gameTime);
            }

            #region Client/Server

            #region Server Beskeder

            if (playerGo.transform.Position != changeInPosition)
            {
                client.cq.Enqueue("PO" + playerGo.transform.Position);
                changeInPosition = playerGo.transform.Position;
            }
            string superservermessage;
            superservermessage = serverMessage;

            if (superservermessage != null && superservermessage != serverMessageIsTheSame) //if not empty or same
            {
                #region Create Opponent GameObjects Equal to total opponents (virker med dig selv, men ikke med flere spillere endnu)

                Debug.WriteLine(">");
                if (superservermessage.StartsWith("PO"))
                {
                    Debug.Write("(PO)");
                    superservermessage = superservermessage.Remove(0, 2);
                    string[] serverMsgArray = superservermessage.Split("_");
                    string ID = serverMsgArray[1].ToString();
                    string Position = serverMsgArray[0].ToString();

                    if (playerInfomationList.Count == 0)
                    {
                        Debug.Write("(cr)");
                        playerInfomationList.Add(new string[] { ID, Position });
                        CreateOpponentObj();
                    }
                    if (playerInfomationList.Count != plInfoListCountIsTheSame)
                    {
                        Debug.Write("(UNEQUAL)");
                        bool foundID = false;
                        for (int i = 0; i < playerInfomationList.Count; i++)
                        {
                            if (playerInfomationList[i].Contains(serverMsgArray[1]))
                            {
                                int testId = i;
                                string test = serverMsgArray[1];
                                foundID = true;
                            }
                            else
                            {
                                int testId = i;
                                string test = serverMsgArray[1];
                                foundID = false;
                            }
                        }
                        if (foundID == false)
                        {
                            Debug.Write("(no ID: " + serverMsgArray[1] + ", adding)");
                            playerInfomationList.Add(new string[] { ID, Position });
                            CreateOpponentObj();
                        }
                        else
                        {
                            Debug.Write("(I already know player ID " + ID + "!)");
                        }
                    }

                    UpdatePos(Convert.ToInt32(ID), Position);

                    serverMessageIsTheSame = "PO" + superservermessage;
                }

                if (superservermessage.StartsWith("ID"))
                {
                    Debug.Write("(ID)");
                    //Din ID
                    superservermessage = superservermessage.Remove(0, 2);
                    playerID = Convert.ToInt32(superservermessage);
                    foreach (var item in playerInfomationList)
                    {
                        string ID = superservermessage[1].ToString();
                        bool foundID = false;

                        if (!item.Contains(superservermessage[1].ToString()))
                        {
                            Debug.Write("(no ID: " + superservermessage[1] + " , adding)");
                            playerInfomationList.Add(new string[] { ID, new Vector2().ToString() });
                            CreateOpponentObj();
                        }
                        else
                            Debug.WriteLine("(I already know player ID " + ID + "!)");
                    }
                    serverMessageIsTheSame = "ID" + superservermessage;
                }
                //Modtagende beskeder.
                if (superservermessage.StartsWith("ME"))
                {
                    superservermessage = superservermessage.Remove(0, 2);
                    chatstring.Add(superservermessage);
                }
                #endregion

                //Send Dine Totale Points til serveren.
                if (totalPoints != changeInTotalPoints)
                {
                    client.cq.Enqueue("TP" + totalPoints);
                    changeInTotalPoints = totalPoints;
                }

                //Modtag Alles Totale Points
                if (superservermessage.StartsWith("TP"))
                {
                    superservermessage = superservermessage.Remove(0, 2);
                }
                //Send Dine Minigame points til serveren.
                if (superservermessage.StartsWith("MP"))
                {
                    superservermessage = superservermessage.Remove(0, 2);
                }
                //Modtage alles Minigame points til serveren.
                if (superservermessage.StartsWith("MP"))
                {
                    superservermessage = superservermessage.Remove(0, 2);
                }

                //Send Done
                if (superservermessage.StartsWith("DO"))
                {
                    superservermessage = superservermessage.Remove(0, 2);
                }
                //Modtage Done
                if (superservermessage.StartsWith("DO"))
                {
                    superservermessage = superservermessage.Remove(0, 2);
                }
                //Send Fail
                if (superservermessage.StartsWith("FA"))
                {
                    superservermessage = superservermessage.Remove(0, 2);
                }
                //Modtage Fails
                if (superservermessage.StartsWith("FA"))
                {
                    superservermessage = superservermessage.Remove(0, 2);
                }
                //Send Username
                if (superservermessage.StartsWith("US"))
                {
                    superservermessage = superservermessage.Remove(0, 2);
                }
                //Modtage Username
                if (superservermessage.StartsWith("US"))
                {
                    superservermessage = superservermessage.Remove(0, 2);
                }
                //Send Color
                if (superservermessage.StartsWith("CO"))
                {
                    superservermessage = superservermessage.Remove(0, 2);
                }
                //Modtage Color
                if (superservermessage.StartsWith("CO"))
                {
                    superservermessage = superservermessage.Remove(0, 2);
                }
                #region Create Opponent GameObjects Equal to total opponents (virker med dig selv, men ikke med flere spillere endnu)
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
                    //UpdatePos(id);
                    UpdateColor(id);
                    UpdateName(id);
                }

                #endregion Create Opponent GameObjects Equal to total opponents (virker med dig selv, men ikke med flere spillere endnu)

                plInfoListCountIsTheSame++;
            }

            #endregion Server Beskeder

            #endregion Client/Server

            // position + message + totalPoints +  minigamePoints + done + failed username + color;
            //                  position,                                                        message,     totalPoints, minigamePoints + done + failed username + color;
            //client.cq.Enqueue(playerGo.transform.ReturnPosition(playerGo).ToString() + "@" + "messageTest" + "@" + "1" + "@" + "9" + "@" + "false" + "@" + "false" + "@" + name + "@" + yourColor);
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
            MiniGamesManager.Instance.DrawNextGame(_spriteBatch);
            MiniGamesManager.Instance.Draw(_spriteBatch);

            foreach (string message in chatstring)
            {
                _spriteBatch.DrawString(font, message, new Vector2(100, 100), Color.Black);
            }
            gameManager.DrawNextGame(_spriteBatch);

            base.Draw(gameTime);
            _spriteBatch.End();
        }

        List<GameObject> games = new List<GameObject>();
        public List<GameObject> Games 
        {
            get
            {
                return games;
            }

            set
            {
                games = value;
            }
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

        public void UpdatePos(int id, string value)
        {
            playerInfomationList[id][1] = value; //update position in ListString
            string som = playerInfomationList[id][1].ToString(); //used for exracting the position from the string
            string cleanString = som.Replace("{X:", "");
            cleanString = cleanString.Replace("Y:", "");
            cleanString = cleanString.Replace("}", "");
            cleanString = cleanString.Replace(".", ",");
            string[] xyVals = cleanString.Split(' ');
            float XPos = float.Parse(xyVals[0]);
            float YPos = float.Parse(xyVals[1]);
            opponents[id].transform.Position = new Vector2(XPos, YPos); //update position in opponentslist.

            //Debug.WriteLine("Upd. Pos. OppList id " + id + ": X" + XPos + ", Y" + YPos + " " + opponents[id].transform.Position + ", InfoVal: " + playerInfomationList[id][1]);
            Debug.Write("[Obj:" + id + ", id:" + playerInfomationList[id][0] + "]");
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

        public void CreateOpponentObj()
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
            //playersId.Add(Convert.ToInt32(playerInfomationList[playerInfomationList.Count - 1][0]));
            //oppSpr.Font = Content.Load<SpriteFont>("Fonts/Arial24");
            //oppSpr.hasLabel = true;
            //oppSpr.Text = playerInfomationList[playerInfomationList.Count - 1][0] + " ";
            oppObj.Awake();
            oppObj.Start();

            Debug.WriteLine("= " + opponents.Count + " total Opponent Objects, 1 new.");
        }
#endregion
    }
}