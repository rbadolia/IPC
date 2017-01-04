using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AutomationAnywhere.Ipc.Common.IpcServers
{
    public sealed class SocketServer : IIpcServer
    {
        private readonly UdpClient _server = new UdpClient(9000);

        void IDisposable.Dispose()
        {
            this.Stop();

            (this._server as IDisposable).Dispose();
        }

        public void Start()
        {
            Task.Factory.StartNew(() =>
            {
                var ip = new IPEndPoint(IPAddress.Any, 0);

                while (true)
                {
                    var bytes = this._server.Receive(ref ip);
                    var data = Encoding.Default.GetString(bytes);
                    this.OnReceived(new DataReceivedEventArgs(data));
                }
            });
        }

        private void OnReceived(DataReceivedEventArgs e)
        {
            var handler = this.Received;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        public void Stop()
        {
            this._server.Close();
        }

        public event EventHandler<DataReceivedEventArgs> Received;
    }
}
