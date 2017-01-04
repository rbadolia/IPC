using System;
using System.Threading;
using AutomationAnywhere.Ipc.Common.IpcClients;
using AutomationAnywhere.Ipc.Common.IpcServers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutomationAnywhere.Ipc.Tests
{
    [TestClass]
    public class IpcTests
    {
        ManualResetEvent flag = new ManualResetEvent(false);

        [TestMethod]
        public void CopyDataIpcTest()
        {
            Thread t1 = new Thread(RunServer);
            t1.Start();

            Thread t2 = new Thread(RunClient);
            t2.Start();
            flag.WaitOne();            
        }

        private void RunServer()
        {
            var server = new CopyDataServer();
            server.Received += (sender, e) =>
            {
                Assert.AreEqual(e.Data, "TestCopyDataIpc");
                flag.Set();
            };
            server.Start();
        }

        private void RunClient()
        {
            CopyDataClient client = new CopyDataClient();
            client.Send("TestCopyDataIpc");
        }

        [TestMethod]
        public void SharedMemoryIpcTest()
        {
        }

        [TestMethod]
        public void SocketIpcTest()
        {
        }
    }
}
