using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///对象池
///<summary>
public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance { get; private set; }

    private Dictionary<string, GamePool> pool;
    public Transform poolRoot;

    public delegate void ActiveEvent();
    public static event ActiveEvent OnActiveEvent;
    private void Awake()
    {
        instance = this;
        pool = new Dictionary<string, GamePool>();
    }

    public GameObject GetGameObject(GameObject go, Vector3 position, Quaternion rotation, Transform parent)
    {
        //对象没有池子创建一个池子
        if (!pool.ContainsKey(go.name))
        {
            pool.Add(go.name, new GamePool());
        }
        //对象有池子没对象创建一个对象
        if(pool[go.name].queue.Count == 0)
        {
            GameObject newGo = Instantiate(go, position, rotation, parent);
            newGo.name = go.name;//避免（Clone）

            return newGo;
        }
        //出队列一个返回
        GameObject returnObject = pool[go.name].queue.Dequeue();


        returnObject.transform.SetParent(parent);
        returnObject.transform.position = position;  
        returnObject.transform.rotation = rotation;
        returnObject.SetActive(true);//agent启动前先设置位置，不然好像有问题
        return returnObject;
    }

    public void SetGameObject(GameObject go, float delay)
    {
        if (!pool.ContainsKey(go.name))
        {
            pool.Add(go.name, new GamePool());
            StartCoroutine(ExcueteSetGameObject(go, delay));
        }
        else if(pool[go.name].queue.Count >= pool[go.name].MaxNumInPool)
        {
            Destroy(go, delay);
        }
         
        else
        {
            //延迟删除如需
            StartCoroutine(ExcueteSetGameObject(go, delay));
        }
    }

    public IEnumerator ExcueteSetGameObject(GameObject go, float delay)
    {
        yield return new WaitForSeconds(delay);
        go.SetActive(false);
        go.transform.SetParent(poolRoot);
        go.transform.localPosition = Vector3.zero;
        pool[go.name].queue.Enqueue(go);
    }

    public void PreLoadGameObject(GameObject go, int num)
    {
        //对象没有池子创建一个池子
        if (!pool.ContainsKey(go.name))
        {
            pool.Add(go.name, new GamePool());
        }

        for(int i = 0; i < num; i++)
        {
            GameObject newGo = Instantiate(go, poolRoot);
            newGo.name = go.name;//避免（Clone）
            newGo.SetActive(false);
            pool[go.name].queue.Enqueue(newGo);
        }
    }
}
