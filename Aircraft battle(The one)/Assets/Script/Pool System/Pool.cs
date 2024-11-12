using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//�������л�
[System.Serializable]
public class Pool 
{
    //�������������ȡ
    public GameObject Prefab =>prefab;

    public int Size => size;
    public int RunTimeSize=>queue.Count;

    [SerializeField]GameObject prefab;
    Queue<GameObject> queue;
    Transform Parent;
    //����ش�С
    [SerializeField] int size = 1;

    //��ʼ��
    public void Init(Transform parent)
    {
        queue = new Queue<GameObject>();
        this.Parent = parent;
        for(int i = 0; i < size; i++)
        {
            queue.Enqueue(Copy());
        }
    }


    //���ɶ���
    GameObject Copy()
    {
        var copy = GameObject.Instantiate(prefab, Parent);
        //��ʾ����û�б�ʹ��
        copy.SetActive(false);
        return copy;
    }

    //ȥ����
    GameObject AvailableObject()
    {

        GameObject availableObject = null;
        //��������� �����ҵ�һ��Ԫ��û����ʹ��
        if(queue.Count > 0&&!queue.Peek().activeSelf)
        {
            
            availableObject= queue.Dequeue();
        }
        else
        {
            Debug.Log(" ssss");
            availableObject=Copy();
        }

        //ȥ��������ټ�������
        queue.Enqueue(availableObject);

        return availableObject;
        

    }

    //���ÿ��ö���
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
