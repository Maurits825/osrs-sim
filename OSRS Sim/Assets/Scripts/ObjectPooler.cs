using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewObjectPooler", menuName = "ObjectPooler")]
public class ObjectPooler : ScriptableObject
{
    [SerializeField] private GameObject gameObject;
    [SerializeField] private int totalPooledObj;
    private List<GameObject> pooledObjects;

    public void CreatePool()
    {
        pooledObjects = new List<GameObject>();
        GameObject tmp;
        for (int i = 0; i < totalPooledObj; i++)
        {
            tmp = Instantiate(gameObject);
            tmp.SetActive(false);
            pooledObjects.Add(tmp);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < totalPooledObj; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        return null;
    }
}
