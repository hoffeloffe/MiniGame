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

        public PlayerInfo(string position, string ip, string port)
        {
            position = this.position;
            this.ip = ip;
            this.port = port;
            Random rnd = new Random();
            Color randomColor = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
            Console.WriteLine(randomColor);
            randomColor = playerColor;
        }

        public Color playerColor;
    }
}