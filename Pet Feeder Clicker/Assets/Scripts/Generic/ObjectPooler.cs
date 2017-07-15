using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [SerializeField]
    private Transform Container;

    [SerializeField]
    private GameObject objectToPool;

    [SerializeField]
    private int numberToPool = 10;

    private List<GameObject> pooledObjects = new List<GameObject>();

    private void Awake()
    {
        for (int i = 0; i < numberToPool; ++i)
        {
            AddNewCopyToPool();
        }
    }

    public GameObject GetObject()
    {
        for (int i = 0; i < pooledObjects.Count; ++i)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                pooledObjects[i].SetActive(true);
                return pooledObjects[i];
            }
        }

        AddNewCopyToPool();
        return pooledObjects[pooledObjects.Count - 1];
    }

    private void AddNewCopyToPool()
    {
        pooledObjects.Add(Instantiate(objectToPool) as GameObject);
        pooledObjects[pooledObjects.Count - 1].transform.SetParent(Container, true);
        pooledObjects[pooledObjects.Count - 1].SetActive(false);
    }
}
