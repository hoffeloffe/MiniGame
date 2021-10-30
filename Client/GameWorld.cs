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
using System.Collections;
using System.Collections.Concurrent;
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
        private Lobby lobby;

        private Random rnd = new Random();
        private Color yourColor;
        private List<int> playersId = new List<int>();
        private string name = "NoName";
        private List<string> names = new List<string>() { "Jeff", "John", "Joe", "Jack", "Jim", "Peter", "Paul", "Ticky", "Tennis", "Egg Salad", "Dingus", "Fred", "Mango", "Cupcake", "Snowball", "Dragonborn" };
        private Player player;
        private string som;
        public List<GameObject> opponents = new List<GameObject>();
        private ConcurrentQueue<string> ServerRecieveQ = new ConcurrentQueue<string>();
        private int OpponentGOBJCounter = 0;
        public int playerID;
        public int totalPoints = 0;
        public int changeInTotalPoints = 0;
        public Vector2 changeInPosition;
        public int positionWait = 0;
        public bool playerMoveFrame = false;
        public bool playerHaltFrame = false;
        public bool firstPersonConnected = false;
        public bool firstUpdateLoop = false;
        public bool spaceDown = false;

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
        private List<string[]> PIL = new List<string[]>(); //PLAYER INFORMATION LIST
        private int plInfoListCountIsTheSame = 0;
        private Color color;
        private Thread sendThread;
        private Thread reciveThread;

        private readonly object recivelock = new object();
        private readonly object sendlock = new object();
        private string comparePrevServerMsg;
        private List<string> chatstring;
        #endregion
        //private List<string> chatstring = new List<string>();
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
            lobby = new Lobby();
            lobby.LobbyMaker();

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
            //cpSprite.Layerdepth = 0.5f;
            gameObjects.Add(goText);
            //--------------
            goText = new GameObject();
            cpSprite = new SpriteRenderer();
            CpText = new Text();
            goText.AddComponent(cpSprite);
            goText.AddComponent(CpText);
            CpText.SetText("Arial24", "Controls:", 100, 190, 1f, -0.05f, Color.Black);
            gameObjects.Add(goText);
            //--------------
            goText = new GameObject();
            cpSprite = new SpriteRenderer();
            CpText = new Text();
            goText.AddComponent(cpSprite);
            goText.AddComponent(CpText);
            CpText.SetText("Hands", "Arrow keys: Move", 100, 250, 0.5f, 0, Color.White);
            cpSprite.hasOutline = true;
            cpSprite.Color2 = Color.Black;
            gameObjects.Add(goText);
            //--------------
            goText = new GameObject();
            cpSprite = new SpriteRenderer();
            CpText = new Text();
            goText.AddComponent(cpSprite);
            goText.AddComponent(CpText);
            CpText.SetText("Hands", "Communicate: Space", 100, 330, 0.5f, 0, Color.White);
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
            //MediaPlayer.Play(monsterSound);
            //MediaPlayer.IsRepeating = true;

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
            positionWait++;
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == Microsoft.Xna.Framework.Input.ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
            {
                Exit();
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == Microsoft.Xna.Framework.Input.ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Space))
            {
                if (spaceDown == false)
                {
                    client.direct = "ME" + "vov";
                    spaceDown = true;
                }                
            }
            else
            {
                spaceDown = false;
            }

            DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            InputHandler.Instance.Excute(player);
            MiniGamesManager.Instance.Update(gameTime);
            

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

            if (playerGo.transform.Position != changeInPosition) //Send Player position if player has moved.
            {
                client.direct = "PO" + playerGo.transform.Position;
                changeInPosition = playerGo.transform.Position;
                playerMoveFrame = true;
            }
            else if (playerMoveFrame == true)
            {
                client.cq.Enqueue("PO" + playerGo.transform.Position);
                playerMoveFrame = false;
            }
            while (ServerRecieveQ.Count != 0)
            {
                //Debug.Write("Queue Count: " + eggSalad.Count);
                // Get the first in the Queue 
                string queueMsg;
                ServerRecieveQ.TryDequeue(out queueMsg);
                //Debug.WriteLine(" Message: " + queueMsg);

                if (queueMsg != null && queueMsg != comparePrevServerMsg || queueMsg.StartsWith("ME")) //If this is a different serverMessage from last, use it
                {
                    string storedSeverMsg, serverMsgMod;
                    storedSeverMsg = serverMsgMod = queueMsg;
                    #region Create Opponent GameObjects Equal to total opponents (virker med dig selv, men ikke med flere spillere endnu)
                    Debug.Write("  <[" + queueMsg + "] ");
                    serverMsgMod = serverMsgMod.Remove(0, 2); //Remove two first letters
                    string[] serverMsgArray = serverMsgMod.Split("_"); //split id and value from eachother
                    string ID = serverMsgArray[1];
                    int intID = Convert.ToInt32(serverMsgArray[1]);
                    string Position = serverMsgArray[0];
                    bool foundID = false; //Whether ID from message is in PIL or not

                    #region Check if message ID is in PIL. If not, create Obj and add opponent to PIL.
                    for (int i = 0; i < PIL.Count; i++)
                    {
                        if (PIL[i][0].Contains(ID)) //Is the client in the PIL?
                        {
                            foundID = true;
                        }
                    }
                    if (foundID == false) //if servermessage's ID is not on the PIL list, add it to the list and create new opponent object.
                    {
                        //0-id 1-position, 2-name, 3-message, 4-color, 5-totalP, 6-miniP, 7-done, 8-failed
                        Debug.Write("(UNKNOWN ID: " + serverMsgArray[1] + ")");
                        PIL.Add(new string[] { ID, Position, "NoName", "NoMessage", "noColor", "NoTotalP", "NMiniP", "NoDone", "NoFailed" });
                        CreateOpponentObj(intID);
                        client.cq.Enqueue("US" + name);
                        firstPersonConnected = true;
                    }
                    else
                    {
                        Debug.Write("(ID " + ID + " found.)");
                    }

                    #endregion

                    if (storedSeverMsg.StartsWith("ID")) //Incoming ID message
                    {
                        Debug.Write("(ID//////////////////////////////////////////////////////)");
                    }
                    else if (storedSeverMsg.StartsWith("PO"))
                    {
                        Debug.Write("(PO)");
                        UpdatePos(intID, Position);
                    }
                    //Modtagende beskeder.
                    else if (storedSeverMsg.StartsWith("ME"))
                    {
                        //chatstring.Add(serverMsgMod);
                        Debug.WriteLine("VOVH!!!!!!!!!!!!!!!");
                        foreach (var obj in opponents)
                        {
                            if (intID == obj.Id)
                            {
                                foreach (var InfoID in PIL)
                                {
                                    if (intID == Convert.ToInt32(InfoID[0]))
                                    {
                                        Opponent oppCp = (Opponent)opponents[intID].GetComponent("Opponent");
                                        oppCp.PlaySound(false, obj.transform.Position.X);
                                    }
                                }
                            }
                        }
                    }

                    //Send Dine Totale Points til serveren.
                    else if (totalPoints != changeInTotalPoints)
                    {
                        client.cq.Enqueue("TP" + totalPoints);
                        changeInTotalPoints = totalPoints;
                    }

                    //Modtag Alles Totale Points
                    else if (storedSeverMsg.StartsWith("TP"))
                    {
                    }
                    //Send Dine Minigame points til serveren.
                    else if (storedSeverMsg.StartsWith("MP"))
                    {
                    }
                    ////Modtage alles Minigame points til serveren.
                    //if (serverMsgMod.StartsWith("MP"))
                    //{
                    //    serverMsgMod = serverMsgMod.Remove(0, 2);
                    //}

                    //Send Done
                    else if (storedSeverMsg.StartsWith("DO"))
                    {
                    }
                    //Modtage Done
                    else if (serverMsgMod.StartsWith("DO"))
                    {
                    }
                    //Send Fail
                    else if (serverMsgMod.StartsWith("FA"))
                    {
                    }
                    //Modtage Fails
                    else if (serverMsgMod.StartsWith("FA"))
                    {
                    }
                    //Send Username
                    else if (serverMsgMod.StartsWith("US"))
                    {
                        
                    }
                    //Modtage Username
                    else if (storedSeverMsg.StartsWith("US"))
                    {
                        UpdateName(intID);
                    }
                    //Send Color
                    else if (serverMsgMod.StartsWith("CO"))
                    {
                        serverMsgMod = serverMsgMod.Remove(0, 2);
                    }
                    //Modtage Color
                    else if (serverMsgMod.StartsWith("CO"))
                    {
                        serverMsgMod = serverMsgMod.Remove(0, 2);
                    }

                    if (opponents.Count < PIL.Count)//er opponents mindre end antallet af array strenge? tilføj ny opponent.
                    {
                        while (opponents.Count < PIL.Count)
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
                            playersId.Add(Convert.ToInt32(PIL[PIL.Count - 1][0]));
                            oppSpr.Font = Content.Load<SpriteFont>("Fonts/Arial24");
                            oppSpr.hasLabel = true;
                            oppSpr.Text = PIL[PIL.Count - 1][0] + " ";
                            oppObj.Awake();
                            oppObj.Start();
                        }
                    }
                    else if (opponents.Count > PIL.Count)//er opponents mindre end antallet af array strenge? tilføj ny opponent.
                    {
                        Debug.WriteLine("Oops, somebody somehow disconnected?");
                    }
                    

                    #endregion Create Opponent GameObjects Equal to total opponents (virker med dig selv, men ikke med flere spillere endnu)


                    plInfoListCountIsTheSame = PIL.Count;
                    Debug.Write("> PLInfo: " + plInfoListCountIsTheSame + ", opponents: " + opponents.Count + " " + comparePrevServerMsg);
                    Debug.WriteLine("");
                    comparePrevServerMsg = storedSeverMsg;
                }
                else
                {
                    //comparePrevServerMsg = serverMessage;
                    //Console.Write("///"+ c);
                }
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

            //foreach (string message in chatstring)
            //{
            //    _spriteBatch.DrawString(, message, new Vector2(100, 100), Color.Black);
            //}
            gameManager.DrawNextGame(_spriteBatch);

            base.Draw(gameTime);
            _spriteBatch.End();
        }

        #region Thread Method

        public void ReceiveThread()
        {
            while (true)
            {
                //serverMessage = client.ReceiveData();
                ServerRecieveQ.Enqueue(client.ReceiveData());
                //Debug.WriteLine("[" + client.ReceiveData() + "]");
            }
        }


        #endregion Thread Method

        public void UpdatePos(int id, string value)
        {
            foreach (var obj in opponents) //go through Objects
            {
                if (id == obj.Id) //if message Id match Obj id
                {
                    foreach (var InfoID in PIL)
                    {
                        if (id == Convert.ToInt32(InfoID[0])) //if message id match in PIL array, update array, update Obj.
                        {
                            InfoID[1] = value;//update position in PIL
                            string cleanString = InfoID[1].ToString(); //position ready to be manipulated
                            string[] xyVals = cleanString.Replace("{X:", "").Replace("Y:", "").Replace("}", "").Replace(".", ",").Split(' ');
                            float XPos = float.Parse(xyVals[0]);
                            float YPos = float.Parse(xyVals[1]);
                            obj.transform.Position = new Vector2(XPos, YPos); //update the Obj position.
                            Debug.Write("[Obj " + obj.Id + " to X:" + XPos +" Y:" + YPos + "]");
                        }
                    }
                }
            }

            //Debug.WriteLine("Upd. Pos. OppList id " + id + ": X" + XPos + ", Y" + YPos + " " + opponents[id].transform.Position + ", InfoVal: " + playerInfomationList[id][1]);
            //Debug.Write("[Obj:" + id + ", id:" + playerInfomationList[id][0] + "]");
        }

        public void UpdateColor(int id)
        {
            string som = PIL[id][8].ToString();
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
            foreach (var obj in opponents) //go through Objects
            {
                if (id == obj.Id) //if message Id match Obj id
                {
                    foreach (var InfoID in PIL)
                    {
                        if (id == Convert.ToInt32(InfoID[0])) //if message id match in PIL array, update array, update Obj.
                        {
                            InfoID[2] = name; //update name in PIL
                            SpriteRenderer srr = (SpriteRenderer)opponents[id].GetComponent("SpriteRenderer");
                            string cleanString = InfoID[1].ToString(); //position ready to be manipulated
                            srr.Text = PIL[id][2]; // update name from value
                        }
                    }
                }
            }
            string som = PIL[id][8].ToString();
            //SpriteRenderer srr = (SpriteRenderer)opponents[id].GetComponent("SpriteRenderer");
            
            //srr.Color = new Color(R, G, B);
            //Debug.WriteLine(srr.Color);
        }

        public void CreateOpponentObj(int theID)
        {
            rnd = new Random();
            GameObject oppObj = new GameObject();
            oppObj.Id = theID;
            SpriteRenderer oppSpr = new SpriteRenderer();
            Opponent oppOpp = new Opponent();
            //oppSpr.Color = randomColor;
            oppObj.AddComponent(oppSpr);
            oppObj.AddComponent(oppOpp);
            Color newColor = new Color(255, 255, 255);
            oppSpr.Color = newColor;
            oppSpr.Text = "Bob " + theID;
            opponents.Add(oppObj);
            //playersId.Add(Convert.ToInt32(playerInfomationList[playerInfomationList.Count - 1][0]));
            oppSpr.Font = Content.Load<SpriteFont>("Fonts/Arial24");
            oppSpr.hasLabel = true;
            //oppSpr.SetSpriteName("Sprites/dog_stand");
            //oppSpr.Layerdepth = 0f;
            //oppSpr.Text = playerInfomationList[playerInfomationList.Count - 1][0] + " ";
            oppObj.Awake();
            oppObj.Start();

            Debug.Write("(Created Obj w/ id:" + oppObj.Id + ". " + opponents.Count + " in total.");
        }
#endregion
    }
}