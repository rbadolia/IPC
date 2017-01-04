using System.Net.Sockets;
using System.Text;

namespace AutomationAnywhere.Ipc.Common.IpcClients
{
    public class SocketClient : IIpcClient
    {
        public void Send(string data)
        {
            if (data == null)
                return;

            using (var client = new UdpClient())
            {
                client.Connect(string.Empty, 9000);

                var bytes = Encoding.Default.GetBytes(data);

                client.Send(bytes, bytes.Length);
            }
        }
    }
}
