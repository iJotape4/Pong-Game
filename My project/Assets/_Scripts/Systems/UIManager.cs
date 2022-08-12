using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public GameManager _gameManager;

    public GameObject _ballPrefab;
    public bool gameStarted = false;
    public bool playerOnService;

    [SerializeField] private TextMeshProUGUI _initialCounter;
    [SerializeField] private TextMeshProUGUI _player1Score;
    [SerializeField] private TextMeshProUGUI _iaScore;


    //On Awake we are creating the UI Manager Instance
    private void Awake()
    {
        if (UIManager.Instance == null)
        {
            UIManager.Instance = this.GetComponent<UIManager>();

        }
        else if (UIManager.Instance != null && UIManager.Instance != this)
        {
            Destroy(gameObject);
            return;

        }
        DontDestroyOnLoad(this);
    }

    //On Start we are calculating the side of the service. Also managment of the initial counter before playing.
    void Start()
    {
        StartCoroutine(InitialCount());
        playerOnService = CalculateFirstService();
        _gameManager = GameManager.Instance;
    }


    //This choose a direction to launch the ball in the first turn/service
    public bool CalculateFirstService()
    {
        if (Random.value >= 0.5)
        {
            return true;
        }
        return false;
    }

    //This manage the showed timer before starting the game
    public  IEnumerator InitialCount()
    {
        _initialCounter.transform.parent.gameObject.SetActive(true);
        int counter = 5;       
        while (counter != 0)
        {
            _initialCounter.text = counter.ToString();
            counter--;
            yield return new WaitForSeconds(1f);
        }

        _initialCounter.text = "START";
        yield return new WaitForSeconds(0.5f);

        _initialCounter.transform.parent.gameObject.SetActive(false);

        gameStarted = true;
        InstantiateABall();     
    }



    //This method is  responsible for ball instantiation.
    public void InstantiateABall()

    {
        float xpos = CalculatePosXBall();
        Vector3 spawiningBallposition = new Vector3(1f * xpos, Random.Range(-6f, 7f) , -1f );
        Instantiate(_ballPrefab, spawiningBallposition, Quaternion.identity);
    }

    //This Method receive the score of the player and shows it in the UI
    public void UpdatePlayerScore(int score)
    {
        _player1Score.text = score.ToString();
        playerOnService = true;
    }

    //This Method receive the score of the AI and shows it in the UI
    public void UpdateIAScore(int score)
    {
        _iaScore.text = score.ToString();
        playerOnService = false;
    }

    //This improve the initial ball position, depends of the side on service. The Idea is position the ball a little behind the center of the scenary, with the objective of give more chances to both players.
    public int CalculatePosXBall()
    {
        if (playerOnService)
        {
            return -1;
        }
        else
        {
            return 1;
        }
    }

    //this Method shows the GameOver Texts When a player Reaches 6 points.
    public void ShowGameOver()
    {
        gameStarted = false;
        if(_gameManager.player1Score == 6)
        {
            _initialCounter.text = "Player 1 Wins";
           
        }else if (_gameManager.IAScore == 6)
        {
            _initialCounter.text = "Player 2 (AI) Wins";
        }
        _initialCounter.text += "\n Press any key \n to play again";

        _initialCounter.transform.parent.gameObject.SetActive(true);
    }
}
