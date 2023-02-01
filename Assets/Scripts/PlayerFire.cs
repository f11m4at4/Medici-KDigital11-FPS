using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//����ڰ� �߻��ư�� ������ �ѱ��� ���ϴ� �������� �ѱ����� �Ѿ��� �߻�ǵ��� �ϰ������.
// �ʿ�Ӽ� : �ѱ�, �Ѿ˰���
// 0. źâ�� ���� ����ϰ� �ʹ�.
// -> �ʿ�Ӽ� : źâ, źâ ũ��
public class PlayerFire : MonoBehaviour
{
    // �ʿ�Ӽ� : �ѱ�, �Ѿ˰���
    public Transform firePosition;
    public GameObject bulletFactory;
    // -> �ʿ�Ӽ� : źâ, źâ ũ��
    public int bulletPoolSize = 10;
    //GameObject[] bulletPool;
    public List<GameObject> bulletPool;

    // ��ź�߻����� �ƴ��� ���
    public bool bGrenade = false;

    // �ʿ�Ӽ� : �Ѿ� ����
    public Transform bulletEffect;
    ParticleSystem psBulletEffect;
    AudioSource asBulletSound;

    void Start()
    {
        // ��ź�߻� ����� ���� �Ѿ� ��������
        if (bGrenade)
        {
            //�Ѿ� ��������
            MakeGrenade();
        }
        else
        {
            psBulletEffect = bulletEffect.GetComponent<ParticleSystem>();
            asBulletSound = bulletEffect.GetComponent<AudioSource>();
        }
    }

    
    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.IsIntro())
        {
            return;
        }
        // ��ź�߻� Ȱ��ȭ �Ǿ� �ִٸ�
        if (bGrenade)
        {
            // �Ѿ˹߻�ǵ���
            FireGrenade();
            return;
        }

        // Ray �� �̿��� �Ѿ˹߻�
        FireRay();
    }

    // Ray �� �̿��� �Ѿ˹߻�
    void FireRay()
    {
        // 1. ����ڰ� �߻��ư�� �������ϱ� 
        if (Input.GetButtonDown("Fire1"))
        {
            // 2. RaycastHit �׸��ʿ�
            RaycastHit hitInfo = new RaycastHit();
            // 3. Ray �ʿ�
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            // 4. �Ѿ˹߻�
            // - �ڱ��ڽ��� �浹���⿡�� ���ܽ�Ű�� �ʹ�.
            //int layer = LayerMask.NameToLayer("Player");
            int layer = 1 << gameObject.layer;
            bool bHit = Physics.Raycast(ray, out hitInfo, 500, ~layer);

            // �ε����ٸ�
            if (bHit)
            {
                // �ε��� �༮�� Enemy ���
                // 1. �̸����� ã�� ���
                // 2. tag���� ã�� ���
                // 3. �ε��� �༮���� Enemy ������Ʈ �޶�� �غ���
                // ���� ���������� Enemyu ������Ʈ�� ������
                // -> Enemy ��
                Enemy enemy = hitInfo.transform.GetComponent<Enemy>();
                if (enemy != null)
                {
                    // -> �� �� �¾Ҿ� ��� �˷��ְ� �ʹ�.
                    // 1. �ε����༮���� Enemy ������Ʈ ������
                    // 2. Enemy ������Ʈ�� �ʿ��ϴ�.
                    // 3. �˷��ְ� �ʹ�.
                    enemy.OnDamageProcess();
                }
                // -> �ε��� ������ �Ѿ� ���� Ƣ������
                bulletEffect.position = hitInfo.point;
                // effect �� ���ϴ� ������ normal �������� ��������
                bulletEffect.forward = hitInfo.normal;
                psBulletEffect.Stop();
                psBulletEffect.Play();
                asBulletSound.Stop();
                asBulletSound.Play();
            }
        }
    }

    private void FireGrenade()
    {
        //����ڰ� �߻��ư�� ������ �ѱ��� ���ϴ� �������� �ѱ����� �Ѿ��� �߻�ǵ��� �ϰ������.
        // 1. ����ڰ� �߻��ư�� �������ϱ�
        if (Input.GetButtonDown("Fire1"))
        {
            // źâ�� �Ѿ��� ���� ���� �߻��ϰ� �ʹ�.
            if (bulletPool.Count > 0)
            {
                GameObject bullet = bulletPool[0];
                // 3. �Ѿ˹߻�(��ġ)
                bullet.transform.position = firePosition.position;
                // ���⼳��
                bullet.transform.forward = firePosition.forward;
                bullet.SetActive(true);
                // ����Ʈ���� ���Ž�������
                bulletPool.RemoveAt(0);
            }
        }
    }

    private void MakeGrenade()
    {
        // źâ�ʱ�ȭ
        bulletPool = new List<GameObject>();
        // źâ�� �Ѿ��� �ϳ��� ���� �ְ� �ʹ�.
        for (int i = 0; i < bulletPoolSize; i++)
        {
            // �Ѿ��� �����.
            GameObject bullet = Instantiate(bulletFactory);
            // źâ�� �ִ´�.
            bulletPool.Add(bullet);
            // �Ѿ��� ��Ȱ��ȭ ��Ų��.
            bullet.SetActive(false);
        }
    }


}
