using System.IO.MemoryMappedFiles;
using System.Text;
using System.Threading;

namespace AutomationAnywhere.Ipc.Common.IpcClients
{
    public class SharedMemoryClient : IIpcClient
    {
        public void Send(string data)
        {
            if (data == null)
                return;

            var evt = null as EventWaitHandle;

            if (EventWaitHandle.TryOpenExisting(typeof(IIpcClient).Name, out evt) == false)
            {
                evt = new EventWaitHandle(false, EventResetMode.AutoReset, typeof(IIpcClient).Name);
            }

            using (evt)
            using (var file = MemoryMappedFile.CreateOrOpen(typeof(IIpcClient).Name + "File", 1024))
            using (var view = file.CreateViewAccessor())
            {
                var bytes = Encoding.Default.GetBytes(data);

                view.WriteArray(0, bytes, 0, bytes.Length);

                evt.Set();
            }
        }
    }
}
