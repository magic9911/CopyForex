using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace testUMD {
    public partial class Form1 : Form {
        System.Net.Sockets.TcpClient clientSocket = new System.Net.Sockets.TcpClient();
        NetworkStream serverStream;

        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            msg("Client Started");
            clientSocket.Connect("127.0.0.1", 9000);
            label1.Text = "Client Socket Program - Server Connected ...";
        }

        public void msg(string mesg)
        {
            textBox1.Text = textBox1.Text + Environment.NewLine + " >> " + mesg;
        }

        public string SendData(string data)
        {
            NetworkStream serverStream = clientSocket.GetStream();
            byte[] outStream = System.Text.Encoding.ASCII.GetBytes(data);
            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();

            byte[] inStream = new byte[10025];
            serverStream.Read(inStream, 0, (int)clientSocket.ReceiveBufferSize);
            string returndata = System.Text.Encoding.ASCII.GetString(inStream);
            msg("Data from Server : " + returndata);
            return returndata;
        }
    }
}
