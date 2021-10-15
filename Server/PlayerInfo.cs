using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Text;

namespace Server
{
    public class PlayerInfo
    {
        public string position;
        public string ip;
        public string port;
        public string message;
        public float totalPoints;
        public float minigamePoints;
        public bool done;
        public bool failed;
        public string username;
        public string color;

        public PlayerInfo(string ip, string port, string position, string message, float totalpoints, float minigamePoints, bool done, bool failed, string username, string color)
        {
            this.ip = ip;
            this.port = port;
            this.position = position;
            this.message = message;
            this.totalPoints = totalpoints;
            this.minigamePoints = minigamePoints;
            this.done = done;
            this.failed = failed;
            this.username = username;
            this.color = color;

            Random rnd = new Random();
            Color randomColor = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
            Console.WriteLine(randomColor);
            randomColor = playerColor;
        }

        public Color playerColor;
    }
}