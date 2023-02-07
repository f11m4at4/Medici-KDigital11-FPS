using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
    // ������ ��¦�δ�
    // hp�� ���̸� �̹����� Ȱ��ȭ �Ǿ��ٰ� ��Ȱ��ȭ�� �ȴ�.
    // Ȱ��ȭ �Ǿ��ٰ� ��Ȱ��ȭ�� �ȴ�.
    // �����ð����� Ȱ��ȭ�� �ȴ�.
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
            // ī�޶����ũ ȿ�� ���
            CameraShakeManager.Instance.Play(CameraShakeManager.EShakeType.Anim);

            // ������ ȿ�� �ֱ�
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
    // hp ���� �����ϴ� ���
    public void SetHP(int value)
    {
        StartCoroutine(TakeDmg());
        // ������ ȿ�� �ֱ�
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
