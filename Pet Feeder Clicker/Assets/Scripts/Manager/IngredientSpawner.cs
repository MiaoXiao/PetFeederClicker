using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientSpawner : Singleton<IngredientSpawner>
{
    [SerializeField]
    private AudioClip windAudio;

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

    public bool spawnerStart = false;

    private void Start()
    {
        //StartWave();
    }

    public void StartSpawner()
    {
        spawnerStart = true;
        SpawnWave();
        StartCoroutine(SpawnWaveTimer());
        StartCoroutine(SpawnAdditionalTimer());
    }


    private IEnumerator SpawnWaveTimer()
    {
        while(spawnerStart)
        {
            yield return new WaitForSeconds(waveLengthInSeconds);
            SpawnWave();
        }
    }

    private IEnumerator SpawnAdditionalTimer()
    {
        while (spawnerStart)
        {
            yield return new WaitForSeconds(additionalIngredientInterval);
            SpawnAdditional();
        }
    }

    public void SpawnWave()
    {
        AudioMana.Instance.PlayAudio(windAudio);
        //Clear all outside ingredients
        ClearAllOutsideFood();

        SpawnFood(numberOfWaveStartIngredients);
    }

    public void SpawnAdditional()
    {
        SpawnFood(numberOfAdditionalIngredients);
    }

    private void SpawnFood(int number)
    {
        if (number <= 0)
            return;

        //Spawn initial ingredients
        for (int i = 0; i < number; ++i)
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
            food.transform.localScale = new Vector3(1, 1, 1);
            //print(rand_x);
            //print(rand_y);

        }
    }

    private void ClearAllOutsideFood()
    {
        for (int i = spawningArea.transform.childCount - 1; i >= 0; --i)
        {
            spawningArea.transform.GetChild(i).gameObject.SetActive(false);
            spawningArea.transform.GetChild(i).SetParent(UIManager.Instance.foodTransform, true);
        }
    }
}

[Serializable]
public class Spawner
{
    public ObjectPooler objectPooler;
}
