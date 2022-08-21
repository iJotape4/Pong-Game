using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(PlayerInput))]
public class PlayerController : GameActorController
{
    private PlayerInput _playerInput;
    private Vector2 _moveInput;
    

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        _playerInput = GetComponent<PlayerInput>();
        _speed = 10f;

    }

    //On fixed update, we are moving the player, according to Movement Input 
    public void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + _moveInput * _speed * Time.fixedDeltaTime);
       
    }

    //On update we are updtaing the scor and checking when the Game Is over, in order to disable the player object if true.
    public void Update()
    {
        _score = _gameManager.player1Score;
        if (_gameManager._gameOver)
        {
            DisableOnGameOver();
        }
    }

    //The only Input method for this Game (is called using InputSystem)
    public void OnMove(InputValue value)
    {
        if (!uimanager.gameStarted || !isLocalPlayer)
        {
            return;
        }
        _moveInput = value.Get<Vector2>();
     
    }

    //The Input Method that can be called when the game is Over, in order to press any key to play. 
    public void OnPlayAgain(InputValue value)
    {
        if (!_gameManager._gameOver)
        {
            return;
        }
        StartCoroutine(NetworkManagerPong.Instance.InitialCount());
        _gameManager.PlayAgain();
        EnableOnPlay();

    }
}
