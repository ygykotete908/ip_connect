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
using System.Windows.Forms;

namespace ServerTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private readonly ManualResetEvent TimeoutObject = new ManualResetEvent(false);
        //--异步回调方法       
        private void CallBackMethod(IAsyncResult asyncresult)
        {
            //使阻塞的线程继续        
            TimeoutObject.Set();
        }
        private bool AddressPort(string ipAddress, int portNum)
        {
            try
            {
                IPAddress ip = IPAddress.Parse(ipAddress);
                IPEndPoint point = new IPEndPoint(ip, portNum);
                using (Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    sock.BeginConnect(point, CallBackMethod, sock);
                    //阻塞当前线程           
                    if (TimeoutObject.WaitOne(1000, false))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine(e.Message);
            }
            return false;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            string ip = "192.168.12.58";
            int port = 2090;
            Console.WriteLine(AddressPort(ip, port));
        }
    }
}
