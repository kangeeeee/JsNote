using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace ConsoleApp4
{
    public class ClientObject
    {
        protected internal string Id { get; private set; }
        public NetworkStream Stream { get; private set; }
        
        TcpClient client;
        ServerObject server;
         
        public ClientObject(TcpClient tcpClient, ServerObject serverObject)
        {
            Id = Guid.NewGuid().ToString();
            client = tcpClient;
            server = serverObject;
            server.AddConnection(this); 
        } 
        public void Process()
        {
            
            Random random = new Random();
            int num = 0;

            Stream = client.GetStream();
            string message;

            while (true)
            {
                try
                {
                    message = GetMessage();
                    if (client.Connected)
                    {

                        var parts = message.Split(' ');
                        string a = Convert.ToString(parts[0]);
                        if (a == "/send")
                        {
                            //Database database = new Database();
                            //Console.Write(a);
                            //string to = Convert.ToString(parts[2]);

                            //string text = message.Remove(0, message.IndexOf(':') + 1);
                            //database.CheckUser(to);
                            //Console.WriteLine("아이디: " + to);
                        }
                        else if (a == "a") // 인증번호 전송
                        {
                            num = random.Next(1, 100);
                            Smtp smtp = new Smtp(message, num);
                        }
                        else if (a == "b") // 인증번호 비교
                        {
                            string Res = null;
                            byte[] msg;
                            StringBuilder sb = new StringBuilder("b ");

                            if (num == int.Parse(parts[1]))
                            {
                                Res = "인증이완료되었습니다.";
                                Console.WriteLine("[Log] {0}님이 이메일 인증을 성공하였습니다.", Id);
                                sb.Append(Res);
                                msg = Encoding.UTF8.GetBytes(sb.ToString());
                                Stream.Write(msg, 0, msg.Length);
                                break;
                            }
                            else
                            {
                                Res = "다시입력하세요.";
                                sb.Append(Res);
                                msg = Encoding.UTF8.GetBytes(sb.ToString());
                                Stream.Write(msg, 0, msg.Length);
                                Console.WriteLine("거짓"); 
                            }
                        }
                        else if (a == "c") // 회원가입
                        {
                            string Res = null;
                            byte[] msg;
                            StringBuilder sb = new StringBuilder("c ");

                            try
                            {
                                string nick = parts[1];
                                string id = parts[2];
                                string pass = parts[3];
                                string email = parts[4];
                                Database database = new Database();

                                database.Create(nick, id, pass, email);
                                Console.WriteLine("[Log] {0}님이 회원가입 성공", Id);
                                Res = "회원가입성공";
                                sb.Append(Res);
                                msg = Encoding.UTF8.GetBytes(sb.ToString());
                                Stream.Write(msg, 0, msg.Length);
                                break;
                            }
                            catch (Exception)
                            {

                            }

                        }
                        else if (a == "d") // 중복아이디
                        {
                            string Res = null;
                            byte[] msg;
                            StringBuilder sb = new StringBuilder("d ");
                            try
                            {
                                string id = parts[1];
                                Console.WriteLine(id);
                                Database database = new Database();
                                if (database.CheckID(id) == true)
                                {

                                    Res = "사용가능";
                                    sb.Append(Res);
                                    msg = Encoding.UTF8.GetBytes(sb.ToString());
                                    Stream.Write(msg, 0, msg.Length);
                                    //break;

                                }
                                else
                                {
                                    Res = "사용불가능";
                                    sb.Append(Res);
                                    msg = Encoding.UTF8.GetBytes(sb.ToString());
                                    Stream.Write(msg, 0, msg.Length);

                                    // break;
                                }


                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                            }

                        }
                        else if (a == "e") // 강제종료했을 시
                        {
                            break;
                        }
                        else if (a == "f")
                        {
                            
                            string id = parts[1];
                            Database database = new Database();
                            byte[] msg;
                            StringBuilder sb = new StringBuilder("f ");
                            database.CheckUser(client);
                            foreach(KeyValuePair<TcpClient,string> k in Program.count1)
                            { 
                                sb.Append(k.Value);
                                sb.Append(' ');
                                Console.WriteLine("아이디" + k.Value); //로그
                            }
                            
                            msg = Encoding.UTF8.GetBytes(sb.ToString());
                            Stream.Write(msg, 0, msg.Length);

                        }
                        else if (a == "l") // 로그인시
                        {
                            string Res = null; 
                            byte[] msg;
                            StringBuilder sb = new StringBuilder("l ");
                           
                            try
                            {
                                string id = parts[1];
                                string pass = parts[2];                                
                                Database database = new Database();
                                if (database.Login(id, pass) == true)
                                {
                                    database.OnlineState(id); 
                                    Res = "환영합니다.";
                                    sb.Append(Res);
                                    msg = Encoding.UTF8.GetBytes(sb.ToString());
                                    Stream.Write(msg, 0, msg.Length); 
                                }
                                else
                                {
                                    Res = "아이디/비밀번호오류입니다";
                                    sb.Append(Res);
                                    msg = Encoding.UTF8.GetBytes(sb.ToString());
                                    Stream.Write(msg, 0, msg.Length);
                                }
                            }
                            catch (Exception)
                            {

                            }
                        }
                        else if (a == "x")
                        {
                            string id = parts[1];
                            Database database = new Database();
                          //  Program.count.Remove(Id); // 현재접속자가 종료했을 시 온라인 삭제
                            Program.count1.Remove(client); // 현재접속자가 종료했을 시 온라인 삭제
                            database.OfflineState(client,id);
                            break;
                        }
                    }
                     
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    break;
                }

            }
            server.RemoveConnection(this);
            Close();
        }


        private string GetMessage()
        {
            byte[] data = new byte[1024];
            StringBuilder builder = new StringBuilder();
            int bytes = 0;
            do
            {
                bytes = Stream.Read(data, 0, data.Length);
                builder.Append(Encoding.UTF8.GetString(data, 0, bytes));
            }
            while (Stream.DataAvailable);


            return builder.ToString();
        }

         
        public void Close()
        {
            if (Stream != null)
                Stream.Close();
            if (client != null)
                client.Close();
        }
    }
}
