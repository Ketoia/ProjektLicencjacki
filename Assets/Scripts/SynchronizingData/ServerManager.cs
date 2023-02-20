using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using SaveSystem;

public class ServerManager : MonoBehaviour
{
    private IMyServer MyServerInterface = new();
    private IMyConnection MyConnectionInterface = new();

    public SocketManager server;
    public ConnectionManager connection;
    Guid goodGuid;

    public void Start()
    {
        try
        {
            SteamClient.Init(1789450);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
        goodGuid = Guid.NewGuid();

        //SaveData data = new test2() { pasta = 1 };
        //data.id = goodGuid;

        //byte[] bytes = SaveSystemSerializer.Serialize(data);
        //SaveData obj = SaveSystemSerializer.Deserialize<SaveData>(bytes);

        //switch (obj)
        //{
        //    case test1 x:
        //        Debug.Log(x.pizza + " " + x.pizza);
        //        break;
        //    case test2 x:
        //        Debug.Log(x.pasta + " " + x.pasta);
        //        break;
        //}

        SaveInt saveInt = new SaveInt();
        saveInt.id = goodGuid;
        saveInt.value = 5;

        EventManager<int>.StartListening(saveInt.id, pizza);

        EventManager<int>.Invoke(saveInt.id, saveInt.value);

        //EventManager<int>.StopListening(saveInt.id, pizza);

        //var obj = MessagePack.MessagePackSerializer.Deserialize<SaveData>(bytes, options);
        //Debug.Log(obj.id);

        server = SteamNetworkingSockets.CreateRelaySocket(0, MyServerInterface);
        connection = SteamNetworkingSockets.ConnectRelay(SteamClient.SteamId, 0, MyConnectionInterface);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SaveInt saveInt = new SaveInt();
            saveInt.id = Guid.NewGuid();
            saveInt.value = 5;

            foreach (var item in server.Connected)
            {
                item.SendMessage(SaveSystemSerializer.Serialize(saveInt));
            }
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            SaveInt saveInt = new SaveInt();// { id = goodGuid, value = 5 };

            saveInt.id = goodGuid;
            saveInt.value = 5;
            SaveData toSave = saveInt;
            byte[] toSend = SaveSystemSerializer.Serialize(toSave);

            connection.Connection.SendMessage(toSend);
        }

        if (server != null)
            server.Receive();

        if (connection != null)
            connection.Receive();
    }

    private void OnDestroy()
    {
        if (server != null)
            server.Close();

        if (connection != null)
            connection.Close();
    }

    private void pizza(int tescik)
    {
        Debug.Log(tescik);
    }
}
