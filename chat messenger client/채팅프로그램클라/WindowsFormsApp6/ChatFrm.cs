using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
delegate void listviewadd(string id);
delegate void refreshlistview();
namespace WindowsFormsApp6
{


    public partial class ChatFrm : Form
    {
        
        TcpClient tcp;
        public ChatFrm()
        {
            InitializeComponent(); 
        }
        public ChatFrm(Form1 form1)
        {
            InitializeComponent();
            this.tcp = form1.tcp;
            Thread t1 = new Thread(new ThreadStart(Recv)); 
            t1.Start();
        }
        NetworkStream Stream;
        private void Recv()
        {
            Socket sock = tcp.Client;
             
            byte[] data = new byte[1024];
            fGenerationPacket fgenerationPacket = new fGenerationPacket(); 
            Send(fgenerationPacket.GetData);
            Stream = tcp.GetStream();
            string message;
           
            do
            {
                message = GetMessage();

                var parts = message.Split(' ');
                string a = Convert.ToString(parts[0]);
                if (a == "f")
                { 
                    for (int i = 1; i < parts.Length; i++)
                    {
                        Addlist(parts[i]);
                    }
                }

            } while (Stream.DataAvailable);
        }
        public void Refreshlistview() // test
        {
            if (InvokeRequired)
            {
                Invoke(new refreshlistview(Refreshlistview));
            }
            else
            {
                listView1.Refresh();
            }
        }
        public void Addlist(string id)
        {
            if (InvokeRequired)
            {
                Invoke(new listviewadd(Addlist), id);
            }
            else
            {
                listView1.Items.Add(id);
                 
            }
        }
        private void ChatFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void ChatFrm_Load(object sender, EventArgs e)
        {


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

        private void button1_Click(object sender, EventArgs e)
        {
            Refreshlistview(); // 테스트용
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            Talk talk = new Talk();
            talk.Show();
        }
    }
}
