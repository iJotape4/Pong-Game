using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : PersistentSingleton<GameManager>
{

    private UIManager _uiManager;

    [SerializeField] private int _player1Score = 0;
    public int player1Score { get => _player1Score; }

    [SerializeField] private int _IAScore= 0;
    public int IAScore { get => _IAScore; }

    private int _scoreDiff;
    public int ScoreDiff { get => _scoreDiff; }

    public bool _gameOver = false;


    // On Start we are setting the Game Manager instance
    void Start()
    {
        _uiManager = UIManager._instance;
    }

    //This method updates the score properly for each player. Also calculates the difference, in order to set difficulty levels for AI 
    public void UpdateScore(int player)
    {
        if (player == 1)
        {
            _player1Score++;
            _uiManager.RpcUpdatePlayerScore(player1Score);
        }
        else
        {
            _IAScore++;
            _uiManager.UpdateIAScore(IAScore);

        }

        if(_player1Score ==6 || IAScore == 6)
        {
            EndGame();
        }

        _scoreDiff = _player1Score - _IAScore;
    }

    //This method triggers the Game Over.
    public void EndGame()
    {
       _gameOver = true;
        _uiManager.ShowGameOver();
    }

    //This method sets the Game variables again to their initial state.
    public void PlayAgain()
    {
        _gameOver = false;

        _player1Score = 0;
        _IAScore = 0;

        _uiManager.RpcUpdatePlayerScore(_player1Score);
        _uiManager.UpdateIAScore(_IAScore);

    }
}
