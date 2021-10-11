using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Numerics;
using System.Text;

namespace Server
{
    internal class Program
    {
        private static string playerPosition;

        private static void Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;

            List<PlayerInfo> PlayerList = new List<PlayerInfo>();
            //Creates a UdpClient for reading incoming data.
            UdpClient receivingUdpClient = new UdpClient(12001);
            while (true)
            {
                IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

                try
                {
                    // Blocks until a message returns on this socket from a remote host.
                    Byte[] receiveBytes = receivingUdpClient.Receive(ref RemoteIpEndPoint);

                    string returnData = Encoding.ASCII.GetString(receiveBytes);

                    if (PlayerList.Count != 0)
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
                    {
                        PlayerList.Add(new PlayerInfo(returnData.ToString(), RemoteIpEndPoint.Address.ToString(), RemoteIpEndPoint.Port.ToString()));
                        Debug.WriteLine("Address: " + RemoteIpEndPoint.Address);
                        Debug.WriteLine("Port: " + RemoteIpEndPoint.Address);
                    }
                        

                    Console.WriteLine("This is the message you received " +
                                              returnData.ToString());
                    Console.WriteLine("This message was sent from " +
                                                RemoteIpEndPoint.Address.ToString() +
                                                " on their port number " +
                                                RemoteIpEndPoint.Port.ToString());

                    foreach (PlayerInfo item in PlayerList)
                    {
                        Debug.WriteLine("ip: " + item.ip);
                        receivingUdpClient.Connect(item.ip, Int32.Parse(item.port));
                        for (int i = 0; i < PlayerList.Count; i++)
                        {
                            playerPosition += PlayerList[i].position + "c" + PlayerList[i].playerColor + ",";
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