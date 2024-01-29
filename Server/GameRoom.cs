using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Session;

namespace Server
{
    internal class GameRoom
    {
        /*
         * 리스트, 딕셔너리와 같은 자료형은
         * 멀티쓰레드 환경에서 문제가 발생할 확률이 높기 때문에,
         * lock을 사용하여 동기화 한다.
         */
        List<ClientSession> _sessions = new List<ClientSession>();
        object _lock = new object();

        public void Broadcast(ClientSession session, string chat)
        {
            S_Chat packet = new S_Chat();
            packet.playerId = session.SessionID;
            packet.chat = $"{chat} I am {packet.playerId}";
            ArraySegment<byte> segment = packet.Write();

            // 공유하는 변수(sessions)를 다루면 무조건 lock
            /*
             * 아래와 같이 lock을 잡으면, 다수의 상호작용을 처리할 때
             * 처리가 밀리는 현상이 발생한다.
             * 이를 처리하기 위해서는, 각각의 세션들이 Broadcast를 호출하는 것이 아니라
             * 이 게임룸을 관리하는 것은 단 하나의 쓰레드이고, 세션들은 작업을 큐에 넣어두고 가는 방식으로 처리해야한다.
             * 이 큐를 JobQueue 라고 부른다.
             */
            lock (_lock)
            {
                foreach (ClientSession s in _sessions)
                {
                    s.Send(segment);
                }
            }
        }

        public void Enter(ClientSession session)
        {
            lock (_lock)
            {
                _sessions.Add(session);
                session.Room = this;
            }
            
        }
        public void Leave(ClientSession session)
        {
            lock (_lock)
            { 
                _sessions.Remove(session);
            }
        }
    }
}
