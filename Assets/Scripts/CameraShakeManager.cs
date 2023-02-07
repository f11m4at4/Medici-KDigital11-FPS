using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 요청이 들어오면 카메라 셰이크 재생 담당한다.
public class CameraShakeManager : MonoBehaviour
{
    public enum EShakeType
    {
        Random,
        Sine,
        Anim
    }

    public static CameraShakeManager Instance;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    // 카메라셰이크해달라
    public void Play(EShakeType shakeType = EShakeType.Random)
    {
        switch(shakeType)
        {
            case EShakeType.Random:
                GetComponent<CameraShakeRandom>().OnStartShake();
                break;
            case EShakeType.Sine:
                GetComponent<CameraShakeSine>().OnStartShake();
                break;
            case EShakeType.Anim:
                GetComponent<CameraShakeAnim>().OnStartShake();
                break;
        }
    }
}
