using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������������ ī�޶� �����ð����� ���� �ʹ�.
public class CameraShakeRandom : MonoBehaviour
{
    // �ʿ�Ӽ� : ī�޶����ŷ�ð�, ��鸮�� ����(�ð�), ��ŭ ũ�� ��鸱��, ������, ����ũ ����ð�, ����ũ ���� ����ð�
    public float cameraShakeTime = 0.5f;
    public float cameraShakeDelayTime = 0.02f;
    public float cameraShakeDistance = 0.205f;
    Vector3 startPos;
    float curCameraShakeTime;
    float curCameraShakeDelayTime;

    // ī�޶����ũ �������� ����
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

    // ������������ ī�޶� �����ð����� ���� �ʹ�.
    private void RunCameraShake()
    {
        if(isStart == false)
        {
            return;
        }
        // ������������ ī�޶� �����ð����� ���� �ʹ�.
        // 0. ����ũ �ð��� �귯�� �Ѵ�.
        curCameraShakeTime += Time.deltaTime;
        // 1. ī�޶� ����ũ ���̴ϱ�(����ũ ����ð��� ���� �ȳ������ϱ�)
        if (curCameraShakeTime < cameraShakeTime)
        {
            // 2. ��鸱 �ð��� �귶���ϱ�
            curCameraShakeDelayTime += Time.deltaTime;
            // 3. ��鸱 �ð��� �����ϱ�
            if (curCameraShakeDelayTime > cameraShakeDelayTime)
            {
                // 4. ī�޶� �̵���Ű�� �ʹ�.
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
