using System;
using System.Threading;
//First Check your buffer 
//Second Check state can add queue or not queue
//Third Check state can pop queue or not pop
//IF State == 1 (Full) will Dequeue until state == 0 !2 !0
//IF State == 0 (Empty) Cannot Dequeue until state == 1 !1 !2
//IF State == 2 (Have object) will continue to add queue and can dequeue !1 !0

namespace OS_Problem_02
{
    class Thread_safe_buffer
    {
        static int[] TSBuffer = new int[10];
        static int Front = 0;
        static int Back = 0;
        static int Count = 0;

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
                if(Count != 10)
                {
                    EnQueue(i);
                    Thread.Sleep(5);
                }
               
            }
        }

        static void th011()
        {
            int i;
            
            for (i = 100; i < 151; i++)
            {
                if(Count != 10)
                {
                    EnQueue(i);
                    Thread.Sleep(5);
                }
               
                
            }
        }

        static void th02(object t)
        {
            int i;
            int j;
          
            for (i=0; i < 60; i++)
            {       
                if(Count != 0){
                    j = DeQueue();
                    Console.WriteLine("j={0}, thread:{1}", j, t);
                    Thread.Sleep(100); 
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
