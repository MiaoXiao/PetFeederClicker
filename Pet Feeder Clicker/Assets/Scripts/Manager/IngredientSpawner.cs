using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientSpawner : MonoBehaviour
{
    [SerializeField]
    private RectTransform spawningArea;

    [Space(10)]

    [SerializeField]
    private int waveLengthInSeconds = 30;

    [SerializeField]
    private int numberOfWaveStartIngredients = 5;

    [SerializeField]
    private int additionalIngredientInterval = 5;

    [SerializeField]
    private int numberOfAdditionalIngredients = 2;

    [Space(10)]

    [SerializeField]
    private List<Spawner> allSpawners = new List<Spawner>();

    private void Awake()
    {
        StartWave();
    }

    public void StartWave()
    {
        //Clear all outside ingredients
        for (int i = 0; i < spawningArea.transform.childCount; ++i)
        {
            spawningArea.transform.GetChild(i).gameObject.SetActive(false);
            spawningArea.transform.GetChild(i).transform.SetParent(null, true);
        }

        //Spawn initial ingredients
        for (int i = 0; i < numberOfWaveStartIngredients; ++i)
        {
            int rand_index = UnityEngine.Random.Range(0, allSpawners.Count);
            GameObject food = allSpawners[rand_index].objectPooler.GetObject();
            food.transform.SetParent(spawningArea.transform, true);

            Rect rect = RectTransformUtility.PixelAdjustRect(spawningArea, UIManager.Instance.GetComponent<Canvas>());
            //print(worldpos);
            //Rect rect = RectTransformToScreenSpace(spawningArea);
            //print(rect.min);
            //print(rect.max);
            //float rand_x = UnityEngine.Random.Range(rect.min.x, rect.max.x);
            //float rand_y = UnityEngine.Random.Range(rect.min.y, rect.max.y);

            //food.transform.position = new Vector2(rand_x, rand_y);
         

        }
    }

    public static Rect RectTransformToScreenSpace(RectTransform transform)
    {
        Vector2 size = Vector2.Scale(transform.rect.size, transform.lossyScale);
        float x = transform.position.x + transform.anchoredPosition.x;
        float y = Screen.height - transform.position.y - transform.anchoredPosition.y;

        return new Rect(x, y, size.x, size.y);
    }
}

[Serializable]
public class Spawner
{
    public ObjectPooler objectPooler;
}
