using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    public LayerMask hitLayer;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 MousePos = Input.mousePosition;
            Ray CameraPoint = Camera.main.ScreenPointToRay(MousePos);
            RaycastHit ray;
            if (Physics.Raycast(CameraPoint,out ray,Mathf.Infinity,hitLayer))
            {
                transform.position = ray.point;
            }
        }
    }
    
}
