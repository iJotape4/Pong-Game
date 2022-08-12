using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAController : GameActorController
{
    [SerializeField] private Transform _ballPosition;
    private Rigidbody2D _ballRigidbody;

    [SerializeField] private float _lazyAISpeed = 0.05f;
    [SerializeField] private float _standardAISpeed = 0.08f;
    [SerializeField] private float _hardCoreAISpeed = 0.18f;

    private void Start()
    {
        base.Start();
        _speed = _standardAISpeed;
    }


    // Start is called before the first frame update
    private void FixedUpdate()
    {
        if (_ballPosition == null)
        {
            return;
        }
        ReturnToMiddlePosition();
    }

    void Update()
    {
        //Manage the activation of the AI according to GameOver and play Screens
        if (_gameManager._gameOver)
        {
            DisableOnGameOver();
        }
        else if(!_sprite.enabled)
        {
            EnableOnPlay();
        }

        //When there is not a ball reference, try to find it. If not exists, there is no problem, it will try again.
        if (_ballPosition != null)
        {
            return;
        }
        try
        {
            FindBallReference();
        }
        catch
        {
            //There are not a ball in game yet, AI waiting....
        }

        _score = _gameManager.IAScore;
        //Calls the difficulty manager each frame, to set the AI power properly.
        DifficultyChanger();  

    }

    //The Basic movement of the AI in order to hit the ball
    void CatchBall()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, _ballPosition.position.y, -1f), _speed);
    }

    //This allows the IA return to the middle of the GameBoard after hit the ball
    void ReturnToMiddlePosition()
    {
        if (_ballRigidbody.velocity.x <= 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, 0f, -1f), _speed);
        }
        else
        {
            CatchBall();
        }
    }

    //This helps to find the BallReference in the scene
    void FindBallReference()
    {
        _ballPosition = FindObjectOfType<BallController>().GetComponent<Transform>();
        _ballRigidbody = _ballPosition.GetComponent<Rigidbody2D>();
    }


    private void DifficultyChanger()
    {
        //When the player is amazing, the AI will do his best effort, and will increase their speed
        if (_gameManager.ScoreDiff % 5 == 0 && _gameManager.ScoreDiff  !> 0)
        {
            _speed = _hardCoreAISpeed;
        }

        //when the player has an standard score diff , the AI becomes in his default speed.
        //This is because it is not in hurry anymore, so it can rest a little bit 
        else if (_gameManager.ScoreDiff < 5 && _speed > _standardAISpeed || _gameManager.ScoreDiff > -3)
        {
            _speed = _standardAISpeed;
        }
        else
        {
            //When the player is really novice, the AI will get overconfident and his speed will slow down a bit, giving him more chances to win 
            if (_gameManager.ScoreDiff <= -4)
            {
                _speed = _lazyAISpeed;
            }

        }
    }

}
