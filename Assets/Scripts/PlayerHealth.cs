using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
    // 맞으면 반짝인다
    // hp가 깎이면 이미지가 활성화 되었다가 비활성화가 된다.
    // 활성화 되었다가 비활성화가 된다.
    // 일정시간동안 활성화가 된다.
public class PlayerHealth : MonoBehaviour
{
    public Image takeDmg;
    private int hp = 300;

    public static PlayerHealth Instance;

    public int HP
    {
        get
        {
            return hp;
        }
        set
        {
            // 카메라셰이크 효과 재생
            CameraShakeManager.Instance.Play(CameraShakeManager.EShakeType.Anim);

            // 데미지 효과 주기
            StartCoroutine(TakeDmg());
            hp = value;
            if (hp <= 0)
            {
                Destroy(gameObject);
            }
        }
    }


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
    private void Start()
    {
        takeDmg.enabled = false;
    }
    public int GetHP()
    {
        return hp;
    }
    // hp 값을 설정하는 기능
    public void SetHP(int value)
    {
        StartCoroutine(TakeDmg());
        // 데미지 효과 주기
        hp = value;
        if(hp <= 0)
        {
            Destroy(gameObject);
        }
    }
    IEnumerator TakeDmg()
    {
        takeDmg.enabled = true;
        yield return new WaitForSeconds(0.3f);
        takeDmg.enabled = false;
    }
}
