using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private int _currentScore = 0;
    public int currentScore
    {
        get { return _currentScore; }
        set
        {
            if (value < 0)
                _currentScore = 0;
            else
                _currentScore = value;
        }
    }

    public int discardBonus = 10;

    [Tooltip("Length of game."), SerializeField]
    private int startingMinutes = 5;

    public int scorePenalty = 20;

    public int totalCutsPerIngredient = 5;

    [Space(10)]

    public ObjectPooler extraFoodPooler;

    [Space(10)]

    public bool timerStart = false;

    private int currentMinutes;
    public int currentSeconds = 0;
    private float secondCounter = 0f;


    //Game Start Up
    private void Awake()
    {
        ShowTutorial();
    }

    private IEnumerator TickSecond()
    {
        while(timerStart)
        {
            yield return new WaitForSeconds(1f);
            currentSeconds--;
            if (currentSeconds == 0 && currentMinutes == 0)
            {
                EndGame();
            }
            else if (currentSeconds == -1)
            {
                currentMinutes--;
                currentSeconds = 59;
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
        UIManager.Instance.SetTime(currentMinutes, currentSeconds);
        timerStart = true;
        StartCoroutine(TickSecond());

        IngredientSpawner.Instance.StartSpawner();

        //Close Tips
        UIManager.Instance.tipController.CloseAllTips();

        //Generate recipe
        RecipeRandomizer.Instance.StartGeneration();
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
        IngredientSpawner.Instance.spawnerStart = false;
        timerStart = false;
    }

    /// <summary>
    /// Quit the game.
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }

    private void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            QuitGame();
        }
    }
}
