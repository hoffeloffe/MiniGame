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
        #region Importering af ekstra konsol farve funktioner
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetConsoleMode(IntPtr hConsoleHandle, int mode);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetConsoleMode(IntPtr handle, out int mode);

        [DllImport("kernel32.dll", SetLastError = true)]
        #endregion
        public static extern IntPtr GetStdHandle(int handle);

        private static void Main(string[] args)
        {
            Console.Write("\x1b[38;5;" + 238 + "m"); //dark-gray console text color

            int k = 0;
            int idmaker = 0;
            List<PlayerInfo> PlayerList = new List<PlayerInfo>();
            //Creates a UdpClient for reading incoming data.
            UdpClient receivingUdpClient = new UdpClient(12000);
            IPEndPoint RemoteIpEndPoint;
            Console.WriteLine(receivingUdpClient.Client.LocalEndPoint);
            
            while (true)
            {
                RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

                try
                {
                    // Blocks until a message returns on this socket from a remote host.
                    Byte[] receiveBytes = receivingUdpClient.Receive(ref RemoteIpEndPoint);
                    string IPEnd = RemoteIpEndPoint.Address.ToString();
                    int PortEnd = Convert.ToInt32(RemoteIpEndPoint.Port);
                    Console.WriteLine("WHILE LOOP ------------- " + PlayerList.Count + " " + IPEnd + " " + PortEnd + " .....");
                    string returnData = Encoding.ASCII.GetString(receiveBytes); ViewReceivedMsg(returnData); Console.WriteLine(IPEnd + " ok.");
                    string RDNoPrefix = returnData.Remove(0, 2);
                    if (PlayerList.Count > 0)
                    {

                        if (!PlayerList.Any(playerInfo => playerInfo.ip == IPEnd))
                        {
                            idmaker++;
                            //PlayerList.Add(new PlayerInfo(RemoteIpEndPoint.Address.ToString(), RemoteIpEndPoint.Port.ToString(), returnData));
                            PlayerList.Add(new PlayerInfo(idmaker, IPEnd, PortEnd));

                            ViewAddedMsg("ADDED A---------   " + IPEnd + " " + PortEnd + " ID: " + idmaker + " to PlayerList", ConsoleColor.Yellow, "");

                            Byte[] sendBytes0 = Encoding.ASCII.GetBytes("ID" + idmaker + "_" + idmaker);
                            receivingUdpClient.Send(sendBytes0, sendBytes0.Length, IPEnd, PortEnd); ViewSentMsg(sendBytes0, ConsoleColor.Green, "\x1b[38;5;" + 48 + "m=> " + PortEnd);
                        }
                    }
                    else
                    {
                        PlayerList.Add(new PlayerInfo(idmaker, IPEnd, PortEnd));
                        string consoleMsg = "ADDED B---------   " + IPEnd + " " + PortEnd + " ID: " + idmaker + " to PlayerList"; ViewAddedMsg(consoleMsg, ConsoleColor.Yellow, "");

                        Byte[] sendBytes0 = Encoding.ASCII.GetBytes("ID0_0");//You are the first in the server, congrats
                        receivingUdpClient.Send(sendBytes0, sendBytes0.Length, IPEnd, PortEnd); ViewSentMsg(sendBytes0, ConsoleColor.Yellow, "=> " + PortEnd + ": ");
                    }
                    //Console.WriteLine("IP:  " + RemoteIpEndPoint.Address.ToString() + " Port: " + RemoteIpEndPoint.Port.ToString() + " Position " + returnData.ToString());
                    Byte[] sendBytes = Encoding.ASCII.GetBytes("");
                    if (returnData.StartsWith("PO"))
                    {
                        foreach (PlayerInfo player in PlayerList)
                        {
                            if (player.ip == IPEnd)
                                player.position = RDNoPrefix; //store value

                            sendBytes = Encoding.ASCII.GetBytes("PO" + player.position + "_" + player.id);
                            for (int i = 0; i < PlayerList.Count; i++) //send to every player
                            {
                                Sending(sendBytes, i);
                            }
                        }
                    }
                    else if (returnData.StartsWith("ME"))
                    {
                        foreach (PlayerInfo item in PlayerList)
                        {
                            if (item.ip == IPEnd)
                            {
                                item.message = RDNoPrefix;
                                sendBytes = Encoding.ASCII.GetBytes("ME" + RDNoPrefix + "_" + item.id);
                            }
                        }
                        for (int i = 0; i < PlayerList.Count; i++)
                        {
                            Sending(sendBytes, i);
                        }
                    }

                    else if (returnData.ToString().StartsWith("TP"))
                    {
                        foreach (PlayerInfo item in PlayerList)
                        {
                            Byte[] sendBytes2 = Encoding.ASCII.GetBytes(PlayerList[k].totalPoints.ToString() + "_" + PlayerList[k].id);
                            receivingUdpClient.Send(sendBytes2, sendBytes2.Length, item.ip, item.port); ViewSentMsg(sendBytes2, ConsoleColor.Green, "=> " + item.port + ": ");
                            k++;
                        }
                    }
                    else if (returnData.ToString().StartsWith("MP"))
                    {
                        foreach (PlayerInfo item in PlayerList)
                        {
                            Byte[] sendBytes3 = Encoding.ASCII.GetBytes(PlayerList[k].minigamePoints.ToString() + "_" + PlayerList[k].id);
                            receivingUdpClient.Send(sendBytes3, sendBytes3.Length, item.ip, item.port); ViewSentMsg(sendBytes3, ConsoleColor.Green, "=> " + item.port + ": ");
                            k++;
                        }
                    }
                    else if (returnData.ToString().StartsWith("DO"))
                    {
                        foreach (PlayerInfo item in PlayerList)
                        {
                            Byte[] sendBytes4 = Encoding.ASCII.GetBytes(PlayerList[k].done.ToString() + "_" + PlayerList[k].id);
                            receivingUdpClient.Send(sendBytes4, sendBytes4.Length, item.ip, item.port); ViewSentMsg(sendBytes4, ConsoleColor.Green, "=> " + item.port + ": ");
                            k++;
                        }
                    }
                    else if (returnData.ToString().StartsWith("FA"))
                    {
                        foreach (PlayerInfo item in PlayerList)
                        {
                            Byte[] sendBytes5 = Encoding.ASCII.GetBytes(PlayerList[k].failed.ToString() + "_" + PlayerList[k].id);
                            receivingUdpClient.Send(sendBytes5, sendBytes5.Length, item.ip, item.port); ViewSentMsg(sendBytes5, ConsoleColor.Green, "=> " + item.port + ": ");
                            k++;
                        }
                    }
                    else if (returnData.ToString().StartsWith("US"))
                    {
                        foreach (PlayerInfo player in PlayerList)
                        {
                            if (player.ip == IPEnd)
                                player.username = RDNoPrefix;

                            sendBytes = Encoding.ASCII.GetBytes("US" + player.username + "_" + player.id);
                            for (int i = 0; i < PlayerList.Count; i++)
                            {
                                Sending(sendBytes, i);
                            }
                        }
                    }
                    else if (returnData.ToString().StartsWith("CO"))
                    {
                        foreach (PlayerInfo item in PlayerList)
                        {
                            if (item.ip == IPEnd)
                                item.color = RDNoPrefix;

                            sendBytes = Encoding.ASCII.GetBytes("CO" + item.color + "_" + item.id);
                            for (int i = 0; i < PlayerList.Count; i++)
                            {
                                Sending(sendBytes, i);
                            }
                        }
                    }

                    // Sends a message to the host to which you have connected.
                    Console.WriteLine("LOOP END   ------------- " + PlayerList.Count + " " + IPEnd + " " + PortEnd + " ID: " + idmaker);
                    
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
            void Sending(byte[] sendBytes0, int i)
                    {
                        receivingUdpClient.Send(sendBytes0, sendBytes0.Length, PlayerList[i].ip, PlayerList[i].port); ViewSentMsg(sendBytes0, ConsoleColor.Green, "=> " + PlayerList[i].port + ": ");
                    }
            Console.WriteLine("Error, Server is supposed to stay in its while-loop");
            void ViewReceivedMsg(string m)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("MES: ");
                Console.WriteLine(m);
                Console.Write("\x1b[38;5;" + 238 + "m");
            }
            void ViewSentMsg(Byte[] m, ConsoleColor color, string Port)
            {
                Console.ForegroundColor = color;
                Console.Write("SEN: ");
                Console.WriteLine(Encoding.ASCII.GetString(m) + " " + Port);
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