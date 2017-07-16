using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public TipController tipController;

    public Transform viewableTransform;

    [SerializeField]
    private Text clockTimer;

    [SerializeField]
    private Text scoreTracker;

    private void Awake()
    {
        
    }

    /// <summary>
    /// Update timer visual
    /// </summary>
    public void SetTime(int minutes, int seconds)
    {
        string minutes_text = minutes.ToString();
        if (minutes_text.Length == 1)
            minutes_text = "0" + minutes_text;
        string seconds_text = seconds.ToString();
        if (seconds_text.Length == 1)
            seconds_text = "0" + seconds_text;
        clockTimer.text = minutes_text + ":" + seconds_text; 
    }

    /// <summary>
    /// Set score to 0
    /// </summary>
    public void ResetScore()
    {
        scoreTracker.text = "0000";
    }

    /// <summary>
    /// Update timer visual
    /// </summary>
    public void SetScore(int score)
    {
        scoreTracker.text = "";
        string score_text = score.ToString();
        int extra_digits = 4 - score_text.Length;
        for (int i = 0; i < extra_digits; ++i)
        {
            scoreTracker.text += "0";
        }
        scoreTracker.text += score_text;
    }
}
