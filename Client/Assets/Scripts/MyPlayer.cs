using ServerCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlayer : Player
{
    NetworkManager _network;
    void Start()
    {
        StartCoroutine("CoSendPacket");
        _network = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
    }

    void Update()
    {

    }
    IEnumerator CoSendPacket()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.25f);

            C_Move movePacket = new C_Move();
            movePacket.posX = UnityEngine.Random.Range(-10, 10);
            movePacket.posY = 1;
            movePacket.posZ = UnityEngine.Random.Range(-10, 10);
            _network.Send(movePacket.Write());
        }
    }
}
