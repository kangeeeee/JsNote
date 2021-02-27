using System;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApp6
{

    public partial class JoinFrm : Form
    {
       
        

        public JoinFrm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                TcpClient tcp = new TcpClient();
                tcp.Connect(Program.ip, Program.port);
                StringBuilder sb = new StringBuilder("c");

                cGenerationPacket cgenerationPacket = new cGenerationPacket();
                cgenerationPacket.SetData(nicktxt.Text, cidtxt.Text, cpasstxt.Text, emailtxt.Text);

                byte[] data = new byte[1024];

                if (DataSend(tcp, cgenerationPacket.GetData, ref data)> 0)
                {
                    string res = Encoding.UTF8.GetString(data);
                    res = res.Split(' ')[1];
                    MessageBox.Show(res);
                    if (String.Compare(res, "회원가입성공") == 0)
                    {
                        tcp.Close();
                        tcp.Dispose();
                        this.Close();
                    }
                }
            }
            catch(Exception)
            {

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {           
            GeneratorFrm generatorFrm = new GeneratorFrm(emailtxt.Text);
            generatorFrm.Show(); 
        } 

        private void JoinFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
          
        }

        private int DataSend(TcpClient t, byte[] Msg, ref byte[] recvData)
        {
            try
            {
                Socket sock = t.Client;
                sock.Send(Msg);
                byte[] data = new byte[1024];
                return sock.Receive(recvData);
            }
            catch (Exception)
            {
                return -1;
            }
        }

        private void button3_Click(object sender, EventArgs e) // 아이디 중복 확인
        {
            try
            {
                TcpClient tcp = new TcpClient();
                tcp.Connect(Program.ip, Program.port);
                StringBuilder sb = new StringBuilder("d");

                dGenerationPacket dgenerationPacket = new dGenerationPacket();
                dgenerationPacket.SetData(cidtxt.Text);
                byte[] data = new byte[1024];
                if (DataSend(tcp, dgenerationPacket.GetData, ref data) > 0)
                {
                    string res = Encoding.UTF8.GetString(data);
                    
                        res = res.Split(' ')[1];
                        MessageBox.Show(res);
                     
                    tcp.Close();
                    tcp.Dispose();
                }
            }
            catch (Exception)
            {
                  
            }
        }
        
         
    }
}
