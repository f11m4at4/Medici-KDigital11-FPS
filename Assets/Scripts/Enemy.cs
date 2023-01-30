using System;
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
        }
    }

    // 타겟이 공격범위 안에 들어오면 상태를 공격을 전환하고 싶다.
    // 필요속성 : 공격범위
    //[HideInInspector]
    [SerializeField]
    private float attackRange = 2;
    private void Move()
    {
        // 타겟쪽으로 이동하고 싶다.
        // 1. 방향이 필요
        Vector3 dir = target.position - transform.position;
        float distance = dir.magnitude;
        // NavMeshAgent 를 이용 
        agent.destination = target.position;
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
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
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
            //플레이어의 체력을 -1 깍는다.
            // 1. Player 가 있어야한다.
            // 2. PlayerHealth 가 있어야한다.
            // hp 설정
            // 3. 체력 -1 감소
            // hp = hp - 1
            PlayerHealth.Instance.SetHP(PlayerHealth.Instance.GetHP() - 1);
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
            // 코루틴실행
            StartCoroutine(Damage());
        }
        // 4. 그렇지 않으면
        else
        {
            // Die로 이동
            m_State = EnemyState.Die;
            // 충돌체 꺼버리자
            StartCoroutine(Die());
            cc.enabled = false;
        }
    }

    // 필요속성 : 대기시간
    public float damageDelayTime = 2;
    private IEnumerator Damage()
    {
        // 일정시간 기다렸다가
        yield return new WaitForSeconds(damageDelayTime);
        // 상태를 대기로 전환하고 싶다.
        m_State = EnemyState.Idle;
    }

    // 
    // 필요속성 : 이동속도
    public float dieSpeed = 0.5f;
    private IEnumerator Die()
    {
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
