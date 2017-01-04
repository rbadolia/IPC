using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AutomationAnywhere.Ipc.Common;
using Microsoft.Practices.ServiceLocation;

namespace AutomationAnywhere.Ipc.Receiver
{
    public partial class ReceiverWindow : Form
    {
        private readonly IEnumerable<IIpcServer> _servers;
        public ReceiverWindow()
        {
            _servers = ServiceLocator.Current.GetAllInstances<IIpcServer>();
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            foreach (var server in _servers)
            {
                server.Received += Server_Received;
                server.Start();
            }
        }

        private void Server_Received(object sender, DataReceivedEventArgs e)
        {
            lstMessages.Items.Add(e.Data);
            lstMessages.SelectedIndex = lstMessages.Items.Count - 1;
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (var server in _servers)
            {
                server.Stop();
            }
        }
    }
}
