using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField] Pool[] enemyPools;
    [SerializeField] Pool[] playerProjectilePools;
    [SerializeField] Pool[] enemyProjectilePools;
    [SerializeField] Pool[] VFXPools;
    [SerializeField] Pool[] LootItemPools;
    static Dictionary<GameObject, Pool> dictionary;

    void Awake()
    {
        dictionary = new Dictionary<GameObject, Pool>();
        Init(playerProjectilePools);
        Init(enemyProjectilePools);
        Init(VFXPools);
        Init(enemyPools);
        Init(LootItemPools);


    }

    void Init(Pool[] pools)
    {
        foreach (Pool pool in pools)
        {

        #if UNITY_EDITOR//让这段代码只在debug时运行
            if (dictionary.ContainsKey(pool.Prefab))
            {
                continue;
            }
        #endif
            dictionary.Add(pool.Prefab, pool);
            Transform poolParent = new GameObject("Pool:" + pool.Prefab.name).transform;
            poolParent.parent =transform;
            pool.Init(poolParent);
            
        }
    }
#if UNITY_EDITOR
    private void OnDestroy()
    {
        CheckPoolSize(playerProjectilePools);
        CheckPoolSize(enemyProjectilePools);
        CheckPoolSize(VFXPools);
        CheckPoolSize(enemyPools);
        CheckPoolSize(LootItemPools);
    }
    //检测对象池运行尺寸
    void CheckPoolSize(Pool[] pools)
    {
        foreach(var pool in pools)
        {
            if (pool.RunTimeSize > pool.Size)
            {
                Debug.LogWarning(string.Format("Pool:{0} has a runtime size {1} bigger than its initial size {2}! ",
                    pool.Prefab.name, pool.RunTimeSize, pool.Size));
            }
        }
    }
#endif
    /// <summary>
    /// <para>Retrun a specified<paramref name="prefab"></paramref>gameObject in the pool.</para>
    /// <para>根据传入的<paramref name="prefab"></paramref>参数，返回对象池中预备好的游戏对象。</para>
    /// </summary>
    /// <param name="prefab">
    /// <para>Specified gameObject prefab.</para>
    /// <para>指定的游戏对象预制体</para>
    /// </param>
    /// <returns>
    /// <para>Prepared gameObject in the Pool.</para>
    /// <para>对象池中预备好的预制体</para>
    /// </returns>
    //释放对象池中预备好的对象，使用静态更方便被其他类调用
    public static GameObject Release(GameObject prefab)
    {
        #if UNITY_EDITOR
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.Log("PoolManager could NOT find prefab:"+prefab.name);
            return null;
        }
        #endif
        return dictionary[prefab].preparedObject();
       
    }
    public static GameObject Release(GameObject prefab,Vector3 pos)
    {
        #if UNITY_EDITOR
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.Log("PoolManager could NOT find prefab:" + prefab.name);
            return null;
        }
        #endif
        return dictionary[prefab].preparedObject(pos);

    }
    public static GameObject Release(GameObject prefab, Vector3 pos,Quaternion rot)
    {
        #if UNITY_EDITOR
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.Log("PoolManager could NOT find prefab:" + prefab.name);
            return null;
        }
        #endif
        return dictionary[prefab].preparedObject(pos,rot);

    }

    public static GameObject Release(GameObject prefab, Vector3 pos, Quaternion rot,Vector3 localScale)
    {
        #if UNITY_EDITOR
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.Log("PoolManager could NOT find prefab:" + prefab.name);
            return null;
        }
        #endif
        return dictionary[prefab].preparedObject(pos, rot,localScale);

    }



}
