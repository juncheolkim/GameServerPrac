using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Session;
using ServerCore;

namespace Server
{
    internal class GameRoom : IJobQueue
    {
        /*
         * 리스트, 딕셔너리와 같은 자료형은
         * 멀티쓰레드 환경에서 문제가 발생할 확률이 높기 때문에,
         * lock을 사용하여 동기화 한다.
         */
        List<ClientSession> _sessions = new List<ClientSession>();
        object _lock = new object();

        // 각각의 행동들을 jobQueue에 밀어넣어준다.
        JobQueue _jobQueue = new();

        List<ArraySegment<byte>> _pendingList = new List<ArraySegment<byte>>();

        public void Push(Action job)
        {
            _jobQueue.Push(job);
        }

        public void Flush()
        {
            foreach (ClientSession s in _sessions)
                s.Send(_pendingList);

            Console.WriteLine($"Flushed {_pendingList.Count} items");
            _pendingList.Clear();
        }

        // jobQueue가 Broadcast, Enter, Leave를 단일로 실행시키기 때문에
        // lock을 해당 함수 내부에서 사용할 이유가 없다.
        public void Broadcast(ClientSession session, string chat)
        {
            S_Chat packet = new S_Chat();
            packet.playerId = session.SessionID;
            packet.chat = $"{chat} I am {packet.playerId}";
            ArraySegment<byte> segment = packet.Write();

            // 클라이언트 단에서 패킷 모아 보내기
            _pendingList.Add(segment);

        }
        public void Enter(ClientSession session)
        {
            _sessions.Add(session);
            session.Room = this;
        }
        public void Leave(ClientSession session)
        {
            _sessions.Remove(session);
        }
    }
}
