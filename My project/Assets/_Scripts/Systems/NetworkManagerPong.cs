using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkManagerPong : NetworkManager
{
     public static NetworkManagerPong Instance;

    public Transform LeftPlayerSpawnPoint;
    public Transform RightPlayerSpawnPoint;

    public GameObject _ball;
    public GameObject _ballPrefab;

    public UIManager uIManager = UIManager.Instance;

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

        if(numPlayers==2){
            StartCoroutine(InitialCount());
        }

    }

    public  IEnumerator InitialCount()
    {
        uIManager._initialCounter.transform.parent.gameObject.SetActive(true);
        int counter = 5;       
        while (counter != 0)
        {
            uIManager._initialCounter.text = counter.ToString();
            counter--;
            yield return new WaitForSeconds(1f);
        }

        uIManager._initialCounter.text = "START";
        yield return new WaitForSeconds(0.5f);

        uIManager._initialCounter.transform.parent.gameObject.SetActive(false);

        uIManager.gameStarted = true;
        NetworkManagerPong.Instance.InstantiateABall();
    }



    public void InstantiateABall()

    {
        float xpos = CalculatePosXBall();
        Vector3 spawiningBallposition = new Vector3(1f * xpos, Random.Range(-6f, 7f) , -1f );
        _ball = Instantiate(_ballPrefab, spawiningBallposition, Quaternion.identity);
        NetworkServer.Spawn(_ball);
    }

    public int CalculatePosXBall()
    {
        if (UIManager.Instance.playerOnService)
        {
            return -1;
        }
        else
        {
            return 1;
        }
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        if(_ball!=null){
            NetworkServer.Destroy(_ball);
        }


        base.OnServerDisconnect(conn);
    }

}
