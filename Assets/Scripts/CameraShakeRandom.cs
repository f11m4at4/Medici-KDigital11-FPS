using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 랜덤방향으로 카메라를 일정시간동안 흔들고 싶다.
public class CameraShakeRandom : MonoBehaviour
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

    void Start()
    {
        
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
        startPos = Camera.main.transform.localPosition;
    }

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
        // 1. 카메라 셰이크 중이니까(셰이크 경과시간이 아직 안끝났으니까)
        if (curCameraShakeTime < cameraShakeTime)
        {
            // 2. 흔들릴 시간이 흘렀으니까
            curCameraShakeDelayTime += Time.deltaTime;
            // 3. 흔들릴 시간이 됐으니까
            if (curCameraShakeDelayTime > cameraShakeDelayTime)
            {
                // 4. 카메라 이동시키고 싶다.
                transform.localPosition = startPos + Random.insideUnitSphere * cameraShakeDistance;
                curCameraShakeDelayTime = 0;
            }
        }
        else
        {
            isStart = false;
        }
    }
}
