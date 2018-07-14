using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class PointingController : MonoBehaviour
{
    public GameObject ground;
    public static Vector3 pointerVector;  //マウスポインタのオブジェクトが動いた距離
    public LayerMask layer;

    public void Start()
    {
        
    }

    public void Update()
    {
        Vector3 screenPoint= Input.mousePosition;
        screenPoint.z = 1;

        Ray ray = Camera.main.ScreenPointToRay(screenPoint);
        RaycastHit hit;
        Vector3 targetPosition = Vector3.zero;

        pointerVector = this.transform.position;  //マウスポインタのオブジェクトが動いた距離計算

        if (ground.GetComponent<Collider>().Raycast(ray, out hit, 500))
        {
            targetPosition = hit.point;
            this.gameObject.transform.position =  new Vector3(targetPosition.x, this.transform.position.y, targetPosition.z);
        }
        pointerVector -= this.transform.position;  //マウスポインタのオブジェクトが動いた距離計算
        pointerVector *= -1;
    }
}
