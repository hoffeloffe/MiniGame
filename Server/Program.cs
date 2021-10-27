using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;

namespace Server
{
    internal class Program
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetConsoleMode(IntPtr hConsoleHandle, int mode);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetConsoleMode(IntPtr handle, out int mode);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetStdHandle(int handle);

        private static void Main(string[] args)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("\x1b[38;5;" + 238 + "m");

            int k = 0;
            int idmaker = 0;
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
                    Console.WriteLine("WHILE LOOP ------------- " + PlayerList.Count + " " + RemoteIpEndPoint.Address + " " + RemoteIpEndPoint.Port + " .....");
                    string returnData = Encoding.ASCII.GetString(receiveBytes); ViewReceivedMsg(returnData);
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
                            idmaker++;
                            //PlayerList.Add(new PlayerInfo(RemoteIpEndPoint.Address.ToString(), RemoteIpEndPoint.Port.ToString(), returnData));
                            PlayerList.Add(new PlayerInfo(idmaker, RemoteIpEndPoint.Address.ToString(), RemoteIpEndPoint.Port.ToString()));

                            ViewAddedMsg("ADDED ----------   " + RemoteIpEndPoint.Address.ToString() + " " + RemoteIpEndPoint.Port.ToString() + " ID: " + idmaker + " to PlayerList", ConsoleColor.Yellow, "");

                            Byte[] sendBytes0 = Encoding.ASCII.GetBytes("ID" + idmaker);
                            receivingUdpClient.Send(sendBytes0, sendBytes0.Length, RemoteIpEndPoint.Address.ToString(), RemoteIpEndPoint.Port); ViewSentMsg(sendBytes0, ConsoleColor.Green, "\x1b[38;5;" + 48 + "m");
                        }
                    }
                    else
                    {
                        PlayerList.Add(new PlayerInfo(idmaker, RemoteIpEndPoint.Address.ToString(), RemoteIpEndPoint.Port.ToString()));
                        string consoleMsg = "ADDED ----------   " + RemoteIpEndPoint.Address.ToString() + " " + RemoteIpEndPoint.Port.ToString() + " ID: " + idmaker + " to PlayerList"; ViewAddedMsg(consoleMsg, ConsoleColor.Yellow, "");

                        Byte[] sendBytes0 = Encoding.ASCII.GetBytes("ID" + 0);
                        receivingUdpClient.Send(sendBytes0, sendBytes0.Length, RemoteIpEndPoint.Address.ToString(), RemoteIpEndPoint.Port); ViewSentMsg(sendBytes0, ConsoleColor.Yellow, "");
                    }
                    //Console.WriteLine("IP:  " + RemoteIpEndPoint.Address.ToString() + " Port: " + RemoteIpEndPoint.Port.ToString() + " Position " + returnData.ToString());

                    if (returnData.ToString().StartsWith("PO"))
                    {
                        string playerPostion = returnData.ToString();
                        playerPostion = playerPostion.Remove(0, 2);
                        foreach (PlayerInfo itemh in PlayerList)
                        {
                            if (itemh.ip == RemoteIpEndPoint.Address.ToString())
                            {
                                itemh.position = playerPostion;
                            }

                            Byte[] sendBytes0 = Encoding.ASCII.GetBytes("PO" + itemh.position + "_" + itemh.id);
                            for (int i = 0; i < PlayerList.Count; i++)
                            {
                                receivingUdpClient.Send(sendBytes0, sendBytes0.Length, PlayerList[i].ip, Int32.Parse(PlayerList[i].port)); ViewSentMsg(sendBytes0, ConsoleColor.Green, "");
                            }
                        }
                    }

                    if (returnData.ToString().StartsWith("ME"))
                    {
                        foreach (PlayerInfo item in PlayerList)
                        {
                            Byte[] sendBytes1 = Encoding.ASCII.GetBytes(PlayerList[k].message + "_" + PlayerList[k].id);
                            receivingUdpClient.Send(sendBytes1, sendBytes1.Length, item.ip, Int32.Parse(item.port)); ViewSentMsg(sendBytes1, ConsoleColor.Green, "");
                            k++;
                        }
                    }

                    if (returnData.ToString().StartsWith("TP"))
                    {
                        foreach (PlayerInfo item in PlayerList)
                        {
                            Byte[] sendBytes2 = Encoding.ASCII.GetBytes(PlayerList[k].totalPoints.ToString() + "_" + PlayerList[k].id);
                            receivingUdpClient.Send(sendBytes2, sendBytes2.Length, item.ip, Int32.Parse(item.port)); ViewSentMsg(sendBytes2, ConsoleColor.Green, "");
                            k++;
                        }
                    }
                    if (returnData.ToString().StartsWith("MP"))
                    {
                        foreach (PlayerInfo item in PlayerList)
                        {
                            Byte[] sendBytes3 = Encoding.ASCII.GetBytes(PlayerList[k].minigamePoints.ToString() + "_" + PlayerList[k].id);
                            receivingUdpClient.Send(sendBytes3, sendBytes3.Length, item.ip, Int32.Parse(item.port)); ViewSentMsg(sendBytes3, ConsoleColor.Green, "");
                            k++;
                        }
                    }

                    if (returnData.ToString().StartsWith("DO"))
                    {
                        foreach (PlayerInfo item in PlayerList)
                        {
                            Byte[] sendBytes4 = Encoding.ASCII.GetBytes(PlayerList[k].done.ToString() + "_" + PlayerList[k].id);
                            receivingUdpClient.Send(sendBytes4, sendBytes4.Length, item.ip, Int32.Parse(item.port)); ViewSentMsg(sendBytes4, ConsoleColor.Green, "");
                            k++;
                        }
                    }

                    if (returnData.ToString().StartsWith("FA"))
                    {
                        foreach (PlayerInfo item in PlayerList)
                        {
                            Byte[] sendBytes5 = Encoding.ASCII.GetBytes(PlayerList[k].failed.ToString() + "_" + PlayerList[k].id);
                            receivingUdpClient.Send(sendBytes5, sendBytes5.Length, item.ip, Int32.Parse(item.port)); ViewSentMsg(sendBytes5, ConsoleColor.Green, "");
                            k++;
                        }
                    }
                    if (returnData.ToString().StartsWith("US"))
                    {
                        foreach (PlayerInfo item in PlayerList)
                        {
                            Byte[] sendBytes6 = Encoding.ASCII.GetBytes(PlayerList[k].username + "_" + PlayerList[k].id);
                            receivingUdpClient.Send(sendBytes6, sendBytes6.Length, item.ip, Int32.Parse(item.port)); ViewSentMsg(sendBytes6, ConsoleColor.Green, "");
                            k++;
                        }
                    }

                    if (returnData.ToString().StartsWith("CO"))
                    {
                        foreach (PlayerInfo item in PlayerList)
                        {
                            Byte[] sendBytes7 = Encoding.ASCII.GetBytes(PlayerList[k].playerColor.ToString() + "_" + PlayerList[k].id);
                            receivingUdpClient.Send(sendBytes7, sendBytes7.Length, item.ip, Int32.Parse(item.port)); ViewSentMsg(sendBytes7, ConsoleColor.Green, "");
                            k++;
                        }
                    }

                    // Sends a message to the host to which you have connected.
                    Console.WriteLine("LOOP END   ------------- " + PlayerList.Count + " " + RemoteIpEndPoint.Address + " " + RemoteIpEndPoint.Port + " ID: " + idmaker);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
            void ViewReceivedMsg(string m)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("MES: ");
                Console.WriteLine(m);
                Console.Write("\x1b[38;5;" + 238 + "m");
            }
            void ViewSentMsg(Byte[] m, ConsoleColor color, string customColor)
            {
                Console.ForegroundColor = color;
                Console.Write(customColor + "SEN: ");
                Console.WriteLine(Encoding.ASCII.GetString(m));
                //Console.ForegroundColor = ConsoleColor.;
                Console.Write("\x1b[38;5;" + 238 + "m");
            }
            void ViewAddedMsg(string m, ConsoleColor color, string customColor)
            {
                Console.ForegroundColor = color;
                Console.WriteLine(m);
                //Console.ForegroundColor = ConsoleColor.;
                Console.Write("\x1b[38;5;" + 238 + "m");
            }
        }
    }
}

//Pink: "\x1b[38;5;" + 177 + "m"