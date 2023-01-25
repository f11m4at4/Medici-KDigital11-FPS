using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ÿ�������� �̵��ϰ� �ʹ�.
// �ʿ�Ӽ� : �̵��ӵ�, Character Controller, Ÿ��
// FSM ����(����) �����
public class Enemy : MonoBehaviour
{
    // �ʿ�Ӽ� : �̵��ӵ�, Character Controller, Ÿ��
    public float speed = 5;
    CharacterController cc;
    Transform target;

    // FSM ����(����) �����
    public enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Damage,
        die
    };

    public EnemyState m_State;

    // Start is called before the first frame update
    void Start()
    {
        // 1. Ÿ��ã��
        GameObject player = GameObject.Find("Player");
        if(player != null)
        {
            target = player.transform;
        }
        // 2. �ʿ��� ������Ʈ ������
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        print("State : " + m_State);
        // ����
        switch(m_State)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Move:
                Move();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Damage:
                Damage();
                break;
            case EnemyState.die:
                die();
                break;
        }
    }

    // �����ð��� ������ ���¸� Move �� ��ȯ�ϰ� �ʹ�.
    // �ʿ�Ӽ� : ���ð�, ����ð�
    public float idleDelayTime = 2;
    float currentTime = 0;
    private void Idle()
    {
        currentTime += Time.deltaTime;
        if(currentTime > idleDelayTime)
        {
            currentTime = 0;
            m_State = EnemyState.Move;
        }
    }

    private void Move()
    {
        // Ÿ�������� �̵��ϰ� �ʹ�.
        // 1. ������ �ʿ�
        Vector3 dir = target.position - transform.position;
        dir.Normalize();
        // 2. �̵��ϰ� �ʹ�.
        cc.SimpleMove(dir * speed);

        // 3. Ÿ�ٹ������� ȸ���ϰ� �ʹ�.
        //transform.LookAt(target);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
    }

    private void Attack()
    {
        throw new NotImplementedException();
    }

    private void Damage()
    {
        throw new NotImplementedException();
    }

    private void die()
    {
        throw new NotImplementedException();
    }


}
