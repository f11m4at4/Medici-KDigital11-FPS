using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 타겟쪽으로 이동하고 싶다.
// 필요속성 : 이동속도, Character Controller, 타겟
// FSM 뼈대(목차) 만들기
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
        die
    };

    public EnemyState m_State;

    // Start is called before the first frame update
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
                Damage();
                break;
            case EnemyState.die:
                die();
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
        }
    }

    private void Move()
    {
        // 타겟쪽으로 이동하고 싶다.
        // 1. 방향이 필요
        Vector3 dir = target.position - transform.position;
        dir.Normalize();
        // 2. 이동하고 싶다.
        cc.SimpleMove(dir * speed);

        // 3. 타겟방향으로 회전하고 싶다.
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
