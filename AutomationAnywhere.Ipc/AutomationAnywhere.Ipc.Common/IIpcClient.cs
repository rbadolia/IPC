using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationAnywhere.Ipc.Common
{
    public interface IIpcClient
    {
        void Send(string data);
    }
}
