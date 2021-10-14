using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Numerics;
using System.Text;

namespace Server
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;

            Color randomColor = Color.FromArgb(232, 123, 10);
            Console.WriteLine(randomColor);
            List<PlayerInfo> PlayerList = new List<PlayerInfo>();
            //Creates a UdpClient for reading incoming data.
            UdpClient receivingUdpClient = new UdpClient(12000);
            Console.WriteLine(receivingUdpClient.Client.LocalEndPoint);
            while (true)
            {
                IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

                try
                {
                    // Blocks until a message returns on this socket from a remote host.
                    Byte[] receiveBytes = receivingUdpClient.Receive(ref RemoteIpEndPoint);

                    string returnData = Encoding.ASCII.GetString(receiveBytes);

                    if (PlayerList.Count > 0)
                    {
                        foreach (PlayerInfo item in PlayerList)
                        {
                            item.position = returnData.ToString();
                        }
                        if (!PlayerList.Any(playerInfo => playerInfo.ip == RemoteIpEndPoint.Address.ToString()))
                        {
                            PlayerList.Add(new PlayerInfo(returnData.ToString(), RemoteIpEndPoint.Address.ToString(), RemoteIpEndPoint.Port.ToString()));
                            Console.WriteLine("Ip joined " + RemoteIpEndPoint.Address.ToString() + " on their port number " + RemoteIpEndPoint.Port.ToString());
                        }
                    }
                    else
                    {
                        PlayerList.Add(new PlayerInfo(returnData.ToString(), RemoteIpEndPoint.Address.ToString(), RemoteIpEndPoint.Port.ToString()));
                        Console.WriteLine("Ip joined " + RemoteIpEndPoint.Address.ToString() + " on port " + RemoteIpEndPoint.Port.ToString());
                    }
                    Console.WriteLine("IP:  " + RemoteIpEndPoint.Address.ToString() + " Port: " + RemoteIpEndPoint.Port.ToString() + " Position " + returnData.ToString());

                    string playerPosition = "";
                    foreach (PlayerInfo item in PlayerList)
                    {
                        for (int i = 0; i < PlayerList.Count; i++)
                        {
                            if (PlayerList[i].position != null || PlayerList[i].position != "")
                                playerPosition += i + "_" + PlayerList[i].position + ",";
                        }
                        playerPosition = playerPosition.Remove(playerPosition.Length - 1);

                        Byte[] sendBytes = Encoding.ASCII.GetBytes(playerPosition);
                        receivingUdpClient.Send(sendBytes, sendBytes.Length, item.ip, Int32.Parse(item.port));
                    }

                    // Sends a message to the host to which you have connected.
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
        }
    }
}