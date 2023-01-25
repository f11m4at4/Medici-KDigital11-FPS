using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    public Transform target;
    public float speed = 5;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    bool isStart = false;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            isStart = true;
        }
        if(isStart == false)
        {
            return;
        }
        // 위치따라가기
        transform.position = Vector3.Lerp(transform.position, target.position, speed * Time.deltaTime);
        // 회전 따라가기
        transform.forward = Vector3.Lerp(transform.forward, target.forward, speed * Time.deltaTime);
    }
}
