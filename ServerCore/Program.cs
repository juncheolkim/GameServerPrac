namespace ServerCore
{
    internal class Program
    {
        static void MainThread(object state)
        {
            for (int i = 0; i < 5; i++)
                Console.WriteLine("Hello, Thread!");
        }
        static void Main(string[] args)
        {
            ThreadPool.SetMinThreads(1, 1);
            ThreadPool.SetMaxThreads(5, 5);


            for (int i = 0; i < 5; i++)
            {
                Task t = new Task(() => { while (true) { } }, TaskCreationOptions.LongRunning); // 쓰레드가 할 일감
                // TaskCreationOptions.LongRunning 옵션은 워커 쓰레드 풀에서 뽑아서 실행하는 것이 아닌, 별도 실행을 한다.
                t.Start();
            }

            for (int i = 0;i < 5; i++)
               ThreadPool.QueueUserWorkItem((obj) => { while(true) { } });
            ThreadPool.QueueUserWorkItem(MainThread);
            /*
            new Thread와 ThreadPool의 차이?
            new Thread의 모든 책임은 우리가 맡는다.
            -> 필요할 때 마다 생성
            ThreadPool은 이미 다 마련되어있는 쓰레드 사용.
            -> 이미 대기중인 쓰레드 사용 및 반환
            -> 쓰레드 풀링이라고 한다.
            -> 가급적이면 짧은 일감을 주는 것이 좋다. 남는 쓰레드가 없는 경우가 발생할 수 있기 때문에.
             */

            //Thread t = new Thread(MainThread);  // 쓰레드가 실행할 메인 함수 인자로 넣어준다.
            //t.Name = "Test Thread"; // 쓰레드 이름 명명
            //t.IsBackground = true;  // 백그라운드 쓰레드 설정
            //t.Start();  // 쓰레드 실행
            //Console.WriteLine("Waiting for Thread!");
            //t.Join();   // 쓰레드 작업이 끝날 때까지 대기
            //Console.WriteLine("Hello, World!");
        }
    }
}