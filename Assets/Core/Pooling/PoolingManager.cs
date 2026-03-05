
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace CoreGame
{
    public static class PoolingManager
{
    private const int Default_Pool_Size = 3;
    private static Dictionary<int, Pool> pooling = new Dictionary<int, Pool>();

    public static void Init(GameObject prefab = null, int initQuantity = Default_Pool_Size)
    {
        pooling ??= new Dictionary<int, Pool>();
        if (prefab != null && !pooling.ContainsKey(prefab.GetInstanceID()))
        {
            pooling[prefab.GetInstanceID()] = new Pool(prefab, initQuantity);
        }
    }

    public static void PoolPreload(GameObject prefab, int quantity, Transform newParent = null)
    {
        Init(prefab,1);
        pooling[prefab.GetInstanceID()].PreLoad(quantity,newParent);
    }
    public static GameObject Spawn(GameObject prefab, Vector3 pos = default, Quaternion rot = default, Transform par = null)
    {
        Init(prefab);
        GameObject gameObject = pooling[prefab.GetInstanceID()].Spawn(pos, rot);

        if (par != null)
        {
            gameObject.transform.SetParent(par,true);
        }

        return gameObject;
    }

    public static GameObject Spawn(GameObject prefab, Transform par)
    {
        GameObject gameObject = Spawn(prefab, default, default, par);
        return gameObject;
    }
    public static void Despawn(GameObject gameObject, UnityAction actionDespawn = null)
    {
       
        Pool p = pooling.Values.FirstOrDefault(pool => pool.memberIDs.Contains(gameObject.GetInstanceID()));
        if (p == null)
        {
            Debug.LogFormat("Object '{0}' wasn't spawned from a pool. Destroying it instead.", gameObject.name);
            Object.Destroy(gameObject);
        }
        else
        {
            actionDespawn?.Invoke();
            p.Despawn(gameObject);
        }

    }
}

public class Pool
{
    private int id = 1;
    //is used to save inactive object
    private Stack<GameObject> notActiveObjects;
    // object will pool
    private GameObject prefab;
    //is used to hold object
    private Transform holder;
    public readonly HashSet<int> memberIDs;
    public Pool(GameObject prefabModel, int initQuantity)
    {
        notActiveObjects = new Stack<GameObject>(initQuantity);
        prefab = prefabModel;
        memberIDs = new HashSet<int>();
    }

    public void PreLoad(int initQuantity, Transform parent = null)
    {
        for (int i = 0; i < initQuantity; ++i)
        {
            GameObject gameObject = Object.Instantiate(prefab, parent);
            gameObject.name = prefab.name + " " + id;
            gameObject.gameObject.SetActive(false);
            notActiveObjects.Push(gameObject);
            memberIDs.Add(gameObject.GetInstanceID());
        }
    }
    private GameObject GetObjectFromStack()
    {
        GameObject gameObject;
        while (notActiveObjects.Count > 0)
        {
            gameObject = notActiveObjects.Pop();
            if (gameObject != null) return gameObject;
        }
        gameObject = Object.Instantiate(prefab);
        gameObject.SetActive(false);
        memberIDs.Add(gameObject.GetInstanceID());
        return gameObject;
    }
    public GameObject Spawn(Vector3 pos, Quaternion rot)
    {
        GameObject gameObject = GetObjectFromStack();
       
        gameObject.transform.SetPositionAndRotation(pos,rot);
        gameObject.name = prefab.name + " " + id;
        if(holder != null) gameObject.transform.SetParent(holder);
        gameObject.SetActive(true);
        return gameObject;
    }

    public T Spawn<T>(Vector3 pos, Quaternion rot)
    {
        return Spawn(pos, rot).GetComponent<T>();
    }

    public void Despawn(GameObject gameObject)
    {
        if (gameObject.activeSelf) gameObject.SetActive(false);
        notActiveObjects.Push(gameObject);
    }

}

}
