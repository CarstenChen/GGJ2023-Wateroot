using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadModel : MonoBehaviour
{
    public GameObject model;
    public Transform root;
    // Start is called before the first frame update
    void Start()
    {
        ObjectPool.instance.GetGameObject(model, root.position, model.transform.rotation, root);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
