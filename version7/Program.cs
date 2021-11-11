using System;
using System.Diagnostics;
using System.Threading;

namespace OS_Problem_02
{
    class Thread_safe_buffer
    {
        static int[] TSBuffer = new int[10];
        static int Front = 0;
        static int Back = 0;
        static int Count = 0;
        static object en_Lock = new object(); //Lock for EnQueue
        static object de_Lock = new object(); //Lock for DeQueue
        static object th02_Lock = new object(); //Lock for multi-thread
        static bool end_th01 = false ;
        static bool end_th011 = false ;
        static Boolean endTask = false;
        static string time = "0";

        static void EnQueue(int eq)
        {      
            lock (en_Lock)
            {
                if (endTask)
                    return;
                while (Count >= 10);
                TSBuffer[Back] = eq;
                Count += 1;
                Back++;
                Back %= 10;
            }
        }

        static int DeQueue()
        {   
            lock (de_Lock) 
            {
                if (endTask)
                    return -1;
                while (Count <= 0);
                
                int x = 0;
                x = TSBuffer[Front];
                Front++;
                Front %= 10;
                Count -= 1;
                return x;
            }
        }

        static void th01()
        {
            int i;

            for (i = 1; i < 51; i++)
            {   
                if (endTask) 
                    return;
                EnQueue(i);
                if (i == 50)
                    end_th01 = true;
                Thread.Sleep(5);
                
            }
        }

        static void th011()
        {
            int i;

            for (i = 100; i < 151; i++)
            {
                if (endTask)
                    return;
                EnQueue(i);
                if (i == 150)
                    end_th011 = true; 
                Thread.Sleep(5);
                
            }
        }

        static void th02(object t)
        {
            int i;
            int j;

            for (i=0; i < 60; i++)
            {
                lock (th02_Lock) 
                {
                    j = DeQueue();
                    if (j == -1)
                        return;
                    Console.WriteLine("j={0}, thread:{1}", j, t);
                }
                
                Thread.Sleep(100);
                if (endTask)
                    return;
            }
        }
        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();

            Thread t1 = new Thread(th01);
            Thread t11 = new Thread(th011);
            Thread t2 = new Thread(th02);
            Thread t21 = new Thread(th02);
            Thread t22 = new Thread(th02);

            sw.Start();
            t1.Start();
            t11.Start();
            t2.Start(1);
            t21.Start(2);
            t22.Start(3);

            while (!(end_th01 == true && end_th011 == true && Count <= 0)); //Check for endTask
            sw.Stop();
            endTask = true;
            time = sw.ElapsedMilliseconds.ToString();
            Console.WriteLine("Done.");
            Console.WriteLine("Time used: " + time + "ms");
        }
    }
}