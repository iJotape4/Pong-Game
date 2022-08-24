using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PersistentSingleton<T> : NetworkBehaviour where T :Component

{
     public static T _instance;

private void Awake()
    {
        if (_instance == null)
        {
            _instance = this.GetComponent<T>();

        }
        else if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;

        }
        DontDestroyOnLoad(this);
    }

}
