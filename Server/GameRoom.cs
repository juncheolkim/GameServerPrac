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
            packet.chat = chat;
            ArraySegment<byte> segment = packet.Write();

            // 공유하는 변수(sessions)를 다루면 무조건 lock
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
