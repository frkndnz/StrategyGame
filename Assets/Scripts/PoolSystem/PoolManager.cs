using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager scr;
    private void Awake()
    {
        scr = this;
    }
   

   [System.Serializable]
   public class Pool
    {
        public enum Type { barrack,powerplant,soldier,enemysoldier,tower}
        public Type type;
        public GameObject prefab;
        public int poolSize;
    }

    public List<Pool> pools;
    
    private Dictionary<Pool.Type, List<GameObject> > objectPoolDictionary = new Dictionary<Pool.Type, List<GameObject>>();
    
    private void Start()
    {
       // objectPoolDictionary = new Dictionary<Pool.Type, List<GameObject>>();
        foreach (Pool item in pools)
        {
            List<GameObject> objectPool = new List<GameObject>();

            for (int i = 0; i < item.poolSize; i++)
            {
                GameObject obj = Instantiate(item.prefab,transform);
                obj.SetActive(false);
                objectPool.Add(obj);
            }
            objectPoolDictionary.Add(item.type, objectPool);
        }
    }

    public GameObject GetObjectFromPool(Pool.Type type)
    {
        if (objectPoolDictionary.ContainsKey(type))
        {
            List<GameObject> objectPool = objectPoolDictionary[type];
            for (int i = 0; i < objectPool.Count; i++)
            {
                if (!objectPool[i].activeInHierarchy)
                {
                    return objectPool[i];
                }

            }
            return null;
        }
        else
        {
            print("object not found");
            return null;
        }
    }

    public void ReturnObjectToPool(Pool.Type type,GameObject obj)
    {
        if (objectPoolDictionary.ContainsKey(type))
        {
            objectPoolDictionary[type].Add(obj);
        }
        obj.SetActive(false);
    }
}
