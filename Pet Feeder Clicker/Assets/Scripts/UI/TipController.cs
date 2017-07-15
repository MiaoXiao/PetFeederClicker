using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipController : MonoBehaviour
{
    private List<GameObject> allTips = new List<GameObject>();
    private int currentTipIndex = 0;

    private void Awake()
    {
        //Init all tips
        for(int i = 0; i < transform.childCount; ++i)
        {
            allTips.Add(transform.GetChild(i).gameObject);
            allTips[i].SetActive(false);
        }
    }

    /// <summary>
    /// Start from first tip
    /// </summary>
    public void RestartTips()
    {
        if (allTips.Count == 0)
            return;

        //Deactivate all tips
        CloseAllTips();

        currentTipIndex = 0;
        allTips[0].SetActive(true);

    }

    /// <summary>
    /// Close every tip
    /// </summary>
    public void CloseAllTips()
    {
        //Deactivate all tips
        for (int i = 0; i < allTips.Count; ++i)
        {
            allTips[i].SetActive(false);
        }
    }

    /// <summary>
    /// Go to next tip
    /// </summary>
    public void NextTip()
    {
        if (currentTipIndex == allTips.Count - 1)
            return;

        allTips[currentTipIndex].SetActive(false);
        currentTipIndex++;
        allTips[currentTipIndex].SetActive(true);
    }

    /// <summary>
    /// Go to previous tip
    /// </summary>
    public void PrevTip()
    {
        if (currentTipIndex == 0)
            return;

        allTips[currentTipIndex].SetActive(false);
        currentTipIndex--;
        allTips[currentTipIndex].SetActive(true);
    }

}
