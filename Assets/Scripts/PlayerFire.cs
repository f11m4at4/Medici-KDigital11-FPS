using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//사용자가 발사버튼을 누르면 총구가 향하는 방향으로 총구에서 총알이 발사되도록 하고시프다.
// 필요속성 : 총구, 총알공장
// 0. 탄창을 만들어서 사용하고 싶다.
// -> 필요속성 : 탄창, 탄창 크기
public class PlayerFire : MonoBehaviour
{
    // 필요속성 : 총구, 총알공장
    public Transform firePosition;
    public GameObject bulletFactory;
    // -> 필요속성 : 탄창, 탄창 크기
    public int bulletPoolSize = 10;
    //GameObject[] bulletPool;
    public List<GameObject> bulletPool;

    // 유탄발사인지 아닌지 기억
    public bool bGrenade = false;

    // 필요속성 : 총알 파편
    public Transform bulletEffect;
    ParticleSystem psBulletEffect;
    AudioSource asBulletSound;

    void Swap(ref int a, ref int b)
    {
        int temp = a;
        a = b;
        b = temp;
    }

    void Start()
    {
        int c = 10;
        int d = 20;
        Swap(ref c, ref d);
        print("c : " + c + ", d : " + d);

        // 유탄발사 사용할 때만 총알 만들어놓자
        if(bGrenade)
        {
            //총알 만들어놓자
            MakeGrenade();
        }
        else
        {
            psBulletEffect = bulletEffect.GetComponent<ParticleSystem>();
            asBulletSound = bulletEffect.GetComponent<AudioSource>();
        }
    }

    
    // Update is called once per frame
    void Update()
    {
        // 유탄발사 활성화 되어 있다면
        if (bGrenade)
        {
            // 총알발사되도록
            FireGrenade();
            return;
        }

        // Ray 를 이용한 총알발사
        FireRay();
    }

    // Ray 를 이용한 총알발사
    void FireRay()
    {
        // 1. 사용자가 발사버튼을 눌렀으니까 
        if (Input.GetButtonDown("Fire1"))
        {
            // 2. RaycastHit 그릇필요
            RaycastHit hitInfo = new RaycastHit();
            // 3. Ray 필요
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            // 4. 총알발사
            bool bHit = Physics.Raycast(ray, out hitInfo);

            // 부딪혔다면
            if (bHit)
            {
                // -> 부딪힌 지점에 총알 파편 튀게하자
                bulletEffect.position = hitInfo.point;
                // effect 가 향하는 방향을 normal 방향으로 맞춰주자
                bulletEffect.forward = hitInfo.normal;
                psBulletEffect.Stop();
                psBulletEffect.Play();
                asBulletSound.Stop();
                asBulletSound.Play();
            }
        }
    }

    private void FireGrenade()
    {
        //사용자가 발사버튼을 누르면 총구가 향하는 방향으로 총구에서 총알이 발사되도록 하고시프다.
        // 1. 사용자가 발사버튼을 눌렀으니까
        if (Input.GetButtonDown("Fire1"))
        {
            // 탄창에 총알이 있을 때만 발사하고 싶다.
            if (bulletPool.Count > 0)
            {
                GameObject bullet = bulletPool[0];
                // 3. 총알발사(위치)
                bullet.transform.position = firePosition.position;
                // 방향설정
                bullet.transform.forward = firePosition.forward;
                bullet.SetActive(true);
                // 리스트에서 제거시켜주자
                bulletPool.RemoveAt(0);
            }
        }
    }

    private void MakeGrenade()
    {
        // 탄창초기화
        bulletPool = new List<GameObject>();
        // 탄창에 총알을 하나씩 만들어서 넣고 싶다.
        for (int i = 0; i < bulletPoolSize; i++)
        {
            // 총알을 만든다.
            GameObject bullet = Instantiate(bulletFactory);
            // 탄창에 넣는다.
            bulletPool.Add(bullet);
            // 총알을 비활성화 시킨다.
            bullet.SetActive(false);
        }
    }


}
