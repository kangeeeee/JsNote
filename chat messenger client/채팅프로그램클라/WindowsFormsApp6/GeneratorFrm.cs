using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp6
{
    public partial class GeneratorFrm : Form
    {
        TcpClient tcp = new TcpClient();
       
        private string Generationmsg;
        public GeneratorFrm(string _msg)
        {
            InitializeComponent();
            Generationmsg = _msg;
            try
            {
                tcp.Connect(Program.ip, Program.port);
                StringBuilder sb = new StringBuilder("a");

                aGenerationPacket agenerationPacket = new aGenerationPacket();
                agenerationPacket.SetData(Generationmsg);
                Send(agenerationPacket.GetData);
            }
            catch (Exception)
            {
                MessageBox.Show("서버 점검 중");
                Application.Exit();
            }           
        }      

        private void button1_Click(object sender, EventArgs e) // 인증 전송
        {
            try
            {
                bGenerationPacket bgenerationPacket = new bGenerationPacket();
                bgenerationPacket.SetData(genertxt.Text);
                Send(bgenerationPacket.GetData);
                Socket sock = tcp.Client;

                byte[] data = new byte[1024];
                sock.Receive(data);
                if (data.Length > 0)
                {
                    string res = Encoding.UTF8.GetString(data);
                    res = res.Split(' ')[1];
                    MessageBox.Show(res);
                    if (String.Compare(res, "인증이완료되었습니다.") == 0)
                    {
                        tcp.Close();
                        this.Close();
                    }
                   
                }
            }
            catch (Exception)
            {

            }
        }
        private void Send(byte[] Msg)
        {
            try
            {
                Socket sock = tcp.Client;
                sock.Send(Msg);
            }
            catch(Exception)
            {
                
            }
        }

        private void GeneratorFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                eGenerationPacket egenerationPacket = new eGenerationPacket();
                egenerationPacket.SetData("e");
                Send(egenerationPacket.GetData);
                tcp.Close();
            }
            catch (Exception)
            {

            }
        }
    }
}
