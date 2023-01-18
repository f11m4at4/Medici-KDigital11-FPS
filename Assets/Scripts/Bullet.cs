using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ڽ��� ���ϴ� �������� �߻�ǰ� �ʹ�.
// �ʿ�Ӽ� : �߻�ӵ�
//2���Ŀ� �Ѿ��� �������� ����
public class Bullet : MonoBehaviour
{
    // �ʿ�Ӽ� : �߻�ӵ�
    public float speed = 5;
    float currentTime = 0;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Ȱ��ȭ�ɶ� ȣ��Ǵ� �̺�Ʈ �Լ�
    private void OnEnable()
    {
        // �ڽ��� ���ϴ� �������� �߻�ǰ� �ʹ�.
        // rigidbody �� �̿��� �߻��ϱ�
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.AddForce(transform.forward * speed);
        
    }

    // Update is called once per frame
    void Update()
    {
        //2���Ŀ� �Ѿ��� �������� ����
        // 1. �ð��� �귯���Ѵ�.
        currentTime += Time.deltaTime;
        // 2. �ð��̵����� -> ����ð��� ���� �ʰ�
        if (currentTime > 2)
        {
            currentTime = 0;
            // 3. ������
            gameObject.SetActive(false);
            // 1. Player GameObject
            GameObject player = GameObject.Find("Player");
            // 2. PlayerFire ������Ʈ�� �ʿ��ϴ�.
            PlayerFire pf = player.GetComponent<PlayerFire>();
            // 3. źâ�ȿ� �־�����Ѵ�.
            pf.bulletPool.Add(gameObject);
        }
    }
}
