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

    private void Start()
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

            float min_x = spawningArea.transform.position.x - ((spawningArea.rect.width * UIManager.Instance.mainCanvas.scaleFactor) / 2f);
            float max_x = spawningArea.transform.position.x + ((spawningArea.rect.width * UIManager.Instance.mainCanvas.scaleFactor) / 2f);
            float min_y = spawningArea.transform.position.y - ((spawningArea.rect.height * UIManager.Instance.mainCanvas.scaleFactor) / 2f);
            float max_y = spawningArea.transform.position.y + ((spawningArea.rect.height * UIManager.Instance.mainCanvas.scaleFactor) / 2f);
            float rand_x = UnityEngine.Random.Range(min_x, max_x);
            float rand_y = UnityEngine.Random.Range(min_y, max_y);
            food.transform.position = new Vector2(rand_x, rand_y);
            //print(rand_x);
            //print(rand_y);

        }
    }
}

[Serializable]
public class Spawner
{
    public ObjectPooler objectPooler;
}
