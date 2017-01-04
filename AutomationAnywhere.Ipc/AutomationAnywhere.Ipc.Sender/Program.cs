using System;
using AutomationAnywhere.Ipc.Common;
using Microsoft.Practices.ServiceLocation;

namespace AutomationAnywhere.Ipc.Sender
{
    class Program
    {      
        static void Main(string[] args)
        {
            Bootstrapper.Init();

            Console.WriteLine("Fill details and press ENTER to send (quit to terminate): ");

            while (true)
            {
                Console.WriteLine("Please type person name...");
                var name = Console.ReadLine();
                if (name == "quit")
                {
                    break;
                }
                while (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
                {
                    Console.WriteLine("Please type person name...");
                    name = Console.ReadLine();
                }

                Console.WriteLine("Please type text to send...");
                var text = Console.ReadLine();
                if (text == "quit")
                {
                    break;
                }
                while (string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text))
                {
                    Console.WriteLine("Please type text to send...");
                    text = Console.ReadLine();
                }

                Console.WriteLine("Please type sending strategy (smc/sc/cdc)...");
                var strategy = Console.ReadLine();
                if (strategy == "quit")
                {
                    break;
                }
                while (string.IsNullOrEmpty(strategy) || string.IsNullOrWhiteSpace(strategy) && (strategy != "smc" || strategy != "sc" || strategy != "cdc"))
                {
                    Console.WriteLine("Please type sending strategy (msg/soc/mem)...");
                    strategy = Console.ReadLine();
                }

                var iIpcClient = ServiceLocator.Current.GetInstance<IIpcClient>(strategy);
                iIpcClient.Send(name + " : " + text);
                var quit = Console.ReadLine();
                if (quit == "quit")
                {
                    break;
                }
            }
        }
    }
}
