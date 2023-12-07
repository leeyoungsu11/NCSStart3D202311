using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    bool isOpen = false;
    Quaternion quat = Quaternion.identity;
    bool isRotate;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            isOpen = !isOpen;
            if(isOpen)
            {
                quat = Quaternion.Euler(0, 90, 0);
            }
            else
            {
                quat = Quaternion.Euler(0, 0, 0);
            }
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, quat, Time.deltaTime*5);
    }
}
