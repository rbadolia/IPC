using System;
using System.IO.MemoryMappedFiles;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutomationAnywhere.Ipc.Common.IpcServers
{
    public sealed class SharedMemoryServer : IIpcServer
    {
        private readonly ManualResetEvent killer = new ManualResetEvent(false);
        private const int capacity = 1024;

        private void OnReceived(DataReceivedEventArgs e)
        {
            var handler = this.Received;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        public event EventHandler<DataReceivedEventArgs> Received;

        public void Start()
        {
            Task.Factory.StartNew(() =>
            {
                var evt = null as EventWaitHandle;

                if (EventWaitHandle.TryOpenExisting(typeof(IIpcClient).Name, out evt) == false)
                {
                    evt = new EventWaitHandle(false, EventResetMode.AutoReset, typeof(IIpcClient).Name);
                }

                using (evt)
                using (var file = MemoryMappedFile.CreateOrOpen(typeof(IIpcClient).Name + "File", capacity))
                using (var view = file.CreateViewAccessor())
                {
                    var data = new byte[capacity];

                    while (WaitHandle.WaitAny(new WaitHandle[] { this.killer, evt }) == 1)
                    {
                        view.ReadArray(0, data, 0, data.Length);

                        this.OnReceived(new DataReceivedEventArgs(Encoding.Default.GetString(data)));
                    }
                }
            });
        }

        public void Stop()
        {
            this.killer.Set();
        }

        void IDisposable.Dispose()
        {
            this.Stop();

            this.killer.Dispose();
        }
    }
}
