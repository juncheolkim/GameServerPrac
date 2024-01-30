﻿using Server;
using Server.Session;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class PacketHandler
{
    public static void C_ChatHandler(PacketSession session, IPacket packet)
    {
        C_Chat chatPacket = packet as C_Chat;
        ClientSession clientSession = session as ClientSession;

        if (clientSession == null)
        {
            return;
        }

        // 해야하는 행동 자체를 push
        GameRoom room = clientSession.Room;
        if(room != null)
            room.Push(
                ()=> room.Broadcast(clientSession, chatPacket.chat)
            );
    }
}

