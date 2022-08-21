using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkManagerPong : NetworkManager
{
     public static NetworkManagerPong Instance;

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
        base.OnServerAddPlayer(conn);
        //if(numPlayers);

    }

}
