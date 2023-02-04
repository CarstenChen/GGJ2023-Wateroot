using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBox : MonoBehaviour
{
    Vector3 mPrevPos = Vector3.zero;
    Vector3 mPosDelta = Vector3.zero;

    public static bool blockInput = false;

    void Start()
    {
    }

    private void Update()
    {
        if (PlayerPrefs.GetInt("BlockInput")==0)
            return;

        if (Input.GetKey(KeyCode.W))  
        {
            transform.Rotate(Vector3.right, 1.0f, Space.World);  
        }
        if (Input.GetKey(KeyCode.S)) 
        {
            transform.Rotate(Vector3.left, 1.0f, Space.World);  
        }
        if (Input.GetKey(KeyCode.A))  
        {
            transform.Rotate(Vector3.up, 1.0f, Space.World);  
        }
        if (Input.GetKey(KeyCode.D))  
        {
            transform.Rotate(Vector3.down, 1.0f, Space.World);  
        }
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(Vector3.forward, 1.0f, Space.World);
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(Vector3.back, 1.0f, Space.World);
        }

        //if (Input.GetMouseButton(0))  
        //{
        //    mPosDelta = Input.mousePosition - mPrevPos;
        //    transform.Rotate(Camera.main.transform.up, -mPosDelta.x, Space.World);
        //    transform.Rotate(Camera.main.transform.forward, mPosDelta.y, Space.World);
        //}
        mPrevPos = Input.mousePosition;
    }

}