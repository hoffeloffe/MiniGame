using System;
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
        public UdpClient udpClient = new UdpClient(13777);

        private string serverip = "127.0.0.1";
        private int serverPort = 12000;

        public void SendData(string message)
        {
            udpClient.Connect(serverip, serverPort);
            while (true)
            {
                try
                {
                    // Sends a message to the host to which you have connected.
                    Byte[] sendBytes = Encoding.ASCII.GetBytes(message);

                    udpClient.Send(sendBytes, sendBytes.Length);
                    Thread.Sleep(50);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
        }

        public void SendDataOnce(string message)
        {
            udpClient.Connect(serverip, serverPort);
            try
            {
                // Sends a message to the host to which you have connected.
                Byte[] sendBytes = Encoding.ASCII.GetBytes(message);

                udpClient.Send(sendBytes, sendBytes.Length);
                Thread.Sleep(100);
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