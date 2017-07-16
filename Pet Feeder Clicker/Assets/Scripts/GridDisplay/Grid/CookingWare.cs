using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CookingWare : Grid
{
    [SerializeField]
    private float cookingIntervalTime = 2f;
    [SerializeField]
    private float lastIntervalTime = 2f;

    [Space(10)]

    [SerializeField]
    private List<Image> blackenList = new List<Image>();

    [Space(10)]

    [SerializeField]
    private Color charredColor;

    private float intervalTimer = 0f;

    public bool Paused = false;

    [SerializeField]
    private bool _isBurned = false;
    public bool isBurned
    {
        get { return _isBurned; }
        private set
        {
            if (value == _isBurned)
                return;

            if (value)
            {
                for (int i = 0; i < blackenList.Count; ++i)
                {
                    blackenList[i].color = charredColor;
                }
                ClearAllSteam();
            }
            else
            {
                for (int i = 0; i < blackenList.Count; ++i)
                {
                    blackenList[i].color = Color.white;
                }
            }

            _isBurned = value;
        }
    }

    [SerializeField]
    private GameObject steamList;
    private int _steamIndex = 0;
    private int steamIndex
    {
        get { return _steamIndex; }
        set
        {
            if (value < 0)
                _steamIndex = 0;
            else if (value >= steamList.transform.childCount - 1)
                _steamIndex = steamList.transform.childCount - 1;
            else
                _steamIndex = value;
        }
    }

    private float _currentTime = 0f;
    public float currentTime
    {
        get { return _currentTime; }
        set
        {
            if (value < 0)
                _currentTime = 0;
            else
                _currentTime = value;
        }
    }

    private void Awake()
    {
        ClearAllSteam();
    }

    private void Update()
    {
        if (Paused || isBurned)
            return;

        if (!isVacant)
        {
            intervalTimer += Time.deltaTime;
            if (steamIndex == steamList.transform.childCount - 1)
            {
                if (intervalTimer >= lastIntervalTime)
                {
                    //Cookingware burns
                    isBurned = true;
                    return;
                }
            }
            if (intervalTimer >= cookingIntervalTime)
            {
                steamList.transform.GetChild(steamIndex).gameObject.SetActive(false);
                steamIndex++;
                steamList.transform.GetChild(steamIndex).gameObject.SetActive(true);
                intervalTimer = 0f;
            }
        }
        else
        {
            currentTime = 0f;
            ClearAllSteam();
            steamIndex = 0;
        }

    }

    public override void AddItem()
    {
        if (isBurned)
            return;

        currentTime -= (cookingIntervalTime + (cookingIntervalTime / 2));
        steamList.transform.GetChild(steamIndex).gameObject.SetActive(false);
        steamIndex--;
        steamList.transform.GetChild(steamIndex).gameObject.SetActive(true);
    }

    private void ClearAllSteam()
    {
        for (int i = 0; i < steamList.transform.childCount; ++i)
        {
            steamList.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
