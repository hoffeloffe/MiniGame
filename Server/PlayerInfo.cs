using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Text;

namespace Server
{
    public class PlayerInfo
    {
        public PlayerInfo(string position, string ip, string port)
        {
            position = this.position;
            ip = this.ip;
            port = this.port;
            Random rnd = new Random();
            Color randomColor = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
            randomColor = playerColor;
        }

        public Color playerColor;
        public string position;
        public string ip;
        public string port;
    }
}