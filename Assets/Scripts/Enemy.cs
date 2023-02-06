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
        // ��Ʈ�λ����϶��� �ƹ��͵� ���ϵ��� �ϰ� �ʹ�.
        if(GameManager.Instance.IsIntro())
        {
            return;
        }

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
                if (canDieStart == false)
                {
                    currentTime += Time.deltaTime;
                    if (currentTime > 2)
                    {
                        canDieStart = true;
                    }
                }
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
            curPatrol = Random.Range(0, patrolPositions.Length);
        }
    }

    // Ÿ���� ���ݹ��� �ȿ� ������ ���¸� ������ ��ȯ�ϰ� �ʹ�.
    // �ʿ�Ӽ� : ���ݹ���
    //[HideInInspector]
    [SerializeField]
    private float attackRange = 2;

    // �����Ÿ� ���̸� ��Ʈ���ϰ�, �׷��� ������ Ÿ�������� �̵�
    public float patrolDistance = 10;
    public Transform[] patrolPositions;
    int curPatrol;
    private void Move()
    {
        // Ÿ�������� �̵��ϰ� �ʹ�.
        // 1. ������ �ʿ�
        Vector3 dir = target.position - transform.position;
        float distance = dir.magnitude;

        // �����Ÿ� ���̸� ��Ʈ���ϰ�,
        // ���� Ÿ�ٰ��� �Ÿ��� ��Ʈ�ѰŸ� ����ٸ�(��Ʈ�ѹ���)
        // ��Ʈ�� �����ϴٸ� Ȥ��
        // �þ߹����ȿ� �ȵ��Դٸ�
        float cosFov = Mathf.Cos(fov * Mathf.Deg2Rad);
        float dot = Vector3.Dot(transform.forward, dir.normalized);
        print("dot : " + dot + ", cosFov : " + cosFov);
        if (distance > patrolDistance || dot < cosFov)
        {
            // ��Ʈ���ϱ�
            // ���忡 ��ġ�� Ư�� ��ġ�߿� �ϳ��� �̵��Ѵ�.
            agent.destination = patrolPositions[curPatrol].position;
            // Ư����ġ�� ���ؾ��Ѵ�.
            // -> Ư����ġ�� ���� ���ؾ� �ϳ�?
            // -> �̵� ���·� ��ȯ�ɶ�
            // -> Ư����ġ�� �������� ��
            if(Vector3.Distance(transform.position, patrolPositions[curPatrol].position) < 0.1f)
            {
                curPatrol = Random.Range(0, patrolPositions.Length);
            }
        }
        // �׷��� ������ Ÿ�������� �̵�
        else
        {
            // NavMeshAgent �� �̿� 
            agent.destination = target.position;
        }
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
    // �þ߰��� ǥ���غ���
    // �ʿ�Ӽ� : �þ߰�
    public float fov = 90;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, targetPos);

        // �þ߰��� �׷�����
        Vector3 dir1 = Quaternion.AngleAxis(-fov, transform.up) * transform.forward;
        Vector3 dir2 = Quaternion.AngleAxis(fov, transform.up) * transform.forward;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + dir1 * 10);
        Gizmos.DrawLine(transform.position, transform.position + dir2 * 10);
    }


    // �����ð��� �ѹ��� �����ϰ� �ʹ�.
    // �ʿ�Ӽ� : ���ݴ��ð�
    public float attackDelayTime = 2;
    private void Attack()
    {
        Vector3 dir = target.position - transform.position;
        dir.y = 0;

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);

        // �����ð��� �ѹ��� �����ϰ� �ʹ�.
        currentTime += Time.deltaTime;
        if(currentTime > attackDelayTime)
        {
            currentTime = 0;
            print("����");
            anim.SetTrigger("Attack");
            
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
            anim.SetTrigger("Move");
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

            anim.SetTrigger("Damage");
            // �ڷ�ƾ����
            StartCoroutine(Damage());
        }
        // 4. �׷��� ������
        else
        {
            // Die�� �̵�
            m_State = EnemyState.Die;
            anim.SetTrigger("Die");
            // �浹ü ��������
            StartCoroutine(Die());
            cc.enabled = false;
        }
    }

    // �ʿ�Ӽ� : ���ð�
    public float damageDelayTime = 2;
    // �˹�Ÿ�
    public float knockbackPower = 2;
    Vector3 targetPos;
    private IEnumerator Damage()
    {
        // �ڷ�(Shoot ����) ���� �Ÿ���ŭ �и����� ����
        // ���� ��ġ, �и��Ÿ�
        Vector3 dir = Camera.main.transform.forward;
        dir.y = 0;
        targetPos = transform.position + dir * knockbackPower;

        // �����ð���ŭ �и����� ����
        //while (currentTime < 0.2f)
        while(Vector3.Distance(transform.position, targetPos) > 0.5f)
        {
            currentTime += Time.deltaTime;
            // ��� �̵�����
            transform.position = Vector3.Lerp(transform.position, targetPos, knockbackPower * Time.deltaTime);
            //cc.Move(Camera.main.transform.forward * knockbackPower * Time.deltaTime);
            yield return null;
        }

        // �����ð� ��ٷȴٰ�
        yield return new WaitForSeconds(damageDelayTime);
        // ���¸� ���� ��ȯ�ϰ� �ʹ�.
        m_State = EnemyState.Idle;
    }

    

    // 
    // �ʿ�Ӽ� : �̵��ӵ�
    public float dieSpeed = 0.5f;
    bool canDieStart = false;
    private IEnumerator Die()
    {
        // ���� ���� ������ ���� ������ 1�� �ø��� �ʹ�.
        ScoreManager.Instance.CurScore++;

        //2�� ��ٷȴٰ� �Ʒ��� �������� ����
        //yield return new WaitForSeconds(2);
        while (canDieStart == false)
        {
            yield return null;
        }
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
