using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 이스크립트는 반드시 Animator 컴포넌트를 갖고 있어야 사용할 수 있다.
[RequireComponent(typeof(Animator))]
// 랜덤방향으로 카메라를 일정시간동안 흔들고 싶다.
public class CameraShakeAnim : MonoBehaviour
{
    // 필요속성 : 카메라셰이킹시간, 흔들리는 간격(시간), 얼만큼 크게 흔들릴지, 시작점, 셰이크 경과시간, 셰이크 간격 경과시간
    public float cameraShakeTime = 0.5f;
    public float cameraShakeDelayTime = 0.02f;
    public float cameraShakeDistance = 0.205f;
    Vector3 startPos;
    float curCameraShakeTime;
    float curCameraShakeDelayTime;

    // 카메라셰이크 시작할지 여부
    bool isStart = false;

    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        RunCameraShake();
    }

    public void OnStartShake()
    {
        isStart = true;
        curCameraShakeTime = 0;
        curCameraShakeDelayTime = 0;
        startPos = transform.localEulerAngles;
        anim.Play("CameraShake");
    }
    float curTime;
    // 랜덤방향으로 카메라를 일정시간동안 흔들고 싶다.
    private void RunCameraShake()
    {
        if(isStart == false)
        {
            return;
        }
        // 랜덤방향으로 카메라를 일정시간동안 흔들고 싶다.
        // 0. 셰이크 시간이 흘러야 한다.
        curCameraShakeTime += Time.deltaTime;
        // 1. 카메라 셰이크 중이니까(셰이크 경과시간이 끝났으니까)
        if (curCameraShakeTime > cameraShakeTime)
        {
            isStart = false;
            transform.localEulerAngles = startPos;
            // Animation stop
            anim.StopPlayback();
        }
    }
}
