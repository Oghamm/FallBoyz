using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MyNetworkManager : NetworkManager
{
    [SerializeField] private GameObject LifeBonus = null;

    List<NetworkPlayer> players = new List<NetworkPlayer>();

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);

        Debug.Log("[Server] A player is added");

        GameObject instance = Instantiate(LifeBonus, new Vector3(0, 1, 0), Quaternion.identity);
        NetworkServer.Spawn(instance);

        NetworkPlayer player = conn.identity.GetComponent<NetworkPlayer>();
        player.SetDisplayName($"Player {numPlayers}");
        player.SetColor(new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));

        players.Add(player);
    }

/*
    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);
        NetworkPlayer player = NetworkClient.connection.identity.GetComponent<NetworkPlayer>();
        players.Remove(player);

        if (players.Count == 1)
        {
            players[0].ClientSayCongrat();
            NetworkServer.Shutdown();
        }

    }*/

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        NetworkPlayer player = conn.identity.GetComponent<NetworkPlayer>();
        players.Remove(player);

        if (players.Count == 1)
        {
            players[0].ClientSayCongrat();
            //NetworkServer.Shutdown();
        }

    }

}
