using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Define;

public class ObjectPool : MonoBehaviour
{
    private static readonly string PATH_OBJECTS = "Prefabs/PooledObject";
    private static readonly string PATH_UNITS = "Prefabs/Unit";

    private static Dictionary<string, Queue<PooledObject>> pooledObjects =
        new Dictionary<string, Queue<PooledObject>>();

    private static Dictionary<string, PooledObject> objectPrefabs = new Dictionary<string, PooledObject>();


    public static void Init()
    {
        objectPrefabs = Resources.LoadAll<PooledObject>(PATH_OBJECTS)
            .ToDictionary(pref => pref.name, pref => pref);
        foreach (var pref in objectPrefabs)
        {
            pooledObjects.Add(pref.Key, new Queue<PooledObject>());
        }
    }

    public static T GetObject<T>(PooledEnum id, Transform par = null) where T : PooledObject
    {
        try
        {
            if (!pooledObjects[$"{id}"].TryDequeue(out var item))
                item = Instantiate(objectPrefabs[$"{id}"], par);
            else
                item.transform.SetParent(par);

            item.gameObject.SetActive(true);
            item.Init(id);

            return item as T;
        }
        catch (KeyNotFoundException)
        {
            
        }

        return null;
    }

    public static void ReleaseObject(PooledObject item)
    {
        item.gameObject.SetActive(false);
        pooledObjects[$"{item.ID}"].Enqueue(item);
    }
}