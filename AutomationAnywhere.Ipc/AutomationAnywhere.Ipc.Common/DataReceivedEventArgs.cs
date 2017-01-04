using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationAnywhere.Ipc.Common
{

    [Serializable]
    public sealed class DataReceivedEventArgs : EventArgs
    {
        public DataReceivedEventArgs(string data)
        {
            this.Data = data;
        }

        public string Data { get; private set; }
    }
}
