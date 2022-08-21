using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]


 //This Class is parent for ball, Player and AI. Contains commons attributtes and methods for these elements.
 public class GameActorController : NetworkBehaviour
{
    protected UIManager uimanager;
    protected GameManager _gameManager;

    protected BoxCollider2D _bc;
    protected Rigidbody2D _rb;
    protected SpriteRenderer _sprite;

    [SerializeField] protected float _speed;
    [SerializeField] protected float _score;


    //On Start we are obtaining the references of all the necessary components for each Game Actor script.
    protected void Start()
    {
        
        uimanager = UIManager.Instance;
       _gameManager = GameManager.Instance;

        _rb = GetComponent<Rigidbody2D>();
        _bc = GetComponent<BoxCollider2D>();
        _sprite = GetComponent<SpriteRenderer>();
        _score = 0f;
    }

    //This disable the players in order to see properly the win screen. 
    public void DisableOnGameOver()
    {
        _sprite.enabled = false;
        _bc.enabled =  false ;
    }

    //This Enables the players when the Game Starts Again. Also moves them to the zero position.
    public void EnableOnPlay()
    {
        transform.position = new Vector3(transform.position.x, 0f, -1f);
        _sprite.enabled = true;
        _bc.enabled = true;
    }
}
