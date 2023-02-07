using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �̽�ũ��Ʈ�� �ݵ�� Animator ������Ʈ�� ���� �־�� ����� �� �ִ�.
[RequireComponent(typeof(Animator))]
// ������������ ī�޶� �����ð����� ���� �ʹ�.
public class CameraShakeAnim : MonoBehaviour
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
        // 1. ī�޶� ����ũ ���̴ϱ�(����ũ ����ð��� �������ϱ�)
        if (curCameraShakeTime > cameraShakeTime)
        {
            isStart = false;
            transform.localEulerAngles = startPos;
            // Animation stop
            anim.StopPlayback();
        }
    }
}
