using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp4
{
    public class Database
    {
         
        public Database()
        {

        }
        public void CheckUser(TcpClient tcp)
        { 
            using (MySqlConnection conn = new MySqlConnection(Program.connection))
            { 
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("select nickname from chat where state='Online'", conn))
                {
                    MySqlDataReader sqlDataReader = cmd.ExecuteReader();
                    while (sqlDataReader.Read())
                    {
                        if (!Program.count1.ContainsValue(sqlDataReader["nickname"].ToString()))
                        {
                            Program.count1.Add(tcp, sqlDataReader["nickname"].ToString());
                        }
                        else // 중복로그인시 소켓연결끊어버림
                        {
                            tcp.Close();
                        }
                    }
                }
            } 
        }
        public void Create(string nick, string id, string pass, string email)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Program.connection))
                {
                    conn.Open(); 
                    MySqlCommand cmd = new MySqlCommand("INSERT INTO chat(nickname,id,password,email) VALUES('" + nick + "','" + id + "','" + pass + "','" + email + "') ", conn);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }
        public Boolean CheckID(string id)
        {
            using (MySqlConnection conn = new MySqlConnection(Program.connection))
            {

                conn.Open();
                try
                {
                    MySqlCommand cmd = new MySqlCommand("select id from chat where id='" + id + "'", conn);
                    MySqlDataReader sqlDataReader = cmd.ExecuteReader();
                    if (sqlDataReader.HasRows)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }

                }
                catch (Exception)
                {
                    return false;

                }
            }

        }
        public Boolean Login(string id, string pass)
        {

            using (MySqlConnection conn = new MySqlConnection(Program.connection))
            {

                conn.Open(); 
                    MySqlCommand cmd = new MySqlCommand("select id,password from chat where id='" + id + "' and password='" + pass + "'", conn);
                    MySqlDataReader sqlDataReader = cmd.ExecuteReader();
                    if (sqlDataReader.HasRows)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                 
                 
            }
        }
        public void OnlineState(string id)
        {
            using (MySqlConnection conn = new MySqlConnection(Program.connection))
            {

                conn.Open();
                try
                {
                    MySqlCommand cmd = new MySqlCommand("update chat set state='Online' where id='"+id+"'", conn);
                    cmd.ExecuteNonQuery();
                   

                }
                catch (Exception)
                {
                    

                }
            }

        }
        public void OfflineState(TcpClient tcpClient,string id)
        {
            using (MySqlConnection conn = new MySqlConnection(Program.connection))
            {

                conn.Open();
                try
                {
                    MySqlCommand cmd = new MySqlCommand("update chat set state='Offline' where id='" + id + "'", conn);
                    Program.count1.Remove(tcpClient);
                    cmd.ExecuteNonQuery(); 
                }
                catch (Exception)
                {


                }
            }
        }
    }
}
