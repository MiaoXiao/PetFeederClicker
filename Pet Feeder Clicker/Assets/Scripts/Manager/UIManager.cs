using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public TipController tipController;

    [SerializeField]
    private Text clockTimer;

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
}
