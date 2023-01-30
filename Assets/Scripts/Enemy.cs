using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Ÿ�������� �̵��ϰ� �ʹ�.
// �ʿ�Ӽ� : �̵��ӵ�, Character Controller, Ÿ��
// FSM ����(����) �����
// NavMeshAgent �� �̿��ؼ� �̵��ϰ�ʹ�.
// �ʿ�Ӽ� : NavMeshAgent ������Ʈ
// �ִϸ��̼��� �����ϰ� �ʹ�.
// -> �ʿ�Ӽ� : animator ������Ʈ
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

    // �ʿ�Ӽ� : NavMeshAgent ������Ʈ
    NavMeshAgent agent;

    // -> �ʿ�Ӽ� : animator ������Ʈ
    Animator anim;

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
        agent = GetComponent<NavMeshAgent>();

        anim = GetComponentInChildren<Animator>();
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
                //Damage();
                break;
            case EnemyState.Die:
                //Die();
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
            // �ִϸ��̼� ���µ� Move�� ��ȯ�ϰ� �ʹ�.
            anim.SetTrigger("Move");
            // ��ã�� Ȱ��ȭ
            agent.enabled = true;
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
        // NavMeshAgent �� �̿� 
        agent.destination = target.position;
        /*dir.y = 0;
        dir.Normalize();
        // 2. �̵��ϰ� �ʹ�.
        cc.SimpleMove(dir * speed);

        // 3. Ÿ�ٹ������� ȸ���ϰ� �ʹ�.
        //transform.LookAt(target);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
        */
        // Ÿ���� ���ݹ��� �ȿ� ������ ���¸� �������� ��ȯ�ϰ� �ʹ�.
        // 1. Ÿ�ٰ� ������ �Ÿ� ���
        // 2. Ÿ���� ���ݹ��� �ȿ� �������ϱ�
        if (distance < attackRange)
        {
            // ��ã�� ����
            agent.enabled = false;
            // 3. ���¸� �������� ��ȯ�ϰ� �ʹ�.
            m_State = EnemyState.Attack;
            // �ٷ� ������ �� �ְ� ����ð��� ���ݽð�����
            // ������ ������
            currentTime = attackDelayTime;
            anim.SetTrigger("AttackStart");

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
            anim.SetTrigger("Attack");
            //�÷��̾��� ü���� -1 ��´�.
            // 1. Player �� �־���Ѵ�.
            // 2. PlayerHealth �� �־���Ѵ�.
            // hp ����
            // 3. ü�� -1 ����
            // hp = hp - 1
            PlayerHealth.Instance.SetHP(PlayerHealth.Instance.GetHP() - 1);
        }

        // Ÿ���� ���ݹ����� ����� ���¸� �̵����� ��ȯ�ϰ� �ʹ�. 
        // 1. Ÿ�ٰ��� �Ÿ�
        float distance = Vector3.Distance(target.position, transform.position);
        // 2. Ÿ���� ���ݹ����� ����ٸ� �������ϱ�
        if (distance > attackRange)
        {
            // 3. ���¸� �̵����� ��ȯ
            m_State = EnemyState.Move;
            agent.enabled = true;
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
        // �ڷ�ƾ ������Ű��
        StopAllCoroutines();

        // ��ã�� ����
        agent.enabled = false;
        currentTime = 0;
        // HP�� -1�� ���δ�.
        basicHp--;
        // 3. ü���� ������
        if(basicHp>0)
        {
            // Damage�� �̵�
            m_State = EnemyState.Damage;
            // �ڷ�ƾ����
            StartCoroutine(Damage());
        }
        // 4. �׷��� ������
        else
        {
            // Die�� �̵�
            m_State = EnemyState.Die;
            // �浹ü ��������
            StartCoroutine(Die());
            cc.enabled = false;
        }
    }

    // �ʿ�Ӽ� : ���ð�
    public float damageDelayTime = 2;
    private IEnumerator Damage()
    {
        // �����ð� ��ٷȴٰ�
        yield return new WaitForSeconds(damageDelayTime);
        // ���¸� ���� ��ȯ�ϰ� �ʹ�.
        m_State = EnemyState.Idle;
    }

    // 
    // �ʿ�Ӽ� : �̵��ӵ�
    public float dieSpeed = 0.5f;
    private IEnumerator Die()
    {
        // �Ʒ��� ����̵��ϸ� ��ġ�� -2 ���� ��������
        // -> ��ġ�� -2 ���� ū����
        while (transform.position.y > -2)
        {
            //  -> �Ʒ��� ����̵��ϰ�
            // 1. �Ʒ��� ����̵��ϸ�
            transform.position += Vector3.down * dieSpeed * Time.deltaTime;
            //  -> ��� ���ڴ�.
            yield return null;
        }
        // -> ������ ������
        Destroy(gameObject);
    }


}
