using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;

namespace ConsoleApp4
{
    class Program
    {
        public static string connection= "Server=127.0.0.1;Database=chatserver;Uid=root;Pwd=root";
        //public static List<string> list = new List<string>();
        //public static List<string> count = new List<string>();
        public static Dictionary<TcpClient, string> count1 = new Dictionary<TcpClient, string>();
        static void Main(string[] args)
        {
            ServerObject server = new ServerObject();
             
            while (true)
            {
                TcpClient tcpClient = server.Accept();
                
                ClientObject clientObject = new ClientObject(tcpClient, server);
                Thread clientThread = new Thread(new ThreadStart(clientObject.Process));
                clientThread.Start();
            }            
        }
    }
}
