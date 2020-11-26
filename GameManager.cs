using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public delegate  void GameDelegate();
    public static event GameDelegate OnGameStarted;
    public static event GameDelegate OnGameOverConfirmed;
    public static GameManager Instance;
    public GameObject startPage;
    public GameObject gameOverPage;
    public GameObject countPage;
    public Text scoreText;
    private int score = 0;
    private bool gameOver = true;
    

    enum PageState
    {
        None,
        Start,
        GameOver,
        Count
    }
    

    public bool GameOver
    {
        get { return gameOver; }
    }

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        CountText.OnCountdownFinished += OnCountdownFinished;
        BirdControl.OnPlayerScored += OnPlayerScorred;
        BirdControl.OnPlayerDied += OnPlayerDied;
    }

    private void OnDisable()
    {
        CountText.OnCountdownFinished -= OnCountdownFinished;
        BirdControl.OnPlayerScored -= OnPlayerScorred;
        BirdControl.OnPlayerDied -= OnPlayerDied;
    }

    void OnCountdownFinished()
    {
        
        SetPageState(PageState.None);
        if (OnGameStarted != null)
        {
            OnGameStarted();
        } 
        score = 0;
        gameOver = false;
    }

    void OnPlayerDied()
    {
        
        gameOver = true;
        int savedScore = PlayerPrefs.GetInt("HighScore");
        if (score > savedScore)
        {
            PlayerPrefs.SetInt("HighScore",score);
        }
        SetPageState(PageState.GameOver);
    }

    void OnPlayerScorred()
    {
        score++;
        scoreText.text = score.ToString();
    }


    void SetPageState(PageState state)
    {
        switch (state)
        {
            case PageState.None :
                startPage.SetActive(false);
                gameOverPage.SetActive(false);
                countPage.SetActive(false);
                break;
            case PageState.Start :
                startPage.SetActive(true);
                gameOverPage.SetActive(false);
                countPage.SetActive(false);
                break;
            case PageState.GameOver:
                startPage.SetActive(false);
                gameOverPage.SetActive(true);
                countPage.SetActive(false);
                break;
            case PageState.Count:
                startPage.SetActive(false);
                gameOverPage.SetActive(false);
                countPage.SetActive(true);
                break;
            
        }
    }

    public void ConfirmGameOver()
    {
        //activated when hit the replay button
        if (OnGameOverConfirmed != null)
        {
            OnGameOverConfirmed();
        }
        scoreText.text = "0";
        SetPageState(PageState.Start);
    }

    public void StartGame()
    {
        //activated when hit the play button
        SetPageState(PageState.Count);
    }
}
