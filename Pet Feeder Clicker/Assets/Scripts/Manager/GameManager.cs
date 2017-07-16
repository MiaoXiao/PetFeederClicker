using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public int currentScore = 0;

    [Tooltip("Length of game."), SerializeField]
    private int startingMinutes = 5;

    public int totalCutsPerIngredient = 5;

    public ObjectPooler extraFoodPooler;

    private int currentMinutes;
    private int currentSeconds = 0;
    private float secondCounter = 0f;
    private bool timerStart = false;

    //Game Start Up
    private void Awake()
    {
        ShowTutorial();
    }

    private void Update()
    {
        if (timerStart)
        {
            secondCounter += Time.deltaTime;
            if (secondCounter >= 1f)
            {
                if (currentSeconds == 1 && currentMinutes == 0)
                {
                    EndGame();
                }
                else if (currentSeconds == 0)
                {
                    currentMinutes--;
                    currentSeconds = 59;
                }
                else
                {
                    currentSeconds--;
                }

                secondCounter = 0f;
            }
            UIManager.Instance.SetTime(currentMinutes, currentSeconds);
        }
    }

    /// <summary>
    /// Start or restart the game and begin the timer
    /// </summary>
    public void StartGame()
    {
        //Timer init
        currentMinutes = startingMinutes;
        currentSeconds = 0;
        secondCounter = 0f;
        UIManager.Instance.SetTime(currentMinutes, currentSeconds);
        timerStart = true;

        //Close Tips
        UIManager.Instance.tipController.CloseAllTips();
    }

    /// <summary>
    /// Begin the timer
    /// </summary>
    private void ShowTutorial()
    {
        //Reset visual timer
        UIManager.Instance.SetTime(startingMinutes, 0);
        //Close Tips
        UIManager.Instance.tipController.RestartTips();
    }

    /// <summary>
    /// Game is over, do final end game calculations
    /// </summary>
    private void EndGame()
    {
        print("end");
        timerStart = false;
    }

    /// <summary>
    /// Quit the game.
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
}
