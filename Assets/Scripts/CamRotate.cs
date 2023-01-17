using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 사용자의 마우스 입력에 따라 물체를 회전시키고 싶다.
// 필요속성 : 회전속도
// 상하회전이 -60 ~ 60도 까지만 회전되록 하고 싶다.
public class CamRotate : MonoBehaviour
{
    // 필요속성 : 회전속도
    public float rotSpeed = 200;
    float mx;
    float my;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 사용자의 마우스 입력에 따라 물체를 회전시키고 싶다.
        // 1. 마우스입력으로
        float h = Input.GetAxis("Mouse X");
        float v = Input.GetAxis("Mouse Y");
        // 2. 방향이 필요
        mx += h * rotSpeed * Time.deltaTime;
        my += v * rotSpeed * Time.deltaTime;
        // 상하회전이 -60 ~ 60도 까지만 회전되록 하고 싶다.
        my = Mathf.Clamp(my, -60, 60);
        transform.eulerAngles = new Vector3(-my, mx, 0);
    }
}
