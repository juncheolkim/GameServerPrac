namespace ServerCore
{
    class Lock
    {
        // bool <- 커널이 관리하는 변수
        AutoResetEvent _available = new AutoResetEvent(true);

        public void Acquire()
        {
            _available.WaitOne();   // 입장 시도 -> 입장하면 자동으로 문 닫는다
            //_available.Reset(); // bool = false , 이 작업을 WaitOne이 알아서 한다.
        }

        public void Release()
        {
            _available.Set();   // flag = true
        }
    }
    internal class Program
    {
        static int _num = 0;
        static Lock _lock = new Lock();
        static void Thread_1()
        {
            for (int i = 0; i < 10000; i++)
            {
                _lock.Acquire();
                _num++;
                _lock.Release();
            }
        }

        static void Thread_2()
        {
            for (int i = 0; i < 10000; i++)
            {
                _lock.Acquire();
                _num--;
                _lock.Release();
            }
        }

        static void Main(string[] args)
        {
            Task t1 = new Task(Thread_1);
            Task t2 = new Task(Thread_2);
            t1.Start();
            t2.Start();
            Task.WaitAll(t1, t2);
            Console.WriteLine(_num);

        }
    }
}