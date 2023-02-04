using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreLoadModels : MonoBehaviour
{
    public GameObject objectPool;

    public GameObject[] preLoadObjects;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(objectPool);
        GameObject.DontDestroyOnLoad(GameObject.Find("EventSystem"));

        foreach (var obj in preLoadObjects)
        {
            ObjectPool.instance.PreLoadGameObject(obj, 1);
        }      

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
