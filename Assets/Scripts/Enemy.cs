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
        Die
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
            case EnemyState.Die:
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

    // Ÿ���� ���ݹ��� �ȿ� ������ ���¸� ������ ��ȯ�ϰ� �ʹ�.
    // �ʿ�Ӽ� : ���ݹ���
    //[HideInInspector]
    [SerializeField]
    private float attackRange = 2;
    private void Move()
    {
        // Ÿ�������� �̵��ϰ� �ʹ�.
        // 1. ������ �ʿ�
        Vector3 dir = target.position - transform.position;
        float distance = dir.magnitude;
        dir.y = 0;
        dir.Normalize();
        // 2. �̵��ϰ� �ʹ�.
        cc.SimpleMove(dir * speed);

        // 3. Ÿ�ٹ������� ȸ���ϰ� �ʹ�.
        //transform.LookAt(target);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);

        // Ÿ���� ���ݹ��� �ȿ� ������ ���¸� �������� ��ȯ�ϰ� �ʹ�.
        // 1. Ÿ�ٰ� ������ �Ÿ� ���
        // 2. Ÿ���� ���ݹ��� �ȿ� �������ϱ�
        if (distance < attackRange)
        {
            // 3. ���¸� �������� ��ȯ�ϰ� �ʹ�.
            m_State = EnemyState.Attack;
            // �ٷ� ������ �� �ְ� ����ð��� ���ݽð�����
            // ������ ������
            currentTime = attackDelayTime;
        }

    }

    // ����ڰ� �ʿ��� ������(�����) �� �׸��� �ְ� ���ش�.
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    // �����ð��� �ѹ��� �����ϰ� �ʹ�.
    // �ʿ�Ӽ� : ���ݴ��ð�
    public float attackDelayTime = 2;
    private void Attack()
    {
        // �����ð��� �ѹ��� �����ϰ� �ʹ�.
        currentTime += Time.deltaTime;
        if(currentTime > attackDelayTime)
        {
            currentTime = 0;
            print("����");
        }

        // Ÿ���� ���ݹ����� ����� ���¸� �̵����� ��ȯ�ϰ� �ʹ�. 
        // 1. Ÿ�ٰ��� �Ÿ�
        float distance = Vector3.Distance(target.position, transform.position);
        // 2. Ÿ���� ���ݹ����� ����ٸ� �������ϱ�
        if (distance > attackRange)
        {
            // 3. ���¸� �̵����� ��ȯ
            m_State = EnemyState.Move;
        }
    }

    // �ǰݴ����� �� ȣ��Ǵ� �̺�Ʈ �Լ�
    // ü���� ���� ������ ���¸� Damage �� ��ȯ�ϰ�
    // �׷��� ������ ���¸� Die �� ��ȯ
    // 1. ���� �⺻ HP 3ĭ
    public int basicHp = 3;
    // 2. ������ ������
    public void OnDamageProcess()
    {
        // ���� ���°� Die ���
        if (m_State == EnemyState.Die)
        {
            // ���̻� ������� �ʰ� ����.
            return;
        }
        currentTime = 0;
        // HP�� -1�� ���δ�.
        basicHp--;
        // 3. ü���� ������
        if(basicHp>0)
        {
            // Damage�� �̵�
            m_State = EnemyState.Damage;
        }
        // 4. �׷��� ������
        else
        {
            // Die�� �̵�
            m_State = EnemyState.Die;
            // �浹ü ��������
            cc.enabled = false;
        }
    }

    // �����ð� ��ٷȴٰ� ���¸� ���� ��ȯ�ϰ� �ʹ�.
    // �ʿ�Ӽ� : ���ð�
    public float damageDelayTime = 2;
    private void Damage()
    {
        currentTime += Time.deltaTime;
        if(currentTime > damageDelayTime)
        {
            m_State = EnemyState.Idle;
            currentTime = 0;
        }
    }

    // �ʿ�Ӽ� : �̵��ӵ�
    public float dieSpeed = 0.5f;
    private void die()
    {
        // �Ʒ��� ����̵��ϸ� ��ġ�� -2 ���� ��������
        // 1. �Ʒ��� ����̵��ϸ�
        transform.position += Vector3.down * dieSpeed * Time.deltaTime;
        // ���� ��ġ�� -2 ���ϸ�
        if (transform.position.y < -2)
        {
            // ���ֹ�����
            Destroy(gameObject);
        }
    
    }


}
