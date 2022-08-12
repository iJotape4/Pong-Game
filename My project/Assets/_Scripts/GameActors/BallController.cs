using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : GameActorController
{
     
    public float _ballforce = 5f;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();    
        _speed = 80f;      
        BallAwake();
    }

    private void Update()
    {
        //when GameOver Screen is loaded, the ball shouldn't be in the scene.
        if (_gameManager._gameOver)
        {  
            Destroy(gameObject);
        }

    }

    private void FixedUpdate()
    {
        //to avoid ball sliding on game limits
        if(_rb.velocity.y == 0)
        {
            _rb.AddForce(new Vector2(0f, 5f));
        }
    }

    void BallAwake()
    {
        float ballInitialDirection =1f;

        /*If you want that the ball to be served by the player that makes the point ( like in the original game) you can use this method.
        if (!uimanager.playerOnService)
        {
            ballInitialDirection *=-1;
        }
        */

        //The requeriments indicate that the service must be random, this method can be reused for that.
        if (uimanager.CalculateFirstService())
        {
            ballInitialDirection *= -1;
            uimanager.playerOnService = false;
        }
        else
        {
            uimanager.playerOnService = true ;
        }

        //Iin this part, we add the initial forces to the ball. Play with this little gravity scale values improves the movement according to the direccion in a good way.
        _rb.AddForce(new Vector2(_ballforce * ballInitialDirection, Random.Range(-1f, 1f)) * _speed);
       _rb.gravityScale = (Random.Range(-0.1f, 0.1f));

    }


    //Checks when the ball collisions with lateral limits, in order to update the score of the corresponding player.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "GameLimit")
        {

            if(collision.transform.position.x > 0)
            {
                _gameManager.UpdateScore(1);
            }
            else
            {
                _gameManager.UpdateScore(2);
            }

            StartCoroutine(BallRespawn());
        }
    }


    //Hide the ball and wait, after wait create a new ball. Then, destroy this ball.
    private IEnumerator BallRespawn() 
    {
        _sprite.enabled = false;
        yield return new WaitForSeconds(1f);
        uimanager.InstantiateABall();
        Destroy(gameObject);
    }

}
