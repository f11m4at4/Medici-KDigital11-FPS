using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 0. ������� �Է¿����� �յ��¿�� �̵��ϰ� �ʹ�.
// �ʿ�Ӽ� : �̵��ӵ�
// 0. Character Controller ������Ʈ�� �̿��� �̵��ϰ�����
// �߷��� ����޵��� �ϰ� �ʹ�.
// �ʿ�Ӽ� : �߷°��ӵ�, �����ӵ�
// 0. ����ڰ� ������ư�� ������ ������ �ϰ� �ʹ�.
// �ʿ�Ӽ� : �����Ŀ�.
// 0. �̴������� �ϰ� �ʹ�.
// �ʿ�Ӽ� : �������� Ƚ��, ���Ƚ��
public class PlayerMove : MonoBehaviour
{
    // �ʿ�Ӽ� : �̵��ӵ�
    public float speed = 5;
    CharacterController cc = null;
    // �ʿ�Ӽ� : �߷°��ӵ�, �����ӵ�
    public float gravity = -20;
    float yVelocity = 0;
    // �ʿ�Ӽ� : �����Ŀ�.
    public float jumpPower = 5;
    // ���߿� �ִ��� ����
    //bool isInAir = false;
    // �ʿ�Ӽ� : �������� Ƚ��, ���Ƚ��
    public int jumpMaxCount = 2;
    int jmpCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // ������� �Է¿����� �յ��¿�� �̵��ϰ� �ʹ�.
        // 1. ������� �Է¿�����
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        // 2. �������ʿ�
        Vector3 dir = new Vector3(h, 0, v);
        // ī�޶� ���ϴ� �������� �����ؾ��Ѵ�.
        dir = Camera.main.transform.TransformDirection(dir);
        // �߷��� ����޵��� �ϰ� �ʹ�.
        // ��ӿ v = v0 + at
        yVelocity += gravity * Time.deltaTime;
        // y = y0 + at
        // �����׷��� ���������� �Ѵ�.
        // �ٴڿ� ���� ���� 
        // above, side, below
        //if(cc.isGrounded)
        print("collisionFlags : " + cc.collisionFlags);
        if(cc.collisionFlags == CollisionFlags.Below)
        {
            //�����ӵ��� 0���� ����� �Ѵ�.
            yVelocity = 0;
            //isInAir = false;
            jmpCount = 0;
        }

        // �ٴڿ� ���� ��
        // 0. ����ڰ� ������ư�� ������ ������ �ϰ� �ʹ�.
        // ���� Ƚ���� ��������
        // 0. �̴������� �ϰ� �ʹ�.
        // 1. �ִ�����Ƚ������ ���� �پ��� ����
        // -> ���� ���Ƚ���� �ִ�Ƚ������ �۴ٸ�
        // 2. ������ư�� �������ϱ�
        // 3. �����ϰ� �ʹ�.
        if (jmpCount < jumpMaxCount && Input.GetButtonDown("Jump"))
        {
            // ������ �ϸ� Ƚ���� �����Ѵ�.
            jmpCount++;
            yVelocity = jumpPower;           
        }

        dir.y = yVelocity;
        // 3. �̵��ϰ�ʹ�.
        // P = P0 + vt
        //transform.position += dir * speed * Time.deltaTime;
        cc.Move(dir * speed * Time.deltaTime);
    }
}
