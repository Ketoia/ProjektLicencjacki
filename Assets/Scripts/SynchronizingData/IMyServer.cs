using Steamworks.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using SaveSystem;

public class IMyServer : Steamworks.ISocketManager
{
    public void OnConnecting(Connection connection, ConnectionInfo data)
    {
        connection.Accept();
        Debug.Log($"{data.Identity.SteamId} is connecting");
    }

    public void OnConnected(Connection connection, ConnectionInfo data)
    {
        Debug.Log($"{data.Identity.SteamId} has joined the game");

        //Todo: Add other information about this player to other players
    }

    public void OnDisconnected(Connection connection, ConnectionInfo data)
    {
        Debug.Log($"{data.Identity.SteamId} is out of here");
    }

    public void OnMessage(Connection connection, NetIdentity identity, IntPtr data, int size, long messageNum, long recvTime, int channel)
    {
        Debug.Log("Got data on server");
        //Todo: send to other players
        //foreach (var item in ServerInstance.instance.Server.Connected)
        //{
        //    if (item != connection)
        //        item.SendMessage(data, size);
        //}

        //receive data
        byte[] managedArray = new byte[size];
        Marshal.Copy(data, managedArray, 0, size);

        SaveData saveData = SaveSystemSerializer.Deserialize<SaveData>(managedArray);

        switch (saveData)
        {
            case SaveInt x:
                EventManager<int>.Invoke(x.id, x.value);
                break;
            case SaveData x:
                EventManager.Invoke(x.id);
                break;
        }
    }

}
