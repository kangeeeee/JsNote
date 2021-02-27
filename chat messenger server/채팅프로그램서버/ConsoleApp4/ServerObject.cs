using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp4
{
    public class ServerObject
    {
        static TcpListener tcpListener;
        List<ClientObject> clients = new List<ClientObject>();
        
        public ServerObject()
        {
            tcpListener = new TcpListener(IPAddress.Any, 8080);
            tcpListener.Start();
            Console.WriteLine("[Log] 채팅방 서버 생성 완료 : " + DateTime.Now);
        }
        public void AddConnection(ClientObject clientObject)
        {
            Console.WriteLine("[Log] 클라이언트 {0} 접속", clientObject.Id);
            clients.Add(clientObject);  
        }
        public void RemoveConnection(ClientObject clientObject)
        {
            Console.WriteLine("[Log] 클라이언트 {0} 종료",clientObject.Id);
            if (clientObject != null)
                clients.Remove(clientObject);   
        }

        public TcpClient Accept()
        {
            return tcpListener.AcceptTcpClient();            
        }


        
        // 연결끊기
        public void Disconnect()
        {
            tcpListener.Stop(); //서버 종료

            for (int i = 0; i < clients.Count; i++)
            {
                clients[i].Close(); //클라이언트 연결 해제
            }
            Environment.Exit(0); //프로세스 완료
                                 
        }
    }
}
