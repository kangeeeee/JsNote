using System;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApp6
{
    public partial class Form1 : Form
    {
        public TcpClient tcp = new TcpClient();

        public Form1()
        {
            InitializeComponent();
        }

        public void button1_Click(object sender, EventArgs e) // 로그인 버튼
        {
            try
            {
                tcp.Connect(Program.ip, Program.port);
                StringBuilder sb = new StringBuilder("l");

                lGenerationPacket lgenerationPacket = new lGenerationPacket();
                lgenerationPacket.SetData(idtxt.Text,passtxt.Text);
                Send(lgenerationPacket.GetData);
                Socket sock = tcp.Client;

                byte[] data = new byte[1024];
                sock.Receive(data);
                if (data.Length > 0)
                {
                    string res = Encoding.UTF8.GetString(data);
                
                    res = res.Split(' ')[1]; 
                    if (String.Compare(res, "환영합니다.") == 0)
                    {
                          
                        MessageBox.Show(res);
                        ChatFrm chatFrm = new ChatFrm(this);
                        chatFrm.Show();
                        this.Hide();
                     
                    }
                    else
                    {
                        MessageBox.Show(res);
                    } 
                }
            }
            catch (Exception)
            {
                 
            }
        }

        private void button2_Click(object sender, EventArgs e) // 회원가입 버튼
        {
            JoinFrm joinFrm = new JoinFrm();
            joinFrm.Show(); 
        }

        private void Send(byte[] Msg)
        {
            try
            {
                Socket sock = tcp.Client;
                sock.Send(Msg);
            }
            catch (Exception)
            {

            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            xGenerationPacket xgenerationPacket = new xGenerationPacket();
            xgenerationPacket.SetData(idtxt.Text);
            Send(xgenerationPacket.GetData);
            tcp.Close();
        }

       
    }
}
