using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generic object pooling system for efficient reuse of GameObject instances.
/// Reduces garbage collection pressure by reusing objects instead of Instantiate/Destroy.
/// </summary>
public class PoolManager : MonoBehaviour
{
    private static PoolManager instance;
    public static PoolManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject poolManagerObject = new GameObject("PoolManager");
                instance = poolManagerObject.AddComponent<PoolManager>();
            }
            return instance;
        }
        private set => instance = value;
    }

    private Dictionary<GameObject, Queue<GameObject>> poolDictionary = new Dictionary<GameObject, Queue<GameObject>>();

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.LogError("PoolManager already exists!");
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public static PoolManager EnsureInstance()
    {
        return Instance;
    }

    /// <summary>
    /// Get an object from the pool or create a new one if pool is empty.
    /// Calls OnSpawn() to reset object state.
    /// </summary>
    public GameObject GetObject(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        if (!poolDictionary.ContainsKey(prefab))
        {
            poolDictionary[prefab] = new Queue<GameObject>();
        }

        GameObject obj;
        if (poolDictionary[prefab].Count > 0)
        {
            obj = poolDictionary[prefab].Dequeue();
            if (parent != null)
            {
                obj.transform.SetParent(parent);
            }
            obj.transform.SetPositionAndRotation(position, rotation);
            obj.SetActive(true);
        }
        else
        {
            obj = Instantiate(prefab, position, rotation, parent);
            PoolableObject poolable = obj.GetComponent<PoolableObject>();
            if (poolable == null)
            {
                poolable = obj.AddComponent<PoolableObject>();
            }
            poolable.prefab = prefab;
        }

        // Reset object state if it has a resettable component
        IPoolable poolableInterface = obj.GetComponent<IPoolable>();
        if (poolableInterface != null)
        {
            poolableInterface.OnSpawn();
        }

        return obj;
    }

    /// <summary>
    /// Return an object to the pool for reuse.
    /// Calls OnDespawn() to clean up object state.
    /// </summary>
    public void ReturnObject(GameObject obj)
    {
        // Reset object state
        IPoolable poolable = obj.GetComponent<IPoolable>();
        if (poolable != null)
        {
            poolable.OnDespawn();
        }

        obj.SetActive(false);
        obj.transform.SetParent(null);

        GameObject prefab = GetPrefabFromObject(obj);
        if (prefab != null)
        {
            if (!poolDictionary.ContainsKey(prefab))
            {
                poolDictionary[prefab] = new Queue<GameObject>();
            }
            poolDictionary[prefab].Enqueue(obj);
        }
        else
        {
            Destroy(obj); // Fallback if no prefab found
        }
    }

    private GameObject GetPrefabFromObject(GameObject obj)
    {
        PoolableObject poolable = obj.GetComponent<PoolableObject>();
        if (poolable != null)
        {
            return poolable.prefab;
        }
        return null;
    }
}

/// <summary>
/// Interface for objects that need to reset state when pooled/unloaded.
/// </summary>
public interface IPoolable
{
    void OnSpawn();
    void OnDespawn();
}

/// <summary>
/// Component to track prefab reference for pooled objects.
/// </summary>
public class PoolableObject : MonoBehaviour
{
    public GameObject prefab;
}
