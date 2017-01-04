using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationAnywhere.Ipc.Common
{
    public interface IIpcServer : IDisposable
    {
        void Start();
        void Stop();
        event EventHandler<DataReceivedEventArgs> Received;
    }
}
