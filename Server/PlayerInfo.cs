using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Text;

namespace Server
{
    public class PlayerInfo
    {   //0-id 1-position, 2-name, 3-message, 4-color, 5-totalP, 6-miniP, 7-done, 8-failed
        public int id { get; set; }
        public string position { get; set; }
        public string ip { get; set; }
        public int port { get; set; }
        public string message { get; set; }
        public float totalPoints { get; set; }
        public float minigamePoints { get; set; }
        public bool done { get; set; }
        public bool failed { get; set; }
        public string username { get; set; }
        public string color { get; set; }

        //, string message, float totalpoints, float minigamePoints, bool done, bool failed, string username, string color
        //i + position + message + totalRPoints + minigamePoints + done + "@" failed + username + color;
        public PlayerInfo(int id, string ip, int port)
        {
            this.id = id;
            this.ip = ip;
            this.port = port;

            //Random rnd = new Random();
            //Color randomColor = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
            //Console.WriteLine(randomColor);
            //randomColor = playerColor;
        }

        public Color playerColor;
    }
}

//this.totalPoints = Convert.ToInt32(totalPoints);
//this.minigamePoints = Convert.ToInt32(minigamePoints);
//this.done = bool.Parse(done);
//this.failed = bool.Parse(failed);