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
            int idmaker = 0;
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
                            if (item.ip == RemoteIpEndPoint.Address.ToString())
                            {
                                item.position = returnData.ToString();
                            }
                        }
                        if (!PlayerList.Any(playerInfo => playerInfo.ip == RemoteIpEndPoint.Address.ToString()))
                        {
                            string[] array = returnData.ToString().Split('@');

                            idmaker++;
                            //PlayerList.Add(new PlayerInfo(RemoteIpEndPoint.Address.ToString(), RemoteIpEndPoint.Port.ToString(), returnData));
                            PlayerList.Add(new PlayerInfo(idmaker, RemoteIpEndPoint.Address.ToString(), RemoteIpEndPoint.Port.ToString()));

                            Console.WriteLine("Ip joined " + RemoteIpEndPoint.Address.ToString() + " on their port number " + RemoteIpEndPoint.Port.ToString());

                            Byte[] sendBytes0 = Encoding.ASCII.GetBytes("ID" + idmaker);
                            receivingUdpClient.Send(sendBytes0, sendBytes0.Length, RemoteIpEndPoint.Address.ToString(), RemoteIpEndPoint.Port);
                        }
                    }
                    else
                    {
                        string[] array = returnData.ToString().Split('@');
                        PlayerList.Add(new PlayerInfo(idmaker, RemoteIpEndPoint.Address.ToString(), RemoteIpEndPoint.Port.ToString()));

                        //PlayerList.Add(new PlayerInfo(RemoteIpEndPoint.Address.ToString(), RemoteIpEndPoint.Port.ToString(), returnData));
                        Console.WriteLine("Ip joined " + RemoteIpEndPoint.Address.ToString() + " on port " + RemoteIpEndPoint.Port.ToString());
                    }
                    Console.WriteLine("IP:  " + RemoteIpEndPoint.Address.ToString() + " Port: " + RemoteIpEndPoint.Port.ToString() + " Position " + returnData.ToString());

                    foreach (PlayerInfo item in PlayerList)
                    {
                        for (int i = 0; i < PlayerList.Count; i++)
                        {
                            if (PlayerList[i].position != null || PlayerList[i].position != "")
                            {
                                if (returnData.ToString().StartsWith("PO"))
                                {
                                    string playerPostion = returnData.ToString();
                                    playerPostion.Remove(0, 2);
                                    foreach (PlayerInfo itemh in PlayerList)
                                    {
                                        if (itemh.ip == RemoteIpEndPoint.Address.ToString())
                                        {
                                            itemh.position = playerPostion;
                                        }
                                    }
                                    Byte[] sendBytes0 = Encoding.ASCII.GetBytes(PlayerList[i].position + "_" + PlayerList[i].id);
                                    receivingUdpClient.Send(sendBytes0, sendBytes0.Length, item.ip, Int32.Parse(item.port));
                                }

                                if (returnData.ToString().StartsWith("ME"))
                                {
                                    Byte[] sendBytes1 = Encoding.ASCII.GetBytes(PlayerList[i].message);
                                    receivingUdpClient.Send(sendBytes1, sendBytes1.Length, item.ip, Int32.Parse(item.port));
                                }

                                if (returnData.ToString().StartsWith("TP"))
                                {
                                    Byte[] sendBytes2 = Encoding.ASCII.GetBytes(PlayerList[i].totalPoints.ToString());
                                    receivingUdpClient.Send(sendBytes2, sendBytes2.Length, item.ip, Int32.Parse(item.port));
                                }
                                if (returnData.ToString().StartsWith("MP"))
                                {
                                    Byte[] sendBytes3 = Encoding.ASCII.GetBytes(PlayerList[i].minigamePoints.ToString());
                                    receivingUdpClient.Send(sendBytes3, sendBytes3.Length, item.ip, Int32.Parse(item.port));
                                }

                                if (returnData.ToString().StartsWith("DO"))
                                {
                                    Byte[] sendBytes4 = Encoding.ASCII.GetBytes(PlayerList[i].done.ToString());
                                    receivingUdpClient.Send(sendBytes4, sendBytes4.Length, item.ip, Int32.Parse(item.port));
                                }

                                if (returnData.ToString().StartsWith("FA"))
                                {
                                    Byte[] sendBytes5 = Encoding.ASCII.GetBytes(PlayerList[i].failed.ToString());
                                    receivingUdpClient.Send(sendBytes5, sendBytes5.Length, item.ip, Int32.Parse(item.port));
                                }
                                if (returnData.ToString().StartsWith("US"))
                                {
                                    Byte[] sendBytes6 = Encoding.ASCII.GetBytes(PlayerList[i].username);
                                    receivingUdpClient.Send(sendBytes6, sendBytes6.Length, item.ip, Int32.Parse(item.port));
                                }

                                if (returnData.ToString().StartsWith("CO"))
                                {
                                    Byte[] sendBytes7 = Encoding.ASCII.GetBytes(PlayerList[i].playerColor.ToString());
                                    receivingUdpClient.Send(sendBytes7, sendBytes7.Length, item.ip, Int32.Parse(item.port));
                                }
                            }
                        }
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