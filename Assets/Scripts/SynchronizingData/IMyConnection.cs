using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using SaveSystem;
using Steamworks.Data;
using System.Runtime.InteropServices;
using System;

public class IMyConnection : IConnectionManager
{
    public void OnConnecting(ConnectionInfo data)
    {
        Debug.Log($"{data.Identity.SteamId} is connecting");
    }

    public void OnConnected(ConnectionInfo data)
    {
        Debug.Log($"{data.Identity.SteamId} has joined the game");
    }

    public void OnDisconnected(ConnectionInfo data)
    {
        Debug.Log($"{data.Identity.SteamId} is out of here");
    }

    public void OnMessage(IntPtr data, int size, long messageNum, long recvTime, int channel)
    {
        Debug.Log("Got data on client");

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
