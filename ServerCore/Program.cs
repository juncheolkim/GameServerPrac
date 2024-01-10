namespace ServerCore
{
    internal class Program
    {
        // 메모리 베리어
        // 1. 코드의 재배치 억제
        // 2. 가시성

        // 1. Full Memory Barrier (ASM MFENCE, C# Thread.MemoryBarrier)
        // 2. Store Memory Barrier (ASM SFENCE) : Store만 막는다
        // 3. Load Memory Barrier (ASM LFENCE) : Load만 막는다

        static int x = 0;
        static int y = 0;
        static int r1 = 0;
        static int r2 = 0;

        static void Thread_1()
        {
            y = 1;  // Store y
            Thread.MemoryBarrier(); // 이 시점에 메모리에 y 값을 갱신한다. (가시성)
            r1 = x; // Load x
        }
        static void Thread_2()
        {
            x = 1;  // Store x
            Thread.MemoryBarrier(); // 이 시점에 메모리에 x 값을 갱신한다. (가시성)
            r2 = y; // Load y
        }

        static void Main(string[] args)
        {
            int count = 0;
            while (true)
            {
                count++;
                x = y = r1 = r2 = 0;
                Task t1 = new Task(Thread_1);
                Task t2 = new Task(Thread_2);
                t1.Start();
                t2.Start();

                Task.WaitAll(t1, t2);

                if (r1 == 0 && r2 == 0)
                    break;
            }

            Console.WriteLine($"{count}번만에 빠져나옴!");
        }
    }
}