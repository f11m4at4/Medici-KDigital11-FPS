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
    // Start is called before the first frame update
    void Start()
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

    // Update is called once per frame
    void Update()
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
}
