using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace NotAGame
{
    public class Client
    {
        // This constructor arbitrarily assigns the local port number.
        public UdpClient udpClient = new UdpClient(42068);

        private string serverip = "192.168.1.7";
        private int serverPort = 12000;
        public ConcurrentQueue<string> cq = new ConcurrentQueue<string>();
        public string direct;
        private string prevSentMsg;
        private string prevRecievedMsg;

        public void SendData()
        {
            udpClient.Connect(serverip, serverPort);
            while (true)
            {
                if (direct != null || direct != "")
                {
                    try
                    {
                        // Sends a message to the host to which you have connected.
                        //string message;
                        //cq.TryDequeue(out message);
                        Byte[] sendBytes = Encoding.ASCII.GetBytes(direct);

                        udpClient.Send(sendBytes, sendBytes.Length);
                        Thread.Sleep(50);
                        direct = null;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                }
            }
        }

        public void SendDataOnce(string message)
        {
            try
            {
                // Sends a message to the host to which you have connected.
                Byte[] sendBytes = Encoding.ASCII.GetBytes(message);

                udpClient.Send(sendBytes, sendBytes.Length, serverip, serverPort);
                Thread.Sleep(200);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public string ReceiveData()
        {
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
            try
            {
                //IPEndPoint object will allow us to read datagrams sent from any source.

                // Blocks until a message returns on this socket from a remote host.
                Byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);
                string returnData = Encoding.ASCII.GetString(receiveBytes).ToString();

                return returnData;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }
    }
}