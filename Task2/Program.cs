using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Task2
{
    class Program
    {
        static Random rnd = new Random();
        static void Random10Numbers(object obj) {
            bool stop = false;
            Semaphore s = obj as Semaphore;
            while (!stop)
            {
                if (s.WaitOne(500))
                {
                    try
                    {
                        Console.WriteLine("Thread {0} got a lock", Thread.CurrentThread.ManagedThreadId);
                        Thread.Sleep(2000);
                    }
                    finally
                    {
                        stop = true;
                        Console.WriteLine("Thread {0} remove a lock", Thread.CurrentThread.ManagedThreadId);
                        s.Release();
                    }
                }
                else
                    Console.WriteLine("Timeout for thread {0} expired. Re-waiting...", Thread.CurrentThread.ManagedThreadId);
            }
        }
        static void Main(string[] args)
        {
            //Semaphore s = new Semaphore(3, 3);
            //Thread[] thread = new Thread[10];
            //for (int i = 0; i < 10; i++)
            //{
            //    thread[i] = new Thread(Random10Numbers);
            //}
            Semaphore s = new Semaphore(3, 3);

            for (int i = 0; i < 10; ++i)
                ThreadPool.QueueUserWorkItem(Random10Numbers, s);

            Console.ReadKey();
        }
    }
}
