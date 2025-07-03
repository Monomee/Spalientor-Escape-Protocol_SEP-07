using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance;
    public List<GameObject> pooledObjects;
    public GameObject objectToPool;
    public int amountToPool;

    private void OnEnable()
    {
        Instance = this;
    }
    private void OnDisable()
    {
        Instance = null;
    }

    void Start()
    { 
        pooledObjects = new List<GameObject>();
        GameObject tmp;
        for (int i = 0; i < amountToPool; i++)
        {
            tmp = Instantiate(objectToPool);
            tmp.SetActive(false);
            pooledObjects.Add(tmp);
        }
    }
    public GameObject GetPooledObject()
    {
        for (int i = 0; i < amountToPool; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                pooledObjects[i].transform.rotation = Quaternion.identity;
                return pooledObjects[i];
            }
        }
        return null;
    }
}
