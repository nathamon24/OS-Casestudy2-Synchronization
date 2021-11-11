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
        static object _Lock = new object();
        static string time = "0";

        static void EnQueue(int eq)
        {
            TSBuffer[Back] = eq;
            Back++;
            Back %= 10;
            Count += 1;
        }

        static int DeQueue()
        {
            int x = 0;
            x = TSBuffer[Front];
            Front++;
            Front %= 10;
            Count -= 1;
            return x;
        }

        static void th01()
        {
            int i;
            for (i = 1; i < 51; i++)
            {   
                lock (_Lock){
                    while (Count==10)
                    {
                        Console.WriteLine("<----------Full---------->");
                        Monitor.Wait(_Lock);
                    }
                    EnQueue(i);
                    Thread.Sleep(5);
                    Monitor.Pulse(_Lock);
                }  
            }
        }

        static void th011()
        {
            int i;
            for (i = 100; i < 151; i++)
            {   
                lock (_Lock)
                {
                    while (Count==10)
                    {
                        Console.WriteLine("<----------Full---------->");
                        Monitor.Wait(_Lock);    
                    }
                    EnQueue(i);
                    Thread.Sleep(5);
                    Monitor.Pulse(_Lock);
                }
            }
        }

        static void th02(object t)
        {
            int i;
            int j;
          
             for (i=0; i < 60; i++)
            {   
                lock (_Lock)
                {
                    while (Count==0)
                    {
                        Console.WriteLine("<----------Empty---------->");
                        Monitor.Wait(_Lock);
                    }
                    j = DeQueue();
                    Console.WriteLine("j={0}, thread:{1}", j, t);
                    Thread.Sleep(100); 
                    Monitor.Pulse(_Lock);
                }
                
            }
           
        }
        
        static void Main(string[] args)
        {
            Thread t1 = new Thread(th01);
            Thread t11 = new Thread(th011);
            Thread t2 = new Thread(th02);
            Thread t21 = new Thread(th02);
            Thread t22 = new Thread(th02);
            
            t1.Start();
            t11.Start();
            t2.Start(1);
            t21.Start(2);
            t22.Start(3);
        }
    }
}
