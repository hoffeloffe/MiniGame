using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NotAGame
{
    public class Client
    {
        // This constructor arbitrarily assigns the local port number.
        public UdpClient udpClient = new UdpClient(13000);

        private string serverip = "127.0.0.1";
        private int serverPort = 12000;

        public void SendData(string message)
        {
            while (true)
            {
                try
                {
                    udpClient.Connect(serverip, serverPort);

                    // Sends a message to the host to which you have connected.
                    Byte[] sendBytes = Encoding.ASCII.GetBytes(message);

                    udpClient.Send(sendBytes, sendBytes.Length);

                    udpClient.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
        }

        public string ReceiveData()
        {
            while (true)
            {
                try
                {
                    //IPEndPoint object will allow us to read datagrams sent from any source.
                    IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, serverPort);

                    // Blocks until a message returns on this socket from a remote host.
                    Byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);
                    string returnData = Encoding.ASCII.GetString(receiveBytes).ToString();

                    if (returnData == "")
                        return null;
                    else
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
}