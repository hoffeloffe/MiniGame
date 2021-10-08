using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Server
{
    public class PlayerInfo
    {
        public PlayerInfo(string position, String ip)
        {
            position = this.position;
            ip = this.ip;
        }

        public string position;
        public string ip;
    }
}