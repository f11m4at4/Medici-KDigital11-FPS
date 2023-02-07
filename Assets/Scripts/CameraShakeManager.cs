using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��û�� ������ ī�޶� ����ũ ��� ����Ѵ�.
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

    // ī�޶����ũ�ش޶�
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
