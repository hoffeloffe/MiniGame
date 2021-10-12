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
            while (true)
            {
                IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
                IPEndPoint FakeIpEndPoint = new IPEndPoint(IPAddress.Parse("0.0.0.0"), 0);

                if (!IPEndPoint.Equals(RemoteIpEndPoint, FakeIpEndPoint))
                    try
                    {
                        // Blocks until a message returns on this socket from a remote host.
                        Byte[] receiveBytes = receivingUdpClient.Receive(ref RemoteIpEndPoint);

                        string returnData = Encoding.ASCII.GetString(receiveBytes);

                        if (PlayerList != null)
                        {
                            foreach (PlayerInfo item in PlayerList)
                            {
                                if (item.ip == RemoteIpEndPoint.Address.ToString())
                                {
                                    item.position = returnData.ToString();
                                }
                                else
                                    PlayerList.Add(new PlayerInfo(returnData.ToString(), RemoteIpEndPoint.Address.ToString(), RemoteIpEndPoint.Port.ToString()));
                            }
                        }
                        else
                            PlayerList.Add(new PlayerInfo(returnData.ToString(), RemoteIpEndPoint.Address.ToString(), RemoteIpEndPoint.Port.ToString()));

                        Console.WriteLine("This is the message you received " +
                                                  returnData.ToString());
                        Console.WriteLine("This message was sent from " +
                                                    RemoteIpEndPoint.Address.ToString() +
                                                    " on their port number " +
                                                    RemoteIpEndPoint.Port.ToString());

                        string playerPosition = "";
                        foreach (PlayerInfo item in PlayerList)
                        {
                            receivingUdpClient.Connect(item.ip, Int32.Parse(item.port));
                            for (int i = 0; i < PlayerList.Count; i++)
                            {
                                playerPosition += i + "c" + PlayerList[i].position + "c" + PlayerList[i].playerColor + ",";
                            }

                            Byte[] sendBytes = Encoding.ASCII.GetBytes(playerPosition);
                            receivingUdpClient.Send(sendBytes, sendBytes.Length);
                        }
                        playerPosition = "";

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