using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 자신이 향하는 방향으로 발사되고 싶다.
// 필요속성 : 발사속도
//2초후에 총알이 없어지게 하자
public class Bullet : MonoBehaviour
{
    // 필요속성 : 발사속도
    public float speed = 5;
    float currentTime = 0;
    // Start is called before the first frame update
    void Start()
    {
    }

    // 활성화될때 호출되는 이벤트 함수
    private void OnEnable()
    {
        // 자신이 향하는 방향으로 발사되고 싶다.
        // rigidbody 를 이용해 발사하기
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.AddForce(transform.forward * speed);
        
    }

    // Update is called once per frame
    void Update()
    {
        //2초후에 총알이 없어지게 하자
        // 1. 시간이 흘러야한다.
        currentTime += Time.deltaTime;
        // 2. 시간이됐으니 -> 경과시간이 제거 초과
        if (currentTime > 2)
        {
            currentTime = 0;
            // 3. 없애자
            gameObject.SetActive(false);
            // 1. Player GameObject
            GameObject player = GameObject.Find("Player");
            // 2. PlayerFire 컴포넌트가 필요하다.
            PlayerFire pf = player.GetComponent<PlayerFire>();
            // 3. 탄창안에 넣어줘야한다.
            pf.bulletPool.Add(gameObject);
        }
    }
}
