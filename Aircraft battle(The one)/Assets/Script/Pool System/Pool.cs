using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//允许序列化
[System.Serializable]
public class Pool 
{
    //可在其他类里获取
    public GameObject Prefab =>prefab;

    public int Size => size;
    public int RunTimeSize=>queue.Count;

    [SerializeField]GameObject prefab;
    Queue<GameObject> queue;
    Transform Parent;
    //对象池大小
    [SerializeField] int size = 1;

    //初始化
    public void Init(Transform parent)
    {
        queue = new Queue<GameObject>();
        this.Parent = parent;
        for(int i = 0; i < size; i++)
        {
            queue.Enqueue(Copy());
        }
    }


    //生成对象
    GameObject Copy()
    {
        var copy = GameObject.Instantiate(prefab, Parent);
        //表示对象没有被使用
        copy.SetActive(false);
        return copy;
    }

    //去对象
    GameObject AvailableObject()
    {

        GameObject availableObject = null;
        //如果队列有 对象且第一个元素没有在使用
        if(queue.Count > 0&&!queue.Peek().activeSelf)
        {
            
            availableObject= queue.Dequeue();
        }
        else
        {
            Debug.Log(" ssss");
            availableObject=Copy();
        }

        //去除对象后再加入对象池
        queue.Enqueue(availableObject);

        return availableObject;
        

    }

    //启用可用对象
    public GameObject preparedObject()
    {
        GameObject preparedObject = AvailableObject();
        preparedObject.SetActive(true);
       
        return preparedObject;
    }
    public GameObject preparedObject(Vector3 pos)
    {
        GameObject preparedObject = AvailableObject();
        preparedObject.SetActive(true);
        preparedObject.transform.position = pos;
        return preparedObject;
    }
    public GameObject preparedObject(Vector3 pos,Quaternion rot)
    {
        GameObject preparedObject = AvailableObject();
        preparedObject.SetActive(true);
        preparedObject.transform.position = pos;
        preparedObject.transform.rotation = rot;
        return preparedObject;
    }
    public GameObject preparedObject(Vector3 pos, Quaternion rot,Vector3 localScale)
    {
        GameObject preparedObject = AvailableObject();
        preparedObject.SetActive(true);
        preparedObject.transform.position = pos;
        preparedObject.transform.rotation = rot;
        preparedObject.transform.localScale = localScale;
        return preparedObject;
    }

}
