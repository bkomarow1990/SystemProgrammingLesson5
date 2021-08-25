using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Task1_
{
    class Program
    {


        static void Main(string[] args)
        {
            bool createdNew;

            Mutex m = new Mutex(true, "App", out createdNew);

            if (!createdNew)
            {
                // myApp is already running...
                Console.WriteLine("App is already running!");
                Console.ReadKey();
                return;
            }
            Console.ReadKey();
        }
    }
}
