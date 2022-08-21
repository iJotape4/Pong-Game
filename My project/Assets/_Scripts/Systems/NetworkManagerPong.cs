using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkManagerPong : NetworkManager
{
     public static NetworkManagerPong Instance;

    public Transform LeftPlayerSpawnPoint;
    public Transform RightPlayerSpawnPoint;
     //On Awake we are creating the Network Manager Instance
    private void Awake()
    {
        if (NetworkManagerPong.Instance == null)
        {
            NetworkManagerPong.Instance = this.GetComponent<NetworkManagerPong>();

        }
        else if (NetworkManagerPong.Instance != null && NetworkManagerPong.Instance != this)
        {
            Destroy(gameObject);
            return;

        }
        DontDestroyOnLoad(this);
    }



    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        Transform startPosition = numPlayers == 0 ? LeftPlayerSpawnPoint : RightPlayerSpawnPoint;
        GameObject player = Instantiate(playerPrefab, startPosition.position, Quaternion.identity);
        NetworkServer.AddPlayerForConnection(conn, player);  //Le da al jugador instanciado la autoriadad de esta conexión.

    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        base.OnServerDisconnect(conn);
    }

}
