using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Task3
{
    class Program
    {
        static Random rnd = new Random();
        static void Generate5Pairs(object obj) {
            EventWaitHandle ev = obj as EventWaitHandle;
            int[] arr = new int[10];
            for (int i = 0; i < 10; i++)
            {
                arr[i] = rnd.Next(0, 1200);
            }
            File.WriteAllText("pairs.dat",String.Join(" ", arr));
            ev.Set();
            //using (FileStream fs = new FileStream("pairs.dat", FileMode.OpenOrCreate))
            //{
            //    fs.Write();
            //}
        }
        static void WriteSum(object obj) {
            EventWaitHandle ev = obj as EventWaitHandle;
            ev.WaitOne();
            int sum = 0;
            string[] nums = File.ReadAllText("pairs.dat").Split(' ');
            for (int i = 0; i < nums.Length; i++)
            {
                sum += Int32.Parse(nums[i]);
            }
            File.WriteAllText("sum.dat",$"Summ: {sum}");
            ev.Set();
        }
        static void WriteMult(object obj)
        {
            EventWaitHandle ev = obj as EventWaitHandle;
            ev.WaitOne();
            string[] nums = File.ReadAllText("pairs.dat").Split(' ');
            int[] mult = new int[nums.Length/2];
            for (int i = 0; i < mult.Length; i++)
            {
               mult[i] = Int32.Parse(nums[i]) * Int32.Parse(nums[i+1]) ;
               ++i;
            }
            File.WriteAllText("mult.dat", $"mult: {String.Join(" ",mult)}");
            ev.Set();
        }

        static void Main(string[] args)
        {
            EventWaitHandle ev = new EventWaitHandle(false, EventResetMode.ManualReset);
            //Thread[] thread = new Thread[3];
            //thread[0] = new Thread(Generate5Pairs);
            //thread[1] = new Thread(Generate5Pairs);
            //thread[2] = new Thread(Generate5Pairs);
            ThreadPool.QueueUserWorkItem(Generate5Pairs, ev);
            ThreadPool.QueueUserWorkItem(WriteSum, ev);
            ThreadPool.QueueUserWorkItem(WriteMult, ev);
            Console.ReadKey();
        }
    }
}
