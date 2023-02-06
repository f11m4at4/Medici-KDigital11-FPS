using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// 타겟쪽으로 이동하고 싶다.
// 필요속성 : 이동속도, Character Controller, 타겟
// FSM 뼈대(목차) 만들기
// NavMeshAgent 를 이용해서 이동하고싶다.
// 필요속성 : NavMeshAgent 컴포넌트
// 애니메이션을 적용하고 싶다.
// -> 필요속성 : animator 컴포넌트
public class Enemy : MonoBehaviour
{
    // 필요속성 : 이동속도, Character Controller, 타겟
    public float speed = 5;
    CharacterController cc;
    Transform target;

    // FSM 뼈대(목차) 만들기
    public enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Damage,
        Die
    };

    public EnemyState m_State;

    // 필요속성 : NavMeshAgent 컴포넌트
    NavMeshAgent agent;

    // -> 필요속성 : animator 컴포넌트
    Animator anim;

    void Start()
    {
        // 1. 타겟찾기
        GameObject player = GameObject.Find("Player");
        if(player != null)
        {
            target = player.transform;
        }
        // 2. 필요한 컴포넌트 얻어오기
        cc = GetComponent<CharacterController>();
        agent = GetComponent<NavMeshAgent>();

        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // 인트로상태일때는 아무것도 못하도록 하고 싶다.
        if(GameManager.Instance.IsIntro())
        {
            return;
        }

        print("State : " + m_State);
        // 목차
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

    // 일정시간이 지나면 상태를 Move 로 전환하고 싶다.
    // 필요속성 : 대기시간, 경과시간
    public float idleDelayTime = 2;
    float currentTime = 0;
    private void Idle()
    {
        currentTime += Time.deltaTime;
        if(currentTime > idleDelayTime)
        {
            currentTime = 0;
            m_State = EnemyState.Move;
            // 애니메이션 상태도 Move로 전환하고 싶다.
            anim.SetTrigger("Move");
            // 길찾기 활성화
            agent.enabled = true;
            curPatrol = Random.Range(0, patrolPositions.Length);
        }
    }

    // 타겟이 공격범위 안에 들어오면 상태를 공격을 전환하고 싶다.
    // 필요속성 : 공격범위
    //[HideInInspector]
    [SerializeField]
    private float attackRange = 2;

    // 일정거리 밖이면 패트롤하고, 그렇지 않으면 타겟쪽으로 이동
    public float patrolDistance = 10;
    public Transform[] patrolPositions;
    int curPatrol;
    private void Move()
    {
        // 타겟쪽으로 이동하고 싶다.
        // 1. 방향이 필요
        Vector3 dir = target.position - transform.position;
        float distance = dir.magnitude;

        // 일정거리 밖이면 패트롤하고,
        // 나와 타겟과의 거리가 패트롤거릴 벗어났다면(패트롤범위)
        // 패트롤 가능하다면 혹은
        // 시야범위안에 안들어왔다면
        float cosFov = Mathf.Cos(fov * Mathf.Deg2Rad);
        float dot = Vector3.Dot(transform.forward, dir.normalized);
        print("dot : " + dot + ", cosFov : " + cosFov);
        if (distance > patrolDistance || dot < cosFov)
        {
            // 패트롤하기
            // 월드에 배치된 특정 위치중에 하나로 이동한다.
            agent.destination = patrolPositions[curPatrol].position;
            // 특정위치를 구해야한다.
            // -> 특정위치를 언제 구해야 하나?
            // -> 이동 상태로 전환될때
            // -> 특정위치에 도착했을 때
            if(Vector3.Distance(transform.position, patrolPositions[curPatrol].position) < 0.1f)
            {
                curPatrol = Random.Range(0, patrolPositions.Length);
            }
        }
        // 그렇지 않으면 타겟쪽으로 이동
        else
        {
            // NavMeshAgent 를 이용 
            agent.destination = target.position;
        }
        /*dir.y = 0;
        dir.Normalize();
        // 2. 이동하고 싶다.
        cc.SimpleMove(dir * speed);

        // 3. 타겟방향으로 회전하고 싶다.
        //transform.LookAt(target);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
        */
        // 타겟이 공격범위 안에 들어오면 상태를 공격으로 전환하고 싶다.
        // 1. 타겟과 나와의 거리 계산
        // 2. 타겟이 공격범위 안에 들어왔으니까
        if (distance < attackRange)
        {
            // 길찾기 중지
            agent.enabled = false;
            // 3. 상태를 공격으로 전환하고 싶다.
            m_State = EnemyState.Attack;
            // 바로 공격할 수 있게 경과시간을 공격시간으로
            // 설정해 버리자
            currentTime = attackDelayTime;
            anim.SetTrigger("AttackStart");

        }

    }


    // 사용자가 필요한 아이콘(기즈모) 를 그릴수 있게 해준다.
    // 시야각을 표시해보자
    // 필요속성 : 시야각
    public float fov = 90;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, targetPos);

        // 시야각을 그려보자
        Vector3 dir1 = Quaternion.AngleAxis(-fov, transform.up) * transform.forward;
        Vector3 dir2 = Quaternion.AngleAxis(fov, transform.up) * transform.forward;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + dir1 * 10);
        Gizmos.DrawLine(transform.position, transform.position + dir2 * 10);
    }


    // 일정시간에 한번씩 공격하고 싶다.
    // 필요속성 : 공격대기시간
    public float attackDelayTime = 2;
    private void Attack()
    {
        Vector3 dir = target.position - transform.position;
        dir.y = 0;

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);

        // 일정시간에 한번씩 공격하고 싶다.
        currentTime += Time.deltaTime;
        if(currentTime > attackDelayTime)
        {
            currentTime = 0;
            print("공격");
            anim.SetTrigger("Attack");
            
        }

        // 타겟이 공격범위를 벗어나면 상태를 이동으로 전환하고 싶다. 
        // 1. 타겟과의 거리
        float distance = Vector3.Distance(target.position, transform.position);
        // 2. 타겟이 공격범위를 벗어났다면 들어왔으니까
        if (distance > attackRange)
        {
            // 3. 상태를 이동으로 전환
            m_State = EnemyState.Move;
            agent.enabled = true;
            anim.SetTrigger("Move");
        }
    }

    // 피격당했을 때 호출되는 이벤트 함수
    // 체력이 남아 있으면 상태를 Damage 로 전환하고
    // 그렇지 않으면 상태를 Die 로 전환
    // 1. 나의 기본 HP 3칸
    public int basicHp = 3;
    // 2. 공격을 받으면
    public void OnDamageProcess()
    {
        // 만약 상태가 Die 라면
        if (m_State == EnemyState.Die)
        {
            // 더이상 진행되지 않게 하자.
            return;
        }
        // 코루틴 중지시키자
        StopAllCoroutines();

        // 길찾기 중지
        agent.enabled = false;
        currentTime = 0;
        // HP가 -1씩 깎인다.
        basicHp--;
        // 3. 체력이 남으면
        if(basicHp>0)
        {
            // Damage로 이동
            m_State = EnemyState.Damage;

            anim.SetTrigger("Damage");
            // 코루틴실행
            StartCoroutine(Damage());
        }
        // 4. 그렇지 않으면
        else
        {
            // Die로 이동
            m_State = EnemyState.Die;
            anim.SetTrigger("Die");
            // 충돌체 꺼버리자
            StartCoroutine(Die());
            cc.enabled = false;
        }
    }

    // 필요속성 : 대기시간
    public float damageDelayTime = 2;
    // 넉백거리
    public float knockbackPower = 2;
    Vector3 targetPos;
    private IEnumerator Damage()
    {
        // 뒤로(Shoot 방향) 일정 거리만큼 밀리도록 하자
        // 최종 위치, 밀릴거리
        Vector3 dir = Camera.main.transform.forward;
        dir.y = 0;
        targetPos = transform.position + dir * knockbackPower;

        // 일정시간만큼 밀리도록 하자
        //while (currentTime < 0.2f)
        while(Vector3.Distance(transform.position, targetPos) > 0.5f)
        {
            currentTime += Time.deltaTime;
            // 계속 이동하자
            transform.position = Vector3.Lerp(transform.position, targetPos, knockbackPower * Time.deltaTime);
            //cc.Move(Camera.main.transform.forward * knockbackPower * Time.deltaTime);
            yield return null;
        }

        // 일정시간 기다렸다가
        yield return new WaitForSeconds(damageDelayTime);
        // 상태를 대기로 전환하고 싶다.
        m_State = EnemyState.Idle;
    }

    

    // 
    // 필요속성 : 이동속도
    public float dieSpeed = 0.5f;
    bool canDieStart = false;
    private IEnumerator Die()
    {
        // 적을 잡을 때마다 현재 점수를 1씩 올리고 싶다.
        ScoreManager.Instance.CurScore++;

        //2초 기다렸다가 아래로 내려가게 하자
        //yield return new WaitForSeconds(2);
        while (canDieStart == false)
        {
            yield return null;
        }
        // 아래로 등속이동하며 위치가 -2 미터 내려가면
        // -> 위치가 -2 보다 큰동안
        while (transform.position.y > -2)
        {
            //  -> 아래로 등속이동하고
            // 1. 아래로 등속이동하며
            transform.position += Vector3.down * dieSpeed * Time.deltaTime;
            //  -> 잠깐 쉬겠다.
            yield return null;
        }
        

        // -> 끝나면 없애자
        Destroy(gameObject);

    }


}
