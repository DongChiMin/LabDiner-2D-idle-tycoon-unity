using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalObjectPooling : Singleton<GlobalObjectPooling>
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private int initialSize = 10;
    
    private Queue<GameObject> poolQueue = new Queue<GameObject>();

    void Start()
    {
        for (int i = 0; i < initialSize; i++)
        {
            CreateNewObject();
        }
    }

    private GameObject CreateNewObject()
    {
        GameObject obj = Instantiate(prefab, transform);
        obj.SetActive(false);
        poolQueue.Enqueue(obj);
        return obj;
    }

    public GameObject Get(Vector3 position, Quaternion rotation)
    {
        // Nếu hết pool thì tự tạo thêm (Lazy Loading)
        GameObject obj = (poolQueue.Count > 0) ? poolQueue.Dequeue() : CreateNewObject();

        obj.transform.SetPositionAndRotation(position, rotation);
        obj.SetActive(true);
        return obj;
    }

    public void ReturnToPool(GameObject obj)
    {
        if (obj.activeSelf)
        {
            obj.SetActive(false);
            poolQueue.Enqueue(obj);
        }
    }
}