using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 0. 사용자의 입력에따라 앞뒤좌우로 이동하고 싶다.
// 필요속성 : 이동속도
// 0. Character Controller 컴포넌트를 이용해 이동하게하자
// 중력을 적용받도록 하고 싶다.
// 필요속성 : 중력가속도, 수직속도
// 0. 사용자가 점프버튼을 누르면 점프를 하고 싶다.
// 필요속성 : 점프파워.
public class PlayerMove : MonoBehaviour
{
    // 필요속성 : 이동속도
    public float speed = 5;
    CharacterController cc = null;
    // 필요속성 : 중력가속도, 수직속도
    public float gravity = -20;
    float yVelocity = 0;
    // 필요속성 : 점프파워.
    public float jumpPower = 5;
    // 공중에 있는지 여부
    bool isInAir = false;
    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // 사용자의 입력에따라 앞뒤좌우로 이동하고 싶다.
        // 1. 사용자의 입력에따라
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        // 2. 방향이필요
        Vector3 dir = new Vector3(h, 0, v);
        // 카메라가 향하는 방향으로 변형해야한다.
        dir = Camera.main.transform.TransformDirection(dir);
        // 중력을 적용받도록 하고 싶다.
        // 등가속운동 v = v0 + at
        yVelocity += gravity * Time.deltaTime;
        // y = y0 + at
        // 수직항력을 적용시켜줘야 한다.
        // 바닥에 있을 때는 
        // above, side, below
        //if(cc.isGrounded)
        print("collisionFlags : " + cc.collisionFlags);
        if(cc.collisionFlags == CollisionFlags.Below)
        {
            //수직속도를 0으로 해줘야 한다.
            yVelocity = 0;
            isInAir = false;
        }

        // 바닥에 있을 때
        // 0. 사용자가 점프버튼을 누르면 점프를 하고 싶다.
        if(isInAir == false && Input.GetButtonDown("Jump"))
        {
            yVelocity = jumpPower;
            isInAir = true;
        }
        dir.y = yVelocity;
        // 3. 이동하고싶다.
        // P = P0 + vt
        //transform.position += dir * speed * Time.deltaTime;
        cc.Move(dir * speed * Time.deltaTime);
    }
}
