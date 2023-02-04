using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePool
{
    private int maxNumInPool = int.MaxValue;
    public Queue<GameObject> queue;

    public GamePool()
    {
        queue = new Queue<GameObject>();
    }
    public int MaxNumInPool
    {
        get
        {
            return maxNumInPool;
        }
        set
        {
            //确保是大于0小于int上限的个数
            maxNumInPool = Mathf.Clamp(value, 0, int.MaxValue);
        }
    }
}
