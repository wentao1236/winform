using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Threading;


namespace chatonline
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }
        private void btnSend_Click(object sender, EventArgs e)
        {
            string ip = this.tXtIP.Text;
            UdpClient uc = new UdpClient();
            string msg = "PUBLIC|"+this.txtMsg.Text+"|周大锤";//内部为内容
            byte[] bmsg = Encoding.Default.GetBytes(msg);
            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(ip), 9527);
            uc.Send(bmsg,bmsg.Length,ipep);
        }

        private void listen()
        {

            UdpClient uc = new UdpClient(9527);
            while (true)
            {
                IPEndPoint ipep = new IPEndPoint(IPAddress.Any,0);
                byte[] bmsg = uc.Receive(ref ipep);
                string msg = Encoding.Default.GetString(bmsg);
                string[] s = msg.Split('|');
                if (s[0] == "PUBLIC")
                {
                   this.txtHistory.Text += s[2] +"："+ "\r\n";
                   this.txtHistory.Text += s[1] + "\r\n";

                }
                else if (s[0] == "INROOM")
                {
                    this.txtHistory.Text += s[1] + "登录了" + "\r\n";
                }
                else
                {
                    return;
                }
               

            }
        }
        private void FrmMain_Load(object sender, EventArgs e)
        {
            string ip = this.tXtIP.Text;
            UdpClient uc = new UdpClient();
            string msg = "INROOM|周大锤";//发送给别人的内容
            byte[] bmsg = Encoding.Default.GetBytes(msg);
            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(ip), 9527);
            uc.Send(bmsg, bmsg.Length, ipep);
            Thread th = new Thread(new ThreadStart(listen));
            th.Start();
        }
    }
}
