using System;
using System.Collections.Generic;
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

            List<PlayerInfo> PlayerList = new List<PlayerInfo>();

            //Creates a UdpClient for reading incoming data.
            UdpClient receivingUdpClient = new UdpClient(12000);
            while (true)
            {
                IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

                try
                {
                    // Blocks until a message returns on this socket from a remote host.
                    Byte[] receiveBytes = receivingUdpClient.Receive(ref RemoteIpEndPoint);

                    string returnData = Encoding.ASCII.GetString(receiveBytes);

                    if (!PlayerList.Any())
                    {
                        foreach (PlayerInfo item in PlayerList)
                        {
                            if (item.ip == RemoteIpEndPoint.Address.ToString())
                            {
                                item.position = returnData.ToString();
                            }
                            else
                                PlayerList.Add(new PlayerInfo(returnData.ToString(), RemoteIpEndPoint.Address.ToString()));
                        }
                    }
                    else
                        PlayerList.Add(new PlayerInfo(returnData.ToString(), RemoteIpEndPoint.Address.ToString()));

                    Console.WriteLine("This is the message you received " +
                                              returnData.ToString());
                    Console.WriteLine("This message was sent from " +
                                                RemoteIpEndPoint.Address.ToString() +
                                                " on their port number " +
                                                RemoteIpEndPoint.Port.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
        }
    }
}